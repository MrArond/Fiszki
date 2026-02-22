using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Sites;
using MauiApp1.ViewModels;
using System.Net;
namespace MauiApp1;

public partial class Register : ContentPage
{

    public Register(AuthClient authClient)
    {
        InitializeComponent();
        BindingContext = new RegisterViewModel(authClient);
    }

}