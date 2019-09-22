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

        private void SetTimeButton_Clicked (object sender, EventArgs e)
        {
            setTime.Text = settingsPage.Preferences.SetMinutes + ":" + settingsPage.Preferences.SetSeconds;
            pauseTime.Text = settingsPage.Preferences.PauseMinutes + ":" + settingsPage.Preferences.PauseSeconds;
        }

        private void StartButton_Clicked (object sender, EventArgs e)
        {
            
        }
    }
}
