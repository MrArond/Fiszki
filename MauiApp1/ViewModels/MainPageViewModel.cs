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
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
            RegisterCommand = new Command(async () => await RegisterAsync(), () => !IsBusy);
        }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        private bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                    ((Command)LoginCommand).ChangeCanExecute();
                    ((Command)RegisterCommand).ChangeCanExecute();
                }
            }
        }

        private async Task LoginAsync()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(nameof(Login));
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task RegisterAsync()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(nameof(Register));
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
