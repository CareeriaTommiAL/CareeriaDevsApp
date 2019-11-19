using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CareeriaDevsApp.Models
{
    public class PostitoimipaikkaModel
    {
        public int postitoimipaikka_Id { get; set; }
        public string postinumero { get; set; }
        public string postitoimipaikka { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Opiskelija> Opiskelija { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yritys> Yritys { get; set; }
    }
}