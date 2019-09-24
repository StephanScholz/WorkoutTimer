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
        bool timer_paused = false;
        private CancellationTokenSource cancellation;

        public MainPage()
        {
            InitializeComponent();

            settingsPage = new SettingsPage();
            cancellation = new CancellationTokenSource();

            ResetTimer();
        }

        // returns "0" or an empty string, depending if number is bigger or smaller than 10
        private string GetLeadingZero (int number)
        {
            return number < 10 ? "0" : "";
        }

        private void Settings_Clicked (object sender, EventArgs e)
        {
            Navigation.PushAsync(settingsPage);
        }

        private void ResetTimerButton_Clicked (object sender, EventArgs e)
        {
            timer_started = false;
            timer_paused = false;
            pauseButton.IsVisible = false;
            ResetTimer();
        }

        private void ResetTimer()
        {
            Interlocked.Exchange(ref cancellation, new CancellationTokenSource()).Cancel();

            // Reset timer-label
            int minutes = settingsPage.Preferences.SetMinutes;
            int seconds = settingsPage.Preferences.SetSeconds;
            timer.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();

            // Reset set-label
            setTime.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();

            // Reset pause-label
            minutes = settingsPage.Preferences.PauseMinutes;
            seconds = settingsPage.Preferences.PauseSeconds;
            pauseTime.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();
        }

        private void StartWorkoutButton_Clicked (object sender, EventArgs e)
        {
            if (timer.Text == "00:00") return;
            if (timer_paused) return;

            timer_started = true;
            pauseButton.IsVisible = true;

            int maxSeconds = (settingsPage.Preferences.SetMinutes * 60) + settingsPage.Preferences.SetSeconds;
            int minutes, seconds;
            int secondsElapsed = 0;

            CancellationTokenSource cts = this.cancellation; // safe copy

            // Timer
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (secondsElapsed < maxSeconds)
                {
                    if (cts.IsCancellationRequested) return false;

                    if (!timer_paused)
                    {
                        secondsElapsed++;
                    }
             
                    // calculate remaining time
                    minutes = (maxSeconds - secondsElapsed) / 60;
                    seconds = (maxSeconds - secondsElapsed) % 60;

                    // update time-text
                    timer.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();

                    return true;
                }

                timer_started = false;
                return false;
            });
        }

        private void PauseButton_Clicked (object sender, EventArgs args)
        {
            if (!timer_paused)
            {
                timer_paused = true;
                pauseButton.Text = "Resume";
            }
            else
            {
                timer_paused = false;
                pauseButton.Text = "Pause";
            }
        }
    }
}
