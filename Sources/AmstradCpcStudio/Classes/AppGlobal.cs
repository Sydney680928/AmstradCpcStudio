using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmstradCpcStudio.Classes
{
    internal class AppGlobal
    {
        public static readonly Dictionary<string, BasicCommand> BasicCommands = new();
        
        public static readonly Dictionary<string, BasicFunction> BasicFunctions = new();

        static AppGlobal()
        {
            // Commandes basic

            CreateBasicCommands();

            // Fonctions basic

            CreateBasicFunctions();

        }

        private static void CreateBasicCommands()
        {
            AddBasicCommand("after", "Appelle un sous-programme après qu'un certain temps se soit écoulé.", "after duration gosub @label", "after duration,timer gosub @label");

            AddBasicCommand("border", "Définit la couleur de la bordure de l'écran.", "border couleur", "border couleur1,couleur2");

            AddBasicCommand("call", "Appelle un sous-programme externe", "call address", "call address,param1,param2,...");
        }

        private static void CreateBasicFunctions()
        {
            AddBasicFunction("abs", "Retourne la valeur absolue d'un nombre.", "abs(x)");

            AddBasicFunction("asc", "Retourne le valeur ASCII de la 1ère lettre d'une chaîne de caractères.", "asc(\"xyz\")");

            AddBasicFunction("atn", "Retourne l'Arc Tangente d'un angle.", "atn(x)");

            AddBasicFunction("bin$", "Retourne la conversion en binaire d'un nombre.", "bin$(value)", "bin$(value, size)");
        }

        private static void AddBasicCommand(string name, string description, params string[] samples)
        {
            var command = new BasicCommand(name, description, samples);
            BasicCommands[name] = command;  
        }

        private static void AddBasicFunction(string name, string description, params string[] samples)
        {
            var function = new BasicFunction(name, description, samples);
            BasicFunctions[name] = function;
        }
    }
}
