using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using TrainBooking.Models.Entities;

namespace TrainBooking.Models.Data
{
    public partial class TrainBookingDbContext : DbContext
    {
        public TrainBookingDbContext()
        {
        }

        public TrainBookingDbContext(DbContextOptions<TrainBookingDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Section> Sections { get; set; } = null!;
        public virtual DbSet<Station> Stations { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<Train> Trains { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                      .AddJsonFile("appsettings.json")
                      .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DepartureTime).HasColumnType("datetime");

                entity.Property(e => e.DestinationTime).HasColumnType("datetime");

                entity.HasOne(d => d.DepartureStation)
                    .WithMany(p => p.SectionDepartureStations)
                    .HasForeignKey(d => d.DepartureStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sections_Stations");

                entity.HasOne(d => d.DestinationStation)
                    .WithMany(p => p.SectionDestinationStations)
                    .HasForeignKey(d => d.DestinationStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sections_Stations1");

                entity.HasMany(d => d.Tickets)
                    .WithMany(p => p.Sections)
                    .UsingEntity<Dictionary<string, object>>(
                        "SectionTicket",
                        l => l.HasOne<Ticket>().WithMany().HasForeignKey("TicketId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SectionTickets_Tickets"),
                        r => r.HasOne<Section>().WithMany().HasForeignKey("SectionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SectionTickets_Sections"),
                        j =>
                        {
                            j.HasKey("SectionId", "TicketId");

                            j.ToTable("SectionTickets");
                        });
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tickets_Bookings");
            });

            modelBuilder.Entity<Train>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
