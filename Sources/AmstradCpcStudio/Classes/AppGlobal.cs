using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmstradCpcStudio.Classes
{
    internal class AppGlobal
    {
        public static readonly List<BasicCommand> BasicCommands = new();
        
        public static readonly List<BasicFunction> BasicFunctions = new();

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

            AddBasicCommand("call", "Appelle un sous-programme externe.", "call address", "call address,param1,param2,...");

            AddBasicCommand("chain", "Charge un programme à partir de la cassette dans la mémoire, remplaçant le programme existant.", "chain \"nom_dossier\"", "chain \"nom_dossier\",ligne");

            AddBasicCommand("chain merge", "Amalgame un programme sur cassette avec celui de la mémoire.", "chain \"merge nom_dossie\"r", "chain merge \"nom_dossier,ligne\"", "chain merge \"nom_dossier\",ligne,DELETE debut-fin");

            AddBasicCommand("clear", "Efface toutes les variables et fichiers.", "clear");

            AddBasicCommand("clg", "Efface l'écran graphique.", "clg");

            AddBasicCommand("closein", "Ferme le fichier d'entrée ouvert sur la cassette.", "closein");

            AddBasicCommand("closeout", "Ferme le fichier de sortie ouvert sur la cassette.", "closein");

            AddBasicCommand("cls", "Efface l'écran ou la fenêtre d'écran.", "cls", "cls #x");

            AddBasicCommand("cont", "Continue l'exécution du programme un *Break*, stop ou end, si le programme n'a pas été modifé.", "cont");

            AddBasicFunction("data", "Déclare qu'une donnée est constante pour la durée du programme.", "data 5,3,\"toto\"");

            AddBasicFunction("def fn", "Permet de définir et de se servir de fonctons simples de calcul de valeurs.", "def fn sqare(x)=x*x");

            AddBasicCommand("defint", "Définit le type int par défaut.", "defint G,W-Z");

            AddBasicCommand("defstr", "Définit le type string par défaut.", "defstr G,W-Z");

            AddBasicCommand("defreal", "Définit le type real par défaut.", "defreal G,W-Z");

            AddBasicCommand("deg", "Etablit le mode de calcul en degré.", "deg");

            AddBasicCommand("di", "Annule une interruption jusqu'à ce qu'elle soit remise en route par la commande ei.", "di");

            AddBasicCommand("dim", "Déclare un tableau allant de 0 à value.", "dim b$(value)");

            AddBasicCommand("draw", "Dessine une ligne à partir de la position du curseur vers une position absolue.", "draw x,y,mask");

            AddBasicCommand("drawr", "Dessine une ligne à partir de la position du curseur vers une position relative.", "drawr x,y,mask");

            AddBasicCommand("ei", "Rétablit une interruption supprimée par la commande di.", "di");

            AddBasicCommand("end", "Met fin au programme.", "end");

            AddBasicCommand("ent", "Fait varier le ton d'un son.\r\nLes paramètres step,size,time\r\nou period,pause\r\npeuvent apparaitre 5 fois.", "ent enveloppe,step,size,time", "ent period,pause");

            AddBasicCommand("env", "Fait varier le volume d'un son.\r\nLes paramètres step,size,time\r\nou period,pause\r\npeuvent apparaitre 5 fois.", "env enveloppe,step,size,time", "env period,pause");

            AddBasicCommand("erase", "Supprime de la mémoire un tableau déclaré avec la commande dim.", "erase x,y,z$");

            AddBasicCommand("error", "Déclenche l'erreur avec son code.", "error code");

            AddBasicCommand("every", "Appelle un sous-programme à intervalle régulier.", "every duration gosub @label", "every duration,timer gosub @label");

            AddBasicCommand("for", "Exécute le corps d'un programme un certain nombre de fois, le saut d'une valeur à l'autre éctant commandé par la valeur du 'step'. Si on ne précise pas, 'step' prend la valeur 1 par défaut.", "for i=x to y", "for i=x to y step z");

            AddBasicCommand("gosub", "Appelle un sous-programme BASIC en se branchant sur la ligne indiquée.", "gosub 500", "gosub @label");

            AddBasicCommand("goto", "Se branche sur la ligne indiquée.", "goto 500", "goto @label");

            AddBasicCommand("if", "Détermine le branchement à réaliser suivant le resultat d'une condition.", "if condition then {code}", "if condition then {code} else {code}", "if condition then\n {code} \nendif", "if condition then\n {code} \nelse\n {code} \nendif");

            AddBasicCommand("ink", "Définit pour une encre sa ou ses couleurs.", "ink numink,color", "ink numink,color1,color2");

            AddBasicCommand("input", "Lit les données venant d'un canal précisé. Un ; après la chaine fait qu'un ? apparait, une , supprime le ?.", "input a$", "input \"prompt\n;a$", "input \"prompt\",a$","input #canal;a$", "input #canal;\"prompt\n;a$", "input #canal;\"prompt\",a$");

            AddBasicCommand("key", "Définit une nouvelle touche de fonction. 32 caractères possibles entre 128 et 159.", "key numkey,\"string\"");

            AddBasicCommand("key def", "Change le caractère généré par une touche.", "key def numkey,repeat,char,charwithshift,charwithcontrol");

            AddBasicCommand("input", "Lit une ligne entière venant d'un canal précisé. Un ; après la chaine fait qu'un ? apparait, une , supprime le ?.", "input a$", "input \"prompt\n;a$", "input \"prompt\",a$", "input #canal;a$", "input #canal;\"prompt\n;a$", "input #canal;\"prompt\",a$");

            AddBasicCommand("load", "Lit un programme BASIC depuis la casette remplaçant celui qui s'y trouve déjà.", "load \"progname\"", "load \"progname\",address");

            AddBasicCommand("locate", "Déplace le curseur de texte du canal spécifié.", "locate #canal,posx,posy", "locate posx,posy");

            AddBasicCommand("memory", "Rétablit les paramètres mémoire du BASIC pour changer la quantité de mémoire disponible en fixant l'adresse de l'octet le plus élévé.", "memory address");

            AddBasicCommand("merge", "Remplace un programme sur cassette avec celui de la mémoire.", "merge \"nom_dossier\"");

        }

        private static void CreateBasicFunctions()
        {
            AddBasicFunction("abs", "Retourne la valeur absolue d'un nombre.", "abs(x)");

            AddBasicFunction("asc", "Retourne le valeur ASCII de la 1ère lettre d'une chaîne de caractères.", "asc(\"xyz\")");

            AddBasicFunction("atn", "Retourne l'Arc Tangente d'un angle.", "atn(x)");

            AddBasicFunction("bin$", "Retourne la conversion en binaire d'un nombre.", "bin$(value)", "bin$(value, size)");

            AddBasicFunction("chr$", "Retourne le caractère ASCII d'après son code", "chr$(code)");

            AddBasicFunction("cint", "Convertit une valeur numérique en un entier compris entre -32768 et 32767", "cint(valeur)");

            AddBasicFunction("cos", "Retourne le cosinus d'un angle.", "cos(angle)");

            AddBasicFunction("creal", "Convertit une valeur numérique en un réel.", "creal(value)");

            AddBasicFunction("eof", "Teste si l'entrée cassette est à la fin du fichier.\n-1 = vrai = on est à la fin.\n0 = faux = on n'est pas encore à la fin", "eof");

            AddBasicFunction("err", "Retourne la dernière erreur déclenchée.", "err");

            AddBasicFunction("erl", "Retourne la ligne où s'est déclenchée la dernière erreur.", "erl");

            AddBasicFunction("exp", "Retourne 'e' à la puissance donnée par l'expression numérique.", "exp(value)");

            AddBasicFunction("fix", "a la différence de 'cint', 'fix' n'arrondit pas à l'entier le plus proche, il enlève simplemet la partie décimale.", "fix(value)");

            AddBasicFunction("free", "Retourne la quantité de mémoire disponible. La forme 'free(\"\")' force l'ordinateur à mettre de l'ordre avant de donner la valeur disponible.", "free(0)", "free(\"\")");

            AddBasicFunction("hex$", "Retourne un nombre entier en nombre hexadécimal.", "hex$(value)");

            AddBasicFunction("himem", "Retourne l'adresse de l'octet le plus élevé dans la mémoire utilisée par la BASIC.", "himem");

            AddBasicFunction("inkey", "Interroge le clavier pour indiquer quelles touches sont pressées.\n-1 = non pressée.\n0 = pressée.\n32 = pressé + shift.\n128 = pressé + ctrl.\n160 = pressé + shift + ctrl.", "inkey(touche)");

            AddBasicFunction("inkey$", "Retourne le caractère actuellement tapé au clavier", "inkey$");

            AddBasicFunction("inp", "Retourne la valeur d'entrée dans le canal entrée/sortie précisé par l'adresse.", "inp(adresse)");

            AddBasicFunction("instr", "Retourne l'emplacement d'une chaîne dans une autre en commençant au caractère défini par le premier paramètre.", "instr(2,\"BANANE\",\"AN\")");

            AddBasicFunction("int", "Arrondit au plus petit nombre entier.", "int(value)");

            AddBasicFunction("joy", "Lit l'état sur 6 bits du joystick stipulé.\nbit 0 = haut.\nbit 1 = bas.\nbit 2 = gauche.\nbit 3 = droite.\nbit 4 = fire  2.\n5 = fire 1", "joy(numjoy)");

            AddBasicFunction("left$", "Retourne les x premiers caractères d'une chaîne.", "left$(x$,5)");

            AddBasicFunction("len", "Retourne la longueur d'une chaîne", "len(x$)");

            AddBasicFunction("log", "Retourne le logarithme naturel de l'expression numérique.", "log(value)");

            AddBasicFunction("log10", "Retourne le logarithme à base 10 de l'expression numérique.", "log10(value)");

            AddBasicFunction("lower$", "Retourne le chaîne passée en paramètre en minuscule.", "lower$(x$)");

            AddBasicFunction("max", "Retourne la plus grande valeur de la liste de paramètres.", "max(1,5,8,2)");

            AddBasicFunction("memory", "Retourne le chaîne passée en paramètre en minuscule.", "lower$(x$)");

            AddBasicFunction("mid$", "Retourne une partie de la chaîne passée en paramètre.", "mid$(x$,start)","mid$(x$,start,len)");
        }

        private static void AddBasicCommand(string name, string description, params string[] samples)
        {
            var command = new BasicCommand(name, description, samples);
            BasicCommands.Add(command);  
        }

        private static void AddBasicFunction(string name, string description, params string[] samples)
        {
            var function = new BasicFunction(name, description, samples);
            BasicFunctions.Add(function);
        }
    }
}
