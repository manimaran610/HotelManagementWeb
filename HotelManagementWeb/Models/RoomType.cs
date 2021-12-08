namespace HotelManagementWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoomTypes
    {
        [Key]
        public int RoomTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; }
    }
}
