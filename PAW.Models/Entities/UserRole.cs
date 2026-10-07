using System;
using System.Collections.Generic;

namespace PAW.Models;

public partial class UserRole
{
    public int CrudId { get; set; }
    public decimal? Id { get; set; }

    public decimal? RoldId { get; set; }

    public decimal? UserId { get; set; }
}
