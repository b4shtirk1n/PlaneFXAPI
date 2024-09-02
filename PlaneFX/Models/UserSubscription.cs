using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("user_subscriptions")]
public partial class UserSubscription
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("user")]
    public long User { get; set; }

    [Column("subscribe")]
    public long? Subscribe { get; set; }

    [Column("date")]
    public DateOnly? Date { get; set; }

    [ForeignKey("Subscribe")]
    [InverseProperty("UserSubscriptions")]
    public virtual Subscription? SubscribeNavigation { get; set; }

    [InverseProperty("UserSuscriptionsNavigation")]
    public virtual ICollection<SubscriptionsBilling> SubscriptionsBillings { get; set; } = new List<SubscriptionsBilling>();

    [ForeignKey("User")]
    [InverseProperty("UserSubscriptions")]
    public virtual User UserNavigation { get; set; } = null!;
}
