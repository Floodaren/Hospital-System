using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPhospital
{

    public sealed partial class ShowJournalPostsPage : Page
    {
        private int patientid;
        ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
        private ServiceReference1.PatientData selectedPatient = new ServiceReference1.PatientData();
        public ShowJournalPostsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            selectedPatient = (ServiceReference1.PatientData)e.Parameter;
            patientid = selectedPatient.IdValue;
            FillListView();
        }

        async private void FillListView()
        {
            List<ServiceReference1.HospitalPostlData> hospitalPostsList = new List<ServiceReference1.HospitalPostlData>();
            List<HospitalPostWithDoctor> hospitalPostAndDoctorList = new List<HospitalPostWithDoctor>();

            var returnedPosts = await client.GetHospitalPostForPatientAsync(patientid);
            hospitalPostsList = returnedPosts.ToList();

            foreach (var post in hospitalPostsList)
            {
                ServiceReference1.DoctorData returnedDoctor = await client.GetSpecificDoctorAsync(post.DoctorIdValue);
                HospitalPostWithDoctor newObject = new HospitalPostWithDoctor()
                {
                    date = post.DateTimeValue,
                    postData = post.PostValue,
                    doctorFirstName = returnedDoctor.FirstNameValue,
                    doctorLastname = returnedDoctor.LastNameValue
                };
                hospitalPostAndDoctorList.Add(newObject);
            }
            hospitalPostAndDoctorList.Reverse();
            HospitalPosts.ItemsSource = hospitalPostAndDoctorList;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowPatient),  selectedPatient);
        }
    }
}
