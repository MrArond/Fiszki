using MauiApp1.DTOs;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.ViewModels
{
    internal class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly AuthClient _authClient;

        public RegisterViewModel(AuthClient authClient)
        {
            _authClient = authClient;
            RegisterCommand = new Command(async () => await RegisterAsync(), () => !IsBusy);
        }

        private RegisterDTO _register = new RegisterDTO();


        public RegisterDTO Register
        {
            get { return _register; }
            set { _register = value;
                OnPropertyChanged();
            }
        }
        public bool IsQuestionPlaceholderVisible => Register.IdOfSecretQuestion < 0;


        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((Command)RegisterCommand).ChangeCanExecute();
            }
        }

        public ICommand RegisterCommand { get; }

        private string _repeatPassword = string.Empty;

        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                if (_repeatPassword != value)
                {
                    _repeatPassword = value;
                    OnPropertyChanged(nameof(RepeatPassword));
                }
            }
        }

        private async Task RegisterAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {

                if (string.IsNullOrWhiteSpace(Register.NickName))
                {
                    await Shell.Current.DisplayAlert("Błąd", "Nickname nie zostal podany", "Ok");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Register.Email))
                {
                    await Shell.Current.DisplayAlert("Błąd", "Email nie został podany", "Ok");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Register.Password))
                {
                    await Shell.Current.DisplayAlert("Błąd", "Hasło nie zostało podane", "Ok");
                    return;
                }
                if (RepeatPassword != Register.Password)
                {
                    await Shell.Current.DisplayAlert("Błąd", "Hasła nie są takie same", "Ok");
                    return;
                }
                if (!MailAddress.TryCreate(Register.Email, out var mailResult))
                {
                    await Shell.Current.DisplayAlert("error", "Invalid email", "Ok");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Register.SecretPassword))
                {
                    await Shell.Current.DisplayAlert("error,", "Invalid email", "Ok");
                }

                    var dto = new RegisterDTO
                    {
                        NickName = Register.NickName,
                        Email = Register.Email,
                        Password = Register.Password,
                        IdOfSecretQuestion = Register.IdOfSecretQuestion,
                        SecretPassword = Register.SecretPassword
                    };

                    var response = await _authClient.RegisterAsync(dto);
                    if (response.IsSuccessStatusCode)
                    {
                        await Shell.Current.DisplayAlert("ok", "Created account", "Ok");
                        await Shell.Current.GoToAsync("///MainPage", true);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("error", "Reset failed", "Ok");
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
