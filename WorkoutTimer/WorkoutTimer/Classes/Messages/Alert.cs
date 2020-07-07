using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkoutTimer.Classes.Messages
{
    static class Alert
    {
        private static CancellationTokenSource cancelSrc;

        // Displays an AlertDialog for the given Time. After which it disappears
        public static async Task DisplayAlertForXSeconds(string title, string message, string OKText, int secondsToDismiss)
        {
            cancelSrc = new CancellationTokenSource();

            try
            {
                cancelSrc?.CancelAfter(TimeSpan.FromSeconds(secondsToDismiss));
                var x2 = new Acr.UserDialogs.AlertConfig() { Message = message, OkText = OKText, Title = title };
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(x2, cancelSrc?.Token);
            }
            catch (OperationCanceledException)
            {
                cancelSrc = new CancellationTokenSource();
            }
        }
    }
}
