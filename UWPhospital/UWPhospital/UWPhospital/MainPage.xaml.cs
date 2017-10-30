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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPhospital
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
        public MainPage()
        {
            this.InitializeComponent();
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userName = MailInput.Text.ToString();
            string password = PasswordInput.Password.ToString();
            ServiceReference1.DoctorData returDoktor = await client.CheckLogInAsync(userName, password);
            if (returDoktor == null)
            {
                DisplayNoWifiDialog();
            }
            else
            {

                this.Frame.Navigate(typeof(LoggedIn), returDoktor.IdValue);
            }
            
        }

        private async void DisplayNoWifiDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "OBS",
                Content = "Felaktig login, försök igen",
                CloseButtonText = "OK"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
        }

    }
}
