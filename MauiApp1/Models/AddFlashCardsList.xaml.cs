using MauiApp1.ViewModels;

namespace MauiApp1.Sites;

public partial class AddFlashCardsList : ContentPage
{
	public AddFlashCardsList()
	{
		InitializeComponent();
		BindingContext = new AddFlashCardsListViewModel();
	}
}