﻿// Ignore Spelling: Auth Firebase App

using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Habitualize.SignPages;
using Microsoft.Extensions.Logging;
using Refit;
using System.Net.Http.Headers;

namespace Habitualize;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = "AIzaSyAO6SGqXASw8zw_YD2xqCjBBP7ZOHDDcf0",
            AuthDomain = "habitualize-249ef.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
            UserRepository = new FileUserRepository("Habitualize")
        }));

        builder.Services.AddSingleton<SignInView>();
        builder.Services.AddSingleton<SignInViewModel>();
        builder.Services.AddSingleton<SignUpView>();
        builder.Services.AddSingleton<SignUpViewModel>();

        builder.Services.AddSingleton<FirebaseAuthHttpMessageHandler>();

        builder.Services.AddRefitClient<IGetHabitualizeQuery>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5440"))//Yarik: 7026 Krugloveskiy: 5440
                .AddHttpMessageHandler<FirebaseAuthHttpMessageHandler>();

        return builder.Build();
	}
}

public class FirebaseAuthHttpMessageHandler : DelegatingHandler
{
    private readonly FirebaseAuthClient _authClient;

    public FirebaseAuthHttpMessageHandler(FirebaseAuthClient authClient)
    {
        _authClient = authClient;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string accessToken = await GetAccessToken();

        if (accessToken != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetAccessToken()
    {
        if (_authClient.User == null)
        {
            return null;
        }

        return await _authClient.User.GetIdTokenAsync();
    }
}
