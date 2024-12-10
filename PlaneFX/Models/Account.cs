using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("account")]
[Index("Number", Name = "account_number_uindex", IsUnique = true)]
public partial class Account
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("user")]
    public long User { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("number")]
    public long Number { get; set; }

    [Column("is_cent")]
    public bool IsCent { get; set; }

    [Column("profit")]
    [Precision(10, 2)]
    public decimal Profit { get; set; }

    [Column("profit_today")]
    [Precision(10, 2)]
    public decimal ProfitToday { get; set; }

    [Column("profit_yesterday")]
    [Precision(10, 2)]
    public decimal ProfitYesterday { get; set; }

    [Column("profit_week")]
    [Precision(10, 2)]
    public decimal ProfitWeek { get; set; }

    [Column("drawdown")]
    [Precision(10, 2)]
    public decimal Drawdown { get; set; }

    [Column("margin_level")]
    [Precision(10, 2)]
    public decimal MarginLevel { get; set; }

    [Column("balance")]
    [Precision(10, 2)]
    public decimal Balance { get; set; }

    [Column("profitability")]
    [Precision(10, 2)]
    public decimal? Profitability { get; set; }

    [InverseProperty("AccountNavigation")]
    public virtual ICollection<ClosedOrder> ClosedOrders { get; set; } = new List<ClosedOrder>();

    [InverseProperty("AccountNavigation")]
    public virtual ICollection<Command> Commands { get; set; } = new List<Command>();

    [InverseProperty("AccountNavigation")]
    public virtual ICollection<OpenedOrder> OpenedOrders { get; set; } = new List<OpenedOrder>();

    [ForeignKey("User")]
    [InverseProperty("Accounts")]
    public virtual User UserNavigation { get; set; } = null!;
}
