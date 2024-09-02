using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("account")]
[Index("Number", Name = "account_account_number_key", IsUnique = true)]
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

    [InverseProperty("AccountNavigation")]
    public virtual ICollection<ClosedOrder> ClosedOrders { get; set; } = new List<ClosedOrder>();

    [InverseProperty("AccountNavigation")]
    public virtual ICollection<OpenedOrder> OpenedOrders { get; set; } = new List<OpenedOrder>();

    [ForeignKey("User")]
    [InverseProperty("Accounts")]
    public virtual User UserNavigation { get; set; } = null!;
}
