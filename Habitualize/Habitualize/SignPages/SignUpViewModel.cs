using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Habitualize.SignPages;
using Habitualize.View;

namespace Habitualize.SignPages
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        public SignUpViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            LogoutCommand = new RelayCommand(OnLogout);
        }

        [RelayCommand]
        private async Task SignUp()
        {
            await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password, Username);

            Preferences.Set("IsLoggedIn", true);

            await Shell.Current.GoToAsync("//SignIn");
        }

        [RelayCommand]
        private async Task NavigateSignIn()
        {
            await Shell.Current.GoToAsync("//SignIn");
        }

        private void OnLogout()
        {
            _authClient.SignOut();
            Preferences.Remove("IsLoggedIn");
            Application.Current.MainPage = new AppShell();
        }

        public ICommand LogoutCommand { get; }
    }
}
