//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CareeriaDevsApp.Databases
{
    using System;
    using System.Collections.Generic;
    
    public partial class Postitoimipaikka
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Postitoimipaikka()
        {
            this.Opiskelija = new HashSet<Opiskelija>();
            this.Yritys = new HashSet<Yritys>();
        }
    
        public int postitoimipaikka_Id { get; set; }
        public string postinumero { get; set; }
        public string postitoimipaikka1 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Opiskelija> Opiskelija { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yritys> Yritys { get; set; }
    }
}
