using MauiApp1.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using API.DTOs;
using MauiApp1.Views;

namespace MauiApp1.ViewModels
{
    internal class HomePageViewModel : INotifyPropertyChanged
    {
        private readonly AuthClient _authClient;

        private ObservableCollection<GetCardsListDTO> _cardsLists = new();
        public ObservableCollection<GetCardsListDTO> CardsLists
        {
            get => _cardsLists;
            set
            {
                _cardsLists = value;
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
                ((Command)LoadCardsListsCommand).ChangeCanExecute();
            }
        }

        public ICommand LoadCardsListsCommand { get; }
        public ICommand GoToCardListCommand { get; }
        public ICommand DeleteCardsListCommand { get; }
        public ICommand RenameCardsListCommand { get; }

        public HomePageViewModel(AuthClient authClient)
        {
            _authClient = authClient;
            LoadCardsListsCommand = new Command(async () => await LoadCardsListsAsync(), () => !IsBusy);
            GoToCardListCommand = new Command<GetCardsListDTO>(async (selectedList) => await GoToCardListAsync(selectedList));
            DeleteCardsListCommand = new Command<GetCardsListDTO>(async (selectedList) => await DeleteCardsListAsync(selectedList));
            RenameCardsListCommand = new Command<GetCardsListDTO>(async (selectedList) => await RenameCardsListAsync(selectedList));
        }

        private async Task GoToCardListAsync(GetCardsListDTO selectedList)
        {
            if (selectedList == null)
                return;

            await Shell.Current.GoToAsync($"CardList?FlashCardsListsCardsListID={selectedList.FlashCardsListsCardsListID}");
        }

        private async Task LoadCardsListsAsync()
        {
            IsBusy = true;
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    await Shell.Current.DisplayAlert("Error", "You are not logged in.", "Ok");
                    return;
                }

                var response = await _authClient.GetUserCardsLists(token);
                if (response.IsSuccessStatusCode)
                {
                    var lists = await response.Content.ReadFromJsonAsync<IEnumerable<GetCardsListDTO>>();
                    if (lists != null)
                    {
                        CardsLists.Clear();
                        foreach (var item in lists)
                        {
                            CardsLists.Add(item);
                        }
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to load flashcards lists.", "Ok");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteCardsListAsync(GetCardsListDTO? selectedList)
        {
            if (selectedList == null)
            {
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Usuń listę", "Czy na pewno chcesz usunąć tę listę?", "Tak", "Nie");
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

            var response = await _authClient.DeleteCardsList(selectedList.FlashCardsListsCardsListID, token);
            if (response.IsSuccessStatusCode)
            {
                await LoadCardsListsAsync();
                return;
            }

            await Shell.Current.DisplayAlert("Error", "Failed to delete flashcards list.", "Ok");
        }

        private async Task RenameCardsListAsync(GetCardsListDTO? selectedList)
        {
            if (selectedList == null)
            {
                return;
            }

            var newName = await Shell.Current.DisplayPromptAsync("Zmień nazwę", "Podaj nową nazwę listy", "Zapisz", "Anuluj", initialValue: selectedList.Name);
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

            var dto = new UpdateCardsListNameDTO
            {
                CardsListID = selectedList.FlashCardsListsCardsListID,
                Name = newName
            };

            var response = await _authClient.UpdateCardsListName(dto, token);
            if (response.IsSuccessStatusCode)
            {
                await LoadCardsListsAsync();
                return;
            }

            await Shell.Current.DisplayAlert("Error", "Failed to update flashcards list name.", "Ok");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

