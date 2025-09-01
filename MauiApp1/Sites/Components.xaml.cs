namespace MauiApp1.Sites;

public partial class Components : ContentView
{
    public Components()
    {
        InitializeComponent();
    }
    private async void TestButton(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}