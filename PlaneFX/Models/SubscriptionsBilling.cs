using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("subscriptions_billing")]
public partial class SubscriptionsBilling
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("amount")]
    [Precision(10, 0)]
    public decimal Amount { get; set; }

    [Column("time")]
    public DateTime Time { get; set; }

    [Column("user_suscriptions")]
    public long UserSuscriptions { get; set; }

    [ForeignKey("UserSuscriptions")]
    [InverseProperty("SubscriptionsBillings")]
    public virtual UserSubscription UserSuscriptionsNavigation { get; set; } = null!;
}
