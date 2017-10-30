using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPhospital
{
    class HospitalPostWithPatient
    {

        public string postData { get; set; }
        public DateTime? date { get; set; }
        public string patientFirstName { get; set; }
        public string patientLastname { get; set; }
    }
}
