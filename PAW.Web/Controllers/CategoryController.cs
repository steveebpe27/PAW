using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class CategoryController(ICategoryService service, ILogger<CategoryController> logger)
    : CrudMvcController<CategoryDTO>(service, logger)
{
}
