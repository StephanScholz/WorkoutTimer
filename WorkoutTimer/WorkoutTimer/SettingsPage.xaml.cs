using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WorkoutTimer.Classes;
using WorkoutTimer.Classes.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutTimer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        public Preferences Preferences { get; set; }

		public SettingsPage ()
		{
			InitializeComponent ();
            Preferences = new Preferences(Application.Current.Properties);
            entryPauseMinutes.Text = Preferences.PauseMinutes.ToString();
            entryPauseSeconds.Text = Preferences.PauseSeconds.ToString();
            entrySetMinutes.Text = Preferences.SetMinutes.ToString();
            entrySetSeconds.Text = Preferences.SetSeconds.ToString();
        }

        private void ButtonSettingsConfirm_Clicked(object sender, EventArgs args)
        {

            string[] prefs = new string[]{
                entryPauseMinutes.Text,
                entryPauseSeconds.Text,
                entrySetMinutes.Text,
                entrySetSeconds.Text,
                entrySetCount.Text};

            if (Preferences.Save(prefs))
            {
                Alert.DisplayAlertForXSeconds("Success", "Preferences succesfully saved!", "OK", 3);
            }

            Navigation.PushAsync(new MainPage());
        }

        private void OnSettingsEntryCompleted(object sender, EventArgs args)
        {
            // get the invoking entry
            Entry entry = (Entry)sender;

            // define regex for special characters
            var regexItem = new Regex("[-.,]");
            StringBuilder sb = new StringBuilder(entry.Text);

            // If there are special characters in entry, clear entry
            if (regexItem.IsMatch(entry.Text))
            {
                sb.Remove(0, sb.Length);
            }

            // Replace empty entry with 00, insert 0 to start of string, if string.Length == 1
            if (sb.Length == 1)
            {
                entry.Text = "0" + sb.ToString();
            }
            else if (sb.Length == 0)
            {
                entry.Text = "00";
            }
            else
            {
                // check if input number > 60
                int n = Convert.ToInt16(sb.ToString());
                if (n > 59)
                    entry.Text = "59";
                else
                    entry.Text = sb.ToString();
            }
        }

        private void OnEntryFocusEnter(object sender, EventArgs args)
        {
            Entry entry = (Entry)sender;

            entry.Text = "";
        }
    }
}