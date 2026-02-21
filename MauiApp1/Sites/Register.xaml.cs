using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Sites;
using System.Net;
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
            Password = PasswordTrue.Text,
            SecretPassword = SecretAnswer.Text,
            IdOfSecretQuestion = SecretQuestionPicker.SelectedIndex
        };

        string PasswordAgainTrue = PasswordAgain.Text;
        if (!string.IsNullOrWhiteSpace(registerDTO.NickName))
        {
            if (string.IsNullOrWhiteSpace(registerDTO.Email))
            {
                await DisplayAlert("Błąd", "Email nie został podany", "Ok");
                return;
            }
            if (string.IsNullOrWhiteSpace(registerDTO.Password))
            {
                await DisplayAlert("Błąd", "Hasło nie zostało podane", "Ok");
                return;
            }
            if(PasswordAgainTrue != registerDTO.Password)
            {
                await DisplayAlert("Błąd", "Hasła nie są takie same", "Ok");
                return;
            }
            if(registerDTO.IdOfSecretQuestion == 0)
            {
                await DisplayAlert("Błąd", "Prosze wybrac pytanie", "Ok");
                return;
            }
            if (EmailTrue.Text.Contains("@"))
            {
                var response = await _authClient.RegisterAsync(registerDTO);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.GoToAsync(nameof(HomePage), true);
                }
                else if (response.StatusCode is HttpStatusCode.BadRequest)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("Konto o podanym mailu istnieje juz"))
                    {
                        await DisplayAlert("Błąd", "Konto o podanym mailu istnieje", "Ok");
                    }
                }
            }     
        }
   
    }

}