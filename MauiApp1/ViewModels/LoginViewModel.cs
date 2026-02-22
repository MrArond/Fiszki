using MauiApp1.DTOs;
using MauiApp1.Services;
using MauiApp1.Sites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.ViewModels
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthClient _AuthClient;

        public LoginViewModel(AuthClient authClient)
        {
            _AuthClient = authClient;
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
            ForgotCommand = new Command(async () => await ForgotAsync(), () => !IsBusy);
        }

        private LoginDTO _login = new LoginDTO();

        public LoginDTO Login
        {
            get => _login;
            set
            { _login = value;
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
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
        public ICommand LoginCommand {get; }
        public ICommand ForgotCommand { get; }

        private async Task LoginAsync()
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Login.Email))
                {
                    await Shell.Current.DisplayAlert("Error", "Please provice email", "Ok");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Login.Password))
                {
                    await Shell.Current.DisplayAlert("Error", "Please provide password", "Ok");
                    return;
                }
                if (!MailAddress.TryCreate(Login.Email, out var mailResult))
                {
                    await Shell.Current.DisplayAlert("error", "Invalid email", "Ok");
                    return;
                }
                var loginDto = new LoginDTO
                {
                    Email = Login.Email,
                    Password = Login.Password
                };

                var response = await _AuthClient.LoginAsync(loginDto);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.GoToAsync("//HomePage", true);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Login failed. Please check your credentials.", "Ok");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task ForgotAsync()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync(nameof(ForgotPassword), true);
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
