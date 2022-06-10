using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Pet_Feeder_Machine_Problem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Pet_Feeder_Machine_Problem
{
    internal class DispenseLogAdapter : RecyclerView.Adapter
    {
        public event EventHandler<RecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<RecyclerViewAdapterClickEventArgs> ItemLongClick;
        List<LogRecord> items;

        public DispenseLogAdapter(List<LogRecord> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.dispenseLog, parent, false);
            
            var vh = new RecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            // Replace the contents of the view with that element
            var holder = viewHolder as RecyclerViewAdapterViewHolder;
            //holder.TextView.Text = items[position];

            string input = items[position].time;
            var timeFromInput = DateTime.ParseExact(input, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None);
            string timeIn12HourFormatForDisplay = timeFromInput.ToString("MM/dd/yyy hh:mm tt", CultureInfo.InvariantCulture);
            holder.time_TV.Text = timeIn12HourFormatForDisplay;

            holder.temperature_TV.Text = items[position].temperature;
            holder.humidity_TV.Text = items[position].humidity;
            holder.serving_TV.Text = items[position].serving;
            holder.mode_TV.Text = items[position].mode;
        }

        public override int ItemCount => items.Count;

        void OnClick(RecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(RecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class RecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView time_TV { get; set; }
        public TextView temperature_TV { get; set; }
        public TextView humidity_TV { get; set; }
        public TextView serving_TV { get; set; }
        public TextView mode_TV { get; set; }


        public RecyclerViewAdapterViewHolder(View itemView, Action<RecyclerViewAdapterClickEventArgs> clickListener,
                            Action<RecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            time_TV = (TextView)itemView.FindViewById(Resource.Id.tvTime);
            temperature_TV = (TextView)itemView.FindViewById(Resource.Id.tvTemp);
            humidity_TV = (TextView)itemView.FindViewById(Resource.Id.tvHum);
            serving_TV = (TextView)itemView.FindViewById(Resource.Id.tvServ);
            mode_TV = (TextView)itemView.FindViewById(Resource.Id.tvMode);

            itemView.Click += (sender, e) => clickListener(new RecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class RecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}