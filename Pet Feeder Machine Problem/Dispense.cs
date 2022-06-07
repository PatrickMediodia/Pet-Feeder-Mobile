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
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Pet_Feeder_Machine_Problem
{
    [Activity(Label = "Dispense", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class Dispense : Activity
    {
        TextView timeTxt;
        Button dispenseManualBtn;
        HttpClient client;
   
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.Dispense);

            dispenseManualBtn = FindViewById<Button>(Resource.Id.dispenseBtn);
            timeTxt = FindViewById<TextView>(Resource.Id.timestampTxt);

            dispenseManualBtn.Click += async (sender, e) => {
                await ManualDispense(sender, e);
            };

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();

            
        }
        private void TimerElapsed(Object source, ElapsedEventArgs e)
        {
            timeTxt.Text = e.SignalTime.ToString();
        }

        public async Task ManualDispense(object source, EventArgs e)
        {
            client = new HttpClient();

            string url = RESTAPI.url() + $"dispenseFood.php";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                Toast.MakeText(this, result, ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "Error", ToastLength.Long).Show();
            }
        }

    }
}