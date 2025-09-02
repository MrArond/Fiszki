using System.Text.Json;
using MauiApp1.DTOs;
using MauiApp1.Services;
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
        
        if(!string.IsNullOrWhiteSpace(registerDTO.NickName) && !string.IsNullOrWhiteSpace(registerDTO.Email) && registerDTO.Password == PasswordAgainTrue )
        {
            var response = await _authClient.RegisterAsync(registerDTO);

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
            }
            else
            {
                await DisplayAlert("B³¹d", "Rejestracja siê nie powiod³a.", "OK");
            }
        }
    }

}