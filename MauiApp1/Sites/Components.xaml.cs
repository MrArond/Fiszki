namespace MauiApp1.Sites;

public partial class Components : ContentView
{
    public Components()
    {
        InitializeComponent();
    }
    private async void Go_Back(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage", true);
    }
}