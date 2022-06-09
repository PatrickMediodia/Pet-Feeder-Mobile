using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Pet_Feeder_Machine_Problem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pet_Feeder_Machine_Problem
{
    [Activity(Label = "ChangeAccountDetails", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ChangeAccountDetails : Activity
    {
        EditText usernameTxt, nameTxt;
        Button changeAccountDetailsBtn;
        HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.change_account_details);

            usernameTxt = FindViewById<EditText>(Resource.Id.usernameTxt);
            nameTxt = FindViewById<EditText>(Resource.Id.nameTxt);

            changeAccountDetailsBtn = FindViewById<Button>(Resource.Id.changeAccountDetailsBtn);
            changeAccountDetailsBtn.Click += async (sender, e) => {
                await ChangeDetails(sender, e);
            };

            LoadUserAccount();
        }

        public void LoadUserAccount()
        {
            client = new HttpClient();

            string url = RESTAPI.url() + $"getAccountDetails.php";

            var task = Task.Run(() => client.GetStringAsync(url));
            task.Wait();
            var response = task.Result;

            if (!response.Contains("No Account"))
            {
                var accountObject = JsonConvert.DeserializeObject<Account>(response);
                usernameTxt.Text = accountObject.username;
                nameTxt.Text = accountObject.name;
            }
            else
            {
                Toast.MakeText(this, response, ToastLength.Long).Show();
            }
        }

        public async Task ChangeDetails(object source, EventArgs e)
        {
            client = new HttpClient();

            string username = usernameTxt.Text;
            string name = nameTxt.Text;

            string url = RESTAPI.url() + $"changeAccountDetails.php?username={username}&name={name}";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                Toast.MakeText(this, result, ToastLength.Long).Show();
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Error", ToastLength.Long).Show();
            }
        }
    }
}