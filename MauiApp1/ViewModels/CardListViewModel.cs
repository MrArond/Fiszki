using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using API.DTOs;

namespace MauiApp1.ViewModels
{
    [QueryProperty(nameof(FlashCardsListsCardsListID), "FlashCardsListsCardsListID")]
    public class CardListViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;

        private int _flashCardsListsCardsListID;
        public int FlashCardsListsCardsListID
        {
            get => _flashCardsListsCardsListID;
            set
            {
                if (_flashCardsListsCardsListID != value)
                {
                    _flashCardsListsCardsListID = value;
                    OnPropertyChanged();
                    LoadCardsAsync().ConfigureAwait(false);
                }
            }
        }

        private ObservableCollection<GetCardDTO> _cards = new();
        public ObservableCollection<GetCardDTO> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropertyChanged();
            }
        }

        private string _newCardName;
        public string NewCardName
        {
            get => _newCardName;
            set
            {
                _newCardName = value;
                OnPropertyChanged();
            }
        }

        private string _newTranslate;
        public string NewTranslate
        {
            get => _newTranslate;
            set
            {
                _newTranslate = value;
                OnPropertyChanged();
            }
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
        public ICommand DeleteCardCommand { get; }
        public ICommand RenameCardCommand { get; }

        public CardListViewModel()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7037");

            LoadCardsCommand = new Command(async () => await LoadCardsAsync());
            AddCardCommand = new Command(async () => await AddCardAsync());
            DeleteCardCommand = new Command<GetCardDTO>(async (selectedCard) => await DeleteCardAsync(selectedCard));
            RenameCardCommand = new Command<GetCardDTO>(async (selectedCard) => await RenameCardAsync(selectedCard));
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
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteCardAsync(GetCardDTO? selectedCard)
        {
            if (selectedCard == null)
            {
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Usuń fiszkę", "Czy na pewno chcesz usunąć tę fiszkę?", "Tak", "Nie");
            if (!confirm)
            {
                return;
            }

            string token = await SecureStorage.Default.GetAsync("auth_token");
            if (string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.DisplayAlert("Error", "You are not logged in.", "Ok");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/Cards/DeleteCard/{selectedCard.CardsId}");
            if (response.IsSuccessStatusCode)
            {
                await LoadCardsAsync();
                return;
            }

            await Shell.Current.DisplayAlert("Error", "Failed to delete card.", "Ok");
        }

        private async Task RenameCardAsync(GetCardDTO? selectedCard)
        {
            if (selectedCard == null)
            {
                return;
            }

            var newName = await Shell.Current.DisplayPromptAsync("Zmień nazwę", "Podaj nową nazwę fiszki", "Zapisz", "Anuluj", initialValue: selectedCard.CardName);
            if (string.IsNullOrWhiteSpace(newName))
            {
                return;
            }

            string token = await SecureStorage.Default.GetAsync("auth_token");
            if (string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.DisplayAlert("Error", "You are not logged in.", "Ok");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var dto = new UpdateCardNameDTO
            {
                CardsId = selectedCard.CardsId,
                CardName = newName
            };

            var response = await _httpClient.PutAsJsonAsync("/api/Cards/UpdateCardName", dto);
            if (response.IsSuccessStatusCode)
            {
                await LoadCardsAsync();
                return;
            }

            await Shell.Current.DisplayAlert("Error", "Failed to update card name.", "Ok");
        }

        public async Task AddCardAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCardName) || string.IsNullOrWhiteSpace(NewTranslate)) return;
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                string token = await SecureStorage.Default.GetAsync("auth_token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var addCardDto = new AddCardDTO 
                {
                    CardName = NewCardName,
                    Translate = NewTranslate,
                    FlashCardsListsCardsListID = FlashCardsListsCardsListID
                };

                var response = await _httpClient.PostAsJsonAsync("/api/Cards/AddCard", addCardDto);
                if (response.IsSuccessStatusCode)
                {
                    NewCardName = string.Empty;
                    NewTranslate = string.Empty;
                    IsBusy = false;
                    await LoadCardsAsync();
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class GetCardsResponse
    {
        public string Message { get; set; }
        public List<GetCardDTO> Cards { get; set; }
    }
}
