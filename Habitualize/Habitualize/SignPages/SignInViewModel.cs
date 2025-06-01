using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Habitualize.Core.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Habitualize.Services;

namespace Habitualize.SignPages
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        private readonly IGetHabitualizeQuery _getHabitualizeQuery;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _currentFireBaseLink;

        public string Username => _authClient.User?.Info?.DisplayName;

        [ObservableProperty]
        private string _habitualizeMessage;

        public SignInViewModel(FirebaseAuthClient authClient, IGetHabitualizeQuery getHabitualizeQuery)
        {
            _authClient = authClient;
            _getHabitualizeQuery = getHabitualizeQuery;
            _getHabitualizeQuery.Execute().ContinueWith(response =>
            {
                //HabitualizeMessage = response?.Result?.Message ?? "";
            });
        }

        [RelayCommand]
        private async Task SignIn()
        {
            await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

            Preferences.Set("IsLoggedIn", true);
            Preferences.Set("Username", Username);

            await Shell.Current.GoToAsync("//MainPage");

            OnPropertyChanged(nameof(Username));

            var saveAndLoadService = new SaveAndLoad();
            var userId = saveAndLoadService.UserId;
            if (!string.IsNullOrEmpty(userId))
            {
                await saveAndLoadService.SaveUserNameToFirebase(userId, Username);
            }

            await (Application.Current as App).PreloadUserDataAsync();
        }

        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }
    }

    public interface IGetHabitualizeQuery
    {
        [Get("/")]
        Task<HabitualizeResponse> Execute();
    }
}
