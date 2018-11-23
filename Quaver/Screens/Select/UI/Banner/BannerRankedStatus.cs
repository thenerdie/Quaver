﻿using System;
using Quaver.API.Enums;
using Quaver.Assets;
using Quaver.Database.Maps;
using Wobble.Graphics;
using Wobble.Graphics.Sprites;

namespace Quaver.Screens.Select.UI.Banner
{
    public class BannerRankedStatus : Sprite
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public BannerRankedStatus() => Size = new ScalableVector2(105, 38);

        /// <summary>
        ///     Updates the ranked status with a new map.
        /// </summary>
        /// <param name="map"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void UpdateMap(Map map)
        {
            switch (map.RankedStatus)
            {
                case RankedStatus.NotSubmitted:
                    Image = UserInterface.StatusNotSubmitted;
                    break;
                case RankedStatus.Unranked:
                    Image = UserInterface.StatusUnranked;
                    break;
                case RankedStatus.Ranked:
                    Image = UserInterface.StatusRanked;
                    break;
                case RankedStatus.DanCourse:
                    Image = UserInterface.StatusDanCourse;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}