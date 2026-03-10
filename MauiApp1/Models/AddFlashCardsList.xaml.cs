using MauiApp1.Services;
using MauiApp1.ViewModels;

namespace MauiApp1.Sites;

public partial class AddFlashCardsList : ContentPage
{
	public AddFlashCardsList(AuthClient authClient)
	{
		InitializeComponent();
		BindingContext = new AddFlashCardsListViewModel(authClient);
	}
}