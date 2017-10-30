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
namespace UWPhospital
{

    public sealed partial class LoggedIn : Page
    {
        public static int doctorId;
        private string doctorname;
        private ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
        public LoggedIn()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (int)e.Parameter;
            doctorId = parameters;
            getDoctorNameAndLastPosts();
        }


        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Journaler.IsSelected)
            {
                this.Frame.Navigate(typeof(JournalPage));
            }
            else if (Installningar.IsSelected)
            {
                this.Frame.Navigate(typeof(InstallningarPage));
            }
            else if (SignOut.IsSelected)
            {
                CheckSignOut("Logga ut", "Vill du verkligne logga ut?");
            }
        }

        private async void CheckSignOut(string titleText, string contentText)
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = titleText,
                Content = contentText,
                CloseButtonText = "Avbryt",
                PrimaryButtonText = "Ja"
            };
            ContentDialogResult result = await noWifiDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                doctorId = 0;
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                this.Frame.Navigate(typeof(LoggedIn), doctorId);
            }
        }

        private async void getDoctorNameAndLastPosts()
        {
            ServiceReference1.DoctorData doctor = await client.GetSpecificDoctorAsync(doctorId);
            List<ServiceReference1.HospitalPostlDataWithPatientName> postList = (await client.GetThreeLastHospitalPostsAsync(doctorId)).ToList();
            List<HospitalPostWithPatient> listToShow = new List<HospitalPostWithPatient>();
            doctorname = doctor.FirstNameValue + " " + doctor.LastNameValue;  
            displayDoctorName.Text = doctorname;

            foreach (var post in postList)
            {
                HospitalPostWithPatient patientToConvert = new HospitalPostWithPatient
                {
                    postData = post.PostValue,
                    date = post.DateTimeValue,
                    patientFirstName = post.patientFirstNameValue,
                    patientLastname = post.patientLastNameValue
                };
                listToShow.Add(patientToConvert);
            }
            HospitalPosts.ItemsSource = listToShow;
        }

    }
}
