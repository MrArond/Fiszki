namespace MauiApp1;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void LoginButton(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Login));
    }
    private async void RegisterButton(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Register));
    }
}
