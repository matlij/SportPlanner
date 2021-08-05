using SportPlanner.Bootstrap;
using SportPlanner.Models;
using SportPlanner.Services;
using SportPlanner.ViewModels;
using Xamarin.Forms;

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
    }
}