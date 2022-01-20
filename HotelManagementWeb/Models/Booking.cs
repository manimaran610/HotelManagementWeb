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
        [MinLength(4, ErrorMessage = "Name is too short")]
        [MaxLength(16, ErrorMessage = "Name is too long")]
        [RegularExpression("^(?!.*([ ])\\1)(?!.*([A-Za-z])\\2{2})\\w[a-zA-Z ]*$", ErrorMessage = "Name should not contain special characters or numerical digits")]
        public string CustomerName { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
      
        [Display(Name ="CheckIn")]
        public DateTime BookingFrom { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "CheckOut")]
        public DateTime BookingTo { get; set; }

        public int? AssignRoomId { get; set; }

        [Display(Name ="No. of persons")]
        [Required]
        public int? NoOfMembers { get; set; }

        public string CustomerEmail { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[6-9][0-9]{9}", ErrorMessage = "Should start with 6-9 followed by 9 numeric digits")]
        public string MobileNo { get; set; }

        public decimal? TotalAmount { get; set; }




        [NotMapped]
        public decimal? RoomPrice { get; set; }
        [NotMapped]
        public decimal NumberOfDays { get; set; }
        [NotMapped]
        public decimal? ValueAddedTax { get; set; }
        
       
        [NotMapped]
        public int? MaxCapacity { get; set; }

        [NotMapped]
        public int? RoomNumber { get; set; }

        [NotMapped]
        public string QRCode { get; set; }

    }

}
