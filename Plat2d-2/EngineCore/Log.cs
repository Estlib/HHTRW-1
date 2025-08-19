using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class Log
    {
        public int MSGCOUNT;
        public int INFOCOUNT;
        public int WARNINGCOUNT;
        public int ERRORCOUNT;
        public int HIGHCOUNT;
        public int startingline = 2;
        public static int runtimeframes = 0;

        public static void DoNothing(string msg = "", int writeToLine = 7) { }
        /// <summary>
        /// log normal message to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Normal(string msg, int writeToLine = 7)
        {
            writeToLine = ConsoleLineArrange(DemoGame.nextusableline);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[MSG] - {msg}");
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.White;
            DemoGame.nextusableline += 1;
        }
        /// <summary>
        /// log an info message to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Info(string msg, int writeToLine = 7)
        {
            writeToLine = ConsoleLineArrange(DemoGame.nextusableline);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[INFO] - {msg}");
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.White;
            DemoGame.nextusableline += 1;
        }
        /// <summary>
        /// log an info message, specified on python debugging, to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void InfoPython(string msg, int writeToLine = 7)
        {
            writeToLine = ConsoleLineArrange(DemoGame.nextusableline);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[PYTHON] - {msg.TrimEnd(' ')}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(new string(' ', Console.WindowWidth));
            DemoGame.nextusableline += 1;
        }
        /// <summary>
        /// log warning message to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Warning(string msg, int writeToLine = 7)
        {
            writeToLine = ConsoleLineArrange(DemoGame.nextusableline);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARNING] - {msg}");
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.White;
            DemoGame.nextusableline += 1;
        }
        /// <summary>
        /// log error message to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Error(string msg, int writeToLine = 7)
        {
            writeToLine = ConsoleLineArrange(DemoGame.nextusableline);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] - {msg}");
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.White;
            DemoGame.nextusableline += 1;
        }
        /// <summary>
        /// highlight a message in the console log
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Highlight(string msg, int writeToLine = 7)
        {
            writeToLine = ConsoleLineArrange(DemoGame.nextusableline);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[HIGHLIGHTED MSG] - {msg}");
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.White;
            DemoGame.nextusableline += 1;
        }
        /// <summary>
        /// log a player select message
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Select(string msg, int writeToLine = 7)
        {
            writeToLine = ConsoleLineArrange(DemoGame.nextusableline);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[PLAYER SELECT] - {msg}");
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.White;
            DemoGame.nextusableline += 1;
        }
        /// <summary>
        /// Log info about use of a debug message
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void DebugFunction(string msg, int writeToLine = 7)
        {
            writeToLine = ConsoleLineArrange(DemoGame.nextusableline);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[DEBUG FUNCTION] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
            DemoGame.nextusableline += 1;
        }
        public static void InitiateLogWindow()
        {
            //total usable lines 30
            Console.SetCursorPosition(0, 0 );
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("== Harold Harrison The Rabbit Warrior ==");
            Console.SetCursorPosition(0, 1);
            Console.WriteLine("== == == == == == ==== == == == == == ==");
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            // line 3 - game framecount
            // line 4 - animationclock
            // clear from 5


        }
        private static int ConsoleLineArrange(int writeToLine)
        {
            ;
            if (DemoGame.nextusableline > 27)
            {
                DemoGame.nextusableline = 7;
                Console.SetCursorPosition(0, 7);
            }
            else if (DemoGame.nextusableline > 7 && DemoGame.nextusableline < 27)
            {
                Console.SetCursorPosition(0, DemoGame.nextusableline);
            }

            return writeToLine;
        }
    }
    public class LogUtility
    {
        /// <summary>
        /// Wrapper for Directory.EnumerateFiles to log all files in a directory
        /// </summary>
        /// <param name="dirloc">directory of enumeration</param>
        public static void LogAllFilesFromDir(string dirloc)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[//LOGUTILITY] - Listing all files:");
            Console.ForegroundColor = ConsoleColor.White;
            var items = Directory.EnumerateFiles(dirloc);
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }

        }
        public static void LogCurrentFrame(string msg, int writeToLine = 3)
        {
            Console.SetCursorPosition(0,writeToLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[//LOGUTILITY] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogCurrentAnimationState(string msg, int writeToLine = 4)
        {
            Console.SetCursorPosition(0, writeToLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[//LOGUTILITY] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static void LogCurrentWeaponState(string msg, bool isWarning = false, int writeToLine = 5)
        {
            Console.SetCursorPosition(0, writeToLine);
            if (isWarning)
            { Console.ForegroundColor = ConsoleColor.Yellow; }
            else
            { Console.ForegroundColor = ConsoleColor.White; }
            Console.WriteLine($"[//LOGUTILITY] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void MonitorEnemy(string msg, int writeToLine = 6)
        {
            Console.SetCursorPosition(0, writeToLine);
            Console.ForegroundColor = ConsoleColor.Green; 
            Console.WriteLine($"[//LOGUTILITY] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
    public class DebugUtility : RuntimeDebuggingTools
    {
        // See RuntimeDebuggingTools for tools, this class is an appendix remainder
    }
}
