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
    using System.ComponentModel.DataAnnotations;

    public partial class Login
    {
        public int login_Id { get; set; }

        [Required(ErrorMessage = "Ole hyv� ja sy�t� k�ytt�j�nimi uudestaan.")]
        public string kayttajaNimi { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Ole hyv� ja sy�t� salasana uudestaan.")]
        public string salasana { get; set; }
        public Nullable<int> opiskelija_Id { get; set; }
        public Nullable<int> yritys_Id { get; set; }
        public Nullable<int> paaKayttaja_Id { get; set; }
    
        public virtual Opiskelija Opiskelija { get; set; }
        public virtual Yritys Yritys { get; set; }
        public virtual PaaKayttaja PaaKayttaja { get; set; }

        //ERROR -messagea varten manuaalisesti tehty
        public string LoginVirhe { get; set; }
    }
}
