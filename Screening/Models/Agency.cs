using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Screening.Models
{
    public class Agency
    {
        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public bool IsVisible { get; set; }
    }
}