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
            ToggleFormCommand = new Command(ToggleForm);
        }

        private bool _isBusy;
        //public bool IsBusy
        //{
        //    get => _isBusy;
        //    set
        //    {
        //        _isBusy = value;
        //        OnPropertyChanged();
        //        ((Command)AddFlashCardsListCommand).ChangeCanExecute();
        //    }
        //}

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

        public ICommand ToggleFormCommand { get; }

        private void ToggleForm()
        {
            IsFormVisible = !IsFormVisible;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}