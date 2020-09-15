using System.ComponentModel;
using Xamarin.Forms;
using SportPlanner.ViewModels;

namespace SportPlanner.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}