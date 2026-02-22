using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Sites;
using MauiApp1.ViewModels;

namespace MauiApp1;

public partial class Login : ContentPage
{
    public Login(AuthClient authClient)
    {
        InitializeComponent();
        BindingContext = new LoginViewModel(authClient);
    }
}