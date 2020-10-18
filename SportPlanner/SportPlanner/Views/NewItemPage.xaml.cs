using Xamarin.Forms;

using SportPlanner.Models;
using SportPlanner.ViewModels;
using SportPlanner.Bootstrap;
using SportPlanner.Services;

namespace SportPlanner.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            var dataStore = Appcontiner.Resolve<IDataStore<Event>>();
            BindingContext = new NewItemViewModel(dataStore);
        }
    }
}