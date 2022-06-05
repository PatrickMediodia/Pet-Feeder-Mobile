using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pet_Feeder_Machine_Problem
{
    public class RESTAPI
    {
        static string IPAddress = "192.168.1.2";
        static string directory = "PetFeeder";

        public static string url() 
        {
            return $"http://{IPAddress}/{directory}/";
        }
    }
}