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
        public int AmmoConsumption { get; set; } = 0;
        public int AmmoLeft { get; set; } = 1;
        public bool FiringLock { get; set; } //can weapon currently be fired
        public int FiringLockTimer { get; set; } //how many frames is lock applied for
        public List<Bitmap> Graphics { get; set; } // weapon graphics, DOES NOT APPLY FOR WIELDED OR SCREEN
        public int PreFireFrames { get; set; } //how many frames to wait until bullet is fired
        public int FiringLockTimerOriginal {  get; set; }

        public Weapon(
            string weaponName,
            WeaponType weaponType,
            int maxBulletcount,
            int ammoConsumption,
            bool firingLock,
            int firingLockFrameCount,
            List<Bitmap> projectileBitmaps,
            int ammoLeft,
            int firingLockTimerOriginal
            )
        {
            WeaponName = weaponName;
            ThisWeaponType = weaponType;
            MaxBulletCount = maxBulletcount;
            AmmoConsumption = ammoConsumption;
            FiringLock = firingLock;
            FiringLockTimer = firingLockFrameCount;
            Graphics = projectileBitmaps;
            AmmoLeft = ammoLeft;
            FiringLockTimerOriginal = firingLockTimerOriginal;

        }
        public Weapon(
            string weaponName,
            WeaponType weaponType,
            int maxBulletcount,
            int ammoConsumption,
            bool firingLock,
            int firingLockFrameCount,
            List<Bitmap> projectileBitmaps,
            int ammoLeft,
            int preFireFrames,
            int firingLockTimerOriginal
            )
        {
            WeaponName = weaponName;
            ThisWeaponType = weaponType;
            MaxBulletCount = maxBulletcount;
            AmmoConsumption = ammoConsumption;
            FiringLock = firingLock;
            FiringLockTimer = firingLockFrameCount;
            Graphics = projectileBitmaps;
            AmmoLeft = ammoLeft;
            PreFireFrames = preFireFrames;
            FiringLockTimerOriginal = firingLockTimerOriginal;
        }

        internal static Weapon GetWeapon(string weaponName = "")
        {
            switch (weaponName)
            {
                case "debug":
                    Log.Normal("Player is given weapon \"debug\"");
                    return DebugWeapon();
                case "plasma":
                    Log.Normal("Player is given weapon \"plasma\"");
                    return PlasmaRifle();
                case "flint":
                    Log.Normal("Player is given weapon \"flint\"");
                    return FlintLock();
                default:
                    Log.Warning("No weapon is selected, giving sword.");
                    return Sword();
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
                    },
                12,
                10
            );
        }
        private static Weapon FlintLock()
        {
            return new Weapon(
                "Barker",
                WeaponType.Shot,
                1,
                0,
                false,
                25,
                new List<Bitmap>()
                    {
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon2A.png"))
                    },
                12,
                0,
                25
            );
        }
        private static Weapon Sword()
        {
            return new Weapon(
                "Väits",
                WeaponType.Wielded,
                3,
                0,
                false,
                5,
                new List<Bitmap>()
                    {
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon4A.png")),
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon4B.png")),
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon4C.png"))
                    },
                12,
                3,
                3
            );
        }
        private static Weapon PlasmaRifle()
        {
            return new Weapon(
                "Willo",
                WeaponType.Shot,
                10,
                0,
                false,
                3,
                new List<Bitmap>()
                    {
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon3A.png")),
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon3B.png")),
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon3C.png")),
                        new Bitmap(Image.FromFile($"assets/sprites/bullets/weapon3D.png"))
                    },
                12,
                0,
                3
            );
        }

    }
}
