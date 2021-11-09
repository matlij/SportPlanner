using SportPlanner.Models;
using SportPlanner.Services;
using SportPlanner.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace SportPlanner.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserLoginService _userLoginService;
        private string userName;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public Command LoginCommand { get; }

        public LoginViewModel(IUserLoginService userLoginService)
        {
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            LoginCommand = new Command(OnLoginClicked, CanRegisterUser);
            PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }

        private bool CanRegisterUser(object arg)
        {
            return !string.IsNullOrEmpty(UserName);
        }

        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;

            try
            {
                var user = new User(Guid.NewGuid())
                {
                    Name = UserName
                };

                await _userLoginService.UpsertUser(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Register user '{UserName}' failed. {e.Message}", e.StackTrace);
            }

            IsBusy = false;

            await Shell.Current.GoToAsync($"///{nameof(LoadingPage)}");
        }
    }
}
