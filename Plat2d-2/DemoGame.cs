using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Plat2d_2.EngineCore;
using System.Windows.Forms;
using Box2DX.Common;
using System.IO;
using Box2DX.Dynamics;

namespace Plat2d_2
{
    class DemoGame : EngineCore.EngineCore
    {
        Sprite2d player;
        int steps = 0;
        int slowDownFrameRate = 1;
        int playerSpeed = 10;
        int currentSprite;
        List<Bitmap> playerSpritesBitmap = new List<Bitmap>();
        public static int currentLevel = 0;
        bool[] levelClear = new bool[6] {false,false,false,false,false,false};

        int facedirection;
        bool left;
        bool right;
        bool up;
        bool down;
        bool jump;
        bool jumpmode;
        bool nokey;
        bool LRDcheck;

        //Vector2 lastPos = Vector2.Zero();
        //List<Level> levels = new List<Level>();
        List<string[,]> levelMaps = new List<string[,]>();
        //List<Sprite2d> NoArtRefs = new List<Sprite2d>();
        public Sprite2d[] NoArtRefs = new Sprite2d[30];

        //string[,] Map =
        //{
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {"P",".",".",".","F" },
        //    {".",".","G",".","." },
        //    {"G","G","G","G","G" }
        //}; 
        //string[,] Map2 =
        //{
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {".",".",".",".","." },
        //    {"P",".",".",".","." },
        //    {".",".",".",".","F" },
        //    {".",".",".",".","." },
        //    {"G","G","G","G","G" }
        //};
        string[,] Map ={
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":","1","2","3","4","5","6","7","8","9",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":","P",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":","F",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        };
        string[,] Map2 ={
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":","l","v","e","s","w","t",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":","F",":",":","Q",":","W",":","E",":","R",":","T",":","Y",":","U",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":","P",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        };
        string[,] Map3 = {
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G","G",".",".",".",".","C",".","C",".",".",".",".",".",".",".",".",".",".","G" },
            {"G","G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G","G","G","G","G","G",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","F","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
            {"G",".","P",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
            {"G",".",".",".",".",".",".","G","G","G",".",".",".",".",".",".","G",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","C",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".","C",".","C",".","G",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        };
        string[,] Map4 = {
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G",".","P",".",".",".",".","C","C","G",".",".",".",".",".",".","G",".",".","G" },
            {"G",".",".",".",".",".",".","C","C","G",".",".",".","C",".",".","G",".",".","G" },
            {"G","G","G","G","G",".","C",".",".","G",".",".",".",".","C",".",".",".",".","G" },
            {"G","C",".","C",".",".",".",".",".",".",".","C","C","C","C","C",".",".",".","G" },
            {"G",".","C",".","C",".","C",".",".",".",".",".",".",".","C",".",".",".",".","G" },
            {"G",".",".",".",".",".",".","G",".",".",".",".",".","C",".",".",".",".","F","G" },
            {"G",".",".","G","G","G","G","G",".",".",".",".","G","G","G","G","G","G","G","G" },
            {"G",".",".",".",".",".",".","G",".",".","C",".","G","G",".",".",".","G","G","G" },
            {"G","C",".","C",".","C",".","G",".",".",".","G","G","G",".",".",".","G","G","G" },
            {"G",".","C",".","C",".",".","G",".",".",".",".","G","G",".",".",".","G","G","G" },
            {"G","G","G","G","G",".",".","G",".","C","G",".","G","G",".",".",".","G","G","G" },
            {"G","C","C","C","C","C",".",".",".",".",".",".","G","G","C",".",".","G","G","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        };
        string[,] Map5 = {
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","F","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G","G","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".",".",".",".","C","C","C",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".","G" },
            {"G",".","P",".",".",".","G","G","G","G",".",".","C",".","C",".",".",".",".","G" },
            {"G",".",".",".",".",".","G","G","G","G",".",".",".",".",".",".",".",".",".","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        };
        string[,] Map6 = {
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C","C","C",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","G","G","G","G","G","G",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","G","G","G","G","G","G","C",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".","C",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G","G",".",".",".",".",".",".",".",".","G","G","G","G","G","G","C",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".",".",".","C",".",".",".",".",".",".",".",".","C","C",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G","G",".",".",".",".",".",".",".",".","G","G","G","G","G","G","C",".",".","G","G","G","G","G","G","G","G",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G","G",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G","C",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".","C",".","C",".",".",".",".","C",".","C",".",".",".","G","G","G","G","G","G",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","F" },
            {"G",".",".",".",".",".","C","C","C",".",".",".",".","G","G","G","G","G","G","G","G","G","G",".",".",".","G","G","G","G","G","G","G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".","C",".","C",".",".","G","G","G","G","G","G","G",".",".",".","G","G","G","G","G","G",".",".","G",".",".",".",".",".","." },
            {"G",".","P",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".",".",".","G","G","G","G","G","G","G",".",".",".",".","G","G","G","G","G","G","G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","." },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".",".",".",".","G","G","G","G","G","G","G",".",".",".","G","G","G","G","G","G","G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G","G","G","G","G","G",".",".",".",".",".",".",".",".","." },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G",".",".","G",".",".",".",".",".",".","G","G","G","G","G","G","G",".",".","G","G","G","G","G","G","G",".",".","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","." },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G",".",".","G",".",".",".",".",".",".",".","G","G","G","G","G","G","G",".","G","G","G","G","G","G","G",".",".","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","." },
        };

        //string[,] Map ={
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":","1","2","3","4","5","6","7","8","9",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":","P",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":","F",":",":",":" },
        //    {":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":",":" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //};
        //string[,] Map2 = {
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G"," "," "," "," "," ","C"," ","C"," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","C"," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," ","G","G","G"," "," "," "," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","F","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," ","G","G","G"," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G"," "," ","G" },
        //    {"G"," ","P"," "," "," "," "," "," "," "," "," "," "," "," "," ","G"," "," ","G" },
        //    {"G"," "," "," "," "," "," ","G","G","G"," "," "," "," "," "," ","G"," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G","C"," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," ","C"," ","C"," ","G"," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G"," "," ","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //};
        //string[,] Map3 = {
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G"," ","P"," "," "," "," ","C","C","G"," "," "," "," "," "," ","G"," "," ","G" },
        //    {"G"," "," "," "," "," "," ","C","C","G"," "," "," ","C"," "," ","G"," "," ","G" },
        //    {"G","G","G","G","G"," ","C"," "," ","G"," "," "," "," ","C"," "," "," "," ","G" },
        //    {"G","C"," ","C"," "," "," "," "," "," "," ","C","C","C","C","C"," ","F"," ","G" },
        //    {"G"," ","C"," ","C"," ","C"," "," "," "," "," "," "," ","C"," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," ","G"," "," "," "," "," ","C"," "," "," "," "," ","G" },
        //    {"G"," "," ","G","G","G","G","G"," "," "," "," ","G","G","G","G","G","G","G","G" },
        //    {"G"," "," "," "," "," "," ","G"," "," ","C"," ","G","G"," "," "," ","G","G","G" },
        //    {"G","C"," ","C"," ","C"," ","G"," "," "," ","G","G","G"," "," "," ","G","G","G" },
        //    {"G"," ","C"," ","C"," "," ","G"," "," "," "," ","G","G"," "," "," ","G","G","G" },
        //    {"G","G","G","G","G"," "," ","G"," ","C","G"," ","G","G"," "," "," ","G","G","G" },
        //    {"G","C","C","C","C","C"," "," "," "," "," "," ","G","G","C"," "," ","G","G","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //};
        //string[,] Map4 = {
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","C"," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","F","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," ","G","G","G","G","G","G","G","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," "," "," ","C","C","C"," "," "," "," "," "," "," "," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","C"," ","G" },
        //    {"G"," ","P"," "," "," ","G","G","G","G"," "," ","C"," ","C"," "," "," "," ","G" },
        //    {"G"," "," "," "," "," ","G","G","G","G"," "," "," "," "," "," "," "," "," ","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //};
        //string[,] Map5 = {
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G"," ","P"," "," "," "," ","C","C","G"," "," "," "," "," "," ","G"," "," ","G" },
        //    {"G"," "," "," "," "," "," ","C","C","G"," "," "," ","C"," "," ","G"," "," ","G" },
        //    {"G","G","G","G","G"," ","C"," "," ","G"," "," "," "," ","C"," "," "," "," ","G" },
        //    {"G","C"," ","C"," "," "," "," "," "," "," ","C","C","C","C","C"," "," "," ","G" },
        //    {"G"," ","C"," ","C"," ","C"," "," "," "," "," "," "," ","C"," "," "," "," ","G" },
        //    {"G"," "," "," "," "," "," ","G"," "," "," "," "," ","C"," "," "," "," ","F","G" },
        //    {"G"," "," ","G","G","G","G","G"," "," "," "," ","G","G","G","G","G","G","G","G" },
        //    {"G"," "," "," "," "," "," ","G"," "," ","C"," ","G","G"," "," "," ","G","G","G" },
        //    {"G","C"," ","C"," ","C"," ","G"," "," "," ","G","G","G"," "," "," ","G","G","G" },
        //    {"G"," ","C"," ","C"," "," ","G"," "," "," "," ","G","G"," "," "," ","G","G","G" },
        //    {"G","G","G","G","G"," "," ","G"," ","C","G"," ","G","G"," "," "," ","G","G","G" },
        //    {"G","C","C","C","C","C"," "," "," "," "," "," ","G","G","C"," "," ","G","G","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //    {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        //};


        public DemoGame() :  base(new Vector2(615, 615),"HHTRW-engine1 demo")
        {

        }

        public override void OnLoad()
        {

            LevelSprites[0] = new List<Sprite2d>();
            LevelSprites[1] = new List<Sprite2d>();
            LevelSprites[2] = new List<Sprite2d>();
            LevelSprites[3] = new List<Sprite2d>();
            LevelSprites[4] = new List<Sprite2d>();
            LevelSprites[5] = new List<Sprite2d>();
            //List<Sprite2d> playerSprites = new List<Sprite2d>();
            Console.WriteLine("OnLoad works.");
            BGColor = System.Drawing.Color.Black;
            //CameraZoom = new Vector2(.1f,.1f);
            SetReferencesInArray();
            Sprite2d groundRef = NoArtRefs[0];
            Sprite2d airRef = NoArtRefs[1];
            Sprite2d coinRef = NoArtRefs[2];
            Sprite2d levelEndRef = NoArtRefs[3];
            Sprite2d tsairRef = NoArtRefs[4];
            Sprite2d ts1 = NoArtRefs[5];
            Sprite2d ts2 = NoArtRefs[6];
            Sprite2d ts3 = NoArtRefs[7];
            Sprite2d ts4 = NoArtRefs[8];
            Sprite2d ts5 = NoArtRefs[9];
            Sprite2d ts6 = NoArtRefs[10];
            Sprite2d ts7 = NoArtRefs[11];
            Sprite2d ts8 = NoArtRefs[12];
            Sprite2d ts9 = NoArtRefs[13];
            Sprite2d lss1 = NoArtRefs[14];
            Sprite2d lss2 = NoArtRefs[15];
            Sprite2d lss3 = NoArtRefs[16];
            Sprite2d lss4 = NoArtRefs[17];
            Sprite2d lss5 = NoArtRefs[18];
            Sprite2d lss6 = NoArtRefs[19];
            Sprite2d numbertile01 = NoArtRefs[20];
            Sprite2d numbertile02 = NoArtRefs[21];
            Sprite2d numbertile03 = NoArtRefs[22];
            Sprite2d numbertile04 = NoArtRefs[23];
            Sprite2d numbertile05 = NoArtRefs[24];
            Sprite2d numbertile06 = NoArtRefs[25];
            Sprite2d numbertile07 = NoArtRefs[26];
            Sprite2d numbertile08 = NoArtRefs[27];
            Sprite2d numbertile09 = NoArtRefs[28];
            Sprite2d numbertile0A = NoArtRefs[29];
            levelMaps.Add(Map);
            levelMaps.Add(Map2);
            levelMaps.Add(Map3);
            levelMaps.Add(Map4);
            levelMaps.Add(Map5);
            levelMaps.Add(Map6);

            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand1.png"))); //0
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run1.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run2.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run3.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run4.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run5.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run6.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand2.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3duck.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3jump.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/ALTfall.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand1flip.png"))); //11
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run1flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run2flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run3flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run4flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run5flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run6flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand2flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3duckflip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3jumpflip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/ALTfallFLIP.png")));
            currentSprite = 0; 
            LoadNextLevel(levelMaps.ElementAt(currentLevel), NoArtRefs);
            //for (int i = 0; i < Map.GetLength(1); i++)
            //{
            //    for (int j = 0; j < Map.GetLength(0); j++)
            //    {
            //        if (Map[j,i] == "G")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), groundRef, "Ground").CreateStatic();
            //        }
            //        if (Map[j, i] == ".")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), airRef, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "C")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), coinRef, "Coin")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "F")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), levelEndRef, "Finish")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == ":")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), tsairRef, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "1")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts1, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "2")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts2, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "3")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts3, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "4")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts4, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "5")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts5, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "6")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts6, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "7")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts7, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "8")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts8, "Air")/*.CreateStatic()*/;
            //        }
            //        if (Map[j, i] == "9")
            //        {
            //            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), ts9, "Air")/*.CreateStatic()*/;
            //        }
            //    }
            //}
            //for (int i = 0; i < Map.GetLength(1); i++)
            //{
            //    for (int j = 0; j < Map.GetLength(0); j++)
            //    {
            //        if (Map[j, i] == "P")
            //        {
            //            player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerSpritesBitmap[0], "Player");
            //            //player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerStand, "Player");
            //            player.CreateDynamic();
            //            //pass a list of sprites here, changing happens by animating list numbers, limited by if limits
            //            //see https://www.youtube.com/results?search_query=box2d+tutorial+c%23
            //            //playercollision = new Shape2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), "Player");
            //            //playercollision.CreateDynamic();
            //        }
            //    }
            //}
            //player = new Sprite2d(new Vector2(64, 96), new Vector2(32, 32), "player/wipspriteset/stand1", "Player");
            //player2 = new Sprite2d(new Vector2(128, 192), new Vector2(32, 32), "player/wipspriteset/stand1", "Player2");
        }
        /// <summary>
        /// function to load player separately to level loading, currently unused.
        /// </summary>
        /// <param name="playerSpritesBitmap"> sprites for the player object </param>
        private void LoadPlayerFromNextLevel(List<Bitmap> playerSpritesBitmap)
        {
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "P")
                    {
                        player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerSpritesBitmap[0], "Player");
                        player.CreateDynamic();
                    }
                }
            }
        }
        /// <summary>
        /// this function updates visuals and levels when window is being drawn
        /// </summary>
        public override void OnDraw()
        {
            if (levelClear[currentLevel] == true)
            {
                UnLoadCurrentLevel();
                currentLevel++;
                LoadNextLevel(levelMaps.ElementAt(currentLevel),NoArtRefs);
            }
            if (up)
            {
                if (up == true && left == true)
                {
                    up = false;
                }
                if (up == true && right == true)
                {
                    up = false;
                }
                Log.Warning("Up has no animation currently.");
            }
            if (down)
            {
                if (down == true && left == true)
                {
                    down = false;
                }
                if (down == true && right == true)
                {
                    down = false;
                }
                if (facedirection == 0)
                {
                    AnimatePlayer(19, 19);
                }
                else if (facedirection == 1)
                {
                    AnimatePlayer(8, 8);
                }
            }
            if (left)
            {
                facedirection = 0;
                AnimatePlayer(12, 17);
            }
            if (right)
            {
                facedirection = 1;
                AnimatePlayer(1, 6);
            }
            if (jump)
            {
                if (facedirection == 0)
                {
                    AnimatePlayer(21, 21);
                }
                else if (facedirection == 1)
                {
                    AnimatePlayer(10, 10);
                }
            }
            if (jump == false && jumpmode == true)
            {
                if (facedirection == 0)
                {
                    AnimatePlayer(20, 20);
                }
                else if (facedirection == 1)
                {
                    AnimatePlayer(9, 9);
                }
            }
            if (StillInput(nokey))
            {
                if (facedirection == 0)
                {
                    AnimatePlayer(11, 11);
                }
                else if (facedirection == 1)
                {
                    AnimatePlayer(0, 0);
                }
            }
            //if (player.Position.X < 160)
            //{
            //    CameraPosition.X = 0;
            //}
            //else
            //if (player.Position.X > 160)
            //{
            //    CameraPosition.X -= (player.Position.X-CameraPosition.X);
            //}
        }
        /// <summary>
        /// Unloads all of the content on screen. Including player and level tiles
        /// 
        /// TODO: fix problem of it not removing collisions/causing phantom collisions
        /// </summary>
        private void UnLoadCurrentLevel()
        {
            RemoveAllSprites();
        }
        /// <summary>
        /// Sets the references in NoArtRefs array
        /// </summary>
        private void SetReferencesInArray()
        {
            NoArtRefs = new Sprite2d[30];
            NoArtRefs[0] = new Sprite2d("tiles/noart/testblock1");
            NoArtRefs[1] = new Sprite2d("tiles/noart/testblock5");
            NoArtRefs[2] = new Sprite2d("tiles/noart/testobject2");
            NoArtRefs[3] = new Sprite2d("tiles/noart/testobject3");
            NoArtRefs[4] = new Sprite2d("tiles/noart/testblock6");
            NoArtRefs[5] = new Sprite2d("tiles/noart/ts1");
            NoArtRefs[6] = new Sprite2d("tiles/noart/ts2");
            NoArtRefs[7] = new Sprite2d("tiles/noart/ts3");
            NoArtRefs[8] = new Sprite2d("tiles/noart/ts4");
            NoArtRefs[9] = new Sprite2d("tiles/noart/ts5");
            NoArtRefs[10] = new Sprite2d("tiles/noart/ts6");
            NoArtRefs[11] = new Sprite2d("tiles/noart/ts7");
            NoArtRefs[12] = new Sprite2d("tiles/noart/ts8");
            NoArtRefs[13] = new Sprite2d("tiles/noart/ts9");
            NoArtRefs[14] = new Sprite2d("tiles/noart/lss1");
            NoArtRefs[15] = new Sprite2d("tiles/noart/lss2");
            NoArtRefs[16] = new Sprite2d("tiles/noart/lss3");
            NoArtRefs[17] = new Sprite2d("tiles/noart/lss4");
            NoArtRefs[18] = new Sprite2d("tiles/noart/lss5");
            NoArtRefs[19] = new Sprite2d("tiles/noart/lss6");
            NoArtRefs[20] = new Sprite2d("tiles/noart/numbertile01");
            NoArtRefs[21] = new Sprite2d("tiles/noart/numbertile02");
            NoArtRefs[22] = new Sprite2d("tiles/noart/numbertile03");
            NoArtRefs[23] = new Sprite2d("tiles/noart/numbertile04");
            NoArtRefs[24] = new Sprite2d("tiles/noart/numbertile05");
            NoArtRefs[25] = new Sprite2d("tiles/noart/numbertile06");
            NoArtRefs[26] = new Sprite2d("tiles/noart/numbertile07");
            NoArtRefs[27] = new Sprite2d("tiles/noart/numbertile08");
            NoArtRefs[28] = new Sprite2d("tiles/noart/numbertile09");
            NoArtRefs[29] = new Sprite2d("tiles/noart/numbertile0A");
        }
        /// <summary>
        /// Loads the next level using the id of the current level and art references for the selected level.
        /// </summary>
        /// <param name="currentLevel"> integer value </param>
        /// <param name="noArtRefs"> Sprite2d[] array that holds the references.</param>
        private void LoadNextLevel(string[,] currentLevel, Sprite2d[] noArtRefs)
        {
            Log.Info("New Level is being loaded");
           

            string[,] Map = currentLevel;
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "G")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[0], "Ground").CreateStatic();
                    }
                    if (Map[j, i] == ".")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[1], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "C")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[2], "Coin")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "F")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[3], "Finish")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == ":")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[4], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "1")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[5], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "2")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[6], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "3")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[7], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "4")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[8], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "5")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[9], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "6")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[10], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "7")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[11], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "8")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[12], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "9")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[13], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "l")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[14], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "v")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[15], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "e")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[16], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "s")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[17], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "w")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[18], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "t")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[19], "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "Q")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[20], "SetLevel1")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "W")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[21], "SetLevel2")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "E")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[22], "SetLevel3")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "R")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[23], "SetLevel4")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "T")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[24], "SetLevel5")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "Y")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[25], "SetLevel6")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "U")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), noArtRefs[26], "SetLevel7")/*.CreateStatic()*/;
                    }
                }

            }
            //LoadPlayerFromNextLevel(playerSpritesBitmap);
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "P")
                    {
                        player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerSpritesBitmap[0], "Player");
                        //player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerStand, "Player");
                        player.CreateDynamic();
                        //pass a list of sprites here, changing happens by animating list numbers, limited by if limits
                        //see https://www.youtube.com/results?search_query=box2d+tutorial+c%23
                        //playercollision = new Shape2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), "Player");
                        //playercollision.CreateDynamic();
                    }
                }
            }
        }

        //int timeframe = 0;
        //float x = 1;
        int times = 0; //unused
        int remainingJumpSteps = 0; //controls the applied force for the jump for this many frames. by default is 0.
        int remainingJumpFrames = 0; //unused, custom animation function variable for the jump
        int start; //used for the animateplayer function
        int end; //used for the animateplayer function

        /// <summary>
        /// function to animate the players sprites. can technically be used to animate objects too, so far not implemented.
        /// </summary>
        /// <param name="start">integer to define what element the animations starting frame is</param>
        /// <param name="end">integer to define what element the animations ending frame is</param>
        private void AnimatePlayer(int start, int end)
        {
            //Log.Info("AnimatePlayer has been called");
            slowDownFrameRate += 1;
            if (slowDownFrameRate == 4)
            {
                steps++;
                slowDownFrameRate = 0;
            }
            if (steps > end || steps < start)
            {
                steps = start;
            }
            //player = new Sprite2d(new Vector2(player.Position.X, player.Position.Y), new Vector2(32, 32), playerSprites[steps], "Player");
            player.Sprite = playerSpritesBitmap[steps];
        }

        int[] jumpFramesL = new int[] {7, 9 }; //frames for custom, out of order, jump animation. left facing.
        int[] jumpFramesR = new int[] {18, 20 }; //frames for custom, out of order, jump animation. right facing.
        /// <summary>
        /// function to animate the players sprites in a one-way manner, where animation stops at the last frame and doesnt continue animating until its fully called again.
        /// currently unused
        /// 
        /// TODO: accept custom frame definitions that are out of order.
        /// TODO: write better description for the array input
        /// </summary>
        /// <param name="start">integer to define what element the animations starting frame is</param>
        /// <param name="end">integer to define what element the animations ending frame is</param>
        /// <param name="frames">array where the frame elements are loaded from and animated from</param>
        private void AnimatePlayerOneWayOnly(int start, int end, int[] frames)
        {
            Log.Info("AnimatePlayer has been called");
            steps++;
            if (steps > end || steps < start)
            {
                steps = end;
            }
            //player.UpdateSprite(steps);
            player.Sprite = playerSpritesBitmap[steps];
        }

        /// <summary>
        /// this function updates visuals and levels when window is being updated.
        /// essentially identical in purpose to OnDraw, but i have both because its convenient to group visual aspect changes into one
        /// and physical aspect changes in functioning into the other.
        /// </summary>
        public override void OnUpdate()
        {
            if (player == null)
            {
                //return;
                //when player is null (as in doesnt exist) function is exited.
            }
            //times++;

            if (up) //applies an impulse in the up direction when up key boolean is true
            {
                player.ApplyImpulse(new Vector2(0, -160000), Vector2.Zero());
            }
            if (down) //return a console message when down key boolean is true
            {
                Log.Warning("Down has no action currently. Sprite change is visual only.");
                //TODO: shrink the collider when ducking.
            }
            if (left) //applies a velocity to the player in the left direction when left key boolean is true
            {
                player.SetVelocity(new Vector2(-120, player.GetYVelocity()));

            }
            if (right) //applies a velocity to the player in the right direction when right key boolean is true
            {
                player.SetVelocity(new Vector2(120, player.GetYVelocity()));
            }
            if (jump) //performs jumpstepping when the jump key boolean is true
            {
                if (player.IsColliding("Ground", currentLevel)!=null) //if player is colliding with the ground, only then allow the player to jump
                {
                    remainingJumpSteps = 9; //jump steps are set to 9 frames
                    remainingJumpFrames = 9; //jump aimation frames are set to 9 frames, currently unused.
                }
                jumpmode = true; //sets the jumpmode as true, the player is currently jumping
            }
            if (remainingJumpSteps > 0) //if the jump steps are greater than 0
            {
                player.SetVelocity(new Vector2(player.GetXVelocity(), -12800)); //then it applies a velocity to the player in the up direction, forming a jump
                remainingJumpSteps--; //subtract a frame from the jumpsteps
            }
            //Sprite2d ground = player.IsColliding("Ground", currentLevel);
            if (player.IsColliding("Ground",currentLevel) != null) //if player is colliding with ground then it sets current jumpmode to false, as player is not currently jumping and logs a message
            {
                jumpmode = false;
                //Log.Info("Player is colliding with Ground");
                //ground.DestroyStatic(player);
            }
            else if (player.IsColliding("Ground", currentLevel) == null) //if player is not colliding with ground then it logs a warning to the console that the player cant press jump key.
            {
                //Log.Warning("Player is not colliding with Ground and thus cannot jump.");
                //jumpstate handling for this is done in the if() structure that checks for jumpsteps
                
            }
            player.UpdatePosition(); //updates players position

            Sprite2d coin = player.IsColliding("Coin", currentLevel); //checks for collisions between the player and the coin.

            if (coin != null) //if the coin is being touched
            {
                Log.Info("Coin is being touched"); //then it logs a message to the console
                coin.DestroySelf(); //and destroys the object
            }
            Sprite2d levelfinish = player.IsColliding("Finish", currentLevel); //checks for collisions between the player and the level finishing trigger object.

            if (levelfinish != null) //if the trigger object is being touched
            {
                Log.Info("Player has triggered level finish"); //then it logs a message to the console
                levelfinish.DestroySelf(); //destroys itself
                levelClear[currentLevel] = true;
            }

            Sprite2d setlevel1 = player.IsColliding("SetLevel1", currentLevel);
            Sprite2d setlevel2 = player.IsColliding("SetLevel2", currentLevel);
            Sprite2d setlevel3 = player.IsColliding("SetLevel3", currentLevel);
            Sprite2d setlevel4 = player.IsColliding("SetLevel4", currentLevel);
            Sprite2d setlevel5 = player.IsColliding("SetLevel5", currentLevel);
            Sprite2d setlevel6 = player.IsColliding("SetLevel6", currentLevel);
            Sprite2d setlevel7 = player.IsColliding("SetLevel7", currentLevel);
            if (setlevel1 != null)
            {
                Log.Select($"player has triggered {setlevel1}");
                setlevel1.DestroySelf();
                SelectLevel(levelClear, 1); //and sets the current level as being completed
            }
            if (setlevel2 != null)
            {
                Log.Select($"player has triggered {setlevel2}");
                setlevel2.DestroySelf();
                SelectLevel(levelClear, 2); //and sets the current level as being completed
            }
            if (setlevel3 != null)
            {
                Log.Select($"player has triggered {setlevel3}");
                setlevel3.DestroySelf();
                SelectLevel(levelClear, 3); //and sets the current level as being completed
            }
            if (setlevel4 != null)
            {
                Log.Select($"player has triggered {setlevel4}");
                setlevel4.DestroySelf();
                SelectLevel(levelClear, 4); //and sets the current level as being completed
            }
            if (setlevel5 != null)
            {
                Log.Select($"player has triggered {setlevel5}");
                setlevel5.DestroySelf();
                SelectLevel(levelClear, 5); //and sets the current level as being completed
            }
            if (setlevel6 != null)
            {
                Log.Select($"player has triggered {setlevel6}");
                setlevel6.DestroySelf();
                SelectLevel(levelClear, 6); //and sets the current level as being completed
            }
            if (setlevel7 != null)
            {
                Log.Select($"player has triggered {setlevel7}");
                setlevel7.DestroySelf();
                SelectLevel(levelClear, 7); //and sets the current level as being completed
            }


            //if (player.IsColliding("Ground") != null)
            //{
            //    //Log.Info($"Collision is happening. {times}");
            //    //times ++;
            //    //player.Position.X = lastPos.X;
            //    //player.Position.Y = lastPos.Y;
            //}
            //else
            //{
            //    //lastPos.X = player.Position.X;
            //    //lastPos.Y = player.Position.Y;
            //}
            //CameraPosition.X++;
            //CameraAngle += .1f;
            //player.Position.X += x;          
            //if (timeframe>400)
            //{
            //    if (player != null)
            //    {
            //        player.DestroySelf();
            //        player = null;
            //    }
            //}
            //timeframe++;
            //Console.WriteLine($"Framecount: {frame}.");
            //frame++;
            // SPRITE LOGGING       Log.Info($"Currentsprite should be # {steps}");

        }

        private void SelectLevel(bool[] levelClear, int selectedlevel)
        {
            for (int i = 0; i < selectedlevel; i++)
            {
                levelClear[i] = true;
            }
            for (int i = selectedlevel; i < levelClear.Length; i++)
            {
                levelClear[i] = false;
            }
            levelClear[currentLevel] = true;
        }

        /// <summary>
        /// check if the player is touching any of the keys. basically an OR element for key type detection
        /// </summary>
        /// <param name="nokey">boolean for the detection</param>
        /// <returns></returns>
        public bool StillInput(bool nokey)
        {
            bool check;
            if (up == false && down == false && left == false && right == false && jump == false)
            {
                check = true;
            }
            else 
            {
                check = false;
            }
            nokey = check;
            return nokey;
        }
        /// <summary>
        /// method that restricts an unwanted non-animation unintentionality.
        /// when crouching, and pressing a movement key, without this
        /// the player will slide stuck in one of the walking animation frames
        /// this check overrides that, forcing player to prefer walking animation over crouching or standing animation
        /// </summary>
        /// <param name="LRDcheck"></param>
        /// <returns></returns>
        public bool NoCrouchWalk(bool LRDcheck)
        {
            return LRDcheck;
        }
        /// <summary>
        /// gets the keydown for keys, and sets their corresponding boolean to true
        /// </summary>
        /// <param name="e"></param>
        public override void GetKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = true; }
            if (e.KeyCode == Keys.A) { left = true; }
            if (e.KeyCode == Keys.S) { down = true; }
            if (e.KeyCode == Keys.D) { right = true; }
            if (e.KeyCode == Keys.Z) { jump = true; }
            //if (e.KeyCode == Keys.Return) { EngineCore.EngineCore.pausebuttoninput = !pausebuttoninput; }
        }


        /// <summary>
        /// gets the keyup for keys, and sets their corresponding boolean to false
        /// </summary>
        /// <param name="e"></param>
        public override void GetKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = false; }
            if (e.KeyCode == Keys.A) { left = false; }
            if (e.KeyCode == Keys.S) { down = false; }
            if (e.KeyCode == Keys.D) { right = false; }
            if (e.KeyCode == Keys.Z) { jump = false; }
        }
    }
}
