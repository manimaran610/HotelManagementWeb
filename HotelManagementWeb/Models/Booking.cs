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
        [Key]
        public int BookingId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
      
        public DateTime BookingFrom { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime BookingTo { get; set; }

        public int AssignRoomId { get; set; }

        [Display(Name ="No. of persons")]
        [Required]
        public int? NoOfMembers { get; set; }

        public string CustomerEmail { get; set; }


        public decimal? TotalAmount { get; set; }

        [NotMapped]
        public decimal? RoomPrice { get; set; }
        [NotMapped]
        public decimal NumberOfDays { get; set; }
        [NotMapped]
        public decimal? ValueAddedTax { get; set; }
        [NotMapped]
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[6-9][0-9]{9}", ErrorMessage = "Mobile Number is Invalid")]
        public decimal? Phone { get; set; }

        [NotMapped]
        public int? MaxCapacity { get; set; }

        [NotMapped]
        public int? RoomNumber { get; set; }


    }
}
