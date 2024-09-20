using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("command")]
public partial class Command
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("account")]
    public long Account { get; set; }

    [Column("order")]
    public long? Order { get; set; }

    [Column("tiker")]
    public int Tiker { get; set; }

    [Column("type")]
    public int Type { get; set; }

    [Column("is_complete")]
    public bool IsComplete { get; set; }

    [ForeignKey("Account")]
    [InverseProperty("Commands")]
    public virtual Account AccountNavigation { get; set; } = null!;

    [ForeignKey("Type")]
    [InverseProperty("Commands")]
    public virtual CommandType TypeNavigation { get; set; } = null!;
}
