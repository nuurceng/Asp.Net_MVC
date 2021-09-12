namespace Mvcproje.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yorum")]
    public partial class Yorum
    {
        public int yorumid { get; set; }

        [StringLength(500)]
        public string icerik { get; set; }

        public int? uyeid { get; set; }

        public int? makaleid { get; set; }

        public DateTime? tarih { get; set; }

        public virtual Makale Makale { get; set; }

        public virtual Uye Uye { get; set; }
    }
}
