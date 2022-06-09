using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pet_Feeder_Machine_Problem
{
    [Activity(Label = "ChangePassword", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ChangePassword : Activity
    {
        EditText oldPasswordTxt, newPasswordTxt, confirmNewPasswordTxt;
        Button changePasswordBtn;
        HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.change_password);

            oldPasswordTxt = FindViewById<EditText>(Resource.Id.oldPasswordTxt);
            newPasswordTxt = FindViewById<EditText>(Resource.Id.newPasswordTxt);
            confirmNewPasswordTxt = FindViewById<EditText>(Resource.Id.confirmNewPasswordTxt);

            changePasswordBtn = FindViewById<Button>(Resource.Id.changePasswordBtn);
            changePasswordBtn.Click += async (sender, e) => {
                await ChangeDetails(sender, e);
            };
        }

        public async Task ChangeDetails(object source, EventArgs e)
        {
            client = new HttpClient();

            string oldPassword = oldPasswordTxt.Text;
            string newPassword = newPasswordTxt.Text;
            string confirmNewPassword = confirmNewPasswordTxt.Text;

            if (newPassword != confirmNewPassword)
            {
                Toast.MakeText(this, "New Passwords does not match", ToastLength.Long).Show();
            }
            else 
            {
                string url = RESTAPI.url() + $"changePassword.php?oldPassword={oldPassword}&newPassword={newPassword}";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("Account Password Updated"))
                    {
                        Finish();
                    }
                    Toast.MakeText(this, result, ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Error", ToastLength.Long).Show();
                }
            }
        }
    }
}