using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Newtonsoft.Json;
using Pet_Feeder_Machine_Problem.Adapters;
using Pet_Feeder_Machine_Problem.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pet_Feeder_Machine_Problem
{
    [Activity(Label = "DispenseSlots", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DispenseSlots : Activity
    {
        RecyclerView recyclerView;
        HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.dispenseSlotsActivity);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            RunOnUiThread(async () =>
            {
                await UpdateSlots();
            });
        }

        public async Task UpdateSlots()
        {
            client = new HttpClient();

            string url = RESTAPI.url() + $"getDispenseSlots.php";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<List<DispenseSlot>>(result);

                recyclerView.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(recyclerView.Context));
                DispenseSlotsAdapter adapter = new DispenseSlotsAdapter(responseObject);
                recyclerView.SetAdapter(adapter);
            }
            else
            {
                Toast.MakeText(this, "Error in fetching slots", ToastLength.Long).Show();
            }
        }
    }
}