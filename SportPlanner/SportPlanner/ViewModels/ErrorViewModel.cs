using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    [QueryProperty(nameof(ErrorMessage), nameof(ErrorMessage))]
    [QueryProperty(nameof(ErrorStackTrace), nameof(ErrorStackTrace))]
    public class ErrorViewModel : BaseViewModel
    {
        private string errorMessage;
        private string errorStackTrace;

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public string ErrorStackTrace
        {
            get => errorStackTrace;
            set => SetProperty(ref errorStackTrace, value);
        }
    }
}
