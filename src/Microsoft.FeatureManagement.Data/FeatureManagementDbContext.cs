using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Data.Models;

namespace Microsoft.FeatureManagement.Data
{
    /// <inheritdoc cref="IFeatureManagementDbContext" />
    public class FeatureManagementDbContext : DbContext, IFeatureManagementDbContext
    {
        public FeatureManagementDbContext(DbContextOptions<FeatureManagementDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Audience> Audiences { get; set; }
        public virtual DbSet<BrowserRestriction> BrowserRestrictions { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<GroupRollout> GroupRollouts { get; set; }
        public virtual DbSet<TimeWindow> TimeWindows { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SupportedBrowser> SupportedBrowsers { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Feature>(builder =>
            {
                builder.Property(feature => feature.Id)
                    .ValueGeneratedOnAdd();
                
                builder.HasIndex(feature => feature.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<BrowserRestriction>(builder =>
            {
                builder.Property(restriction => restriction.SupportedBrowserId)
                    .HasConversion<int>();
                
                builder.HasIndex(restriction => new {restriction.FeatureId, restriction.SupportedBrowserId})
                    .IsUnique();
            });

            modelBuilder.Entity<Audience>(entity => entity.Property(a => a.Id)
                .ValueGeneratedOnAdd());
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(a => a.Id).ValueGeneratedOnAdd();
                entity.HasIndex(a => a.UserName).IsUnique();
            });
            modelBuilder.Entity<GroupRollout>(entity =>
            {
                entity.Property(a => a.Id).ValueGeneratedOnAdd();
                entity.HasIndex(a => a.Name).IsUnique();
            });

            modelBuilder.Entity<SupportedBrowser>(builder =>
            {
                builder.Property(browser => browser.SupportedBrowserId).HasConversion<int>();
                builder.HasIndex(browser => new {browser.SupportedBrowserId, browser.Name}).IsUnique();
                builder.HasData(Enum.GetValues(typeof(SupportedBrowserId))
                    .Cast<SupportedBrowserId>()
                    .Select(sb => new SupportedBrowser
                    {
                        SupportedBrowserId = sb,
                        Name = sb.ToString()
                    }));
            });
        }
    }
}