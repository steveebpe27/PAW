(() => {
    const element = document.getElementById('crudModal');
    if (!element) return;
    const modal = bootstrap.Modal.getOrCreateInstance(element);
    const body = document.getElementById('crudModalBody');
    const title = document.getElementById('crudModalTitle');
    let pending;
    function showError(message) {
        let box = body.querySelector('[data-request-error]');
        if (!box) { box = document.createElement('div'); box.className = 'alert alert-danger'; box.dataset.requestError = ''; box.setAttribute('role', 'alert'); body.prepend(box); }
        box.textContent = message;
    }
    function loadHtml(html) {
        body.innerHTML = html;
        if (window.jQuery?.validator?.unobtrusive) jQuery.validator.unobtrusive.parse(body);
    }
    document.addEventListener('click', async event => {
        const link = event.target.closest('a[data-crud]');
        if (!link) return;
        event.preventDefault();
        pending?.abort(); pending = new AbortController();
        title.textContent = link.dataset.title || 'Detalle';
        body.textContent = 'Cargando…'; modal.show();
        try {
            const response = await fetch(link.href, { signal: pending.signal, headers: { 'X-Requested-With': 'XMLHttpRequest' } });
            if (!response.ok) { body.textContent = ''; showError(response.status === 404 ? 'El registro ya no existe. Recargá el listado.' : 'No se pudo cargar el formulario o detalle. Revisá la conexión con la API.'); return; }
            loadHtml(await response.text());
        } catch (error) { if (error.name !== 'AbortError') { body.textContent = ''; showError('No se pudo conectar. Intentá de nuevo.'); } }
    });
    body.addEventListener('submit', async event => {
        const form = event.target.closest('form[data-crud-form]');
        if (!form) return;
        event.preventDefault();
        if (window.jQuery?.validator && !jQuery(form).valid()) return;
        const button = form.querySelector('[type="submit"]');
        button.disabled = true;
        body.querySelector('[data-request-error]')?.remove();
        try {
            const response = await fetch(form.action, { method: 'POST', body: new FormData(form), headers: { 'X-Requested-With': 'XMLHttpRequest' } });
            if (response.ok) { const result = await response.json(); if (result.saved) window.location.assign(result.redirect); return; }
            if (response.status === 422) { loadHtml(await response.text()); return; }
            showError(response.status === 400 ? 'La solicitud expiró o contiene datos inválidos. Recargá el listado.' : await response.text());
        } catch { showError('No se pudo conectar. Revisá el listado antes de reintentar para comprobar si el cambio se guardó.'); }
        finally { button.disabled = false; }
    });
    element.addEventListener('hidden.bs.modal', () => { pending?.abort(); });
})();
