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
using System.Timers;

namespace Pet_Feeder_Machine_Problem
{
    [Activity(Label = "AccountManagement", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class AccountManagement : Activity
    {
        TextView usernameTxt;
        Button changeAccountDetailsBtn, changePasswordBtn;
        HttpClient client;
        Account accountObject;
        Timer RefreshDataTimer;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.account_management);

            usernameTxt = FindViewById<TextView>(Resource.Id.usernameTxt);
            changeAccountDetailsBtn = FindViewById<Button>(Resource.Id.changeAccountDetailsBtn);
            changePasswordBtn = FindViewById<Button>(Resource.Id.changePasswordBtn);
            
            changeAccountDetailsBtn.Click += ChangeAccountDetails;
            changePasswordBtn.Click += ChangePassword;

            RunOnUiThread(async () =>
            {
                await LoadUserAccount();
            });

            RefreshDataTimer = new Timer(1000);
            RefreshDataTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            RefreshDataTimer.AutoReset = true;
            RefreshDataTimer.Enabled = true;
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RunOnUiThread(async() =>
            {
                await LoadUserAccount();
            });
        }

        public async Task LoadUserAccount()
        {
            client = new HttpClient();

            string url = RESTAPI.url() + $"getAccountDetails.php";

            var response = await client.GetStringAsync(url);

            if (!response.Contains("No Account"))
            {
                accountObject = JsonConvert.DeserializeObject<Account>(response);
                usernameTxt.Text = $"Username: {accountObject.username}";
            }
            else 
            {
                Toast.MakeText(this, response, ToastLength.Long).Show();
            }
        }

        public void ChangeAccountDetails(object source, EventArgs e)
        {
            Intent i = new Intent(this, typeof(ChangeAccountDetails));
            StartActivity(i);
        }

        public void ChangePassword(object source, EventArgs e)
        {
            Intent i = new Intent(this, typeof(ChangePassword));
            StartActivity(i);
        }
    }
}