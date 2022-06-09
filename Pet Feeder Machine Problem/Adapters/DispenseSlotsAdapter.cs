using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using Pet_Feeder_Machine_Problem.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Linq;
using Android.Content;
using System.Threading.Tasks;

namespace Pet_Feeder_Machine_Problem.Adapters
{
    internal class DispenseSlotsAdapter : RecyclerView.Adapter, View.IOnClickListener
    {
        public event EventHandler<DispenseSlotsAdapterClickEventArgs> ItemClick;
        public event EventHandler<DispenseSlotsAdapterClickEventArgs> ItemLongClick;
        
        Context _context;
        List<DispenseSlot> items;
        HttpClient client;
        
        public DispenseSlotsAdapter(List<DispenseSlot> data, Context context)
        {
            items = data;
            _context = context;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.dispenseSlots, parent, false);

            var vh = new DispenseSlotsAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            // Replace the contents of the view with that element
            var holder = viewHolder as DispenseSlotsAdapterViewHolder;
            //holder.TextView.Text = items[position];
            holder.time_TV.Text = items[position].dispenseTime;
            holder.serving_TV.Text = items[position].serving;

            holder.imageDel_TV.SetOnClickListener(this);
            holder.imageDel_TV.Tag = position;
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.imageDel)
            {
                client = new HttpClient();
                string url = RESTAPI.url() + $"deleteDispenseSlot.php?dispenseTime={items[Convert.ToInt32(v.Tag.ToString())].dispenseTime}";

                var task = Task.Run(() => client.GetStringAsync(url));
                task.Wait();
                var response = task.Result;
                Toast.MakeText(this._context, response, ToastLength.Short).Show();
            }
        }

        public void RefreshItems(List<DispenseSlot> newItems)
        {
            items.Clear();
            items = newItems;
            NotifyDataSetChanged();
        }

        public override int ItemCount => items.Count;

        void OnClick(DispenseSlotsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(DispenseSlotsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
    }

    public class DispenseSlotsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView time_TV { get; set; }
        public TextView serving_TV { get; set; }
        public ImageButton imageDel_TV { get; set; }

        public DispenseSlotsAdapterViewHolder(View itemView, Action<DispenseSlotsAdapterClickEventArgs> clickListener,
                            Action<DispenseSlotsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            time_TV = (TextView)itemView.FindViewById(Resource.Id.tvTime_DispenseSlot);
            serving_TV = (TextView)itemView.FindViewById(Resource.Id.tvServing_DispenseSlot);
            imageDel_TV = (ImageButton)itemView.FindViewById(Resource.Id.imageDel);

            itemView.Click += (sender, e) => clickListener(new DispenseSlotsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new DispenseSlotsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class DispenseSlotsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}