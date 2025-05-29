using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class Enemy
    {
        public Sprite2d sprite2d;
        public int lastXpos;
        public int lastYpos;
        public int walkleftanimstart;
        public int walkleftanimend;
        public int walkrightanimstart;
        public int walkrightanimend;
        public int enemyleftwalkframes;
        public int enemyrightwalkframes;
        public int wherewalking;
        public bool isfacingleft;
        public int animationsteps;
        /// <summary>
        /// Default enemy constructor, containing sprite and frame animation data
        /// </summary>
        /// <param name="sprite">enemy sprite for which animation data is given</param>
        /// <param name="lastXpos">previous x axis position</param>
        /// <param name="lastYpos">previous y axis position</param>
        /// <param name="walkleftanimstart">number of frame, where left walk animation starts</param>
        /// <param name="walkleftanimend">number of frame, where left walk animation ends</param>
        /// <param name="walkrightanimstart">number of frame, where right walk animation starts</param>
        /// <param name="walkrightanimend">number of frame, where right walk animation ends</param>
        /// <param name="enemyleftwalkframes">amount of spriteframes that the left walk cycle can be animated for</param>
        /// <param name="enemyrightwalkframes">amount of spriteframes that the right walk cycle can be animated for</param>
        /// <param name="wherewalking">which way is the enemy walking currently</param>
        /// <param name="facedirection">what direction is the enemy facing</param>
        public Enemy(
            Sprite2d sprite, 
            int lastXpos, 
            int lastYpos, 
            int walkleftanimstart, 
            int walkleftanimend, 
            int walkrightanimstart, 
            int walkrightanimend, 
            int enemyleftwalkframes, 
            int enemyrightwalkframes, 
            int wherewalking, 
            bool facedirection
            /*, int animationsteps*/)
        {
            this.sprite2d = sprite;
            this.lastXpos = lastXpos; //0
            this.lastYpos = lastYpos; //1
            this.walkleftanimstart = walkleftanimstart; //2
            this.walkleftanimend = walkleftanimend; //3
            this.walkrightanimstart = walkrightanimstart; //4
            this.walkrightanimend = walkrightanimend; //5
            this.enemyleftwalkframes = enemyleftwalkframes; //6
            this.enemyrightwalkframes = enemyrightwalkframes; //7
            this.wherewalking = wherewalking; //old 8
            this.isfacingleft = facedirection; //current 8
            //this.animationsteps = animationsteps;

        }
        /// <summary>
        /// Simplified enemy constructor, containing sprite and frame animation data but where starting and ending frames are set to 0 and walking direction is preset to left
        /// </summary>
        /// <param name="sprite">enemy sprite for which animation data is given</param>
        /// <param name="lastXpos">previous x axis position</param>
        /// <param name="lastYpos">previous y axis position</param>
        /// <param name="enemyleftwalkframes">amount of spriteframes that the left walk cycle can be animated for</param>
        /// <param name="enemyrightwalkframes">amount of spriteframes that the right walk cycle can be animated for</param>
        public Enemy(Sprite2d sprite, int lastXpos, int lastYpos, int enemyleftwalkframes, int enemyrightwalkframes)
        {
            this.sprite2d = sprite;
            this.lastXpos = lastXpos; //0
            this.lastYpos = lastYpos; //1
            this.walkleftanimstart = 0; //2
            this.walkleftanimend = 0; //3
            this.walkrightanimstart = 0; //4
            this.walkrightanimend = 0; //5
            this.enemyleftwalkframes = enemyleftwalkframes; //6
            this.enemyrightwalkframes = enemyrightwalkframes; //7
            this.wherewalking = 0; //old 8
            this.isfacingleft = false; //current 8

        }
        /// <summary>
        /// simplified enemy constructor, containing sprite and frame animation data but where starting and ending frames are set to 0 and walking direction is preset to left. User can set how many frames to wait for next animation.
        /// </summary>
        /// <param name="sprite">enemy sprite for which animation data is given</param>
        /// <param name="lastXpos">previous x axis position</param>
        /// <param name="lastYpos">previous y axis position</param>
        /// <param name="enemyleftwalkframes">amount of spriteframes that the left walk cycle can be animated for</param>
        /// <param name="enemyrightwalkframes">amount of spriteframes that the right walk cycle can be animated for</param>
        /// <param name="animationsteps">how many rendered frames to wait to display next animation frame</param>
        public Enemy(Sprite2d sprite, int lastXpos, int lastYpos, int enemyleftwalkframes, int enemyrightwalkframes, int animationsteps)
        {
            this.sprite2d = sprite;
            this.lastXpos = lastXpos; //0
            this.lastYpos = lastYpos; //1
            this.walkleftanimstart = 0; //2
            this.walkleftanimend = 0; //3
            this.walkrightanimstart = 0; //4
            this.walkrightanimend = 0; //5
            this.enemyleftwalkframes = enemyleftwalkframes; //6
            this.enemyrightwalkframes = enemyrightwalkframes; //7
            this.wherewalking = 0; //old 8
            this.isfacingleft = false; //current 8
            this.animationsteps = animationsteps;

        }
        ~Enemy()
        {

        }
    }
    
}
