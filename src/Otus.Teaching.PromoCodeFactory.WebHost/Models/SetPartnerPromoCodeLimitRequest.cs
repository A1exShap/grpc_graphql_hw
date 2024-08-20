using System;
using System.ComponentModel.DataAnnotations.Schema;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class SetPartnerPromoCodeLimitRequest
    {
        [Column(TypeName = "timestamp with time zone")]
        public DateTime EndDate { get; set; }
        public int Limit { get; set; }
    }
}