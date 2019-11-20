using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CareeriaDevsApp.Models
{
    public class YritysModel
    {
        public int yritys_Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Yrityksen nimi puuttuu")]
        public string yrityksenNimi { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Y-tunnus puuttuu")]
        [RegularExpression(@"^[0-9]{7}-[0-9]$", ErrorMessage = "Syötä kelvollinen Y-tunnus")]
        public string Y_tunnus { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Yrityksen lähiosoite puuttuu")]
        public string lahiosoite { get; set; }
        public Nullable<int> postitoimipaikka_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Login> Login { get; set; }
        public virtual Postitoimipaikka Postitoimipaikka { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PuhelinNumero> PuhelinNumero { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sahkoposti> Sahkoposti { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Viesti> Viesti { get; set; }
    }
}