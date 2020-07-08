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
        public MainPage()
        {
            InitializeComponent();
        }

        private void StartWorkout_Clicked (object sender, EventArgs args)
        {
            if (FixedTimeWorkoutRadioButton.IsChecked)
            {
                Navigation.PushAsync(new FixedTimeWorkoutPage());
            }
            else if (StopWatchWorkoutRadioButton.IsChecked)
            {
                Navigation.PushAsync(new StopWatchWorkoutPage());
            }
        }
    }
}
