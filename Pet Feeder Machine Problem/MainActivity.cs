using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using Pet_Feeder_Machine_Problem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pet_Feeder_Machine_Problem
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText usernameTxt, passwordTxt;
        Button loginBtn;
        HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            usernameTxt = FindViewById<EditText>(Resource.Id.usernameTxt);
            passwordTxt = FindViewById<EditText>(Resource.Id.passwordTxt);

            loginBtn = FindViewById<Button>(Resource.Id.loginBtn);
            loginBtn.Click += async (sender, e) => {
                await Login(sender, e);
            };
        }

        public async Task Login(object sender, EventArgs e)
        {
            client = new HttpClient();

            string username = usernameTxt.Text;
            string password = passwordTxt.Text;

            string url = RESTAPI.url() + $"login.php?username={username}&password={password}";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (!result.Contains("Incorrect Credentials"))
                {
                    Intent i = new Intent(this, typeof(Dashboard));
                    i.PutExtra("account", result);
                    StartActivity(i);
                    Toast.MakeText(this, "Logged In", ToastLength.Long).Show();
                }
                else 
                {
                    Toast.MakeText(this, result, ToastLength.Long).Show();
                }
                
            }
            else
            {
                Toast.MakeText(this, "Error", ToastLength.Long).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}