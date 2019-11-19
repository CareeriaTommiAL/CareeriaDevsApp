using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CareeriaDevsApp.Models
{
    public class LoginModel
    {
        public int login_Id { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sähköposti puuttuu")]
        [DataType(DataType.EmailAddress)]
        public string kayttajaNimi { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Salasana vaaditaan")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Salasanassa tulee olla vähintään 6 merkkiä")]
        public string salasana { get; set; }
        public Nullable<int> opiskelija_Id { get; set; }
        public Nullable<int> yritys_Id { get; set; }
        public Nullable<int> paaKayttaja_Id { get; set; }
        public System.Guid aktivointiKoodi { get; set; }
        public bool onkoEmailAktivoitu { get; set; }

        public virtual Opiskelija Opiskelija { get; set; }
        public virtual Yritys Yritys { get; set; }
        public virtual PaaKayttaja PaaKayttaja { get; set; }

    }
}
