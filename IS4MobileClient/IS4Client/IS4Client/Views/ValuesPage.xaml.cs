using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IS4Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValuesPage : ContentPage
    {
        public class Value
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        protected override async void OnAppearing()
        {
            var dataRequest = await MainPage.Client.GetAsync(EndPoints.APIdomain + "/api/values");
            if (dataRequest.IsSuccessStatusCode)
            {
                var resultString = await dataRequest.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<List<Value>>(resultString);
                MyListView.ItemsSource = resultObject;
            }

            if (dataRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("", "Unauthorized", "OK");
            }
        }

        public ValuesPage()
        {
            InitializeComponent();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
