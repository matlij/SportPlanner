using System;
using System.Collections.Generic;
using SportPlanner.ViewModels;
using SportPlanner.Views;
using Xamarin.Forms;

namespace SportPlanner
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
