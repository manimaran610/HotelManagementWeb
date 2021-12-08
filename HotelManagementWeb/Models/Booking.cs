namespace HotelManagementWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        
        public int BookingId { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [Column(TypeName = "date")]
        public DateTime BookingFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime BookingTo { get; set; }

        public int AssignRoomId { get; set; }

        public int NoOfMembers { get; set; }

        [Required]
        [StringLength(550)]
        public string CustomerAddress { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
