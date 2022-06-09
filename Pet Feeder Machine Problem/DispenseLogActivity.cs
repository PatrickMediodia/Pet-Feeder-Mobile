using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Newtonsoft.Json;
using Pet_Feeder_Machine_Problem.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pet_Feeder_Machine_Problem
{
    [Activity(Label = "DispenseLogActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DispenseLog : Activity
    {
        RecyclerView recyclerView;
        HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.dispenseLogActivity);
            
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);

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
                DispenseLogAdapter adapter = new DispenseLogAdapter(responseObject);
                recyclerView.SetAdapter(adapter);
            }
            else
            {
                Toast.MakeText(this, "Error in fetching log records", ToastLength.Long).Show();
            }
        }
    }
}