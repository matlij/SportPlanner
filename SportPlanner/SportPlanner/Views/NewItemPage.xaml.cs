using Xamarin.Forms;

using SportPlanner.Models;
using SportPlanner.ViewModels;
using SportPlanner.Bootstrap;
using SportPlanner.Services;

namespace SportPlanner.Views
{
    public partial class NewItemPage : ContentPage
    {
        private readonly NewItemViewModel _viewModel;

        public NewItemPage()
        {
            InitializeComponent();
            var eventDataStore = Appcontiner.Resolve<IEventDataStore>();
            var userDataStore = Appcontiner.Resolve<IDataStore<User>>();
            BindingContext = _viewModel = new NewItemViewModel(eventDataStore, userDataStore);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadUsers();
        }
    }
}