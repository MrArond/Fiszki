using MauiApp1.Services;
using MauiApp1.ViewModels;

namespace MauiApp1.Sites;

public partial class HomePage : ContentPage
{
	private HomePageViewModel _viewModel;

	public HomePage(AuthClient authClient)
	{
		InitializeComponent();
		_viewModel = new HomePageViewModel(authClient);
		BindingContext = _viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (_viewModel != null && _viewModel.LoadCardsListsCommand.CanExecute(null))
		{
			_viewModel.LoadCardsListsCommand.Execute(null);
		}
	}
}
