using API.DTOs;
using MauiApp1.Services;
using MauiApp1.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp1.ViewModels
{
    [QueryProperty(nameof(ListId), "FlashCardsListsCardsListID")]
    public class CardListViewModel : INotifyPropertyChanged
    {
        private int _listId;
        public int ListId
        {
            get => _listId;
            set
            {
                _listId = value;
                OnPropertyChanged();
                Task.Run(LoadCardListsAsync); // Ładuj pozycje po przekazaniu ListId z QueryProperty
            }
        }
        private readonly Cards _cards;
        public CardListViewModel(Cards cards)
        {
            _cards = cards;
            LoadCardsCommand = new Command(async () => await LoadCardListsAsync());
            Cards = new ObservableCollection<GetCardDTO>();
        }
        private ObservableCollection<GetCardDTO> _cardsList;
        public ObservableCollection<GetCardDTO> Cards
        {
            get => _cardsList;
            set
            {
                _cardsList = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoadCardsCommand { get; }
        private async Task LoadCardListsAsync()
        {
            if (ListId <= 0) return;

            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await Shell.Current.DisplayAlert("Error", "You are not logged in.", "Ok");
                    return;
                }

                var response = await _cards.GetCardsByListId(ListId, token);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CardsResponse>();
                    if (result != null && result.Cards != null)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Cards.Clear();
                            foreach (var item in result.Cards)
                            {
                                Cards.Add(item);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load card lists: {ex.Message}", "OK");
            }
        }

        public class CardsResponse
        {
            public string Message { get; set; }
            public IEnumerable<GetCardDTO> Cards { get; set; }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
