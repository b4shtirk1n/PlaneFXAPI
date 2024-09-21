using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("opened_orders")]
public partial class OpenedOrder
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("account")]
    public long Account { get; set; }

    [Column("order")]
    public long Order { get; set; }

    [Column("volume")]
    [Precision(10, 5)]
    public decimal Volume { get; set; }

    [Column("time_opened")]
    public DateTime TimeOpened { get; set; }

    [Column("price_opened")]
    [Precision(10, 5)]
    public decimal PriceOpened { get; set; }

    [Column("SL")]
    [Precision(10, 5)]
    public decimal? Sl { get; set; }

    [Column("TP")]
    [Precision(10, 5)]
    public decimal? Tp { get; set; }

    [Column("swap")]
    [Precision(10, 5)]
    public decimal Swap { get; set; }

    [Column("commissions")]
    [Precision(10, 5)]
    public decimal Commissions { get; set; }

    [Column("profit")]
    [Precision(10, 5)]
    public decimal Profit { get; set; }

    [Column("time_update")]
    public DateTime TimeUpdate { get; set; }

    [Column("symbol")]
    [StringLength(6)]
    public string? Symbol { get; set; }

    [ForeignKey("Account")]
    [InverseProperty("OpenedOrders")]
    public virtual Account AccountNavigation { get; set; } = null!;
}
