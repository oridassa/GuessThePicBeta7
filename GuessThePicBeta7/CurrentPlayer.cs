﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace GuessThePicBeta7
{
    public static class CurrentPlayer
    {
        public static Player playerPointer;
        public static string name;
        //
        public static Image tempImage;
        //

        public static void CreatePlayer(Player player)
        {
            playerPointer = player;
            name = player.name;
        }
        public static void InitiatePlayer(string name, bool isAdmin)
        {
            if (playerPointer == null)
            {
                Player player = new Player(name, isAdmin);
                name = "a";
                CurrentPlayer.CreatePlayer(player);
            }
        }
    }
}