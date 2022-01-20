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
        [Display(Name = "Room Number")]

        public int? RoomNumber { get; set; }

        [Display(Name = "Image")]

        public string RoomImage { get; set; }

        [Display(Name = "Price")]
        [Required]
        public decimal? RoomPrice { get; set; }

        [Display(Name = "Booking Status")]
        public int BookingStatusId { get; set; }

        [Display(Name = "Type")]
        public int RoomTypeId { get; set; }
        [Required]
        [Display(Name = "Capacity")]
        [RegularExpression("^[1-9]$|^[1-2]{1}[0-8]{1}$", ErrorMessage = "Range for Capacity is from 1 - 28")]
        public int? RoomCapacity { get; set; }

        [Required]
        [StringLength(550)]
        [Display(Name = "Description")]
        public string RoomDescription { get; set; }


      [NotMapped]
        public Byte[] ImageByte { get; set; }


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


        [NotMapped]

        public string ErrorMessage { get; set; }

        [NotMapped]
        public DateTime CheckIn { get; set; }

        [NotMapped]
        public DateTime CheckOut { get; set; }
    }
}


