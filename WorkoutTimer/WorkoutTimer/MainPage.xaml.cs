using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WorkoutTimer
{
    public partial class MainPage : ContentPage
    {
        SettingsPage settingsPage;
        bool timer_started = false;
        private CancellationTokenSource cancellation;

        public MainPage()
        {
            InitializeComponent();

            settingsPage = new SettingsPage();
            cancellation = new CancellationTokenSource();
        }

        private void Settings_Clicked (object sender, EventArgs e)
        {
            Navigation.PushAsync(settingsPage);
        }

        private void SetTimeButton_Clicked (object sender, EventArgs e)
        {
            if (timer_started) return;
            ResetTimer();
        }

        private void ResetTimer()
        {
            //TODO: Refactor (make it smaller and more elegant)
            //TODO: Leading zeros for pause-time
            int minutes = settingsPage.Preferences.SetMinutes;
            int seconds = settingsPage.Preferences.SetSeconds;
            string zeroMin = "";
            string zeroSec = "";
            if (minutes < 10)
            {
                zeroMin = "0";
            }
            if (seconds < 10)
            {
                zeroSec = "0";
            }
            setTime.Text = zeroMin + minutes.ToString() + ":" + zeroSec + seconds.ToString();
            pauseTime.Text = settingsPage.Preferences.PauseMinutes + ":" + settingsPage.Preferences.PauseSeconds;
        }

        private void StartButton_Clicked (object sender, EventArgs e)
        {
            if (setTime.Text == "00:00") return;
            timer_started = true;
            int maxSeconds = (settingsPage.Preferences.SetMinutes * 60) + settingsPage.Preferences.SetSeconds;
            int minutes, seconds;
            int secondsElapsed = 0;

            CancellationTokenSource cts = this.cancellation; // safe copy

            // Timer
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (cts.IsCancellationRequested) return false;

                if (secondsElapsed < maxSeconds)
                {
                    secondsElapsed++;

                    // calculate remaining time
                    minutes = (maxSeconds - secondsElapsed) / 60;
                    seconds = (maxSeconds - secondsElapsed) % 60;

                    // Determine if leading zero is needed
                    string zeroSec = "";
                    string zeroMin = "";
                    if (minutes < 10)
                    {
                        zeroMin = "0";
                    }
                    
                    if (seconds < 10)
                    {
                        zeroSec = "0";
                    }

                    // update time-text
                    setTime.Text = zeroMin + minutes.ToString() + ":" + zeroSec + seconds.ToString();
                    
                    return true;
                }
                setTime.Text = "00:00";
                timer_started = false;
                return false;
            });
        }

        private void StopButton_Clicked (object sender, EventArgs args)
        {
            Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
            ResetTimer();
            timer_started = false;
        }
    }
}
