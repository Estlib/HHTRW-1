using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Plat2d_2.EngineCore
{
    public enum ActionState
    {
        // enum utility to list all the actionstates
        StandingLeft = 0,
        StandingRight = 1,
        WalkingLeft = 2,
        WalkingRight = 3,
        JumpingLeft = 4,
        JumpingRight = 5,
        FiringLeft = 6,
        FiringRight = 7,
        GenericFlying = 8,
        ErrorState = 9,


    }
    public class EnemyV2
    {
        public Sprite2d sprite2d; //main sprite
        public List<Sprite2d> animationFrames; //list of sprites used for animation
        public List<Bitmap> animationFramesBitmap; //list of sprites used for animation in bitmap form
        public List<int> walkLeftData; //int list that contains frame numbers to animate walking left with
        public List<int> walkRightData; //int list that contains frame numbers to animate walking right with
        public List<int> jumpLeftData; //int list that contains frame numbers to animate jumping left with
        public List<int> jumpRightData; //int list that contains frame numbers to animate jumping right with
        public List<int> stillLeftData; //int list that contains frame numbers to animate facing left with
        public List<int> stillRightData; //int list that contains frame numbers to animate facing right with
        public List<int> fireLeftData; //int list that contains frame numbers to animate firing left with
        public List<int> fireRightData; //int list that contains frame numbers to animate firing right with
        public List<int> flyData; //int list that contains frame numbers to animate generic flight with
        public List<int> errorData; //int list to display an error sprite
        public int lastXpos; //last x position of enemy
        public int lastYpos; //last y position of enemy
        public ActionState CurrentActionState; //what the enemy should be doing right now
        public List<int> BehaviourData; //Determines how the enemy behaves, if it is not directly an "aim-at-player-only" enemy
        public int CurrentBehaviourStep; //Holds the value for what step the enemy behaviour currently is on.
        public bool isCollidingOrFalling; //Sets weather the enemy is currently on a surface or not
        public bool isfacingleft; //is it facing left, used to determine animation frame sides.
        public string enemyName { get; set; } = "Unnamed Enemy"; //name of the enemy, has no particular use other than for bosses
        public string enemyType { get; set; } = "Devtest"; //type of enemy, presently it is a string, but once the game gets everything it needs to implement all required enemy types, this will turn into an enum.


        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="sprite2D">main sprite of object</param>
        /// <param name="animationFrames">other sprites that will replace main sprite as the enemy is animated</param>
        /// <param name="walkLeftData">int list of left walk animation frame data in animationframes</param>
        /// <param name="walkRightData">int list of right walk animation frame data in animationframes</param>
        /// <param name="jumpLeftData">int list of left jump animation frame data in animationframes</param>
        /// <param name="jumpRightData">int list of right jump animation frame data in animationframes</param>
        /// <param name="stillLeftData">int list of left standing animation frame data in animationframes</param>
        /// <param name="stillRightData">int list of right standing animation frame data in animationframes</param>
        /// <param name="fireLeftData">int list of left firing animation frame data in animationframes</param>
        /// <param name="fireRightData">int list of right firing animation frame data in animationframes</param>
        /// <param name="flyData">int list of generic flying animation frame data in animationframes</param>
        /// <param name="errorData">int list of error frame data in animationframes when frame is not encountered</param>
        /// <param name="lastXpos">last x position of enemy </param>
        /// <param name="lastYpos">last y position of enemy</param>
        /// <param name="CurrentActionState">what the enemy should be doing right now</param>
        /// <param name="BehaviourData">Determines how the enemy behaves, if it is not directly an "aim-at-player-only" enemy</param>
        /// <param name="CurrentBehaviourStep"></param>
        /// <param name="isCollidingOrFalling">Sets weather the enemy is currently on a surface or not</param>
        /// <param name="isfacingleft">is it facing left, used to determine animation frame sides.</param>
        /// <param name="enemyName"></param>
        /// <param name="enemyType"></param>
        public EnemyV2(
            Sprite2d sprite2D,
            List<Sprite2d> animationFrames,
            List<int> walkLeftData,
            List<int> walkRightData,
            List<int> jumpLeftData,
            List<int> jumpRightData,
            List<int> stillLeftData,
            List<int> stillRightData,
            List<int> fireLeftData,
            List<int> fireRightData,
            List<int> flyData,
            List<int> errorData,
            int lastXpos,
            int lastYpos,
            ActionState CurrentActionState,
            List<int> BehaviourData,
            int CurrentBehaviourStep,
            bool isCollidingOrFalling,
            bool isfacingleft,
            string enemyName,
            string enemyType
            )
        {
            this.sprite2d = sprite2D;
            this.animationFrames = animationFrames;
            this.walkLeftData = walkLeftData;
            this.walkRightData = walkRightData;
            this.jumpLeftData = jumpLeftData;
            this.jumpRightData = jumpRightData;
            this.stillLeftData  = stillLeftData;
            this.stillRightData = stillRightData;
            this.fireLeftData = fireLeftData;
            this.fireRightData = fireRightData;
            this.flyData = flyData;
            this.errorData = errorData;
            this.lastXpos = lastXpos;
            this.lastYpos = lastYpos;
            this.CurrentActionState = CurrentActionState;
            this.BehaviourData = BehaviourData;
            this.CurrentBehaviourStep = CurrentBehaviourStep;
            this.isCollidingOrFalling = isCollidingOrFalling;
            this.isfacingleft = isfacingleft;
            this.enemyName = enemyName;
            this.enemyType = enemyType;
        }
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="sprite2D">main sprite of object</param>
        /// <param name="animationFrames">other sprites that will replace main sprite as the enemy is animated</param>
        /// <param name="walkLeftData">int list of left walk animation frame data in animationframes</param>
        /// <param name="walkRightData">int list of right walk animation frame data in animationframes</param>
        /// <param name="jumpLeftData">int list of left jump animation frame data in animationframes</param>
        /// <param name="jumpRightData">int list of right jump animation frame data in animationframes</param>
        /// <param name="stillLeftData">int list of left standing animation frame data in animationframes</param>
        /// <param name="stillRightData">int list of right standing animation frame data in animationframes</param>
        /// <param name="fireLeftData">int list of left firing animation frame data in animationframes</param>
        /// <param name="fireRightData">int list of right firing animation frame data in animationframes</param>
        /// <param name="flyData">int list of generic flying animation frame data in animationframes</param>
        /// <param name="errorData">int list of error frame data in animationframes when frame is not encountered</param>
        /// <param name="lastXpos">last x position of enemy </param>
        /// <param name="lastYpos">last y position of enemy</param>
        /// <param name="CurrentActionState">what the enemy should be doing right now</param>
        /// <param name="BehaviourData">Determines how the enemy behaves, if it is not directly an "aim-at-player-only" enemy</param>
        /// <param name="CurrentBehaviourStep"></param>
        /// <param name="isCollidingOrFalling">Sets weather the enemy is currently on a surface or not</param>
        /// <param name="isfacingleft">is it facing left, used to determine animation frame sides.</param>
        /// <param name="enemyName"></param>
        /// <param name="enemyType"></param>
        public EnemyV2(
            Sprite2d sprite2D,
            List<Bitmap> animationFrames,
            List<int> walkLeftData,
            List<int> walkRightData,
            List<int> jumpLeftData,
            List<int> jumpRightData,
            List<int> stillLeftData,
            List<int> stillRightData,
            List<int> fireLeftData,
            List<int> fireRightData,
            List<int> flyData,
            List<int> errorData,
            int lastXpos,
            int lastYpos,
            ActionState CurrentActionState,
            List<int> BehaviourData,
            int CurrentBehaviourStep,
            bool isCollidingOrFalling,
            bool isfacingleft,
            string enemyName,
            string enemyType
            )
        {
            this.sprite2d = sprite2D;
            this.animationFramesBitmap = animationFrames;
            this.walkLeftData = walkLeftData;
            this.walkRightData = walkRightData;
            this.jumpLeftData = jumpLeftData;
            this.jumpRightData = jumpRightData;
            this.stillLeftData = stillLeftData;
            this.stillRightData = stillRightData;
            this.fireLeftData = fireLeftData;
            this.fireRightData = fireRightData;
            this.flyData = flyData;
            this.errorData = errorData;
            this.lastXpos = lastXpos;
            this.lastYpos = lastYpos;
            this.CurrentActionState = CurrentActionState;
            this.BehaviourData = BehaviourData;
            this.CurrentBehaviourStep = CurrentBehaviourStep;
            this.isCollidingOrFalling = isCollidingOrFalling;
            this.isfacingleft = isfacingleft;
            this.enemyName = enemyName;
            this.enemyType = enemyType;
        }
    }
}
