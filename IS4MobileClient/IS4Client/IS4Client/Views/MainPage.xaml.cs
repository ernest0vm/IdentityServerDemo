using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IS4Client.Views;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using System.Net.Http;
using System.Linq;
using Xamarin.Essentials;

namespace IS4Client.Views
{
    public partial class MainPage : ContentPage
    {
        public static Account AuthAccount { get; set; }
        public static HttpClient Client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }
        
        private async void Presenter_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                AuthAccount = e.Account;
                Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthAccount.Properties["access_token"]}");
                //    await AccountStore.Create().SaveAsync(e.Account, "AuthServer");
                await Navigation.PushAsync(new ValuesPage());
            }
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            var oAuth = new OAuth2AuthenticatorEx("xamarin-client", "offline_access values-api",
                new Uri(EndPoints.IS4domain + "/connect/authorize"), new Uri(EndPoints.IS4domain + "/grants"))
            {
                AccessTokenUrl = new Uri(EndPoints.IS4domain + "/connect/token"),
                ShouldEncounterOnPageLoading = false
            };

            var account = await SecureStorageAccountStore.FindAccountsForServiceAsync("AuthServer");
            if (account != null && account.Any())
            {
                AuthAccount = account.First();
                Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthAccount.Properties["access_token"]}");
                await Navigation.PushAsync(new ValuesPage());
            }
            else
            {
                var presenter = new OAuthLoginPresenter();
                presenter.Completed += Presenter_Completed;
                presenter.Login(oAuth);
            }
        }
    }
}