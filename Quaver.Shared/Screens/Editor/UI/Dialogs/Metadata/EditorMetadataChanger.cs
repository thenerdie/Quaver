﻿/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) 2017-2019 Swan & The Quaver Team <support@quavergame.com>.
*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Quaver.API.Enums;
using Quaver.Shared.Assets;
using Quaver.Shared.Helpers;
using Quaver.Shared.Screens.Menu.UI.Jukebox;
using Quaver.Shared.Screens.Menu.UI.Navigation.User;
using Quaver.Shared.Screens.Settings.Elements;
using Wobble.Graphics;
using Wobble.Graphics.Animations;
using Wobble.Graphics.Sprites;
using Wobble.Graphics.UI.Buttons;
using Wobble.Graphics.UI.Dialogs;
using Wobble.Logging;

namespace Quaver.Shared.Screens.Editor.UI.Dialogs.Metadata
{
    public class EditorMetadataChanger : Sprite
    {
        /// <summary>
        /// </summary>
        public EditorMetadataDialog Dialog { get; }

        /// <summary>
        /// </summary>
        public Sprite HeaderBackground { get; private set; }

        /// <summary>
        /// </summary>
        public Sprite FooterBackground { get; private set; }

        /// <summary>
        /// </summary>
        private Button OkButton { get; set; }

        /// <summary>
        /// </summary>
        private Button CancelButton { get; set; }

        /// <summary>
        /// </summary>
        public EditorMetadataScrollContainer Container { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public EditorMetadataChanger(EditorMetadataDialog dialog)
        {
            Dialog = dialog;
            Size = new ScalableVector2(400, 502);
            Tint = ColorHelper.HexToColor($"#414345");
            Alpha = 1f;

            //CreateBorderLines();
            CreateHeader();
            CreateFooter();
            CreateOkButton();
            CreateCancelButton();
            CreateContainer();
        }

        /// <summary>
        /// </summary>
        private void CreateBorderLines()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Sprite
            {
                Parent = this,
                Alignment = Alignment.TopLeft,
                Size = new ScalableVector2(Width, 2),
            };

            // ReSharper disable once ObjectCreationAsStatement
            new Sprite
            {
                Parent = this,
                Alignment = Alignment.TopRight,
                Size = new ScalableVector2(2, Height),
            };

            // ReSharper disable once ObjectCreationAsStatement
            new Sprite
            {
                Parent = this,
                Alignment = Alignment.BotLeft,
                Size = new ScalableVector2(Width, 2),
            };
        }

        /// <summary>
        /// </summary>
        private void CreateHeader()
        {
            HeaderBackground = new Sprite
            {
                Parent = this,
                Size = new ScalableVector2(Width, 45),
                Tint = ColorHelper.HexToColor($"#212121")
            };

            var headerFlag = new Sprite()
            {
                Parent = HeaderBackground,
                Size = new ScalableVector2(5, HeaderBackground.Height),
                Tint = Color.LightGray,
                Alpha = 0
            };

            // ReSharper disable once ObjectCreationAsStatement
            new SpriteText(Fonts.Exo2SemiBold, "Edit Metadata", 14)
            {
                Parent = HeaderBackground,
                Alignment = Alignment.MidLeft,
                X = headerFlag.X + 15,
            };

            var exitButton = new JukeboxButton(FontAwesome.Get(FontAwesomeIcon.fa_times), (sender, args) => Dialog.Close())
            {
                Parent = HeaderBackground,
                Alignment = Alignment.MidRight,
                Size = new ScalableVector2(20, 20)
            };

            exitButton.X -= exitButton.Width / 2f + 5;
        }

        /// <summary>
        /// </summary>
        private void CreateFooter() => FooterBackground = new Sprite
        {
            Parent = this,
            Size = new ScalableVector2(Width, 50),
            Tint = ColorHelper.HexToColor("#212121"),
            Alignment = Alignment.BotLeft,
            Y = 1
        };

        /// <summary>
        /// </summary>
        private void CreateOkButton() => OkButton = new BorderedTextButton("OK", Color.LimeGreen, (sender, args) =>
        {
            var changesMade = false;
            Container.Items.ForEach(x =>
            {
                if (x.HasChanged())
                    changesMade = true;
            });

            if (!changesMade)
            {
                Dialog.Close();
                return;
            }

            // Changes were made to the metadata, so we need to save it appropriately.
            Logger.Important($"Changes made to map metadata and needs to save.", LogType.Runtime);
            DialogManager.Show(new EditorMetadataConfirmationDialog(Dialog.Screen, this));
        })
        {
            Parent = FooterBackground,
            Alignment = Alignment.MidRight,
            X = -20,
            Text = {Font = Fonts.Exo2SemiBold}
        };

        /// <summary>
        /// </summary>
        private void CreateCancelButton() => CancelButton = new BorderedTextButton("Cancel", Color.Crimson, (sender, args) => Dialog.Close())
        {
            Parent = FooterBackground,
            Alignment = Alignment.MidRight,
            X = OkButton.X - OkButton.Width - 20,
            Text = { Font = Fonts.Exo2SemiBold }
        };

        /// <summary>
        /// </summary>
        private void CreateContainer() => Container = new EditorMetadataScrollContainer(this, new List<EditorMetadataItem>()
        {
            new EditorMetadataTextbox(this, "Artist", Dialog.Screen.WorkingMap.Artist, s => Dialog.Screen.WorkingMap.Artist = s),
            new EditorMetadataTextbox(this, "Title", Dialog.Screen.WorkingMap.Title, s => Dialog.Screen.WorkingMap.Title = s),
            new EditorMetadataTextbox(this, "Creator", Dialog.Screen.WorkingMap.Creator, s => Dialog.Screen.WorkingMap.Creator = s),
            new EditorMetadataTextbox(this, "Difficulty", Dialog.Screen.WorkingMap.DifficultyName, s => Dialog.Screen.WorkingMap.DifficultyName = s),
            new EditorMetadataTextbox(this, "Source", Dialog.Screen.WorkingMap.Source, s => Dialog.Screen.WorkingMap.Source = s),
            new EditorMetadataTextbox(this, "Tags", Dialog.Screen.WorkingMap.Tags, s => Dialog.Screen.WorkingMap.Tags = s),
            new EditorMetadataTextbox(this, "Description", Dialog.Screen.WorkingMap.Description, s => Dialog.Screen.WorkingMap.Description = s),
            new EditorMetadataGameMode(this, "Game Mode", Dialog.Screen.WorkingMap.Mode, s =>
            {
                switch (s)
                {
                    case "4 Keys":
                        Dialog.Screen.WorkingMap.Mode = GameMode.Keys4;
                        break;
                    case "7 Keys":
                        Dialog.Screen.WorkingMap.Mode = GameMode.Keys7;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            })
        })
        {
            Parent = this,
            Y = HeaderBackground.Y + HeaderBackground.Height,
        };
    }
}