using System.Text.Json;

namespace MauiApp1;

public partial class Register : ContentPage
{
    public Register()
    {
        InitializeComponent();
    }
    private async void RegisterFunction(object sender, EventArgs e)
    {

        RegisterModel registerModel = new RegisterModel
        {
            Nickname = NickNameTrue.Text,
            Email = EmailTrue.Text,
            Password = PasswordTrue.Text
        };
        string PasswordAgainTrue = PasswordAgain.Text;
        
        if(!string.IsNullOrWhiteSpace(registerModel.Nickname) && !string.IsNullOrWhiteSpace(registerModel.Email) && registerModel.Password == PasswordAgainTrue )
        {
            string json = JsonSerializer.Serialize(registerModel);

            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:5008");

            HttpResponseMessage response = await client.PostAsync("/api/Auth/Register", content);

            await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
        }
    }

}
public class RegisterModel
{

    public string Nickname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}