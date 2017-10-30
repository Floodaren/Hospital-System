using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PatientController : Controller
    {
        HospitaldbEntities db = new HospitaldbEntities();
        private string soc;
        private string password;
        private int loginId;

        [Authorize]
        public ActionResult Index()
        {

            this.loginId = Convert.ToInt32(Session["loginId"]);
            PatientLogIn loggedInPatientLogin = db.PatientLogIn.Find(this.loginId);
            Patient loggedInPaitent = (from patients in db.Patient
                                       where patients.PatientLogInID == this.loginId
                                       select patients).FirstOrDefault();

            ViewBag.loginId = this.loginId;
            ViewBag.loggedInName = loggedInPaitent.Firstname + " " + loggedInPaitent.Lastname;
            ViewBag.firstname = loggedInPaitent.Firstname;
            ViewBag.lastname = loggedInPaitent.Lastname;
            soc = loggedInPaitent.Soc;
            ViewBag.phonenumber = loggedInPaitent.Phonenumber;
            ViewBag.zipcode = loggedInPaitent.Zipcode;

            ViewBag.email = loggedInPatientLogin.UserName;
            password = loggedInPatientLogin.Password;

            return View();
        }

        //Journal
        #region Journal
            [Authorize]
            public ActionResult PatientJournal()
            {
            int patientId = db.PatientLogIn.Find(Convert.ToInt32(Session["loginId"])).Patient.FirstOrDefault().Id;
            int journalId = (from journals in db.Journal
                                      where journals.Patient_id == patientId
                                      select journals.Id).FirstOrDefault();
            List<HospitalPost> journalPostList = (from journalPost in db.HospitalPost
                              where journalPost.Journal_id == journalId
                              select journalPost).ToList();
            List<HospitalPostASP> viewList = ConvertHospitalPostsFromDB(journalPostList);

            viewList.Reverse();
            return View(viewList);
            }
        #endregion


        //Settings
        #region Settings

        #region Firstname
        [Authorize]
        public ActionResult ChangeFirstName(int? userId, string value)
        {
            ViewBag.userid = userId;
            ViewBag.value = value;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeFirstName(Settings newValues)
        {
            int id = Convert.ToInt32(newValues.id);
            var query = db.Patient.Find(id);
            query.Firstname = newValues.value;
            db.SaveChanges();
            return RedirectToAction("Index", "Patient");
        }
        #endregion

        #region Lastname
        [Authorize]
        public ActionResult ChangeLastName(int? userId, string value)
        {
            ViewBag.userid = userId;
            ViewBag.value = value;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeLastName(Settings newValues)
        {
            int id = Convert.ToInt32(newValues.id);
            var query = db.Patient.Find(id);
            query.Lastname = newValues.value;
            db.SaveChanges();
            return RedirectToAction("Index", "Patient");
        }
        #endregion

        #region Email
        [Authorize]
        public ActionResult ChangeEmail(int? userId, string value)
        {
            ViewBag.userid = userId;
            ViewBag.value = value;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeEmail(Settings newValues)
        {
            int id = Convert.ToInt32(newValues.id);
            var query = db.Patient.Find(id);
            var query2 = db.PatientLogIn.Find(query.PatientLogInID);
            query2.UserName = newValues.value;
            db.SaveChanges();
            return RedirectToAction("Index", "Patient");
        }
        #endregion

        #region Phonenumber
            public ActionResult ChangePhoneNumber(int? userId, string value)
            {
                ViewBag.userid = userId;
                ViewBag.value = value;
                return View();
            }

            [HttpPost]
            public ActionResult ChangePhoneNumber(Settings newValues)
            {
                int id = Convert.ToInt32(newValues.id);
                var query = db.Patient.Find(id);
                query.Phonenumber = newValues.value;
                db.SaveChanges();
                return RedirectToAction("Index", "Patient");
            }
        #endregion

        #region Zipcode
        [Authorize]
        public ActionResult ChangeZipCode(int? userId, string value)
        {
            ViewBag.userid = userId;
            ViewBag.value = value;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangeZipCode(Settings newValues)
        {
            int id = Convert.ToInt32(newValues.id);
            var query = db.Patient.Find(id);
            query.Zipcode = newValues.value;
            db.SaveChanges();
            return RedirectToAction("Index", "Patient");
        }
        #endregion

        #region Password
        [Authorize]
        public ActionResult ChangePassword(int? userId)
        {
            ViewBag.userid = userId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(Settings newValues)
        {
            int id = Convert.ToInt32(newValues.id);
            var query = db.Patient.Find(id);
            var query2 = db.PatientLogIn.Find(query.PatientLogInID);
            query2.Password = newValues.value;
            db.SaveChanges();
            return RedirectToAction("Index", "Patient");
        }
        #endregion

        #endregion

        //Converter
        private List<HospitalPostASP> ConvertHospitalPostsFromDB(List<HospitalPost> inputList)
        {
            List<HospitalPostASP> returnList = new List<HospitalPostASP>();
            foreach (var hositalpost in inputList)
            {
                string doctorFirstname_ = db.Doctor.Find(hositalpost.Doctor_id).Firstname;
                string doctorLastname_ = db.Doctor.Find(hositalpost.Doctor_id).Lastname;
                HospitalPostASP convertedObject = new HospitalPostASP
                {
                    id = hositalpost.Id,
                    doctorId = hositalpost.Doctor_id,
                    post = hositalpost.Post,
                    date = hositalpost.Date,
                    doctorFirstname = doctorFirstname_,
                    doctorLastname = doctorLastname_
                };
                returnList.Add(convertedObject);
            }
            return returnList;
        }
    }
}