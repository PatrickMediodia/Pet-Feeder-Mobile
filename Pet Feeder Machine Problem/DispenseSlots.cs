using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Newtonsoft.Json;
using Pet_Feeder_Machine_Problem.Adapters;
using Pet_Feeder_Machine_Problem.Models;
using System;
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
        DispenseSlotsAdapter adapter;
        Button addSlotBtn;
        TextView dispenseMsgTxt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.dispenseSlotsActivity);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.dispenseSlotsRecView);
            addSlotBtn = FindViewById<Button>(Resource.Id.btnAddSlot);
            dispenseMsgTxt = FindViewById<TextView>(Resource.Id.dispenseMsgTxt);

            RunOnUiThread(async () =>
            {
                await UpdateSlots();
            });

            ChangedData();
            addSlotBtn.Click += AddSlot;
        }

        public void AddSlot(object source, EventArgs e)
        {
            Intent i = new Intent(this, typeof(AddSlots));
            StartActivity(i);
        }

        public async Task UpdateSlots()
        {
            client = new HttpClient();

            string url = RESTAPI.url() + $"getDispenseSlots.php";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (result.Contains("No slots found"))
                {
                    dispenseMsgTxt.Visibility = Android.Views.ViewStates.Visible;
                    recyclerView.Visibility = Android.Views.ViewStates.Gone;
                }
                else 
                {
                    dispenseMsgTxt.Visibility = Android.Views.ViewStates.Gone;
                    recyclerView.Visibility = Android.Views.ViewStates.Visible;

                    var responseObject = JsonConvert.DeserializeObject<List<DispenseSlot>>(result);
                    recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
                    adapter = new DispenseSlotsAdapter(responseObject, this);
                    recyclerView.SetAdapter(adapter);
                }
            }
            else
            {
                Toast.MakeText(this, "Error in fetching slots", ToastLength.Short).Show();
            }
        }

        public async void ChangedData()
        {
            await Task.Delay(500).ContinueWith(async t =>
            {
                client = new HttpClient();

                string url = RESTAPI.url() + $"getDispenseSlots.php";

                HttpResponseMessage response = await client.GetAsync(url);

                var result = await response.Content.ReadAsStringAsync();

                if (result.Contains("No slots found"))
                {
                    dispenseMsgTxt.Visibility = Android.Views.ViewStates.Visible;
                    recyclerView.Visibility = Android.Views.ViewStates.Gone;
                }
                else
                {
                    dispenseMsgTxt.Visibility = Android.Views.ViewStates.Gone;
                    recyclerView.Visibility = Android.Views.ViewStates.Visible;

                    var newData = JsonConvert.DeserializeObject<List<DispenseSlot>>(result);
                    adapter.RefreshItems(newData);
                }

                ChangedData();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}