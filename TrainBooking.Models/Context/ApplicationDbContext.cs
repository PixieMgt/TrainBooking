using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBooking.Models.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SectionTicket>().HasKey(sc => new { sc.SectionId, sc.TicketId });
            modelBuilder.Entity<Section>()
                        .HasOne(section => section.DepartureStation)
                        .WithMany(station => station.Departures)
                        .HasForeignKey(section => section.DepartureStationId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Sections_Stations_DepartureStationId");
            modelBuilder.Entity<Section>()
                        .HasOne(section => section.DestinationStation)
                        .WithMany(station => station.Destinations)
                        .HasForeignKey(section => section.DestinationStationId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Sections_Stations_DestinationStationId");
            /*modelBuilder.Entity<Station>()
                        .HasMany(station => station.Departures)
                        .WithOne()
                        .HasForeignKey(section => section.DepartureStation)
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Station>()
                        .HasMany(station => station.Destinations)
                        .WithOne()
                        .HasForeignKey(section => section.DestinationStation)
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.NoAction);*/
            //OnModelCreatingPartial(modelBuilder)
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<SectionTicket> SectionTickets { get; set; }

    }
}
