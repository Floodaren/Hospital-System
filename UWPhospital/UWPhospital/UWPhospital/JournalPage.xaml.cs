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
    public sealed partial class JournalPage : Page
    {
        private ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
        private List<ServiceReference1.PatientData> patientList = new List<ServiceReference1.PatientData>();
        private List<ServiceReference1.PatientData> emptyList = new List<ServiceReference1.PatientData>();
        private bool foundPatient = false;
        public JournalPage()
        {
            this.InitializeComponent();

        }


        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Home.IsSelected)
            {
                this.Frame.Navigate(typeof(LoggedIn), LoggedIn.doctorId);
            }
            else if (Installningar.IsSelected)
            {
                this.Frame.Navigate(typeof(InstallningarPage));
            }

        }

        
        async private void SearchlistBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {         
            if (foundPatient == false)
            {
                foundPatient = true;
                var x = await client.GetPatientsAsync();
                patientList = x.ToList();
            }
            if (patientList != null)
            {
                SearchlistBox.ItemsSource = patientList.Where(a => a.SocNrValue.Contains(SearchlistBox.Text.ToString()));
            }
            if (string.IsNullOrEmpty(SearchlistBox.Text))
            {
                SearchlistBox.ItemsSource = emptyList;
            }

        }

        private void SearchlistBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ServiceReference1.PatientData choosenPatient = (ServiceReference1.PatientData)args.SelectedItem;
            this.Frame.Navigate(typeof(ShowPatient), choosenPatient);
        }
    }
}
