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
    [Activity(Label = "Add Dispense Slot", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class AddSlots : Activity
    {
        TextView servingTxt;
        TimePicker timepicker;
        Button addBtn;
        HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.addslots);

            servingTxt = FindViewById<TextView>(Resource.Id.servingsTxt);
            timepicker = FindViewById<TimePicker>(Resource.Id.timePicker1);
            addBtn = FindViewById<Button>(Resource.Id.btnAddSlot);

            //timepicker.SetIs24HourView(Java.Lang.Boolean.True);

            addBtn.Click += async (sender, e) => {
                await AddSlot(sender, e);
            };
        }

        public async Task AddSlot(object sender, EventArgs e)
        {
            client = new HttpClient();

            string time = $"{timepicker.Hour}:{timepicker.Minute}:00";
            string serving = servingTxt.Text;

            if (serving != "")
            {
                string url = RESTAPI.url() + $"addDispenseSlot.php?dispenseTime={time}&serving={serving}";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result.Contains("Dispense Slot Added"))
                    {
                        Intent i = new Intent(this, typeof(Dashboard));
                        StartActivity(i);
                        Toast.MakeText(this, result, ToastLength.Short).Show();
                    }
                    Toast.MakeText(this, result, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Error", ToastLength.Short).Show();
                }
            }
            else 
            {
                Toast.MakeText(this, "Serving must not be empty", ToastLength.Short).Show();
            }

        }
    }
}