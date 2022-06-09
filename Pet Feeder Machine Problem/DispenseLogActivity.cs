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
    [Activity(Label = "DispenseLogActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DispenseLogActivity : Activity
    {
        RecyclerView recyclerView;
        //List<LogRecord> logRecords;
        HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.dispenseLogActivity);
            
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);

    /*      logRecords = new List<LogRecord>();
            logRecords.Add(new LogRecord { time = "22:51:12", temperature = "33.0 C", humidity = "60%", serving = "Large", mode = "Manual" });
            logRecords.Add(new LogRecord { time = "23:51:12", temperature = "22.0 C", humidity = "63%", serving = "Medium", mode = "Manual" });
            logRecords.Add(new LogRecord { time = "00:51:12", temperature = "25.0 C", humidity = "56%", serving = "Large", mode = "Auto" });
            logRecords.Add(new LogRecord { time = "09:51:12", temperature = "26.0 C", humidity = "58%", serving = "Large", mode = "Manual" });
            logRecords.Add(new LogRecord { time = "07:51:12", temperature = "24.0 C", humidity = "61%", serving = "Small", mode = "Auto" });
    */
            RunOnUiThread(async () =>
            {
                await UpdateLog();
            });
        }

        public async Task UpdateLog()
        {
            client = new HttpClient();

            string url = RESTAPI.url() + $"getDispenseLogs.php";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<List<LogRecord>>(result);

                recyclerView.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(recyclerView.Context));
                RecyclerViewAdapter adapter = new RecyclerViewAdapter(responseObject);
                recyclerView.SetAdapter(adapter);
            }
            else
            {
                Toast.MakeText(this, "Error in fetching log records", ToastLength.Long).Show();
            }
        }
    }
}