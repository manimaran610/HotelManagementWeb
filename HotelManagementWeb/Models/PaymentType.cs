namespace HotelManagementWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaymentType")]
    public partial class PaymentType
    {
        public int PaymentTypeId { get; set; }

        [Column("PaymentType")]
        [Required]
        [StringLength(50)]
        public string PaymentType1 { get; set; }
    }
}
