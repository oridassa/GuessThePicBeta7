using System.Threading;
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
using Xamarin.Essentials;
using Android.Service.Autofill;
using Android.Graphics;
using AndroidX.Browser.Trusted;
using Android.Media;
using System.IO;
//
using Java.IO;
using Java.Nio;
//


namespace GuessThePicBeta7
{
    [Activity(Label = "GameplayScreen")]
    public class GameplayScreen : Activity, View.IOnClickListener
    {
        private TextView roundcounter;
        private ImageView imageView;
        private GridLayout grid;

        private ProgressBar progressBar;
        private Handler handler = new Handler();

        private Button pick;

        private string playersArrayStr = "ori,noam,mori,tuval,shaked";
        List<string> playersArray;

        private Player currentPlayer = CurrentPlayer.playerPointer;

        private int timePoints;

        private Image image;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.gameplay_screen);

            this.roundcounter = FindViewById<TextView>(Resource.Id.roundcount);
            this.imageView = FindViewById<ImageView>(Resource.Id.img);
            this.grid = FindViewById<GridLayout>(Resource.Id.grid);
            this.progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            InitializeButtons();

            this.timePoints = 0;

            image = CurrentPlayer.tempImage;
            SetImage();

            // Start the progress bar update thread
            var thread = new Thread(UpdateProgressBar);
            thread.Start();
        }

        private void SetImage()
        {
            Bitmap bitmap = BitmapFactory.DecodeByteArray(image.ImageBytesData, 0, image.ImageBytesData.Length);

            imageView.SetImageBitmap(bitmap);
        }

        private void UpdateProgressBar()
        {
            for (int i = 0; i <= 1000; i++)
            {
                // Update progress on UI thread
                handler.Post(() => progressBar.Progress = i);

                // Sleep for a short duration to simulate progress
                Thread.Sleep(5);
                timePoints = i;
            }

            // After 5 seconds, post the function to be executed on the UI thread
            handler.Post(() => GameTime());
        }
        private void InitializeButtons()
        {
            playersArray = playersArrayStr.Split(",").ToList();

            foreach (string player in playersArray)
            {
                Button tempButton = new Button(this);
                tempButton.Text = player;
                tempButton.SetOnClickListener(this);
                grid.AddView(tempButton);
            }
        }
        private void GameTime()
        {
            //Intent intent = new Intent(this, typeof(GameLobbyHost));
            //base.StartActivity(intent);
            if (pick != null)
                if (pick.Text == image.sourcePlayer)
                    pick.SetBackgroundColor(Color.Green);
                else
                    pick.SetBackgroundColor(Color.Red);
            Toast.MakeText(this, $"{currentPlayer.points}", ToastLength.Short).Show();
            pick = null;
        }

        public void OnClick(View v)
        {
            Button pressedButton = (Button)v;
            if (pick == null)
            {
                pick = pressedButton;
                if(pick.Text == image.sourcePlayer)
                {
                    currentPlayer.AddPoints(timePoints);
                }
                pick.SetBackgroundColor(Color.DarkGray);
            }

        }
    }
}