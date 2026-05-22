using API.DTOs;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.ViewModels
{
    internal class AddFlashCardsListViewModel : INotifyPropertyChanged
    {
        public AddFlashCardsListViewModel(AuthClient authClient)
        {
            _AuthClient = authClient;
            ToggleFormCommand = new Command(async () => await ToggleForm(), () => !IsBusy);
            SaveCommand = new Command(async () => await AddList(), () => !IsBusy);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((Command)ToggleFormCommand).ChangeCanExecute();
                ((Command)SaveCommand).ChangeCanExecute();
            }
        }

        private AddCardsListDTO _AddCardList = new AddCardsListDTO();

        public AddCardsListDTO ListCard
        {
            get => _AddCardList;
            set
            {
                _AddCardList = value;
                OnPropertyChanged();
            }
        }


        private bool _isFormVisible;
        public bool IsFormVisible
        {
            get => _isFormVisible;
            set
            {
                _isFormVisible = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand ToggleFormCommand { get; }

        private async Task ToggleForm()
        {
            IsFormVisible = !IsFormVisible;
        }
        private async Task AddList()
        {
            if (String.IsNullOrWhiteSpace(ListCard.Name))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a name for the list.", "OK");
            }
            if (String.IsNullOrWhiteSpace(ListCard.Name))
            {
                await Shell.Current.DisplayAlert("Error", "Please provide description.", "OK");
            }
            var AddCardListDTO = new AddCardsListDTO
            {
                Name = ListCard.Name,
                Description = ListCard.Description
            };
            var token = await SecureStorage.Default.GetAsync("auth_token");
            var response = await _AuthClient.AddCardsList(AddCardListDTO, token);

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync("//HomePage", true);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Login failed. Please check your credentials.", "Ok");
            }

        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly AuthClient _AuthClient;
    }
}