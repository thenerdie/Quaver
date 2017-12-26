﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Quaver.Audio;
using Quaver.Beatmaps;
using Quaver.Commands;
using Quaver.Config;
using Quaver.Database;
using Quaver.Discord;
using Quaver.Logging;
using Quaver.Replays;
using Quaver.Tests;

namespace Quaver
{
    /// <summary>
    ///     The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Initialize Config
            Configuration.InitializeConfig();

            // Initialize Discord RichPresence
            InitializeDiscordPresence();

            // Delete Temp Files
            DeleteTemporaryFiles();
 
            // Set up the game
            SetupGame();
      
            // Start game
            using (var game = new QuaverGame())
            {
                game.Run();
            }
        }

        /// <summary>
        ///     Responsible for initializing the Discord Presence
        /// </summary>
        private static void InitializeDiscordPresence()
        {
            if (GameBase.DiscordController != null)
                return;

            try
            {
                GameBase.DiscordController = new DiscordController();
                GameBase.DiscordController.Initialize();
                GameBase.DiscordRichPresencedInited = true;
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, Color.Red);
            }
        }

        /// <summary>
        ///     Deletes all temporary files if there are any.
        /// </summary>
        private static void DeleteTemporaryFiles()
        {
            try
            {
                foreach (var file in new DirectoryInfo(Configuration.DataDirectory + "/temp/").GetFiles("*", SearchOption.AllDirectories))
                    file.Delete();

                foreach (var dir in new DirectoryInfo(Configuration.DataDirectory + "/temp/").GetDirectories("*", SearchOption.AllDirectories))
                    dir.Delete(true);
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, Color.Red);
            }
        }

        /// <summary>
        ///     Responsible for initializing and setting the beatmap database and setting the loaded beatmaps
        /// </summary>
        private static void SetupGame()
        {
            // Set the build version
            GameBase.BuildVersion = BeatmapUtils.GetMd5Checksum(Configuration.GameDirectory + "/" + "Quaver.exe");

            // After initializing the configuration, we want to sync the beatmap database, and load the dictionary of beatmaps.
            var loadGame = Task.Run(async () =>
            {
                await GameBase.LoadAndSetBeatmaps();

                // Create the local scores database if it doesn't already exist
                await LocalScoreCache.CreateScoresDatabase();

                // The visible beatmaps in song select should be every single mapset at the start of the game.
                GameBase.VisibleMapsets = GameBase.Mapsets;
            });

            Task.WaitAll(loadGame);
        }
    }
}