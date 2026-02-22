using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Sites;

namespace MauiApp1;

public partial class Login : ContentPage
{
    private readonly AuthClient _authClient;
    public Login(AuthClient authClient)
    {
        InitializeComponent();
        _authClient = authClient;
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {

        var loginDTO = new LoginDTO
        {
            Email = IsEmailTrue.Text,
            Password = IsPasswordTrue.Text,

        };



        if (!string.IsNullOrWhiteSpace(loginDTO.Email))
        {
            if (!string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                var response = await _authClient.LoginAsync(loginDTO);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.GoToAsync(nameof(HomePage), true);
                }
                else
                {
                    await DisplayAlert("B³¹d", "Has³o lub mail nie prawidlowe", "Ok");
                }
            } 
        }
        else
        {
            await DisplayAlert("B³¹d", "Nie podano maila", "Ok");
        }
    }
    private async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ForgotPassword), true);
    }

}