using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public enum WeaponType
    {
        Wielded, Shot, Gravity, Tool, Screen
    }
    public class Weapon
    {
        public string WeaponName { get; set; }
        public WeaponType ThisWeaponType { get; set; }
        public int MaxBulletCount { get; set; }
        public int AmmoConsumption { get; set; } = -1;
        public bool FiringLock { get; set; } //can weapon currently be fired
        public int FiringLockTimer { get; set; } //how many frames is lock applied for
        public List<Bitmap> Graphics { get; set; } // weapon graphics, DOES NOT APPLY FOR WIELDED OR SCREEN

        public Weapon(
            string weaponName,
            WeaponType weaponType,
            int maxBulletcount,
            int ammoConsumption,
            bool firingLock,
            int firingLockFrameCount,
            List<Bitmap> projectileBitmaps
            )
        {
            WeaponName = weaponName;
            ThisWeaponType = weaponType;
            MaxBulletCount = maxBulletcount;
            AmmoConsumption = ammoConsumption;
            FiringLock = firingLock;
            FiringLockTimer = firingLockFrameCount;
            Graphics = projectileBitmaps;

        }

        internal static Weapon GetWeapon(string weaponName)
        {
            switch (weaponName)
            {
                case "debug":
                    Log.Normal("Player is given weapon \"debug\"");
                    return DebugWeapon();
                    break;
                default:
                    Log.Warning("No weapon is selected");
                    return DebugWeapon();
                    break;
            }
        }

        private static Weapon DebugWeapon()
        {
            return new Weapon(
                "debug",
                WeaponType.Shot,
                3,
                1,
                false,
                10,
                new List<Bitmap>()
                    {
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon1A.png")),
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon1B.png"))
                    }
            );
        }

    }
}
