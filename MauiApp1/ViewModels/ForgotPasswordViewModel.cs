using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using API.DTOs;
using MauiApp1.DTOs;
using MauiApp1.Services;

namespace MauiApp1.ViewModels
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private readonly AuthClient _authClient;

        public ForgotPasswordViewModel(AuthClient authClient)
        {
            _authClient = authClient;
            ResetPasswordCommand = new Command(async () => await ResetPasswordAsync(), () => !IsBusy);
        }

        private ForgotPasswordDTO _forgotPassword = new ForgotPasswordDTO();
        public ForgotPasswordDTO ForgotPassword
        {
            get => _forgotPassword;
            set { _forgotPassword = value; 
                    OnPropertyChanged();
            }
        }

        public bool IsQuestionPlaceholderVisible => ForgotPassword.IdOfSecretQuestion < 0;


        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((Command)ResetPasswordCommand).ChangeCanExecute();
            }
        }

        public ICommand ResetPasswordCommand { get; }

        private string _repeatPassword = string.Empty;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                if (_repeatPassword != value)
                {
                    _repeatPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        private async Task ResetPasswordAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {

                if (string.IsNullOrWhiteSpace(ForgotPassword.Email))
                {
                    await Shell.Current.DisplayAlert("error", "Email is required", "Ok");
                    return;
                }

                if (!MailAddress.TryCreate(ForgotPassword.Email, out var mailResult))
                {
                    await Shell.Current.DisplayAlert("error", "Invalid email", "Ok");
                    return;
                }

                if (ForgotPassword.Email.Contains("@"))
                {
                    await Shell.Current.DisplayAlert("error", "Invalid email", "Ok");
                    return;
                }

                if (ForgotPassword.IdOfSecretQuestion < 0)
                {
                    await Shell.Current.DisplayAlert("error", "Choose the secret question", "Ok");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ForgotPassword.SecretPassword))
                {
                    await Shell.Current.DisplayAlert("error", "Please type the secret answer", "Ok");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ForgotPassword.Password))
                {
                    await Shell.Current.DisplayAlert("error", "New password is required", "Ok");
                    return;
                }

                if (ForgotPassword.Password.Length < 6)
                {
                    await Shell.Current.DisplayAlert("error", "Password must be at least 6 characters", "Ok");
                    return;
                }

                if (ForgotPassword.Password != RepeatPassword)
                {
                    await Shell.Current.DisplayAlert("error", "The passwords doesn't match", "Ok");
                    return;
                }



                var dto = new ForgotPasswordDTO
                {
                    Email = ForgotPassword.Email,
                    IdOfSecretQuestion = ForgotPassword.IdOfSecretQuestion,
                    SecretPassword = ForgotPassword.SecretPassword.Trim(),
                    Password = ForgotPassword.Password
                };

                var response = await _authClient.ForgotPasswordAsync(dto);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("ok", "Password changed", "Ok");
                    await Shell.Current.GoToAsync(nameof(Login), true);
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