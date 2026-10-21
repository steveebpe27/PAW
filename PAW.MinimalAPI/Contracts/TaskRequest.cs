using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace PAW.MinimalAPI.Contracts;

public class TaskRequest
{
    [Required(ErrorMessage = "Ingresá el nombre de la tarea.")]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    [StringLength(1000)]
    public string? Description { get; set; }
    [StringLength(50)]
    public string? Status { get; set; }
    public DateTime? DueDate { get; set; }
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    public Dictionary<string, string[]> Validate()
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(this, new ValidationContext(this), results, true);
        var errors = results.SelectMany(result => result.MemberNames.DefaultIfEmpty(string.Empty)
            .Select(member => (Member: member, Message: result.ErrorMessage ?? "Dato inválido.")))
            .GroupBy(error => error.Member).ToDictionary(group => group.Key,
                group => group.Select(error => error.Message).ToArray());
        if (DueDate is DateTime date && (date < SqlDateTime.MinValue.Value || date > SqlDateTime.MaxValue.Value))
            errors[nameof(DueDate)] = ["La fecha debe estar dentro del rango datetime de SQL Server (desde 1753)."];
        return errors;
    }
}
