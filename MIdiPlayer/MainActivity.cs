
using Android.App;
using Android.Media;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.IO;
using System.Linq;

namespace MidiPlayer {

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public MainActivity() {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnOpenButton_Click(object sender, System.EventArgs e) {
            Log.Info("openButton clicked.");
        }

        public void OnStartButton_Click(object sender, System.EventArgs e) {
            Log.Info("startButton clicked.");
            Player.Start();
        }

        public void OnStopButton_Click(object sender, System.EventArgs e) {
            Log.Info("stopButton clicked.");
            Player.Stop();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // protected Methods [verb]

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            initializeComponent();

            // MIDIファイルの一覧を取得
            var _di = new DirectoryInfo($"/storage/emulated/0/Music/MIDI"); // TODO: 選択出来るように、SDカードを取得するには？
            var _filePathList = _di.GetFiles()
                .Where(x => x.Name.EndsWith(".MID") || x.Name.EndsWith(".mid"))
                .OrderBy(x => x.CreationTime)
                .ToList();

            Player.Target = _filePathList[0].FullName;
        }

        protected override void OnStop() {
            base.OnStop();
            Player.Stop();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        /// <summary>
        /// コンポーネントを初期化します
        /// </summary>
        void initializeComponent() {
            Button _openButton = FindViewById<Button>(Resource.Id.openButton);
            _openButton.Click += OnOpenButton_Click;

            Button _startButton = FindViewById<Button>(Resource.Id.startButton);
            _startButton.Click += OnStartButton_Click;

            Button _stopButton = FindViewById<Button>(Resource.Id.stopButton);
            _stopButton.Click += OnStopButton_Click;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class Player {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Fields

            static string target;

            static MediaPlayer mediaPlayer = null;

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            static Player() {
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjective] 

            public static string Target {
                set => target = value;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // public Methods [verb]

            /// <summary>
            /// MIDIファイル再生
            /// </summary>
            public static void Start() {
                mediaPlayer = new MediaPlayer();
                mediaPlayer.SetDataSource(target);
                mediaPlayer.Prepare();
                mediaPlayer.Looping = true;
                mediaPlayer.Start();
                Log.Info("start.");
            }

            public static void Stop() {
                mediaPlayer.Stop();
                mediaPlayer.Release();
                mediaPlayer = null;
                Log.Info("stop.");
            }
        }
    }
}