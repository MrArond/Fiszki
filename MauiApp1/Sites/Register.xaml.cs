using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Sites;
namespace MauiApp1;

public partial class Register : ContentPage
{
    private readonly AuthClient _authClient;


    public Register(AuthClient authClient)
    {
        InitializeComponent();
        _authClient = authClient;
    }
    private async void RegisterFunction(object sender, EventArgs e)
    {



        var registerDTO = new RegisterDTO
        {
            NickName = NickNameTrue.Text,
            Email = EmailTrue.Text,
            Password = PasswordTrue.Text
        };

        string PasswordAgainTrue = PasswordAgain.Text;

        if (!string.IsNullOrWhiteSpace(registerDTO.NickName)
            && !string.IsNullOrWhiteSpace(registerDTO.Email)
            && registerDTO.Password == PasswordAgainTrue
            && EmailTrue.Text.Contains("@"))
        {
            var response = await _authClient.RegisterAsync(registerDTO);

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}", true);
            }
            else
            {
                await DisplayAlert("Błąd", "Rejestracja się nie powiodła.", "OK");
            }
        }
    }

}