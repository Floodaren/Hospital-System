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
    public sealed partial class ShowPatient : Page
    {
        private int patientId;
        ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
        private ServiceReference1.PatientData selectedPatient = new ServiceReference1.PatientData();
        public ShowPatient()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (ServiceReference1.PatientData)e.Parameter;
            selectedPatient = parameters;
            patientId = parameters.IdValue;
            PatientFirstName.Text = parameters.FirstNameValue;
            PatientLastName.Text = parameters.LastNameValue;
            PatientSoc.Text = parameters.SocNrValue;
            PatientPhoneNr.Text = parameters.PhoneNreValue;
            PatientZipCode.Text = parameters.ZipCodeValue;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(JournalPage));
        }

        async private void SendPost_Click(object sender, RoutedEventArgs e)
        {
            if (PostInput.Text == "")
            {
                DisplayNoWifiDialog("OBS", "Du har inte fyllt i någon journalinformation");
            }
            else
            {
                string postedData = PostInput.Text;
                int doctorId = LoggedIn.doctorId;
                int patientId = this.patientId;
                bool successOrNot = false;
                successOrNot = await client.CreateHospitalPostAsync(doctorId, patientId, postedData);
                if (successOrNot == true)
                {
                    PostInput.Text = "";
                    DisplayNoWifiDialog("Lyckades", "Denna journalpost är nu skapad");
                }
                else
                {
                    DisplayNoWifiDialog("Misslyckades", "Försök igen");
                }
            }      
        }

        private async void DisplayNoWifiDialog(string titleText, string contentText)
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = titleText,
                Content = contentText,
                CloseButtonText = "OK"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }


        private void ShowJournals_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ShowJournalPostsPage), selectedPatient);
        }
    }
}
