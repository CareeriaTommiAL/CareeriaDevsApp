using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareeriaDevsApp.Models
{
    public class UserLogin
    {
        [Display(Name ="Email ID")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Sähköpostiosoite vaaditaan")]
        public string EmailID { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Salasana vaaditaan")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Muista minut")]
        public bool RememberMe { get; set; }
    }
}