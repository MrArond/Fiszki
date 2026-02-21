using MauiApp1.Services;
using MauiApp1.DTOs;
using API.DTOs;

namespace MauiApp1.Sites;

public partial class ForgotPassword : ContentPage

{
    private readonly AuthClient _authClient;
    public ForgotPassword(AuthClient authClient)

	{
		InitializeComponent();
		_authClient = authClient;
	}

	private async void OnResetPassword(Object sender, EventArgs e)
	{
		var ForgotPasswordDTO = new ForgotPasswordDTO
		{
			Email = IsEmailTrue.Text,
			IdOfSecretQuestion = SecretQuestionPicker.SelectedIndex,
			SecretPassword = SecretPassword.Text,
			Password = Password.Text
		};

		var TruePassword = ResetPassword.Text;

		if (!string.IsNullOrEmpty(ForgotPasswordDTO.Email))
		{
			if(ForgotPasswordDTO.IdOfSecretQuestion == -1)
			{
				await DisplayAlert("error", "Choose the secret question", "Ok");
				return;
			}
			if (string.IsNullOrWhiteSpace(ForgotPasswordDTO.SecretPassword))
			{
				await DisplayAlert("error", "Please type the secret answer", "Ok");
				return;
			}
			if(ForgotPasswordDTO.Password != TruePassword)
			{
				await DisplayAlert("error", "The passwords doesnt match", "Ok");
				return;
			}
			if (IsEmailTrue.Text.Contains("@"))
			{
				var response = await _authClient.ForgotPasswordAsync(ForgotPasswordDTO);

				if (response.IsSuccessStatusCode)
				{
					await Shell.Current.GoToAsync(nameof(Login), true);
				}
			}
			

		}
	}
}