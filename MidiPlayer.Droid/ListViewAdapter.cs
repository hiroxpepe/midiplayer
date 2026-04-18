/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Android.Content;
using Android.Views;
using Android.Widget;

using System.Collections.Generic;

namespace MidiPlayer.Droid {

    /// <summary>
    /// a data object for ListView. 
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class ListTitle {
        /// <summary>
        /// the MIDI track name.
        /// </summary>
        public string Name;
        /// <summary>
        /// the instrument (voice) name for this track.
        /// </summary>
        public string Instrument;
        /// <summary>
        /// the MIDI channel number as a display string.
        /// </summary>
        public string Channel;
    }

    /// <summary>
    /// an Adapter class for ListView. 
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class ListTitleAdapter : ArrayAdapter<ListTitle> {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// creates a new ListTitleAdapter.
        /// </summary>
        /// <param name="context">the Android Context used for layout inflation.</param>
        /// <param name="textViewResourceId">the resource ID of the row layout.</param>
        /// <param name="list">the list of ListTitle items to display.</param>
        public ListTitleAdapter(Context context, int textViewResourceId, IList<ListTitle> list)
            : base(context, textViewResourceId, list) {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// returns the view for the list row at the given position, using ViewHolder recycling.
        /// </summary>
        /// <param name="position">the zero-based position of the item in the list.</param>
        /// <param name="convertView">a recycled view to reuse, or null to inflate a new one.</param>
        /// <param name="parent">the parent ViewGroup that this view will be attached to.</param>
        /// <returns>the configured row View for the given position.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent) {
            ViewHolder viewHolder;
            if (convertView == null) {
                convertView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_title, parent, false);
                viewHolder = new();
                viewHolder.TextViewName = convertView.FindViewById<TextView>(Resource.Id.textview_title_name);
                viewHolder.TextViewInstrument = convertView.FindViewById<TextView>(Resource.Id.textview_title_instrument);
                viewHolder.TextViewChannel = convertView.FindViewById<TextView>(Resource.Id.textview_title_channel);
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
            /// <summary>
            /// the TextView that shows the track name.
            /// </summary>
            public TextView TextViewName;
            /// <summary>
            /// the TextView that shows the instrument name.
            /// </summary>
            public TextView TextViewInstrument;
            /// <summary>
            /// the TextView that shows the MIDI channel number.
            /// </summary>
            public TextView TextViewChannel;
        }
    }

    /// <summary>
    /// a data object for ListView. 
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class ListItem {
        /// <summary>
        /// whether this track is currently selected (checked) in the list.
        /// </summary>
        public bool Checked;
        /// <summary>
        /// the MIDI track name.
        /// </summary>
        public string Name;
        /// <summary>
        /// the instrument (voice) name for this track.
        /// </summary>
        public string Instrument;
        /// <summary>
        /// the MIDI channel number as a display string.
        /// </summary>
        public string Channel;
    }

    /// <summary>
    /// an Adapter class for ListView. 
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public class ListItemAdapter : ArrayAdapter<ListItem> {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// creates a new ListItemAdapter.
        /// </summary>
        /// <param name="context">the Android Context used for layout inflation.</param>
        /// <param name="textViewResourceId">the resource ID of the row layout.</param>
        /// <param name="list">the list of ListItem items to display.</param>
        public ListItemAdapter(Context context, int textViewResourceId,  IList<ListItem> list)
            : base(context, textViewResourceId, list) {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        /// <summary>
        /// returns the view for the list row at the given position, using ViewHolder recycling.
        /// </summary>
        /// <param name="position">the zero-based position of the item in the list.</param>
        /// <param name="convertView">a recycled view to reuse, or null to inflate a new one.</param>
        /// <param name="parent">the parent ViewGroup that this view will be attached to.</param>
        /// <returns>the configured row View for the given position.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent) {
            ViewHolder viewHolder;
            if (convertView == null) {
                convertView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item, parent, false);
                viewHolder = new();
                viewHolder.CheckBox = convertView.FindViewById<CheckBox>(Resource.Id.checkbox_item_select);
                viewHolder.TextViewName = convertView.FindViewById<TextView>(Resource.Id.textview_item_name);
                viewHolder.TextViewInstrument = convertView.FindViewById<TextView>(Resource.Id.textview_item_instrument);
                viewHolder.TextViewChannel = convertView.FindViewById<TextView>(Resource.Id.textview_item_channel);
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
            /// <summary>
            /// the CheckBox that indicates whether this track is selected.
            /// </summary>
            public CheckBox CheckBox;
            /// <summary>
            /// the TextView that shows the track name.
            /// </summary>
            public TextView TextViewName;
            /// <summary>
            /// the TextView that shows the instrument name.
            /// </summary>
            public TextView TextViewInstrument;
            /// <summary>
            /// the TextView that shows the MIDI channel number.
            /// </summary>
            public TextView TextViewChannel;
        }
    }

    /// <summary>
    /// type cast from Java.Lang.Object to native CLR type.
    /// </summary>
    /// <author>
    /// h.adachi (STUDIO MeowToon)
    /// </author>
    public static class ObjectTypeHelper {
        /// <summary>
        /// casts a Java.Lang.Object wrapper to the specified CLR type by reading the Instance property via reflection.
        /// </summary>
        /// <typeparam name="T">the target CLR type to cast to.</typeparam>
        /// <param name="obj">the Java.Lang.Object wrapper to cast.</param>
        /// <returns>the unwrapped CLR instance of type T, or null if the Instance property is not found.</returns>
        public static T Cast<T>(this Java.Lang.Object obj) where T : class {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }
    }
}
