using Xamarin.Forms;
using SportPlanner.ViewModels;
using SportPlanner.Bootstrap;
using SportPlanner.Services;
using SportPlanner.Models;

namespace SportPlanner.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            var dataStore = Appcontiner.Resolve<IDataStore<Event>>();
            BindingContext = new ItemDetailViewModel(dataStore);
        }
    }
}