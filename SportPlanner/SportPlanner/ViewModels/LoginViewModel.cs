using ModelsCore.Enums;
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
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = UserName
                };

                var result = await _userLoginService.AddUser(user);

                if (result == CrudResult.AlreadyExists)
                {
                    await Application.Current.MainPage.DisplayAlert("Create account failed", $"User '{UserName}' already exists.", "Ok");
                }
                else if (result == CrudResult.Error)
                {
                    await Application.Current.MainPage.DisplayAlert("Create account failed", $"Server returned an error. Try again in a while.", "Ok");
                }
            }
            catch (Exception e)
            {
                var errorMsg = $"Create account failed. Internal application error: {e.Message}";
                Debug.WriteLine(errorMsg, e.StackTrace);
                await Application.Current.MainPage.DisplayAlert("Create account failed", errorMsg, "Ok");
            }

            IsBusy = false;

            await Shell.Current.GoToAsync($"///{nameof(LoadingPage)}");
        }
    }
}
