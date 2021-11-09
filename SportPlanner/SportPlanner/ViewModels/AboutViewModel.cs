using SportPlanner.Models.Constants;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
        }

        public ICommand OpenWebCommand { get; }

        public string BaseUrl => UriConstants.BaseUri;
    }
}