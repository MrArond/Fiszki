using API.DTOs;
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
        public AddFlashCardsListViewModel()
        {
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

        private AddCardsListDTO AddCardList = new AddCardsListDTO();



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
            if (ListCard.name)
            {

            }

        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}