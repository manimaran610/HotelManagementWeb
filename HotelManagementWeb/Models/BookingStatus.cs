using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelManagementWeb.Models
{
    public class BookingStatus
    {
        [Key]
        public int BookingStatusId { get; set; }
        public string Status { get; set; }
    }
}