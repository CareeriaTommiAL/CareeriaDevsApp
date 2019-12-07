using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareeriaDevsApp.Models
{
    public class SahkopostiModel
    {
        public int sahkoposti_Id { get; set; }
        public string sahkopostiOsoite { get; set; }
        public Nullable<int> paaKayttaja_Id { get; set; }
        public Nullable<int> yritys_Id { get; set; }
        public Nullable<int> opiskelija_Id { get; set; }

        public virtual Opiskelija Opiskelija { get; set; }
        public virtual PaaKayttaja PaaKayttaja { get; set; }
        public virtual Yritys Yritys { get; set; }
    }
}