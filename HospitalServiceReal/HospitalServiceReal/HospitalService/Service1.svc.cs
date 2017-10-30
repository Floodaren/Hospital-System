using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;

namespace HospitalService
{

    public class Service1 : IService1
    {
        HospitaldbEntities db = new HospitaldbEntities();

        #region Login
        public DoctorData CheckLogIn(string username, string password)
        {
            var query = (from doctor in db.DoctorLogIn
                        where
                        username == doctor.UserName && password == doctor.Password
                        select new DoctorData
                        {
                            IdValue = doctor.Doctor.FirstOrDefault().Id,
                            FirstNameValue = doctor.Doctor.FirstOrDefault().Firstname,
                            LastNameValue = doctor.Doctor.FirstOrDefault().Lastname,
                            SocNrValue = doctor.Doctor.FirstOrDefault().Soc,
                            PhoneNreValue = doctor.Doctor.FirstOrDefault().Phonenumber
                        }).FirstOrDefault();

            return query;       
        }
        #endregion

        #region Get methods
        public List<DoctorData> GetDoctors()
        {
            List<DoctorData> convertedDoctorList = new List<DoctorData>();
            foreach (var doctor in db.Doctor)
            {
                DoctorData newDoctor = convertDoctorDbToDoctor(doctor);
                convertedDoctorList.Add(newDoctor);
            }
            return convertedDoctorList;
        }

        public DoctorData GetSpecificDoctor(int? doctorId)
        {
            DoctorData specificDoctorObject = new DoctorData();
            var doctor = (from specificDoctor in db.Doctor
                         where specificDoctor.Id == doctorId
                         select specificDoctor).FirstOrDefault();

            specificDoctorObject = convertDoctorDbToDoctor(doctor); 
            return specificDoctorObject;
        }

        public List<PatientData> GetPatients()
        {
            List<PatientData> convertedPatientList = new List<PatientData>();
            foreach (var patient in db.Patient)
            {
                PatientData newPatient = convertPatientDbToPatient(patient);
                convertedPatientList.Add(newPatient);
            }
            return convertedPatientList;
        }

        public List<JournalData> GetJournals()
        {
            List<JournalData> convertedJournalList = new List<JournalData>();
            foreach (var journal in db.Journal)
            {
                JournalData newJournal = convertJournalDbToJournal(journal);
                convertedJournalList.Add(newJournal);
            }
            return convertedJournalList;
        }

        public List<HospitalPostlData> GetHospitalPosts()
        {
            List<HospitalPostlData> convertedHospitalPosts = new List<HospitalPostlData>();
            foreach (var hospitalPost in db.HospitalPost)
            {
                HospitalPostlData newHospitalPost = convertHospitalPostDbToHospitalPost(hospitalPost);
                convertedHospitalPosts.Add(newHospitalPost);
            }
            return convertedHospitalPosts;
        }

        public List<HospitalPostlData> GetHospitalPostForPatient(int patientId)
        {
            List<HospitalPostlData> convertedHospitalPosts = new List<HospitalPostlData>();
            var getPatientJournalId = (from patient in db.Patient
                                       where patient.Id == patientId
                                       select patient.Journal.FirstOrDefault().Id).FirstOrDefault();

            var getHospitalPosts = from posts in db.HospitalPost
                                   where posts.Journal_id == getPatientJournalId
                                   select posts;

            foreach (var hospitalPost in getHospitalPosts)
            {
                HospitalPostlData newHospitalPost = convertHospitalPostDbToHospitalPost(hospitalPost);
                convertedHospitalPosts.Add(newHospitalPost);
            }
            return convertedHospitalPosts;
        }

        public List<DoctorXPatientData> GetDoctorXPatients()
        {
            List<DoctorXPatientData> convertedDoctorXPatients = new List<DoctorXPatientData>();
            foreach (var doctorXPatient in db.DoctorXPatient)
            {
                DoctorXPatientData newDoctorXPatient = convertDoctorXPatientDbToDoctorXPatient(doctorXPatient);
                convertedDoctorXPatients.Add(newDoctorXPatient);
            }
            return convertedDoctorXPatients;
        }

