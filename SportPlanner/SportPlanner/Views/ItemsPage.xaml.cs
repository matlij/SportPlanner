﻿using Xamarin.Forms;

using SportPlanner.Models;
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
            BindingContext = _viewModel = new ItemsViewModel(dataStore);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}