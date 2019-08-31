// Lic:
// QuickNIL
// NIL prototyping tool
// 
// 
// 
// (c) Jeroen P. Broks, 
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 19.08.31
// EndLic


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
            MKL.Version("QuickNIL - QuickNIL.cs","19.08.31");
            MKL.Lic    ("QuickNIL - QuickNIL.cs","GNU General Public License 3");
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
                var initNIL = "NIL = assert((loadstring or load)(QNIL.NILScript,\"NIL\")())"; //$"NIL = (loadstring or load)(\n\"{qstr.SafeString(nilscript)}\"\n,'NIL')\n()";
                Debug.WriteLine($"initNIL: {initNIL}");
                state.DoString(initNIL, "Internal: NIL");
                state.DoString("NIL.SayFuncs[#NIL.SayFuncs+1]=function(sayit) print(\"#say \"..tostring(sayit)) end","NIL.Say Init");
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
                        state.DoString($"local run = assert(NIL.Load(\"{qstr.SafeString(script)}\"))\nrun()", $"NIL:{args[0]}");
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


