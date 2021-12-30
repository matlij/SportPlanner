using SportPlanner.Models;
using SportPlanner.Models.Constants;
using SportPlanner.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IUserLoginService _userLoginService;
        private User _user;

        public AboutViewModel(IUserLoginService userLoginService)
        {
            Title = "About";
            _userLoginService = userLoginService ?? throw new System.ArgumentNullException(nameof(userLoginService));
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://www.korpen-sundbyberg.se/varasektioner/Innebandy/innebandy4utanmalvakt/Herrar/Division1/"));
            DeleteUserCommand = new Command(async () => await _userLoginService.DeleteUser(User.Id));
        }

        public ICommand DeleteUserCommand { get; }

        public ICommand OpenWebCommand { get; }

        public User User
        {
            get => _user;
            set
            {
                SetProperty(ref _user, value);
            }
        }

        public string BaseUrl => UriConstants.BaseUri;

        public async Task OnAppearing()
        {
            try
            {
                User = await _userLoginService.GetUserFromLocalDb();
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}