// Tipo de cambio: consulta directa desde el navegador y caché con vencimiento.
(() => {
    'use strict';

    const API_URL = 'https://api.hacienda.go.cr/indicadores/tc';
    const CACHE_KEY = 'paw.exchangeRate.v1';
    const CACHE_TTL_MS = 15 * 60 * 1000;
    const MAX_STALE_MS = 24 * 60 * 60 * 1000;
    const TIMEOUT_MS = 10000;
    let memoryCache = null;
    let pendingRequest = null;

    function validDate(value) {
        if (typeof value !== 'string' || !/^\d{4}-\d{2}-\d{2}$/.test(value)) return false;
        const date = new Date(value + 'T00:00:00Z');
        return Number.isFinite(date.getTime()) && date.toISOString().slice(0, 10) === value;
    }

    function validRates(data) {
        const buy = data?.dolar?.compra;
        const sell = data?.dolar?.venta;
        const euro = data?.euro;
        return [buy?.valor, sell?.valor, euro?.dolares, euro?.colones]
            .every(value => typeof value === 'number' && Number.isFinite(value) && value > 0)
            && [buy?.fecha, sell?.fecha, euro?.fecha].every(validDate);
    }

    function validCache(entry) {
        return entry?.version === 1 && validRates(entry.data)
            && Number.isFinite(entry.savedAt) && entry.savedAt > 0
            && entry.savedAt <= Date.now()
            && Date.now() - entry.savedAt < MAX_STALE_MS;
    }

    function readCache() {
        try {
            const stored = JSON.parse(window.localStorage.getItem(CACHE_KEY) || 'null');
            if (validCache(stored)) memoryCache = stored;
            else window.localStorage.removeItem(CACHE_KEY);
        } catch { /* Almacenamiento bloqueado o JSON dañado: usar memoria o consultar. */ }
        if (validCache(memoryCache)) return memoryCache;
        memoryCache = null;
        return null;
    }

    function saveCache(data) {
        const entry = { version: 1, savedAt: Date.now(), data };
        memoryCache = entry;
        try { window.localStorage.setItem(CACHE_KEY, JSON.stringify(entry)); }
        catch { /* El panel funciona en memoria si el navegador no permite guardar. */ }
        return entry;
    }

    // Método público de site.js: window.getExchangeRate({ forceRefresh: true }).
    // La promesa compartida evita duplicar solicitudes simultáneas en esta pestaña.
    window.getExchangeRate = function getExchangeRate({ forceRefresh = false } = {}) {
        if (pendingRequest) return pendingRequest;
        const cached = readCache();
        if (!forceRefresh && cached && Date.now() - cached.savedAt < CACHE_TTL_MS) {
            return Promise.resolve({ data: cached.data, savedAt: cached.savedAt, fromCache: true, stale: false });
        }

        pendingRequest = (async () => {
            const controller = new AbortController();
            const timeout = window.setTimeout(() => controller.abort(), TIMEOUT_MS);
            try {
                const response = await fetch(API_URL, {
                    method: 'GET',
                    mode: 'cors',
                    cache: 'no-store',
                    credentials: 'omit',
                    headers: { Accept: 'application/json' },
                    signal: controller.signal
                });
                if (!response.ok) throw new Error('La fuente respondió con HTTP ' + response.status);
                const data = await response.json();
                if (!validRates(data)) throw new Error('La respuesta no contiene indicadores válidos.');
                const entry = saveCache(data);
                return { data, savedAt: entry.savedAt, fromCache: false, stale: false };
            } catch (error) {
                const previous = readCache();
                if (previous) {
                    // No renovar savedAt: el dato sigue teniendo su antigüedad original.
                    return { data: previous.data, savedAt: previous.savedAt, fromCache: true, stale: true };
                }
                throw error;
            } finally {
                window.clearTimeout(timeout);
                pendingRequest = null;
            }
        })();
        return pendingRequest;
    };

    function initExchangeRate() {
        const widget = document.querySelector('[data-exchange-widget]');
        if (!widget) return;
        const body = widget.querySelector('#exchange-body');
        const values = widget.querySelector('[data-exchange-values]');
        const status = widget.querySelector('[data-exchange-status]');
        const cacheText = widget.querySelector('[data-exchange-cache]');
        const refresh = widget.querySelector('[data-exchange-refresh]');
        const toggle = widget.querySelector('[data-exchange-toggle]');
        const number = new Intl.NumberFormat('es-CR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        const euroNumber = new Intl.NumberFormat('es-CR', { minimumFractionDigits: 4, maximumFractionDigits: 4 });
        const time = new Intl.DateTimeFormat('es-CR', { hour: '2-digit', minute: '2-digit', second: '2-digit' });
        const setText = (key, text) => { widget.querySelector('[data-rate="' + key + '"]').textContent = text; };
        const dateText = iso => iso.split('-').reverse().join('/');

        function render(result) {
            const data = result.data;
            setText('buy', '₡ ' + number.format(data.dolar.compra.valor));
            setText('sell', '₡ ' + number.format(data.dolar.venta.valor));
            setText('buy-date', dateText(data.dolar.compra.fecha));
            setText('sell-date', dateText(data.dolar.venta.fecha));
            setText('euro-usd', '$ ' + euroNumber.format(data.euro.dolares));
            setText('euro-crc', '₡ ' + number.format(data.euro.colones));
            setText('euro-date', dateText(data.euro.fecha));
            values.hidden = false;
            widget.dataset.state = result.stale ? 'stale' : 'ready';
            status.textContent = result.stale ? 'Sin actualizar · copia anterior' : result.fromCache ? 'Datos en caché · 15 min' : 'Consulta actualizada';
            cacheText.textContent = 'Consulta guardada a las ' + time.format(new Date(result.savedAt))
                + (result.stale ? '. No se pudo contactar a Hacienda; no son datos recién consultados.' : '. Las fechas de referencia corresponden a Hacienda.');
        }

        async function load(forceRefresh = false) {
            refresh.disabled = true;
            widget.setAttribute('aria-busy', 'true');
            widget.dataset.loading = 'true';
            status.textContent = 'Consultando Hacienda…';
            try { render(await window.getExchangeRate({ forceRefresh })); }
            catch {
                values.hidden = true;
                widget.dataset.state = 'error';
                status.textContent = 'No se pudo consultar el tipo de cambio.';
                cacheText.textContent = 'Revisá tu conexión y presioná Actualizar para volver a intentar.';
            } finally {
                refresh.disabled = false;
                widget.setAttribute('aria-busy', 'false');
                delete widget.dataset.loading;
            }
        }

        function collapse(collapsed) {
            body.hidden = collapsed;
            toggle.setAttribute('aria-expanded', String(!collapsed));
            toggle.setAttribute('aria-label', collapsed ? 'Expandir tipo de cambio' : 'Minimizar tipo de cambio');
            toggle.querySelector('[data-exchange-toggle-icon]').textContent = collapsed ? '+' : '−';
        }
        collapse(window.matchMedia('(max-width: 700px)').matches);
        toggle.addEventListener('click', () => collapse(!body.hidden));
        refresh.addEventListener('click', () => load(true));

        // Evitar tapar el menú; al hacer scroll, el panel permanece arriba a la derecha.
        let positionPending = false;
        function position() {
            const header = document.querySelector('body > header');
            const bottom = header ? header.getBoundingClientRect().bottom : 0;
            widget.style.setProperty('--exchange-top', Math.max(16, bottom + 14) + 'px');
            positionPending = false;
        }
        function schedulePosition() {
            if (!positionPending) { positionPending = true; window.requestAnimationFrame(position); }
        }
        window.addEventListener('scroll', schedulePosition, { passive: true });
        window.addEventListener('resize', schedulePosition);
        document.addEventListener('shown.bs.collapse', schedulePosition);
        document.addEventListener('hidden.bs.collapse', schedulePosition);
        position();
        load();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', initExchangeRate);
    else initExchangeRate();
})();
