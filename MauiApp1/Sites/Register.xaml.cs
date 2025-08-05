namespace MauiApp1;

public partial class Register : ContentPage
{
    public Register()
    {
        InitializeComponent();
    }
    private async void GoBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}