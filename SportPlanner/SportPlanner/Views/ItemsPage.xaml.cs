using Xamarin.Forms;

using SportPlanner.Models;
using SportPlanner.ViewModels;
using SportPlanner.Bootstrap;
using SportPlanner.Services;
using SportPlanner.Repository.Interfaces;

namespace SportPlanner.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            var dataStore = Appcontiner.Resolve<IEventDataStore>();
            var localStorage = Appcontiner.Resolve< ILocalStorage<User>>();
            BindingContext = _viewModel = new ItemsViewModel(dataStore, localStorage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}