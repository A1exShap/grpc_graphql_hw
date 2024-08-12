using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PartnerPromoCodeLimit
    {
        public Guid Id { get; set; }

        public Guid PartnerId { get; set; }

        public virtual Partner Partner { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreateDate { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? CancelDate { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime EndDate { get; set; }

        public int Limit { get; set; }
    }
}