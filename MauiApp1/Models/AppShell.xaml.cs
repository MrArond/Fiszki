using MauiApp1.Sites;
using MauiApp1.ViewModels;
using MauiApp1.Views;

namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel();


            Routing.RegisterRoute(nameof(Login), typeof(Login));
            Routing.RegisterRoute(nameof(Register), typeof(Register));
            Routing.RegisterRoute(nameof(ForgotPassword), typeof(ForgotPassword));
            Routing.RegisterRoute(nameof(CardList), typeof(Views.CardList));
        }
    }
}
