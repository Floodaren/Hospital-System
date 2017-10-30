using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class HospitalPostASP
    {
        public int id { get; set; }
        public int? doctorId { get; set; }
        public string post { get; set; }
        public DateTime? date { get; set; }
        public string doctorFirstname { get; set; }
        public string doctorLastname { get; set; }
    }
}