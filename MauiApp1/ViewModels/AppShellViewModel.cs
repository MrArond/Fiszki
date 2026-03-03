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
    internal class AppShellViewModel : INotifyPropertyChanged
    {

        public AppShellViewModel()      
        {
            LogOutCommand = new Command(async () => await LogOutAsync(), () => !IsBusy);
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((Command)LogOutCommand).ChangeCanExecute();
            }
        }

        public ICommand LogOutCommand { get; }

        private async Task LogOutAsync()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync("//MainPage", true);
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
