using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.Models;

public partial class PlaneFXContext : DbContext
{
    public PlaneFXContext()
    {
    }

    public PlaneFXContext(DbContextOptions<PlaneFXContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ClosedOrder> ClosedOrders { get; set; }

    public virtual DbSet<Command> Commands { get; set; }

    public virtual DbSet<MainBilling> MainBillings { get; set; }

    public virtual DbSet<OpenedOrder> OpenedOrders { get; set; }

    public virtual DbSet<ReferralBilling> ReferralBillings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionsBilling> SubscriptionsBillings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:PlaneFX");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Number).HasDefaultValueSql("'0'::bigint");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("account_fk1");
        });

        modelBuilder.Entity<ClosedOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("closed_orders_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.ClosedOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("closed_orders_fk1");
        });

        modelBuilder.Entity<Command>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("command_pk");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Commands)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("command_account_id_fk");
        });

        modelBuilder.Entity<MainBilling>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("main_billing_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.MainBillings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("main_billing_fk1");
        });

        modelBuilder.Entity<OpenedOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("opened_orders_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.OpenedOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("opened_orders_fk1");
        });

        modelBuilder.Entity<ReferralBilling>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("referral_billing_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.ReferralBillings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("referral_billing_fk1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subscriptions_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Price).HasDefaultValueSql("'3'::numeric");
        });

        modelBuilder.Entity<SubscriptionsBilling>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subscriptions_billing_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.UserSuscriptionsNavigation).WithMany(p => p.SubscriptionsBillings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("subscriptions_billing_fk3");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.ParentNavigation).WithMany(p => p.InverseParentNavigation).HasConstraintName("user_fk7");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_fk4");
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_subscriptions_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.SubscribeNavigation).WithMany(p => p.UserSubscriptions).HasConstraintName("user_subscriptions_fk2");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.UserSubscriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_subscriptions_fk1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
