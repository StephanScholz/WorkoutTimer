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
        SettingsPage settings;

        public MainPage()
        {
            InitializeComponent();

            settings = new SettingsPage();
        }

        private void Settings_Clicked (object sender, EventArgs e)
        {
            Navigation.PushAsync(settings);
        }
    }
}
