
using Android.App;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Linq;

namespace MIdiPlayer {

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        MediaPlayer mediaPlayer = null;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainActivity() {
            mediaPlayer = new MediaPlayer();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public override bool OnCreateOptionsMenu(IMenu menu) {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item) {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings) {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // protected Methods [verb]

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var _di = new DirectoryInfo($"/storage/emulated/0/Music/MIDI"); // TODO: 選択出来るように、SDカードを取得するには？

            // MIDIファイルの一覧を取得
            var _filePathList = _di.GetFiles()
                .Where(x => x.Name.EndsWith(".MID") || x.Name.EndsWith(".mid"))
                .OrderBy(x => x.CreationTime)
                .ToList();

            // MIDIファイル再生
            mediaPlayer.SetDataSource(_filePathList[0].FullName);
            mediaPlayer.Prepare();
            mediaPlayer.Start();
        }

        protected override void OnStop() {
            mediaPlayer.Stop();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]
    }
}
