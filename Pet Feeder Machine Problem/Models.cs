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

namespace Pet_Feeder_Machine_Problem.Models
{
    public class EnvironmentAndSupply
    {
        public string temperature { get; set; }
        public string humidity { get; set; }
        public string foodLevel { get; set; }
        public string waterLevel { get; set; }
        public string timestamp { get; set; }
    }
}