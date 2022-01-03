using Xamarin.Forms;
using SportPlanner.ViewModels;
using SportPlanner.Bootstrap;
using SportPlanner.Services;

namespace SportPlanner.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            var dataStore = Appcontiner.Resolve<IEventDataStore>();
            var loginService = Appcontiner.Resolve<IUserLoginService>();
            BindingContext = _viewModel = new ItemsViewModel(dataStore, loginService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}