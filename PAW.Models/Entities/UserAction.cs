using System;
using System.Collections.Generic;

namespace PAW.Models;

public partial class UserAction
{
    public int CrudId { get; set; }
    public decimal? Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
