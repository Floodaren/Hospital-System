using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace HospitalService
{

    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        List<DoctorData> GetDoctors();

        [OperationContract]
        DoctorData GetSpecificDoctor(int? doctorId);

        [OperationContract]
        List<PatientData> GetPatients();

        [OperationContract]
        List<JournalData> GetJournals();

        [OperationContract]
        List<HospitalPostlData> GetHospitalPosts();

        [OperationContract]
        List<DoctorXPatientData> GetDoctorXPatients();

        [OperationContract]
        DoctorData CheckLogIn(string username, string password);

        [OperationContract]
        bool ImportNewDoctor(string firstname, string lastname, string soc, string phonenumber, string username, string password);

        [OperationContract]
        bool ImportNewPatient(int journalId, string soc);

        [OperationContract]
        bool CreateNewPatient(string userEmail, string password, string firstname, string lastname, string soc, string phonenr, string zipcode);

        [OperationContract]
        string UpdateDoctorInformation(int doctorId, string newEmail, string newPassword);

        [OperationContract]
        bool CreateHospitalPost(int doctorId, int patientId, string post);

        [OperationContract]
        List<HospitalPostlData> GetHospitalPostForPatient(int patientId);

        [OperationContract]
        List<HospitalPostlDataWithPatientName> GetThreeLastHospitalPosts(int doctorId);
    }


    [DataContract]
    public class DoctorData
    {
        int id;
        string firstName;
        string lastName;
        string socNr;
        string phoneNr;


        [DataMember]
        public int IdValue
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string FirstNameValue
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [DataMember]
        public string LastNameValue
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [DataMember]
        public string SocNrValue
        {
            get { return socNr; }
            set { socNr = value; }
        }

        [DataMember]
        public string PhoneNreValue
        {
            get { return phoneNr; }
            set { phoneNr = value; }
        }
    }

    [DataContract]
    public class PatientData
    {
        int id;
        string firstName;
        string lastName;
        string socNr;
        string phoneNr;
        string zipCodeNr;


        [DataMember]
        public int IdValue
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string FirstNameValue
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [DataMember]
        public string LastNameValue
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [DataMember]
        public string SocNrValue
        {
            get { return socNr; }
            set { socNr = value; }
        }

        [DataMember]
        public string PhoneNreValue
        {
            get { return phoneNr; }
            set { phoneNr = value; }
        }

        [DataMember]
        public string ZipCodeValue
        {
            get { return zipCodeNr; }
            set { zipCodeNr = value; }
        }
    }

    [DataContract]
    public class JournalData
    {
        int id;
        int patientId;
        int doctorId;
        DateTime date;


        [DataMember]
        public int IdValue
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public int PatientIdValue
        {
            get { return patientId; }
            set { patientId = value; }
        }

        [DataMember]
        public int DoctorIdValue
        {
            get { return doctorId; }
            set { doctorId = value; }
        }

        [DataMember]
        public DateTime DateTimeValue
        {
            get { return date; }
            set { date = value; }
        }
    }

    [DataContract]
    public class HospitalPostlData
    {
        int id;
        int? journalId;
        int? doctorId;
        string post;
        DateTime? date;


        [DataMember]
        public int IdValue
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public int? JournalIdValue
        {
            get { return journalId; }
            set { journalId = value; }
        }

        [DataMember]
        public int? DoctorIdValue
        {
            get { return doctorId; }
            set { doctorId = value; }
        }

        [DataMember]
        public string PostValue
        {
            get { return post; }
            set { post = value; }
        }

        [DataMember]
        public DateTime? DateTimeValue
        {
            get { return date; }
            set { date = value; }
        }
    }

    [DataContract]
    public class HospitalPostlDataWithPatientName
    {
        int id;
        int? journalId;
        int? doctorId;
        string post;
        string patientFirstName;
        string patientLastName;
        DateTime? date;


        [DataMember]
        public int IdValue
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public int? JournalIdValue
        {
            get { return journalId; }
            set { journalId = value; }
        }

        [DataMember]
        public int? DoctorIdValue
        {
            get { return doctorId; }
            set { doctorId = value; }
        }

        [DataMember]
        public string PostValue
        {
            get { return post; }
            set { post = value; }
        }

        [DataMember]
        public DateTime? DateTimeValue
        {
            get { return date; }
            set { date = value; }
        }

        [DataMember]
        public string patientFirstNameValue
        {
            get { return patientFirstName; }
            set { patientFirstName = value; }
        }

        [DataMember]
        public string patientLastNameValue
        {
            get { return patientLastName; }
            set { patientLastName = value; }
        }
    }

    [DataContract]
    public class DoctorXPatientData
    {
        int id;
        int patientId;
        int doctorId;
        string hospital;


        [DataMember]
        public int IdValue
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public int PatientIdValue
        {
            get { return patientId; }
            set { patientId = value; }
        }

        [DataMember]
        public int DoctorIdValue
        {
            get { return doctorId; }
            set { doctorId = value; }
        }

        [DataMember]
        public string HospitalValue
        {
            get { return hospital; }
            set { hospital = value; }
        }
    }

    [DataContract]
    public class ImportPatientData
    {
        string post;
        DateTime date;

        [DataMember]
        public string postValue
        {
            get { return post; }
            set { post = value; }
        }

        [DataMember]
        public DateTime dateValue
        {
            get { return date; }
            set { date = value; }
        }
    }
}
