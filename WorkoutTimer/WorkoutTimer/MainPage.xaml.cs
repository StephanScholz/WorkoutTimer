using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WorkoutTimer
{
    public partial class MainPage : ContentPage
    {
        SettingsPage settingsPage;

        public MainPage()
        {
            InitializeComponent();

            settingsPage = new SettingsPage();
        }

        private void Settings_Clicked (object sender, EventArgs e)
        {
            Navigation.PushAsync(settingsPage);
        }
    }
}
