using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public enum ItemType
    {
        Undefined,
        Trigger,
        Ammo,
        Health,
        Life,
        WeaponUnlock,
        StoryItem
    }
    public class Collectable
    {
        public Sprite2d Sprite { get; set; }
        public List<Bitmap> AniFrames { get; set; }
        public int Value { get; set; } = 0;
        public ItemType WhatThisType { get; set; } = ItemType.Undefined;
        public bool IsHidden { get; set; } = true;

        /// <summary>
        /// Templatetype
        /// </summary>
        public Collectable(Sprite2d mainimage, List<Bitmap> frames, int amount, ItemType type )
        {
            Sprite = mainimage;
            AniFrames = frames;
            Value = amount;
            WhatThisType = type;
        }
    }
}
