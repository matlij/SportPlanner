using Xamarin.Forms;
using SportPlanner.ViewModels;
using SportPlanner.Bootstrap;
using SportPlanner.Services;

namespace SportPlanner.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        private ItemDetailViewModel _viewModel;

        public ItemDetailPage()
        {
            InitializeComponent();
            var dataStore = Appcontiner.Resolve<IEventDataStore>();
            var userService = Appcontiner.Resolve<IUserLoginService>();
            BindingContext = _viewModel = new ItemDetailViewModel(dataStore, userService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel.Id != null)
            {
                _viewModel.LoadItemId(_viewModel.Id);
            }
        }
    }
}