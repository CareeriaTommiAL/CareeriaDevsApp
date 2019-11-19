using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CareeriaDevsApp.Models
{
    public class PuhelinNumeroModel
    {
        public int puhelinNumero_Id { get; set; }

        [Display(Name = "Puhelinnro")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Puhelinnumero puuttuu")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", ErrorMessage = "Syötä kelvollinen puhelinnumero")]
        public string numero { get; set; }
        public Nullable<int> puhelinTyyppi_Id { get; set; }
        public Nullable<int> opiskelija_Id { get; set; }
        public Nullable<int> yritys_Id { get; set; }

        public virtual Opiskelija Opiskelija { get; set; }
        public virtual PuhelinTyyppi PuhelinTyyppi { get; set; }
        public virtual Yritys Yritys { get; set; }
    }
}