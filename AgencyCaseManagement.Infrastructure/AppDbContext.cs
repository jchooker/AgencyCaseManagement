using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgencyCaseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AgencyCaseManagement.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Case> Cases => Set<Case>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Meeting> Meetings => Set<Meeting>();
        public DbSet<Organization> Organizations => Set<Organization>();

        //automatically settings for non-seeded saves
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //only set if not already provided (by seeder)
                        if (entry.Entity.CreatedAt == default)
                            entry.Entity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); //<--set up identity tables

            foreach (var entity in builder.Model.GetEntityTypes())
            { 
                if (typeof(IBaseEntity).IsAssignableFrom(entity.ClrType))
                {
                    //use reflection to call generic helper with correct type
                    typeof(AppDbContext)
                        .GetMethod(nameof(ApplySoftDeleteFilter),
                            BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(entity.ClrType)
                        .Invoke(null, new object[] { builder });
                }
            }
        }
        private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
                where TEntity : class, IBaseEntity
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(e => !e.IsDeleted);
        }
    }


}
