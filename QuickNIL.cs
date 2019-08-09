using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TrickyUnits;
using NLua;

namespace QuickNIL {

    class NIL_API {
        public string NILScript;
    }
    static class QuickNIL {

        static void Heading() {
            MKL.Version("", "");
            MKL.Lic("", "");
            Console.WriteLine($"QuickNIL v{MKL.Newest}");
            Console.WriteLine($"(c) Jeroen P. Broks {MKL.CYear(2019)}");
            Console.WriteLine($"Released under the terms of the GPL3\n");
        }

        static void HelpScreen() {
            Heading();
            Console.WriteLine("Usage: QuickNIL <script> [<script parameters>]");
        }

        static void RunScript(string[] args) {
            var API = new NIL_API();
            var argscript = new StringBuilder("AppArgs={}\n");
            API.NILScript = QuickStream.StringFromEmbed("NIL.lua");
            for(int i = 0; i < args.Length; i++) {
                if (i == 0) argscript.Append("AppArgs.Application ="); else argscript.Append($"AppArgs[{i}] = ");
                argscript.Append($"\"{qstr.SafeString(args[i].Replace("\\","/"))}\"\n");
            }
            var state = new Lua();
            try {
                state["QNIL"] = API;
                var initNIL = "NIL = (loadstring or load)(QNIL.NILScript)()"; //$"NIL = (loadstring or load)(\n\"{qstr.SafeString(nilscript)}\"\n,'NIL')\n()";
                Debug.WriteLine($"initNIL: {initNIL}");
                state.DoString(initNIL, "Internal: NIL");
                //var initARGS = $"local act = assert(NIL.Load(\"{qstr.SafeString(argscript.ToString())}\"))\nact()";
                //Debug.WriteLine($"--- ARGUMENTS ---\n{initARGS}\n--- END ARGUMENTS ---");
                Debug.WriteLine($"--- ARGUMENTS ---\n{argscript.ToString()}\n--- END ARGUMENTS ---");
                state.DoString(argscript.ToString(), "Interal: CLI Arguments");
                var script = QuickStream.LoadString(args[0]);
                switch (qstr.ExtractExt(args[0]).ToLower()) {
                    case "lua":
                        state.DoString(script, $"Lua:{args[0]}");
                        break;
                    case "nil":
                        state.DoString($"NIL.Load(\"{qstr.SafeString(script)}\")()", "NIL:{args[0]}");
                        break;
                    default:
                        throw new Exception("Unknown file extension!");
                }
            } catch (Exception Fout) {
                Debug.WriteLine($"ERROR: {Fout.Message}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("ERROR! ");
                Console.Beep();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Fout.Message);
                Console.ResetColor();
            }
        }

        static void Main(string[] args) {
            if (args.Length == 0)
                HelpScreen();
            else
                RunScript(args);
            TrickyDebug.AttachWait();

        }
    }
}
