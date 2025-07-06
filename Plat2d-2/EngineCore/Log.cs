using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        /// <summary>
        /// log normal message to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Normal(string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[MSG] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// log an info message to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[INFO] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// log warning message to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Warning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARNING] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// log error message to console
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// highlight a message in the console log
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Highlight(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[HIGHLIGHTED MSG] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// log a player select message
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void Select(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[PLAYER SELECT] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Log info about use of a debug message
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void DebugFunction(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[DEBUG FUNCTION] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
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
            Console.WriteLine(Directory.EnumerateFiles(dirloc));
        }
    }
}
