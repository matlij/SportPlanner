﻿using SportPlanner.Bootstrap;
using Xamarin.Forms;

namespace SportPlanner
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Appcontiner.RegisterDependencies();
            //DependencyService.Register<CloudStore>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
