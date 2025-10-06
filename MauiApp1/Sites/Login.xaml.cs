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



        if (!string.IsNullOrWhiteSpace(loginDTO.Email) && !string.IsNullOrWhiteSpace(loginDTO.Password))
        {

            var response = await _authClient.LoginAsync(loginDTO);

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync(nameof(Test), true);
            }
            else
            {
                await DisplayAlert("Dane logowania", "Login lub has³o s¹ nieprawid³owe", "Wpisz jeszcze raz");
            }

        }
        else
        {
            await DisplayAlert("Dane logowania", "Login lub has³o s¹ nieprawid³owe", "Wpisz jeszcze raz");
        }
    }

}