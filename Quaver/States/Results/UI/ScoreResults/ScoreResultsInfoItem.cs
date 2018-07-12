﻿using System;
using Microsoft.Xna.Framework;
using Quaver.Graphics;
using Quaver.Graphics.Sprites;
using Quaver.Graphics.Text;
using Quaver.Main;

namespace Quaver.States.Results.UI.ScoreResults
{
    internal class ScoreResultsInfoItem
    {
        /// <summary>
        ///     The "title" of the result.
        /// </summary>
        internal string Title { get; set; }

        /// <summary>
        ///     The value of the result
        /// </summary>
        internal string Value { get; set; }

        /// <summary>
        ///    The text that displays the title.
        /// </summary>
        internal SpriteText TitleText { get; set; }

        /// <summary>
        ///     The text that displays the value.
        /// </summary>
        internal SpriteText ValueText { get; set; }

        /// <summary>
        ///     If there is no set value, then we will display this sprite, to show
        ///     that it is loading.
        /// </summary>
        private Sprite LoadingSprite { get; set; }

        /// <summary>
        ///     If the item has already been initialized. It'll throw an exception
        ///     if it has been initialized once already.
        /// </summary>
        private bool IsInitialized { get; set; }

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="defaultValue"></param>
        internal ScoreResultsInfoItem(string title, string defaultValue = null)
        {
            Title = title;
            Value = defaultValue;
        }

        /// <summary>
        ///    Initializes the item. It can only be initialized one time.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="posX"></param>
        /// <exception cref="InvalidOperationException"></exception>
        internal void Initialize(ScoreResultsInfo parent, float posX)
        {
            if (IsInitialized)
                throw new InvalidOperationException($"ScoreResultsInfoItem has already been initialized.");

            TitleText = new SpriteText()
            {
                Parent = parent,
                Font = Fonts.AllerRegular16,
                TextAlignment = Alignment.MidCenter,
                Text = Title,
                PosX = posX,
                PosY = 20,
                TextScale = 0.80f,
                TextColor = Colors.MainAccent
            };

            if (Value != null)
            {
                ValueText = new SpriteText()
                {
                    Parent = parent,
                    Font = Fonts.AllerRegular16,
                    TextAlignment = Alignment.MidCenter,
                    Text = Value,
                    PosX = posX,
                    PosY = TitleText.PosY + (TitleText.MeasureString() / 2f).Y + 20,
                    TextScale = 0.70f,
                    TextColor = Color.White
                };
            }
            else
            {
                LoadingSprite = new Sprite()
                {
                    Parent = parent,
                    Size = new UDim2D(20, 20),
                    Position = new UDim2D(posX - TitleText.MeasureString().X / 4f + 13, TitleText.PosY + ( TitleText.MeasureString() / 2f ).Y + 10),
                    Image = FontAwesome.Spinner
                };
            }

            IsInitialized = true;
        }

        /// <summary>
        ///     Updates the item. Mainly used for loading animations
        /// </summary>
        /// <param name="dt"></param>
        internal void Update(double dt)
        {
            if (LoadingSprite == null)
                return;

            LoadingSprite.Rotation = (float)(MathHelper.ToDegrees(LoadingSprite.Rotation) + 7 * dt / 30f);
        }
    }
}