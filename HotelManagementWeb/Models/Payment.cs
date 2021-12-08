namespace HotelManagementWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payment
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        public int PaymentTypeId { get; set; }

        public decimal PaymentAmount { get; set; }

        public bool IsActive { get; set; }
    }
}
