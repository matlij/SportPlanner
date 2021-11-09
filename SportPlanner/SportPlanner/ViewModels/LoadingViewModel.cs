using SportPlanner.Services;
using SportPlanner.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    public class LoadingViewModel : BaseViewModel
    {
        private readonly IUserLoginService _userLoginService;

        public LoadingViewModel(IUserLoginService userLoginService)
        {
            _userLoginService = userLoginService ?? throw new System.ArgumentNullException(nameof(userLoginService));
        }

        public async Task Init()
        {
            IsBusy = true;

            var (succeeded, user) = await _userLoginService.LoginUser();

            IsBusy = false;

            if (succeeded && user != null)
            {
                await Shell.Current.GoToAsync($"///{nameof(ItemsPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
        }
    }
}
