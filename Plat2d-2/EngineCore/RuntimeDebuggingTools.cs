using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class RuntimeDebuggingTools
    {
        public RuntimeDebuggingTools() { }

        public static bool exitSettings = false;

        /// <summary>
        /// Displays tool options in console
        /// </summary>
        public static void ToolMenu()
        {
            Console.SetCursorPosition(0, 7);
            Thread.Sleep(1);
            for (int j = 7; j < 30; j++)
            {
                Console.SetCursorPosition(0, j);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            if (exitSettings != false)
            {
                ExceptionSettings();
            }
            Console.SetCursorPosition(0, 7);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[//DEBUGUTILITY] - Select a tool:");
            Console.WriteLine($"" +
                $"enemy - focuses on an enemy currently loaded in array\n" +
                $"cheat - allows you to modify player and world clearing values\n" +
                $"logging - settings for exception logging\n" +
                $"alook - audio engine inspect logger\n" +
                $"exit - leave this menu\" +");

            string selectedTool = Console.ReadLine();
            if (selectedTool.ToLower() == "enemy")
            {
                SelectEnemyToMonitor();
            }
            else if (selectedTool.ToLower() == "cheat")
            {
                ModifyInGameValues();
            }
            else if (selectedTool.ToLower() == "logging")
            {
                exitSettings = true;
                ExceptionSettings();
            }
            else if (selectedTool.ToLower() == "alook")
            {
                exitSettings = true;
                AudioEngineInspector();
            }
            else if (selectedTool.ToLower() == "exit")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                for (int j = 7; j < 30; j++)
                {
                    Console.SetCursorPosition(0, j);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                DemoGame.exitTool = true;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Option not recognised");
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void AudioEngineInspector()
        {
            Console.SetCursorPosition(0, 7);
            Thread.Sleep(1);
            for (int j = 7; j < 30; j++)
            {
                Console.SetCursorPosition(0, j);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 7);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Audio engine inspection 0/3\n" +
                "The inspector will go through a number of data outputs about the audio engine\n" +
                "Just press enter to continue, this is so that the logging doesnt drown in cmd spam.");
            Console.ReadLine();
            Console.SetCursorPosition(0, 7);
            Console.WriteLine("Audio engine inspection 1/3\n" +
                $"songslist size:\n" +
                "Just press enter to continue, this is so that the logging doesnt drown in cmd spam.");

        }

        /// <summary>
        /// Focuses console or main window, depending on which is currently focused
        /// </summary>
        public static void ExceptionSettings()
        {
            Console.SetCursorPosition(0, 7);
            Thread.Sleep(1);
            for (int j = 7; j < 30; j++)
            {
                Console.SetCursorPosition(0, j);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 7);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"To edit a logging setting, input its number.\n" +
                $"then to enable it and its subsettings, type Y for yes or N for no\n" +
                $"under the correct columns. For example: 1yn\n" +
                $"if there is N/A, then the corresponding setting isnt available and just 1y is the input\n" +
                $"type \"exit\" to exit");

            Console.WriteLine($"Settings:\n" +
                $"==== Option name ================= Current value === Dumping to text log ====\n"+
                $"1 - Generic error messages =       {EngineCore.showRuntimeGenericError}             N/A\n" +
                $"2 - Add traces to error messages = {EngineCore.addTraceCLI}             {EngineCore.addTraceTXT}\n" +
                $"3 - IndexOutOfRange =              {EngineCore.logIndexCLI}             {EngineCore.logIndexTXT}\n" +
                $"4 - ThreadAbort =                  {EngineCore.logAbortCLI}             {EngineCore.logAbortTXT}\n" +
                $"5 - InvalidOperation =             {EngineCore.logIopexCLI}             {EngineCore.logIopexTXT}\n" +
                $"6 - Audio player logging =         {EngineCore.logIopexCLI}             {EngineCore.logIopexTXT}\n\\/");
            string settingOption = "sysdefault";
            do
            {
                settingOption = Console.ReadLine();
                if (settingOption == "")
                {
                    Console.WriteLine("Input is empty, try again");
                }
                if (settingOption == "sysdefault")
                {
                    settingOption = Console.ReadLine();
                }
                if (settingOption != "sysdefault") 
                {
                    switch (settingOption[0])
                    {
                        case var digit when !char.IsDigit(digit):
                            Console.WriteLine("not a digit.");
                            exitSettings = false;
                            break;
                        case '1':
                            if (settingOption.Count() > 2)
                            {
                                Console.WriteLine("Too many parameters, please check the input and try again");
                                break;
                            }
                            if (settingOption.ToLower()[1] == 'y')
                            { EngineCore.showRuntimeGenericError = true; }
                            else if (settingOption.ToLower()[1] == 'n')
                            { EngineCore.showRuntimeGenericError = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            break;
                        case '2':
                            if (settingOption.Count() > 3)
                            {
                                Console.WriteLine("Too many parameters, please check the input and try again");
                                break;
                            }
                            if (settingOption.ToLower()[1] == 'y')
                            { EngineCore.addTraceCLI = true; }
                            else if (settingOption.ToLower()[1] == 'n')
                            { EngineCore.addTraceCLI = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            if (settingOption.ToLower()[2] == 'y')
                            { EngineCore.addTraceTXT = true; }
                            else if (settingOption.ToLower()[2] == 'n')
                            { EngineCore.addTraceTXT = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            break;
                        case '3':
                            if (settingOption.Count() > 3)
                            {
                                Console.WriteLine("Too many parameters, please check the input and try again");
                                break;
                            }
                            if (settingOption.ToLower()[1] == 'y')
                            { EngineCore.logIndexCLI = true; }
                            else if (settingOption.ToLower()[1] == 'n')
                            { EngineCore.logIndexCLI = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            if (settingOption.ToLower()[2] == 'y')
                            { EngineCore.logIndexTXT = true; }
                            else if (settingOption.ToLower()[2] == 'n')
                            { EngineCore.logIndexTXT = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            break;
                        case '4':
                            if (settingOption.Count() > 3)
                            {
                                Console.WriteLine("Too many parameters, please check the input and try again");
                                settingOption = "";
                                break;
                            }
                            if (settingOption.ToLower()[1] == 'y')
                            { EngineCore.logAbortCLI = true; }
                            else if (settingOption.ToLower()[1] == 'n')
                            { EngineCore.logAbortCLI = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            if (settingOption.ToLower()[2] == 'y')
                            { EngineCore.logAbortTXT = true; }
                            else if (settingOption.ToLower()[2] == 'n')
                            { EngineCore.logAbortTXT = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            break;
                        case '5':
                            if (settingOption.Count() > 3)
                            {
                                Console.WriteLine("Too many parameters, please check the input and try again");
                                settingOption = "";
                                break;
                            }
                            if (settingOption.ToLower()[1] == 'y')
                            { EngineCore.logIopexCLI = true; }
                            else if (settingOption.ToLower()[1] == 'n')
                            { EngineCore.logIopexCLI = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            if (settingOption.ToLower()[2] == 'y')
                            { EngineCore.logIopexTXT = true; }
                            else if (settingOption.ToLower()[2] == 'n')
                            { EngineCore.logIopexTXT = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            break;
                        case '6':
                            if (settingOption.Count() > 3)
                            {
                                Console.WriteLine("Too many parameters, please check the input and try again");
                                settingOption = "";
                                break;
                            }
                            if (settingOption.ToLower()[1] == 'y')
                            { EngineCore.logAplayCLI = true; }
                            else if (settingOption.ToLower()[1] == 'n')
                            { EngineCore.logAplayCLI = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            if (settingOption.ToLower()[2] == 'y')
                            { EngineCore.logAplayTXT = true; }
                            else if (settingOption.ToLower()[2] == 'n')
                            { EngineCore.logAplayTXT = false; }
                            else
                            { Console.WriteLine($"Invalid parameter {settingOption.ToLower()[1]}"); }
                            break;
                        default:
                            Console.WriteLine($"invalid option {settingOption[0]}");
                            break;
                    } 
                }
            } while (settingOption.ToLower() != "exit");
            if (settingOption.ToLower() == "exit")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                for (int j = 7; j < 30; j++)
                {
                    Console.SetCursorPosition(0, j);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                exitSettings = true;
            }
            else
            {
                exitSettings = false;
            }
            Console.ReadLine();
            return;
        }

        /// <summary>
        /// Focuses console or main window, depending on which is currently focused
        /// </summary>
        public static void FocusToggle()
        {
            Log.DebugFunction("NOT IMPLEMENTED");
        }

        /// <summary>
        /// Allows the runtime modification of health, lives, crystals, ammo, score, and set current level as completed
        /// </summary>
        public static void ModifyInGameValues()
        {
            Log.DebugFunction("NOT IMPLEMENTED");
        }
        /// <summary>
        /// Tool that focuses various data on one specific enemy
        /// </summary>
        public static void SelectEnemyToMonitor()
        {
            Console.SetCursorPosition(0, 7);
            Thread.Sleep(1);
            for (int j = 7; j < 30; j++)
            {
                Console.SetCursorPosition(0, j);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 7);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[//ENEMYMONITOR] - Select enemy to monitor:");
            int i = 0;
            foreach (var enemyToMonitor in DemoGame.enemiesv2)
            {
                Console.WriteLine($"{i} - {enemyToMonitor.enemyName} || {enemyToMonitor.enemyType}");
                i++;
            }
            int selectedEnemy = int.Parse(Console.ReadLine());
            if (selectedEnemy > DemoGame.enemiesv2.Count() || selectedEnemy < 0)
            {
                Log.Error("Value out of range");
                DemoGame.logThisEnemy = false;
                DemoGame.loggedEnemyArrayID = 0;

            }
            else
            {
                DemoGame.logThisEnemy = true;
                DemoGame.loggedEnemyArrayID = selectedEnemy;
            }
            Console.BackgroundColor = ConsoleColor.Black;

        }
    }
}
