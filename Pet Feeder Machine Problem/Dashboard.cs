using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
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
    [Activity(Label = "Dashboard", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class Dashboard : Activity
    {
        TextView temperatureTxt, humidityTxt, foodTxt, waterTxt, timestampTxt;
        Button dispeseBtn, dispenseLogBtn, addSlotBtn, accountManagementBtn;
        HttpClient client;
        Timer RefreshDataTimer;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.dashboard);

            temperatureTxt = FindViewById<TextView>(Resource.Id.temperatureTxt);
            humidityTxt = FindViewById<TextView>(Resource.Id.humidityTxt);
            foodTxt = FindViewById<TextView>(Resource.Id.foodTxt);
            waterTxt = FindViewById<TextView>(Resource.Id.waterTxt);
            timestampTxt = FindViewById<TextView>(Resource.Id.timestampTxt);
            
            dispeseBtn = FindViewById<Button>(Resource.Id.dispenseBtn);
            dispenseLogBtn = FindViewById<Button>(Resource.Id.btnDispenseLog);
            addSlotBtn = FindViewById<Button>(Resource.Id.btnAddSlot);
            accountManagementBtn = FindViewById<Button>(Resource.Id.accountManagementBtn);

            RunOnUiThread(async () =>
            {
                await UpdateStatus();
            });

            RefreshDataTimer = new Timer(1000);
            RefreshDataTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            RefreshDataTimer.AutoReset = true;
            RefreshDataTimer.Enabled = true;

            dispenseLogBtn.Click += DispenseLog;
            dispeseBtn.Click += ManualDispense;
            addSlotBtn.Click += AddSlot;
            accountManagementBtn.Click += AccountManagement;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RunOnUiThread(async () =>
            {
                await UpdateStatus();
            });
        }

        public async Task UpdateStatus() 
        {
            client = new HttpClient();

            string url = RESTAPI.url() + $"getEnvironmentAndSupplyStatus.php";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<EnvironmentAndSupply>(result);

                temperatureTxt.Text = responseObject.temperature;
                humidityTxt.Text = responseObject.humidity;
                foodTxt.Text = responseObject.foodLevel;
                waterTxt.Text = responseObject.waterLevel;
                timestampTxt.Text = responseObject.timestamp;
            }
            else
            {
                Toast.MakeText(this, "Error", ToastLength.Long).Show();
            }
        }

        public void ManualDispense(object source, EventArgs e)
        {
            Intent i = new Intent(this, typeof(Dispense));
            StartActivity(i);
        }

        public void DispenseLog(object source, EventArgs e)
        {
            Intent i = new Intent(this, typeof(DispenseLogActivity));
            StartActivity(i);
        }

        public void AddSlot(object source, EventArgs e)
        {
            Intent i = new Intent(this, typeof(AddSlots));
            StartActivity(i);
        }
        public void AccountManagement(object source, EventArgs e)
        {
            Intent i = new Intent(this, typeof(AccountManagement));
            StartActivity(i);
        }
    }
}