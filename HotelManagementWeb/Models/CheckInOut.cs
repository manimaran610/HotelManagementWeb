using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelManagementWeb.Models
{
    public class CheckInOut
    {
       
        [DataType(DataType.Date)]

        public DateTime CheckIn { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }
    }
}