        public List<HospitalPostlDataWithPatientName> GetThreeLastHospitalPosts(int doctorId)
        {
            List<HospitalPostlDataWithPatientName> threeLastPosts = new List<HospitalPostlDataWithPatientName>();
            List<HospitalPost> query = (from posts in db.HospitalPost
                        where posts.Doctor_id == doctorId
                        select posts).ToList();
            query.Reverse();

            if (query.Count == 0)
            {
                return threeLastPosts;
            }
            else if (query.Count == 1)
            { 
                string firstname = query.ElementAt(0).Journal.Patient.Firstname;
                string lastname = query.ElementAt(0).Journal.Patient.Lastname;
                HospitalPostlDataWithPatientName postToReturn = convertHospitalPostDbToHospitalPostWithPatientName(query[0], firstname, lastname);
                threeLastPosts.Add(postToReturn);
                return threeLastPosts;
            }
            else if (query.Count == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    string firstname = query.ElementAt(i).Journal.Patient.Firstname;
                    string lastname = query.ElementAt(0).Journal.Patient.Lastname;
                    HospitalPostlDataWithPatientName postToReturn = convertHospitalPostDbToHospitalPostWithPatientName(query[i], firstname, lastname);
                    threeLastPosts.Add(postToReturn);
                }
                return threeLastPosts;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    string firstname = query.ElementAt(i).Journal.Patient.Firstname;
                    string lastname = query.ElementAt(0).Journal.Patient.Lastname;
                    HospitalPostlDataWithPatientName postToReturn = convertHospitalPostDbToHospitalPostWithPatientName(query[i], firstname, lastname);
                    threeLastPosts.Add(postToReturn);
                }
                return threeLastPosts;
            }
        }
        #endregion

        #region Import methods
        public bool ImportNewDoctor(string firstname, string lastname, string soc, string phonenumber, string username, string password)
        {
            bool successOrNot = false;
            using (db)
            {
                DoctorLogIn newDoctorLogin = new DoctorLogIn()
                {
                    UserName = username,
                    Password = password
                };
                db.DoctorLogIn.Add(newDoctorLogin);
                db.SaveChanges();
                

                Doctor newDoctor = new Doctor()
                {
                    Firstname = firstname,
                    Lastname = lastname,
                    Soc = soc,
                    Phonenumber = phonenumber,
                    DoctorLogInID = newDoctorLogin.Id
                };
                db.Doctor.Add(newDoctor);
                db.SaveChanges();
                successOrNot = true;
            }
            return successOrNot;
        }

        public bool ImportNewPatient(int journalId, string soc)
        {
            bool successOrNot = false;
            using (db)
            {
                /*
                 * This might come handy in the future. For now we don't need this because the user has already registerd themself. The important part
                 * is to see if there are any previous journal data....
                PatientLogIn newPatientLogin = new PatientLogIn()
                {
                    UserName = username,
                    Password = password
                };
                db.PatientLogIn.Add(newPatientLogin);
                db.SaveChanges();


                Patient newPatient = new Patient()
                {
                    Firstname = firstname,
                    Lastname = lastname,
                    Soc = soc,
                    Phonenumber = phonenumber,
                    Zipcode = zipcode,
                    PatientLogInID = newPatientLogin.Id
                };
                db.Patient.Add(newPatient);
                db.SaveChanges();

                Journal newJournal = new Journal()
                {
                    Patient_id = newPatient.Id,
                    Doctor_id = 5,
                    Date = patientJournal.Date
                };
                db.Journal.Add(newJournal);
                db.SaveChanges();
                */


                List<ImportPatientData> hospitalPostList = new List<ImportPatientData>();
                XDocument doc = XDocument.Load(@"C:\DB\HospitalServiceReal\HospitalServiceReal\HospitalService\Journals.xml");

                var post = doc.Descendants("journal")
                .Where(x => (string)x.Attribute("soc") == soc)
                .Elements("hospitalpost")
                .Select(x => (string)x.Element("post"));

                var date = doc.Descendants("journal")
                .Where(x => (string)x.Attribute("soc") == soc)
                .Elements("hospitalpost")
                .Select(x => (string)x.Element("date"));

                doc.Descendants("journal").Where(x => (string)x.Attribute("soc") == soc).Elements("hospitalpost").Remove();
                Debug.WriteLine("Detta skedde");

                foreach (var posts in post)
                {
                    ImportPatientData newHosptialPost = new ImportPatientData
                    {
                        postValue = posts
                    };
                    hospitalPostList.Add(newHosptialPost);
                }

                int i = 0;
                foreach (var dates in date)
                {
                    hospitalPostList[i].dateValue = Convert.ToDateTime(dates);
                    i++;
                }

                foreach (var importedData in hospitalPostList)
                {
                    HospitalPost newPost = new HospitalPost()
                    {
                        Doctor_id = 5,
                        Journal_id = journalId,
                        Post = importedData.postValue,
                        Date = importedData.dateValue
                    };
                    db.HospitalPost.Add(newPost);
                    db.SaveChanges();
                }
                
                successOrNot = true;
            }
            return successOrNot;
        }
        #endregion

        #region Create methods
        public bool CreateNewPatient(string userEmail, string password, string firstname, string lastname, string soc, string phonenr, string zipcode)
        {
            bool successOrNot = false;
            bool patientExist = false;

            foreach (var patient in db.Patient)
            {
                if (soc == patient.Soc)
                {
                    patientExist = true;
                }
            }

            if (patientExist == false)
                {
                PatientLogIn newLogin = new PatientLogIn()
                {
                    UserName = userEmail,
                    Password = password
                };
                db.PatientLogIn.Add(newLogin);
                db.SaveChanges();

                Patient newPatient = new Patient()
                {
                    Firstname = firstname,
                    Lastname = lastname,
                    Soc = soc,
                    Phonenumber = phonenr,
                    Zipcode = zipcode,
                    PatientLogInID = newLogin.Id
                };
                db.Patient.Add(newPatient);
                db.SaveChanges();

                Journal patientJournal = new Journal()
                {
                    Patient_id = newPatient.Id,
                    Doctor_id = 5,
                    Date = DateTime.Now,

                };
                db.Journal.Add(patientJournal);
                db.SaveChanges();
                successOrNot = true;
            }
            return successOrNot;
        }

        public bool CreateHospitalPost(int doctorId, int patientId, string post)
                {

                    var getPatient = from patient in db.Patient
                                     where patientId == patient.Id
                                     select patient;

                    HospitalPost newPost = new HospitalPost()
                    {
                        Doctor_id = doctorId,
                        Journal_id = getPatient.FirstOrDefault().Journal.FirstOrDefault().Id,
                        Post = post,
                        Date = DateTime.Now
                    };

                    db.HospitalPost.Add(newPost);
                    db.SaveChanges();
                    return true;
                }
        #endregion

        #region Update methods
        public string UpdateDoctorInformation(int doctorId, string newEmail, string newPassword)
        {
            string returnMessage = ""; 
            var selectDoctor = from doctor in db.Doctor
                        where doctorId == doctor.Id
                        select doctor;

            var getLoginDoctor = from loginDoctor in db.DoctorLogIn
                              where selectDoctor.FirstOrDefault().DoctorLogInID == loginDoctor.Id
                              select loginDoctor;

            if (newEmail == "" && newPassword == "")
            {
                
                returnMessage = "Du har inte gjort några ändringar.";
            }
            else if (newEmail == "" && newPassword != "")
            {
                getLoginDoctor.FirstOrDefault().Password = newPassword;
                db.SaveChanges();                            
                returnMessage = "Ditt lösenord är nu ändrat";
            }
            else if (newEmail != "" && newPassword == "")
            {
                getLoginDoctor.FirstOrDefault().UserName = newEmail;
                db.SaveChanges();
                returnMessage = "Din mail är nu ändrad.";
            }
            else if (newEmail != "" && newPassword != "")
            {
                getLoginDoctor.FirstOrDefault().UserName = newEmail;
                getLoginDoctor.FirstOrDefault().Password = newPassword;
                db.SaveChanges();
                returnMessage = "Din mail och lösenord är nu ändrat";
            }
            return returnMessage;
        }
        #endregion

        #region Convert Methods
        //Converters from database to service.
        public DoctorData convertDoctorDbToDoctor(Doctor dbDoctor)
        {
            DoctorData convertedDoctor = new DoctorData()
            {
                IdValue = dbDoctor.Id,
                FirstNameValue = dbDoctor.Firstname,
                LastNameValue = dbDoctor.Lastname,
                SocNrValue = dbDoctor.Soc,
                PhoneNreValue = dbDoctor.Phonenumber
            };
            return convertedDoctor;
        }

        public PatientData convertPatientDbToPatient(Patient dbPatient)
        {
            PatientData convertedPatient = new PatientData()
            {
                IdValue = dbPatient.Id,
                FirstNameValue = dbPatient.Firstname,
                LastNameValue = dbPatient.Lastname,
                SocNrValue = dbPatient.Soc,
                PhoneNreValue = dbPatient.Phonenumber,
                ZipCodeValue = dbPatient.Zipcode
            };
            return convertedPatient;
        }

        public JournalData convertJournalDbToJournal(Journal dbJournal)
        {
            JournalData convertedJournal = new JournalData()
            {
                IdValue = dbJournal.Id,
                PatientIdValue = dbJournal.Patient_id,
                DoctorIdValue = dbJournal.Doctor_id,
                DateTimeValue = dbJournal.Date
            };
            return convertedJournal;
        }

        public HospitalPostlData convertHospitalPostDbToHospitalPost(HospitalPost dbHospitalPost)
        {
            HospitalPostlData convertedHospitalPost = new HospitalPostlData()
            {
                IdValue = dbHospitalPost.Id,
                DoctorIdValue = dbHospitalPost.Doctor_id,
                JournalIdValue = dbHospitalPost.Journal_id,
                PostValue = dbHospitalPost.Post,
                DateTimeValue = dbHospitalPost.Date

            };
            return convertedHospitalPost;
        }

        public HospitalPostlDataWithPatientName convertHospitalPostDbToHospitalPostWithPatientName(HospitalPost dbHospitalPost, string firstname, string lastname)
        {
            HospitalPostlDataWithPatientName convertedHospitalPost = new HospitalPostlDataWithPatientName()
            {
                IdValue = dbHospitalPost.Id,
                DoctorIdValue = dbHospitalPost.Doctor_id,
                JournalIdValue = dbHospitalPost.Journal_id,
                PostValue = dbHospitalPost.Post,
                DateTimeValue = dbHospitalPost.Date,
                patientFirstNameValue = firstname,
                patientLastNameValue = lastname

            };
            return convertedHospitalPost;
        }

        public DoctorXPatientData convertDoctorXPatientDbToDoctorXPatient(DoctorXPatient dbDoctorXPatient)
        {
            DoctorXPatientData convertedDoctorXPatient = new DoctorXPatientData()
            {
                IdValue = dbDoctorXPatient.Id,
                DoctorIdValue = dbDoctorXPatient.Doctor_id,
                PatientIdValue = dbDoctorXPatient.Patient_id,
                HospitalValue = dbDoctorXPatient.Hosptial
            };
            return convertedDoctorXPatient;
        }
        #endregion
    }

}
