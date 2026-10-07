using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class ComponentDTO
{
    public decimal Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Content { get; set; } = string.Empty;

    public static ComponentDTO ConvertFrom(Component entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Content = entity.Content,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(Component entity)
    {
        entity.Name = Name;
        entity.Content = Content;
    }
}
