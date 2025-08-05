﻿namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Login), typeof(Login));
            Routing.RegisterRoute(nameof(Register), typeof(Register));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
