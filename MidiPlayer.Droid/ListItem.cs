
using Android.Content;
using Android.Views;
using Android.Widget;

using System.Collections.Generic;

namespace MidiPlayer.Droid {

    public class ListItem {
        public string Name;
        public string Instrument;
    }

    public class ListItemAdapter : ArrayAdapter<ListItem> {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        private readonly LayoutInflater _layoutInflater;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public ListItemAdapter(Context context, int textViewResourceId,  IList<ListItem> list)
            : base(context, textViewResourceId, list) {
            _layoutInflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public override View GetView(int position, View convertView, ViewGroup parent) {
            var item = GetItem(position);
            var view = _layoutInflater.Inflate(Resource.Layout.list_item, null);
            var textViewName = view.FindViewById<TextView>(Resource.Id.text_view_name);
            textViewName.Text = item.Name;
            var textViewInstrument = view.FindViewById<TextView>(Resource.Id.text_view_instrument);
            textViewInstrument.Text = item.Instrument;
            return view;
        }
    }

    public class ListTitle {
        public string Name;
        public string Instrument;
    }

    public class ListTitleAdapter : ArrayAdapter<ListTitle> {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        private readonly LayoutInflater _layoutInflater;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public ListTitleAdapter(Context context, int textViewResourceId, IList<ListTitle> list)
            : base(context, textViewResourceId, list) {
            _layoutInflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public override View GetView(int position, View convertView, ViewGroup parent) {
            var item = GetItem(position);
            var view = _layoutInflater.Inflate(Resource.Layout.list_title, null);
            var textViewName = view.FindViewById<TextView>(Resource.Id.text_view_name);
            textViewName.Text = item.Name;
            var textViewInstrument = view.FindViewById<TextView>(Resource.Id.text_view_instrument);
            textViewInstrument.Text = item.Instrument;
            return view;
        }
    }
}
