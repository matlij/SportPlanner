using SportPlanner.Bootstrap;
using SportPlanner.Services;
using SportPlanner.ViewModels;
using Xamarin.Forms;

namespace SportPlanner.Views
{
    public partial class AboutPage : ContentPage
    {
        private readonly AboutViewModel _viewModel;

        public AboutPage()
        {
            InitializeComponent();
            var loginService = Appcontiner.Resolve<IUserLoginService>();
            BindingContext = _viewModel = new AboutViewModel(loginService);
        }

        protected override async void OnAppearing()
        {
            await _viewModel.OnAppearing();
            base.OnAppearing();
        }
    }
}