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
        Button dispeseBtn, dispenseLogBtn;
        HttpClient client;
        public Timer RefreshDataTimer;
        RecyclerView recyclerView;
        List<LogRecord> logRecords;

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

            /*RunOnUiThread(async () =>
            {
                await UpdateStatus();
            });

            RefreshDataTimer = new Timer(1000);
            RefreshDataTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            RefreshDataTimer.AutoReset = true;
            RefreshDataTimer.Enabled = true;

            dispeseBtn.Click += async (sender, e) =>
            {
                await ManualDispense(sender, e);
            };*/

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            dispenseLogBtn.Click += this.DispenseLog;

        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            await UpdateStatus();
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

        public void DispenseLog(object source, EventArgs e)
        {
            /*Intent i = new Intent(this, typeof(DispenseLog));
            string username = Intent.GetStringExtra("username");
            i.PutExtra("username", username);
            StartActivity(i);*/

            logRecords = new List<LogRecord>();
            logRecords.Add(new LogRecord { time = "22:51:12", temperature = "33.0 C", humidity = "60%", serving = "Large", mode = "Manual" });
            logRecords.Add(new LogRecord { time = "23:51:12", temperature = "22.0 C", humidity = "63%", serving = "Medium", mode = "Manual" });
            logRecords.Add(new LogRecord { time = "00:51:12", temperature = "25.0 C", humidity = "56%", serving = "Large", mode = "Auto" });
            logRecords.Add(new LogRecord { time = "09:51:12", temperature = "26.0 C", humidity = "58%", serving = "Large", mode = "Manual" });
            logRecords.Add(new LogRecord { time = "07:51:12", temperature = "24.0 C", humidity = "61%", serving = "Small", mode = "Auto" });
            recyclerView.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(recyclerView.Context));
            RecyclerViewAdapter adapter = new RecyclerViewAdapter(logRecords);
            recyclerView.SetAdapter(adapter);
            //SetUpRecyclerView();
        }

        private void SetUpRecyclerView()
        {
        }
    }
}