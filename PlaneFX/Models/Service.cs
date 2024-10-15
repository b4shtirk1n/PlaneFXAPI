using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Keyless]
[Table("service")]
public partial class Service
{
    [Column("tickers", TypeName = "json")]
    public string? Tickers { get; set; }
}
