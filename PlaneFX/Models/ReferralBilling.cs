using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("referral_billing")]
public partial class ReferralBilling
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("user")]
    public long User { get; set; }

    [Column("amount")]
    [Precision(10, 0)]
    public decimal Amount { get; set; }

    [Column("time")]
    public DateTime Time { get; set; }

    [ForeignKey("User")]
    [InverseProperty("ReferralBillings")]
    public virtual User UserNavigation { get; set; } = null!;
}
