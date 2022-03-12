
using Android.Content;
using Android.Views;
using Android.Widget;

using System.Collections.Generic;

namespace MidiPlayer.Droid {

    /// <summary>
    /// a data object for ListView. 
    /// </summary>
    public class ListTitle {
        public string Name;
        public string Instrument;
        public string Channel;
    }

    /// <summary>
    /// an Adapter class for ListView. 
    /// </summary>
    public class ListTitleAdapter : ArrayAdapter<ListTitle> {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public ListTitleAdapter(Context context, int textViewResourceId, IList<ListTitle> list)
            : base(context, textViewResourceId, list) {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public override View GetView(int position, View convertView, ViewGroup parent) {
            ViewHolder viewHolder;
            if (convertView == null) {
                convertView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_title, parent, false);
                viewHolder = new();
                viewHolder.TextViewName = (TextView) convertView.FindViewById(Resource.Id.text_view_title_name);
                viewHolder.TextViewInstrument = (TextView) convertView.FindViewById(Resource.Id.text_view_title_instrument);
                viewHolder.TextViewChannel = (TextView) convertView.FindViewById(Resource.Id.text_view_title_channel);
                convertView.SetTag(Resource.String.view_holder_tag, viewHolder);
            } else {
                viewHolder = (ViewHolder) convertView.GetTag(Resource.String.view_holder_tag);
            }
            ListTitle listTitle = GetItem(position);
            viewHolder.TextViewName.Text = listTitle.Name;
            viewHolder.TextViewInstrument.Text = listTitle.Instrument;
            viewHolder.TextViewChannel.Text = listTitle.Channel;
            return convertView;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        /// <summary>
        /// a ViewHolder class.
        /// </summary>
        class ViewHolder : Java.Lang.Object {
            public TextView TextViewName;
            public TextView TextViewInstrument;
            public TextView TextViewChannel;
        }
    }

    /// <summary>
    /// a data object for ListView. 
    /// </summary>
    public class ListItem {
        public bool Checked;
        public string Name;
        public string Instrument;
        public string Channel;
    }

    /// <summary>
    /// an Adapter class for ListView. 
    /// </summary>
    public class ListItemAdapter : ArrayAdapter<ListItem> {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public ListItemAdapter(Context context, int textViewResourceId,  IList<ListItem> list)
            : base(context, textViewResourceId, list) {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public override View GetView(int position, View convertView, ViewGroup parent) {
            ViewHolder viewHolder;
            if (convertView == null) {
                convertView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item, parent, false);
                viewHolder = new();
                viewHolder.CheckBox = (CheckBox) convertView.FindViewById(Resource.Id.checkbox_item_select);
                viewHolder.TextViewName = (TextView) convertView.FindViewById(Resource.Id.text_view_item_name);
                viewHolder.TextViewInstrument = (TextView) convertView.FindViewById(Resource.Id.text_view_item_instrument);
                viewHolder.TextViewChannel = (TextView) convertView.FindViewById(Resource.Id.text_view_item_channel);
                convertView.SetTag(Resource.String.view_holder_tag, viewHolder);
            } else {
                viewHolder = (ViewHolder) convertView.GetTag(Resource.String.view_holder_tag);
            }
            ListItem listItem = GetItem(position);
            viewHolder.CheckBox.Checked = listItem.Checked;
            viewHolder.TextViewName.Text = listItem.Name;
            viewHolder.TextViewInstrument.Text = listItem.Instrument;
            viewHolder.TextViewChannel.Text = listItem.Channel;
            NotifyDataSetChanged();
            return convertView;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        /// <summary>
        /// a ViewHolder class.
        /// </summary>
        class ViewHolder : Java.Lang.Object {
            public CheckBox CheckBox;
            public TextView TextViewName;
            public TextView TextViewInstrument;
            public TextView TextViewChannel;
        }
    }

    /// <summary>
    /// type cast from Java.Lang.Object to native CLR type.
    /// </summary>
    public static class ObjectTypeHelper {
        public static T Cast<T>(this Java.Lang.Object obj) where T : class {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }
    }
}
