using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using Xamarin.Essentials;

namespace GuessThePicBeta7
{
    [Activity(Label = "GameLobbyHost")]
    public class GameLobbyHost : Activity, View.IOnClickListener
    {
        private ListView listView;
        private TextView gameidview;
        private Button startGameButton;
        private GameInitiator gameInitiator;

        private Player currentPlayer;
        private string gameid;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.game_lobby_host);

            this.currentPlayer = CurrentPlayer.playerPointer;

            listView = FindViewById<ListView>(Resource.Id.list123);
            gameidview = FindViewById<TextView>(Resource.Id.gameidview);
            startGameButton = FindViewById<Button>(Resource.Id.startGameButton);

            startGameButton.Visibility = DetermineIfHost();

            string[] arr = "just,a,test".Split(',');
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, arr);
            listView.Adapter = adapter;
        }

        public override void OnRequestPermissionsResult(int requestCode,
            string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnClick(View v)
        {
            Intent intent;
            Button b = (Button)v;
            if (b.Text == "Quit to main menu")
            {
                //EndGame();
                intent = new Intent(this, typeof(MainActivity));
                base.StartActivity(intent);
            }
            else if (b.Text == "Insert photos")
            {
                //player pressed "Insert photos"
                UploadPictures();

            }
            else if (b.Text == "Start Game")
            {
                //host wants to start the game
                intent = new Intent(this, typeof(GameplayScreen));
                base.StartActivity(intent);
            }
        }
        private async void UploadPictures()
        {   
            byte[] bytes;

            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Upload a picture for the game!"
                });
                var stream = await result.OpenReadAsync();
                var mstream = new MemoryStream();
                stream.CopyTo(mstream);
                bytes = mstream.ToArray();

                CurrentPlayer.tempImage = new Image(bytes, "noam");
            }
            catch (Exception e)
            {
                Toast.MakeText(this, e.ToString(), ToastLength.Short).Show();
            }



        }

        private ViewStates DetermineIfHost()//if player is host there will be a button to start the game, else, there would not be
        {
            if (currentPlayer.isAdmin)
                return ViewStates.Visible;
            return ViewStates.Gone;
        }

        private void SetPlayerListView()
        {

        }
    }
}