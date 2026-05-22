using API.DTOs;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp1.ViewModels
{
    [QueryProperty(nameof(FlashCardsListsCardsListID), "FlashCardsListsCardsListID")]
    public class CardListViewModel : INotifyPropertyChanged
    {
        private readonly Cards _cards;

        private int _flashCardsListsCardsListID;
        public int FlashCardsListsCardsListID
        {
            get => _flashCardsListsCardsListID;
            set
            {
                _flashCardsListsCardsListID = value;
                OnPropertyChanged();
                _ = LoadCardsAsync();
            }
        }

        private ObservableCollection<GetCardDTO> _cardsList = new();
        public ObservableCollection<GetCardDTO> Cards
        {
            get => _cardsList;
            set
            {
                _cardsList = value;
                OnPropertyChanged();
            }
        }

        private string _newCardName = string.Empty;
        public string NewCardName
        {
            get => _newCardName;
            set
            {
                _newCardName = value;
                OnPropertyChanged();
            }
        }

        private string _newTranslate = string.Empty;
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

        public CardListViewModel(Cards cards)
        {
            _cards = cards;
            LoadCardsCommand = new Command(async () => await LoadCardsAsync());
            AddCardCommand = new Command(async () => await AddCardAsync());
            DeleteCardCommand = new Command<GetCardDTO>(async (selectedCard) => await DeleteCardAsync(selectedCard));
            RenameCardCommand = new Command<GetCardDTO>(async (selectedCard) => await RenameCardAsync(selectedCard));
        }

        public async Task LoadCardsAsync()
        {
            if (IsBusy || FlashCardsListsCardsListID <= 0) return;
            IsBusy = true;
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await Shell.Current.DisplayAlert("Error", "You are not logged in.", "Ok");
                    return;
                }

                var response = await _cards.GetCardsByListId(FlashCardsListsCardsListID, token);
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
                await Shell.Current.DisplayAlert("Error", $"Failed to load cards: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task AddCardAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCardName) || string.IsNullOrWhiteSpace(NewTranslate)) return;
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await Shell.Current.DisplayAlert("Error", "You are not logged in.", "Ok");
                    return;
                }

                var addCardDto = new AddCardDTO
                {
                    CardName = NewCardName,
                    Translate = NewTranslate,
                    FlashCardsListsCardsListID = FlashCardsListsCardsListID
                };

                var response = await _cards.AddCard(addCardDto, token);
                if (response.IsSuccessStatusCode)
                {
                    NewCardName = string.Empty;
                    NewTranslate = string.Empty;
                    IsBusy = false;
                    await LoadCardsAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to add card: {ex.Message}", "OK");
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

            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                await Shell.Current.DisplayAlert("Error", "You are not logged in.", "Ok");
                return;
            }

            var response = await _cards.DeleteCard(selectedCard.CardsId, token);
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

            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                await Shell.Current.DisplayAlert("Error", "You are not logged in.", "Ok");
                return;
            }

            var dto = new UpdateCardNameDTO
            {
                CardsId = selectedCard.CardsId,
                CardName = newName
            };

            var response = await _cards.UpdateCardName(dto, token);
            if (response.IsSuccessStatusCode)
            {
                await LoadCardsAsync();
                return;
            }

            await Shell.Current.DisplayAlert("Error", "Failed to update card name.", "Ok");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class GetCardsResponse
    {
        public string? Message { get; set; }
        public List<GetCardDTO>? Cards { get; set; }
    }
}
