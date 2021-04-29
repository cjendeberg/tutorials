using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.ValueObjects;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Setings;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.EF
{
    public class DrawContext : DbContext
    {
        private readonly SqlSettings _settings;
        public DbSet<Core.Domain.Entities.Drawing> Drawing { get; set; }
        public DbSet<Core.Domain.Entities.Schedule> Schedule { get; set; }

        public DrawContext(DbContextOptions<DrawContext> options, SqlSettings settings) : base(options)
        {
            _settings = settings;
            if (_settings.CommandTimeout != null)
            {
                this.Database.SetCommandTimeout(_settings.CommandTimeout);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_settings.InMemory)
            {
                optionsBuilder.UseLazyLoadingProxies().UseInMemoryDatabase(_settings.ConnectionString);
                return;
            }

            optionsBuilder
                 .UseLazyLoadingProxies()
                .UseSqlServer(_settings.ConnectionString,
                              sqlServerOptionsAction: sqlOptions =>
                              {
                                  //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                  sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                              }
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var numberBuilder = modelBuilder.Entity<Number>();
            numberBuilder.Property<int>("Id");
            numberBuilder.HasKey("Id");
            numberBuilder.ToTable("DrawingNumbers");

            var drawingBuilder = modelBuilder.Entity<Core.Domain.Entities.Drawing>();
            drawingBuilder.HasKey(x => x.Id);
            drawingBuilder.HasMany(x => x.Numbers);


            var dayBuilder = modelBuilder.Entity<Day>();
            dayBuilder.Property<int>("Id");
            dayBuilder.HasKey("Id");
            dayBuilder.ToTable("ScheduleDays");

            var scheduleBuilder = modelBuilder.Entity<Core.Domain.Entities.Schedule>();
            scheduleBuilder.HasKey(x => x.Id);
            scheduleBuilder.HasMany(p => p.Days).WithOne().HasForeignKey("ScheduleId").OnDelete(DeleteBehavior.Cascade);
            scheduleBuilder.HasMany(p => p.Drawings);

        }
    }
}
