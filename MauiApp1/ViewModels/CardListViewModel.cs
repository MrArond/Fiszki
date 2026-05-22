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

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCardsCommand { get; }
        public ICommand AddCardCommand { get; }

        public CardListViewModel()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7037");

            LoadCardsCommand = new Command(async () => await LoadCardsAsync());
            AddCardCommand = new Command(async () => await AddCardAsync());
        }

        public async Task LoadCardsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                string token = await SecureStorage.Default.GetAsync("auth_token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"/api/Cards/GetCards/{FlashCardsListsCardsListID}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GetCardsResponse>();
                    Cards.Clear();
                    if (result?.Cards != null)
                    {
                        foreach (var card in result.Cards)
                        {
                            Cards.Add(card);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load card lists: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
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
