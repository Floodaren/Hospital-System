using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        HospitaldbEntities db = new HospitaldbEntities();
        
        public ActionResult Index()
        {
            Session["loginId"] = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Index(PatientCombinedPatientLogin patient)
        {
            var query = db.PatientLogIn.Where(a => a.UserName.Equals(patient.Datausername) && a.Password.Equals(patient.Datapasswords)).FirstOrDefault();
            if (query != null)
            {
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(query.UserName,false);
                Session["loginId"] = query.Id;
            }

            TempData["msg"] = "<script>alert('Felaktigt login, försök igen');</script>";
            return View(patient);
        }

        public ActionResult Registrera()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrera(PatientCombinedPatientLogin patient)
        {
            bool checkIfPatientExist = false;
            List<Patient> patientlist = db.Patient.ToList();
            foreach (var patientinList in patientlist)
            {
                if (patient.soc == patientinList.Soc)
                {
                    checkIfPatientExist = true;
                }
            }

            if (checkIfPatientExist == false)
            {
                PatientLogIn newPatientLogin = db.PatientLogIn.Add(ConvertPatientLoginModelToPatientLogin(patient));
                db.SaveChanges();
                Patient newPatient = db.Patient.Add(ConvertPatientToDBPatient(patient, newPatientLogin.Id));
                db.SaveChanges();
                Journal newJournal = new Journal
                {
                    Patient_id = newPatient.Id,
                    Doctor_id = 5,
                    Date = DateTime.Now
                };
                db.Journal.Add(newJournal);
                db.SaveChanges();
                ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
                bool successOrNot = client.ImportNewPatient(newJournal.Id, patient.soc);
                if (successOrNot == false)
                {
                    TempData["msg"] = "<script>alert('Något gick fel');</script>";
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('Detta personnummer finns redan registrerat');</script>";
            }
            return RedirectToAction("Index", "Home");
        }

        private PatientLogIn ConvertPatientLoginModelToPatientLogin(PatientCombinedPatientLogin patientToConvert)
        {
            PatientLogIn convertedPatient = new PatientLogIn
            {
                UserName = patientToConvert.Datausername,
                Password = patientToConvert.Datapasswords
            };
            return convertedPatient;
        }

        private Patient ConvertPatientToDBPatient(PatientCombinedPatientLogin patientToConvert, int patientloginid)
        {
            Patient convertedPatient = new Patient
            {
                Firstname = patientToConvert.firstname,
                Lastname = patientToConvert.lastname,
                Soc = patientToConvert.soc,
                Phonenumber = patientToConvert.phonenumber,
                Zipcode = patientToConvert.zipcode,
                PatientLogInID = patientloginid
            };
            return convertedPatient;
        }
    }
}