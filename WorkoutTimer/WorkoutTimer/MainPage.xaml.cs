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
        bool timer_paused = false;
        bool isSet = true;

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
            timer_paused = false;
            pauseButton.IsVisible = false;
            isSet = true;
            ResetTimer();
        }

        private int GetIntervalMinutes()
        {
            return isSet ? settingsPage.Preferences.SetMinutes : settingsPage.Preferences.PauseMinutes;
        }

        private int GetIntervalSeconds()
        {
            return isSet ? settingsPage.Preferences.SetSeconds : settingsPage.Preferences.PauseSeconds;
        }

        private void ResetTimer()
        {
            Interlocked.Exchange(ref cancellation, new CancellationTokenSource()).Cancel();

            // Reset timer-label
            int minutes = GetIntervalMinutes();
            int seconds = GetIntervalSeconds();
            
            timer.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();

            // Reset set-label
            setTime.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();

            // Reset pause-label
            minutes = settingsPage.Preferences.PauseMinutes;
            seconds = settingsPage.Preferences.PauseSeconds;
            pauseTime.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();

            // Reset Set Counter
            count.Text = settingsPage.Preferences.SetCounter.ToString();
        }

        private void StartWorkoutButton_Clicked (object sender, EventArgs e)
        {
            if (timer.Text == "00:00") return;
            if (timer_paused) return;

            ResetTimer();

            pauseButton.IsVisible = true;

            int minutes = GetIntervalMinutes();
            int seconds = GetIntervalSeconds();
            int maxSeconds = (minutes * 60) + seconds;
            int secondsElapsed = 0;
            int setCounter = settingsPage.Preferences.SetCounter;

            CancellationTokenSource cts = this.cancellation; // safe copy

            // Timer
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (setCounter == 0)
                {
                    pauseButton.IsVisible = false;
                    return false;
                }
                else if(secondsElapsed < maxSeconds)
                {
                    if (cts.IsCancellationRequested) return false;

                    if (!timer_paused)
                    {
                        secondsElapsed++;
                    }
             
                    // calculate remaining time
                    minutes = (maxSeconds - secondsElapsed) / 60;
                    seconds = (maxSeconds - secondsElapsed) % 60;

                    // Play Audio
                    if (seconds == 0)
                    {
                        var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                        player.Load("audio/endbuzzer.mp3");
                        player.Play();
                    }
                    else if (seconds <= 3)
                    {
                        var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                        player.Load("audio/timeup.mp3");
                        player.Play();
                    }

                    // update time-text
                    timer.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();

                    return true;
                } 
                else
                {
                    SwitchSet();

                    minutes = GetIntervalMinutes();
                    seconds = GetIntervalSeconds();

                    maxSeconds = (minutes * 60) + seconds;
                    secondsElapsed = 0;
                    if (isSet)
                    {
                        setCounter--;
                    }
                    
                    // update time-text
                    timer.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();
                    count.Text = setCounter.ToString();
                    return true;
                }
            });
        }

        // switching between set and pause
        private void SwitchSet ()
        {
            if (isSet)
            {
                current.Text = "Pause";
                isSet = false;
            }
            else
            {
                current.Text = "Set";
                isSet = true;
            }
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
