using MauiApp1.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using API.DTOs;

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

        public HomePageViewModel(AuthClient authClient)
        {
            _authClient = authClient;
            LoadCardsListsCommand = new Command(async () => await LoadCardsListsAsync(), () => !IsBusy);
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

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
