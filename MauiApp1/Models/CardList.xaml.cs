using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class CardList : ContentPage
{
	public CardList()
	{
		InitializeComponent();
		BindingContext = new CardListViewModel();
	}
}