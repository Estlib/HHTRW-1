using Plat2d_2.EngineCore.ObjectTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectControllers
{
    public class LevelController
    {
        public static Sprite2d[] NoArtRefs = ArtData.GetArtRefs("NoArtRefs");
        public static Sprite2d[] PlainsArtRefs = ArtData.GetArtRefs("PlainsArtRefs");
        public static Sprite2d[] TitleMenuMapRefs = ArtData.GetArtRefs("TitleMenuMapRefs");
        public static Sprite2d[] UndergroundArtRefs = ArtData.GetArtRefs("UndergroundArtRefs");
        public static Sprite2d[] DesertArtRefs = ArtData.GetArtRefs("DesertArtRefs");
        public static Sprite2d[] ForestArtRefs = ArtData.GetArtRefs("ForestArtRefs");
        public static Sprite2d[] CastleArtRefs = ArtData.GetArtRefs("CastleArtRefs");
        public LevelController() { }

        //levels
        internal static Level GetWM_1()
        {
            Level harenimusWorldMap = new Level(14,
                "screens",
                ArtData.TitleMenuMapRefsTags(),
                LevelData.RetrieveData("WM1", 0),
                LevelData.RetrieveData("WM1", 1),
                LevelData.RetrieveData("WM1", 2),
                LevelData.RetrieveData("WM1", 3),
                "Screen",
                "World Map",
                Color.FromArgb(255, 0, 0, 0),
                false,
                TitleMenuMapRefs
                )
            { };
            return harenimusWorldMap;
        }
        internal static Level GetL_01()
        {
            Level harenimus1_1 = new Level(
                2,
                "plains",
                ArtData.PlainsArtRefsTags(),
                LevelData.RetrieveData("L_01", 0),
                LevelData.RetrieveData("L_01", 1),
                LevelData.RetrieveData("L_01", 2),
                LevelData.RetrieveData("L_01", 3),
                "Level",
                "Harenimus 1-1 Plains",
                Color.FromArgb(255, 0, 0, 0),
                false, 
                PlainsArtRefs
                )
            { };
            return harenimus1_1;
        }
        internal static Level GetL_02()
        {
            Level harenimus1_2 = new Level(
                4,
                "underground",
                ArtData.UndergroundArtRefsTags(),
                LevelData.RetrieveData("L_02", 0),
                LevelData.RetrieveData("L_02", 1),
                LevelData.RetrieveData("L_02", 2),
                LevelData.RetrieveData("L_02", 3),
                "Level",
                "Harenimus 1-2 Cave",
                Color.FromArgb(255, 0, 0, 0),
                false, 
                UndergroundArtRefs
            )
            { };
            return harenimus1_2;
        }
        internal static Level GetL_03()
        {
            Level harenimus1_3_1 = new Level(
                3,
                "forest",
                ArtData.ForestArtRefsTags(),
                LevelData.RetrieveData("L_03", 0),
                LevelData.RetrieveData("L_03", 1),
                LevelData.RetrieveData("L_03", 2),
                LevelData.RetrieveData("L_03", 3),
                "Level",
                "Harenimus 1-3 Forest 1",
                Color.FromArgb(255, 88, 216, 88),
                false, 
                ForestArtRefs
            )
            { };
            return harenimus1_3_1;
        }
        internal static Level GetL_04()
        {
            Level harenimus1_3_2 = new Level(
                3,
                "forest",
                ArtData.ForestArtRefsTags(),
                LevelData.RetrieveData("L_04", 0),
                LevelData.RetrieveData("L_04", 1),
                LevelData.RetrieveData("L_04", 2),
                LevelData.RetrieveData("L_04", 3),
                "Level",
                "Harenimus 1-3 Forest 2",
                Color.FromArgb(255, 88, 216, 88),
                false, 
                ForestArtRefs
            )
            { };
            return harenimus1_3_2;
        }
        internal static Level GetL_05()
        {
            Level harenimus1_4 = new Level(
                5,
                "desert",
                ArtData.DesertArtRefsTags(),
                LevelData.RetrieveData("L_05", 0),
                LevelData.RetrieveData("L_05", 1),
                LevelData.RetrieveData("L_05", 2),
                LevelData.RetrieveData("L_05", 3),
                "Level",
                "Harenimus 2-1 Desert",
                Color.FromArgb(255, 164, 232, 252),
                false, 
                DesertArtRefs
            )
            { };
            return harenimus1_4;
        }
        internal static Level GetL_06()
        {
            Level harenimus1_5 = new Level(
                2,
                "plains",
                ArtData.PlainsArtRefsTags(),
                LevelData.RetrieveData("L_06", 0),
                LevelData.RetrieveData("L_06", 1),
                LevelData.RetrieveData("L_06", 2),
                LevelData.RetrieveData("L_06", 3),
                "Level",
                "Harenimus 1-5 Plains",
                Color.FromArgb(255, 56, 192, 252),
                false, 
                PlainsArtRefs
            )
            { };
            return harenimus1_5;
        }
        internal static Level GetL_07()
        {
            Level harenimus1_6_1 = new Level(
                6,
                "castle",
                ArtData.CastleArtRefsTags(),
                LevelData.RetrieveData("L_07", 0),
                LevelData.RetrieveData("L_07", 1),
                LevelData.RetrieveData("L_07", 2),
                LevelData.RetrieveData("L_07", 3),
                "Level",
                "Harenimus 1-6 Castle 1",
                Color.FromArgb(255, 0, 0, 0),
                false, 
                CastleArtRefs
            )
            { };
            return harenimus1_6_1;
        }
        internal static Level GetL_08()
        {
            Level harenimus1_6_2 = new Level(
                6,
                "castle",
                ArtData.CastleArtRefsTags(),
                LevelData.RetrieveData("L_08", 0),
                LevelData.RetrieveData("L_08", 1),
                LevelData.RetrieveData("L_08", 2),
                LevelData.RetrieveData("L_08", 3),
                "Level",
                "Harenimus 1-6 Castle 2",
                Color.FromArgb(255, 0, 0, 0),
                false, 
                CastleArtRefs
            )
            { };
            return harenimus1_6_2;
        }

        //utility levels
        internal static Level GetTitleScreen()
        {
            Level titlescreen = new Level(
                1,
                "screens",
                ArtData.TitleMenuMapRefsTags(),
                LevelData.RetrieveData("TS1", 0),
                LevelData.RetrieveData("TS1", 1),
                LevelData.RetrieveData("TS1", 2),
                LevelData.RetrieveData("TS1", 3),
                "Screen",
                "Title Screen",
                Color.FromArgb(255, 0, 0, 0),
                false, 
                TitleMenuMapRefs
            )
            { };
            return titlescreen;
        }
        internal static Level GetGameOverScreen()
        {
            Level titlescreen = new Level(
                6,
                "screens",
                ArtData.CastleArtRefsTags(),
                LevelData.RetrieveData("GO1", 0),
                LevelData.RetrieveData("GO1", 1),
                LevelData.RetrieveData("GO1", 2),
                LevelData.RetrieveData("GO1", 3),
                "Screen",
                "Game Over",
                Color.FromArgb(255, 0, 0, 0),
                false, 
                TitleMenuMapRefs
            )
            { };
            return titlescreen;
        }

        //areas
        internal static Area GetAreaW1_1()
        {
            return new Area
                (
                new List<Level> { GetL_01() },
                1,
                "Harenimus 1-1 Plains"
                );
        }
        internal static Area GetAreaW1_2()
        {
            return new Area
                (
                new List<Level> { GetL_02() },
                2,
                "Harenimus 1-2 Cave"
                );
        }
        internal static Area GetAreaW1_3()
        {
            return new Area
                (
                new List<Level> { GetL_03(),GetL_04() },
                3,
                "Harenimus 1-3 Forest"
                );
        }
        internal static Area GetAreaW1_4()
        {
            return new Area
                (
                new List<Level> { GetL_05() },
                4,
                "Harenimus 2-1 Desert"
                );
        }
        internal static Area GetAreaW1_5()
        {
            return new Area
                (
                new List<Level> { GetL_06() },
                5,
                "Harenimus 1-5 Plains 2"
                );
        }
        internal static Area GetAreaW1_6()
        {
            return new Area
                (
                new List<Level> { GetL_07(), GetL_08() },
                6,
                "Harenimus 1-6 Castle"
                );
        }
    }
}
