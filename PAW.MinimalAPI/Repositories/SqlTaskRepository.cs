using System.Data;
using Microsoft.Data.SqlClient;
using PAW.MinimalAPI.Data;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.MinimalAPI.Repositories;

// ADO.NET exclusivamente. Este proyecto no referencia PAW.DataAccess ni Entity Framework.
public sealed class SqlTaskRepository(ISqlConnectionFactory connections) : ITaskRepository
{
    private const string Columns = "[Id], [Name], [Description], [Status], [DueDate], [CreatedAt], [LastModified], [ModifiedBy]";
    private const string InsertedColumns = "INSERTED.[Id], INSERTED.[Name], INSERTED.[Description], INSERTED.[Status], INSERTED.[DueDate], INSERTED.[CreatedAt], INSERTED.[LastModified], INSERTED.[ModifiedBy]";

    public async Task<PageResult<PawTask>> GetPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 25);
        await using var connection = connections.CreateConnection();
        await connection.OpenAsync(ct);
        await using var countCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Tasks];", connection);
        var count = Convert.ToInt32(await countCommand.ExecuteScalarAsync(ct));
        var pages = Math.Max(1, (int)Math.Ceiling(count / (double)pageSize));
        page = Math.Clamp(page, 1, pages);
        await using var command = new SqlCommand($"SELECT {Columns} FROM [dbo].[Tasks] ORDER BY [Id] OFFSET @offset ROWS FETCH NEXT @size ROWS ONLY;", connection);
        command.Parameters.Add("@offset", SqlDbType.Int).Value = (page - 1) * pageSize;
        command.Parameters.Add("@size", SqlDbType.Int).Value = pageSize;
        await using var reader = await command.ExecuteReaderAsync(ct);
        var items = new List<PawTask>();
        while (await reader.ReadAsync(ct)) items.Add(Read(reader));
        return new PageResult<PawTask> { Items = items, Page = page, PageSize = pageSize, TotalCount = count };
    }

    public async Task<PawTask?> FindAsync(int id, CancellationToken ct = default)
    {
        await using var connection = connections.CreateConnection();
        await connection.OpenAsync(ct);
        await using var command = new SqlCommand($"SELECT {Columns} FROM [dbo].[Tasks] WHERE [Id] = @id;", connection);
        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
        await using var reader = await command.ExecuteReaderAsync(ct);
        return await reader.ReadAsync(ct) ? Read(reader) : null;
    }

    public async Task<PawTask> CreateAsync(PawTask task, CancellationToken ct = default)
    {
        await using var connection = connections.CreateConnection();
        await connection.OpenAsync(ct);
        await using var command = new SqlCommand($"""
            INSERT INTO [dbo].[Tasks] ([Name], [Description], [Status], [DueDate], [CreatedAt], [LastModified], [ModifiedBy])
            OUTPUT {InsertedColumns}
            VALUES (@name, @description, @status, @dueDate, SYSUTCDATETIME(), SYSUTCDATETIME(), @modifiedBy);
            """, connection);
        AddEditableParameters(command, task);
        await using var reader = await command.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct)) throw new InvalidOperationException("La inserción no devolvió la tarea creada.");
        return Read(reader);
    }

    public async Task<PawTask?> UpdateAsync(int id, PawTask task, CancellationToken ct = default)
    {
        await using var connection = connections.CreateConnection();
        await connection.OpenAsync(ct);
        await using var command = new SqlCommand($"""
            UPDATE [dbo].[Tasks]
            SET [Name] = @name, [Description] = @description, [Status] = @status,
                [DueDate] = @dueDate, [LastModified] = SYSUTCDATETIME(), [ModifiedBy] = @modifiedBy
            OUTPUT {InsertedColumns}
            WHERE [Id] = @id;
            """, connection);
        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
        AddEditableParameters(command, task);
        await using var reader = await command.ExecuteReaderAsync(ct);
        return await reader.ReadAsync(ct) ? Read(reader) : null;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var connection = connections.CreateConnection();
        await connection.OpenAsync(ct);
        await using var command = new SqlCommand("DELETE FROM [dbo].[Tasks] WHERE [Id] = @id;", connection);
        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
        return await command.ExecuteNonQueryAsync(ct) > 0;
    }

    private static void AddEditableParameters(SqlCommand command, PawTask task)
    {
        command.Parameters.Add("@name", SqlDbType.NVarChar, 255).Value = (object?)task.Name ?? DBNull.Value;
        command.Parameters.Add("@description", SqlDbType.NVarChar, 1000).Value = (object?)task.Description ?? DBNull.Value;
        command.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = (object?)task.Status ?? DBNull.Value;
        command.Parameters.Add("@dueDate", SqlDbType.DateTime).Value = (object?)task.DueDate ?? DBNull.Value;
        command.Parameters.Add("@modifiedBy", SqlDbType.NVarChar, 255).Value = (object?)task.ModifiedBy ?? DBNull.Value;
    }

    private static PawTask Read(SqlDataReader reader) => new()
    {
        Id = reader.GetInt32(0),
        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
        Status = reader.IsDBNull(3) ? null : reader.GetString(3),
        DueDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
        CreatedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
        LastModified = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
        ModifiedBy = reader.IsDBNull(7) ? null : reader.GetString(7)
    };
}
