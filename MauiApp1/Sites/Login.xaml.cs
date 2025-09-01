using MauiApp1.Sites;
using System.Text.Json;

namespace MauiApp1;

public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();

    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {

        LoginModel loginModel = new LoginModel()
        {
            Login = IsLoginTrue.Text,
            Password = IsPasswordTrue.Text
        };

        

        if (!string.IsNullOrWhiteSpace(loginModel.Login) && !string.IsNullOrWhiteSpace(loginModel.Password))
        {

            string json = JsonSerializer.Serialize(loginModel);

            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:5008");

            HttpResponseMessage response = await client.PostAsync("/api/Auth/Login", content);
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
public class LoginModel
{
    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}