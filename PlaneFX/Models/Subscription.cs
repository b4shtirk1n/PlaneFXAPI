using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("subscriptions")]
public partial class Subscription
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("accounts_count")]
    public long AccountsCount { get; set; }

    [Column("price")]
    [Precision(10, 0)]
    public decimal Price { get; set; }

    [InverseProperty("SubscribeNavigation")]
    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}
