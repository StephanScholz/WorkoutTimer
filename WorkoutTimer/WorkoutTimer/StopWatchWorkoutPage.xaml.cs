using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutTimer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopWatchWorkoutPage : ContentPage
    {
        private bool timer_IsRunning = false;

        public StopWatchWorkoutPage()
        {
            InitializeComponent();
        }

        private string GetLeadingZero(int number)
        {
            return number < 10 ? "0" : "";
        }

        private void StartButton_Clicked(object sender, EventArgs args)
        {
            startButton.IsVisible = false;
            stopButton.IsVisible = true;

            timer_IsRunning = true;

            int minutes = 0;
            int seconds = 0;
            int secondsElapsed = 0;

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (!timer_IsRunning)
                {
                    return false;
                }
                else
                {
                    secondsElapsed++;
                    minutes = secondsElapsed / 60;
                    seconds = secondsElapsed % 60;

                    timer.Text = GetLeadingZero(minutes) + minutes.ToString() + ":" + GetLeadingZero(seconds) + seconds.ToString();
                    return true;
                }
            });
        }

        private void StopButton_Clicked(object sender, EventArgs args)
        {
            timer_IsRunning = false;
            stopButton.IsVisible = false;
            startButton.IsVisible = true;

            lastSetTime.Text = timer.Text;
            timer.Text = "00:00";
        }
    }
}