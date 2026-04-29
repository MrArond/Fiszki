namespace MauiApp1.Sites;

public partial class HomePage : ContentPage
{
	private HomePageViewModel _viewModel;

	public HomePage(Cards cards)
	{
		InitializeComponent();
		_viewModel = new HomePageViewModel(cards);
		BindingContext = _viewModel;
	}
}