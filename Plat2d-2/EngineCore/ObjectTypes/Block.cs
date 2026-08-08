using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public enum BlockFunction
    {
        Button,Enableable,PowerupSolid,Normal,Destructable
    }
    public enum BlockBulletType
    {
        All, ByWeapon, None
    }
    public class Block
    {
        //this is data for block, it needs:
        public Sprite2d Sprite = null;
        public List<Bitmap> AnimationFramesBitmap;
        public BlockFunction WhatBlockDoes { get; set; } = BlockFunction.Normal;
        public bool IsItAnimated { get; set; } = false;
        public bool SpawnAsSolid { get; set; } = true; // use this and next value to determine if player can jump up through the platform
        public bool CurrentlySolid { get; set; } = true; //  see above
        public bool IsItDestructable { get; set; } = false;
        public int BlockHealth { get; set; } = 1; // if 0, cannot be destroyed
        public BlockBulletType BlockFilter { get; set; } = BlockBulletType.ByWeapon;
        public bool DoesItCollideWithEnemy { get; set; } = true;

        //sprites or list of sprites
        //is it animated or not
        //DEFERRED: type - //in the current version, its a tag like "ground" air and whatnot
        //currentstate - solid or notsolid, it is for platforms that are not solid when player is not higher
        //solidstate - yes or no, this is instantiated at level rendertime
        //is it destructable
        //health of block
        //does it block bullets
        //does it stop enemies
        //DEFERRED: list of enemies that it stops
        //DEFERRED: is it square or slope?
        //DEFERRED: slopetype, is enum, can be: 16,32top,32bottom,64hightop,64midtop,64midbottom,64lowbase, custom,
        //DEFERRED: is the slope flipped horizontally
        //DEFERRED: is the slope flipped vertically

        /// <summary>
        /// normal block
        /// </summary>
        /// <param name="frames"></param>
        /// <param name=""></param>
        public Block(List<Bitmap> frames, bool animationOnOff, int health)
        {
            this.AnimationFramesBitmap = frames;
            this.IsItAnimated = animationOnOff;
            this.BlockHealth = health;
        }
        /// <summary>
        /// normal block
        /// </summary>
        /// <param name="frames"></param>
        /// <param name=""></param>
        public Block(List<Bitmap> frames, BlockFunction blockdoesthis, bool animationOnOff)
        {
            this.AnimationFramesBitmap = frames;
            this.IsItAnimated = animationOnOff;
            this.WhatBlockDoes = blockdoesthis;
        }
    }
    //has constructors for combinations of properties, added to the class as needed
}
