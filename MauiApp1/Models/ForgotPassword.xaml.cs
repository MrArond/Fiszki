using MauiApp1.Services;
using MauiApp1.ViewModels;

namespace MauiApp1.Sites;

public partial class ForgotPassword : ContentPage
{
    public ForgotPassword(AuthClient authClient)
    {
        InitializeComponent();
        BindingContext = new ForgotPasswordViewModel(authClient);
    }
}