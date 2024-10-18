using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

[Table("user")]
[Index("TgId", Name = "user_tg_id_uindex", IsUnique = true)]
[Index("Token", Name = "user_token_uindex", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("username")]
    [StringLength(255)]
    public string Username { get; set; } = null!;

    [Column("tg_id")]
    public long TgId { get; set; }

    [Column("token")]
    public string Token { get; set; } = null!;

    [Column("role")]
    public long Role { get; set; }

    [Column("main_balance")]
    [Precision(10, 2)]
    public decimal MainBalance { get; set; }

    [Column("referral_balance")]
    [Precision(10, 2)]
    public decimal ReferralBalance { get; set; }

    [Column("parent")]
    public long? Parent { get; set; }

    [Column("timezone")]
    public int Timezone { get; set; }

    [InverseProperty("UserNavigation")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [InverseProperty("ParentNavigation")]
    public virtual ICollection<User> InverseParentNavigation { get; set; } = new List<User>();

    [InverseProperty("UserNavigation")]
    public virtual ICollection<MainBilling> MainBillings { get; set; } = new List<MainBilling>();

    [ForeignKey("Parent")]
    [InverseProperty("InverseParentNavigation")]
    public virtual User? ParentNavigation { get; set; }

    [InverseProperty("UserNavigation")]
    public virtual ICollection<ReferralBilling> ReferralBillings { get; set; } = new List<ReferralBilling>();

    [ForeignKey("Role")]
    [InverseProperty("Users")]
    public virtual Role RoleNavigation { get; set; } = null!;

    [InverseProperty("UserNavigation")]
    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}
