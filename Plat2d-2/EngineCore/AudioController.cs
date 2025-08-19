using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class AudioController
    {
        /// <summary>
        ///     CTOR
        /// </summary>
        public AudioController()
        {
             
        }
        public static void RunScript(string thisScript)
        {
            var scriptDir = AppDomain.CurrentDomain.BaseDirectory;
            //string[] itemsInDir = Directory.GetFiles(scriptDir);
            //Log.Highlight($"Directory is:\n{scriptDir}");
            //Log.Highlight($"Listed in here:\n");
            //foreach (string item in itemsInDir) 
            //{ 
            //    Log.Info($"{item}"); 
            //}
            Runtime.PythonDLL = @"C:\Program Files (x86)\Python313-32\python313.dll";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                try
                {
                    dynamic sys = Py.Import("sys");
                    sys.path.append(scriptDir);  

                    var pythonScript = Py.Import(thisScript);
                    var message = new PyString("helloworld");

                    var result = pythonScript.InvokeMethod("say_hello");
                    Log.InfoPython($"{result}");
                }
                catch (Exception ex)
                {
                    DemoGame.pausebuttoninput = true;
                    Log.Error("Python has encountered an error:");
                    Log.Error(ex.ToString());
                }
                
            }
            PythonEngine.Shutdown();
        }
    }
}
