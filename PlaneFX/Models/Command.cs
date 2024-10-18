using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("command")]
[Index("IsComplete", Name = "command_is_complete_index")]
public partial class Command
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("account")]
    public long Account { get; set; }

    [Column("order")]
    public long? Order { get; set; }

    [Column("volume")]
    [Precision(10, 5)]
    public decimal? Volume { get; set; }

    [Column("ticker")]
    [StringLength(6)]
    public string? Ticker { get; set; }

    [Column("price")]
    [Precision(10, 5)]
    public decimal? Price { get; set; }

    [Column("type")]
    public int Type { get; set; }

    [Column("order_type")]
    [StringLength(15)]
    public string? OrderType { get; set; }

    [Column("is_complete")]
    public bool IsComplete { get; set; }

    [ForeignKey("Account")]
    [InverseProperty("Commands")]
    public virtual Account AccountNavigation { get; set; } = null!;
}
