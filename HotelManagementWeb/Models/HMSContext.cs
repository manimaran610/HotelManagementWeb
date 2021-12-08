using HotelManagementWeb.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace HotelManagementSystem.Models
{
    public partial class HMSContext : DbContext
    {
        public HMSContext()
            : base("name=HMSContext")
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomTypes> RoomTypes { get; set; }
        public virtual DbSet<BookingStatus> BookingStatus { get; set; }


        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentType>()
                .Property(e => e.PaymentType1)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.RoomNumber)
                .IsUnicode(false);
        }
    }
}
