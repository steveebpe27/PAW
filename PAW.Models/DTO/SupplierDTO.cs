using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class SupplierDTO
{
    public int SupplierId { get; set; }
    [StringLength(255)]
    public string? SupplierName { get; set; }
    [StringLength(255)]
    public string? ContactName { get; set; }
    [StringLength(255)]
    public string? ContactTitle { get; set; }
    [StringLength(255)]
    public string? Phone { get; set; }
    [StringLength(1000)]
    public string? Address { get; set; }
    [StringLength(255)]
    public string? City { get; set; }
    [StringLength(255)]
    public string? Country { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? LastModified { get; set; }
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    public static SupplierDTO ConvertFrom(Supplier entity) => new()
    {
        SupplierId = entity.SupplierId,
        SupplierName = entity.SupplierName,
        ContactName = entity.ContactName,
        ContactTitle = entity.ContactTitle,
        Phone = entity.Phone,
        Address = entity.Address,
        City = entity.City,
        Country = entity.Country,
        LastModified = entity.LastModified,
        ModifiedBy = entity.ModifiedBy,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(Supplier entity)
    {
        entity.SupplierName = SupplierName;
        entity.ContactName = ContactName;
        entity.ContactTitle = ContactTitle;
        entity.Phone = Phone;
        entity.Address = Address;
        entity.City = City;
        entity.Country = Country;
        entity.LastModified = LastModified;
        entity.ModifiedBy = ModifiedBy;
    }
}
