using SportPlanner.Bootstrap;
using SportPlanner.Services;
using SportPlanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        private readonly LoadingViewModel _viewModel;

        public LoadingPage()
        {
            InitializeComponent();

            var userLoginService = Appcontiner.Resolve<IUserLoginService>();
            _viewModel = new LoadingViewModel(userLoginService);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.Init();
        }
    }
}