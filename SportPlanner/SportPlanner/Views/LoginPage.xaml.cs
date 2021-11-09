using SportPlanner.Bootstrap;
using SportPlanner.Services;
using SportPlanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Appcontiner.Resolve<IUserLoginService>());
        }
    }
}