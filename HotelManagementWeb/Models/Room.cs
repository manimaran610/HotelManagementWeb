using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelManagementWeb.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Display(Name = "Image")]
        public string RoomImage { get; set; }

        [Display(Name = "Price")]
        public decimal RoomPrice { get; set; }

        [Display(Name = "Booking Status")]
        public int BookingStatusId { get; set; }

        [Display(Name = "Type")]
        public int RoomTypeId { get; set; }

        [Display(Name = "Capacity")]
        public int RoomCapacity { get; set; }

        [Required]
        [StringLength(550)]
        [Display(Name = "Description")]
        public string RoomDescription { get; set; }
        public bool IsActive { get; set; }




        [NotMapped]
        public HttpPostedFileBase UploadedImage { get; set; }

        [NotMapped]
        public List<BookingStatus> BookStatusList { get; set; }

        [NotMapped]
        public List<RoomTypes> RoomTypeList { get; set; }

        [NotMapped]

        public string Status { get; set; }

        [NotMapped]

        public string Type { get; set; }



    }
}


