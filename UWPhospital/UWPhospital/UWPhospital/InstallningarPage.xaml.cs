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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPhospital
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstallningarPage : Page
    {
        private ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
        public InstallningarPage()
        {
            this.InitializeComponent();
        }


        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Journaler.IsSelected)
            {
                this.Frame.Navigate(typeof(JournalPage));
            }
            else if (Home.IsSelected)
            {
                this.Frame.Navigate(typeof(LoggedIn), LoggedIn.doctorId);
            }

        }


        async private void UpdateInformation_Click(object sender, RoutedEventArgs e)
        {
            string newEmail = NewMailInput.Text;
            string newPassword = NewPasswordInput.Password.ToString();
            string response =  await client.UpdateDoctorInformationAsync(LoggedIn.doctorId,newEmail, newPassword);
            DisplayNoWifiDialog("Uppdaterat", response);
            NewMailInput.Text = "";
            NewPasswordInput.Password = "";
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
    }
}
