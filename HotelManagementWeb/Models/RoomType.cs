namespace HotelManagementWeb.Models
{
  
    using System.ComponentModel.DataAnnotations;
  

    public partial class RoomTypes
    {
        [Key]
        public int RoomTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; }
    }
}
