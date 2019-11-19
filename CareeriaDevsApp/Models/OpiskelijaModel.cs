using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CareeriaDevsApp.Models
{
    public class OpiskelijaModel
    {
        public int opiskelija_Id { get; set; }

        [Display(Name = "Etunimi")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Etunimi puuttuu")]
        public string etunimi { get; set; }

        [Display(Name = "Sukunimi")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sukunimi puuttuu")]
        public string sukunimi { get; set; }
        public Nullable<int> postitoimipaikka_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Login> Login { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OmaSisalto> OmaSisalto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PuhelinNumero> PuhelinNumero { get; set; }
        public virtual Postitoimipaikka Postitoimipaikka { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Viesti> Viesti { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sahkoposti> Sahkoposti { get; set; }
    }
}
