using Microsoft.VisualBasic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmstradCpcStudio.Classes
{
    internal class CodeGenerator
    {
        public static CodeGenerator Default { get; } = new();   

        public GeneratorResult Generate(string? filename, string source)
        {
            Debug.WriteLine("");

            // On enlève les tabulations éventuelles

            source = source.Replace("\t", "");

            // On crée un tableau des lignes de code

            var lines = source.Split("\n").ToList();

            // On ajoute les lignes et on stocke tous les labels avec la ligne qui correspond

            var labels = new Dictionary<string, int>();
            var defs = new Dictionary<string, Definition>();
            var consts = new Dictionary<string, string>();
            int numLine;
            List<ExportLine> exports = new();
            List<string> imports = new();

            numLine = 90;

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i].Trim();

                var lineNumber = i + 1;

                if (line.Length > 0)
                {
                    // Les lignes de plus de 250 caractères ne sont pas supportées

                    if (line.Length > 250)
                    {
                        return new GeneratorResult(ResultStatusEnum.LineTooLong, lineNumber, line);
                    }

                    // Si la ligne commence par un numéro de ligne, c'est interdit

                    var k = line.IndexOf(' ');

                    if (k > -1)
                    {
                        var begin = line.Substring(0, k);

                        if (int.TryParse(begin, out var n))
                        {
                            // On a bien un nombre en 1er sur la ligne, c'est interdit !

                            return new GeneratorResult(ResultStatusEnum.LineNumberNotAllowed, lineNumber, line);
                        }
                    }

                    if (line.StartsWith("//"))
                    { 
                        // Commentaire ignoré dans l'export
                    }
                    else if (line.StartsWith("@"))
                    {
                        // On est en face d'une ligne de déclaration d'un label

                        // On vérifie que le label est valide 

                        if (!IsLabelValid(line))
                        {
                            // Label invalide !

                            return new GeneratorResult(ResultStatusEnum.IllegalLabelDeclaration, lineNumber, line);
                        }

                        // On stocke le label

                        if (labels.ContainsKey(line))
                        {
                            // S'il est déjà présent c'est une définition multiple --> Erreur

                            return new GeneratorResult(ResultStatusEnum.DuplicateLabelDeclaration, lineNumber, line);
                        }

                        labels[line] = numLine + 10;
                        Debug.WriteLine($"LABEL {line} FOUND AT {numLine + 10}");
                    }
                    else if (line.StartsWith("#IMPORT "))
                    {
                        // On est en face d'une ligne d'import 
                        // #IMPORT PATH FICHIER LIB

                        // On se place dans le dossier du code source
                        // Si impossible erreur

                        try
                        {
                            var path = Path.GetDirectoryName(filename);
                            Directory.SetCurrentDirectory(path ?? "");
                        }
                        catch
                        {
                            return new GeneratorResult(ResultStatusEnum.UnabledToDefineCurrentDirectory, lineNumber, line);
                        }

                        var libname = line.Substring(8);

                        if (imports.Contains(libname))
                        {
                            // Cette lib a déjà été importée
                            // Pour le moment on laisse faire
                            // Une lib peut être demandée dans plusieurs libs en cascade
                        }
                        else
                        {
                            if (!File.Exists(libname))
                            {
                                // Le fichier n'existe pas !

                                return new GeneratorResult(ResultStatusEnum.LibraryNotFound, lineNumber, line);
                            }

                            // On charge le code de la lib

                            string? libCode = null;

                            try
                            {
                                var reader = new StreamReader(libname);
                                libCode = reader.ReadToEnd();
                            }
                            catch
                            {
                                // Impossible de charger le code de la lib !

                                return new GeneratorResult(ResultStatusEnum.LibraryLoadError, lineNumber, line);
                            }                       

                            if (string.IsNullOrEmpty(libCode))
                            {
                                // lib vide !

                                return new GeneratorResult(ResultStatusEnum.LibraryIsEmpty, lineNumber, line);
                            }

                            // Si c'est la 1ère lib qu'on importe on ajoute un END de sécurité juste avant

                            if (imports.Count == 0)
                            {
                                lines.Add("END");
                            }

                            // On prend le code de la lib et on ajoute les lines à la fin du code actuel

                            var l = libCode.Replace("\t", "").Split("\n");

                            for (int j = 0; j < l.Length; j++)
                            {
                                lines.Add(l[j]);
                            }

                            // On référence la lib pour ne pas l'utiliser plusieurs fois

                            imports.Add(libname);
                        }
                    }
                    else if (line.StartsWith("#DEF "))
                    {
                        var def = Definition.Parse(line);

                        if (def.IsValid && def.Name != null)
                        {
                            if (defs.ContainsKey(def.Name))
                            {
                                return new(ResultStatusEnum.DuplicateDefinition, lineNumber, line);
                            }
                            else
                            {
                                defs[def.Name] = def;
                            }
                        }
                        else
                        {
                            return new(ResultStatusEnum.DefinitionError, lineNumber, line);
                        }                      
                    }
                    else if (line.StartsWith("#CONST "))
                    {
                        // #CONST KEY_UP=240
                        // #CONST KEY_UP=240

                        var s = line.Substring(7).Trim();
                        k = s.IndexOf('=');

                        if (k == -1)
                        {
                            // Le signe = est manquant !

                            return new GeneratorResult(ResultStatusEnum.ConstantDefinitionError, lineNumber, line); 
                        }

                        var constName = s.Substring(0, k).Trim();
                        var constValue = s.Substring(k + 1).Trim();

                        if (consts.ContainsKey(constName))
                        {
                            // Si la constante est redéfinie avec une autre valeur on sort en erreur
                            // Sinon on ne dit rien

                            if (consts[constName] != constValue)
                            {
                                return new GeneratorResult(ResultStatusEnum.DuplicateConstantDefinition, lineNumber, line); 
                            }
                        }
                        else
                        {
                            consts[constName] = constValue;
                            Debug.WriteLine($"CONST {constName} = {constValue}");
                        }
                    }
                    else
                    {
                        numLine += 10;

                        var l = new ExportLine()
                        {
                            SourceCode = line,
                            SourceLineIndex = i,
                            FinalNumLine = numLine,
                            BasicLine = $"{numLine} {line}"
                        };

                        exports.Add(l);
                    }
                }
            }

            // On trie les constantes de la plus grande à la plus petite

            var constKeys = consts.Keys.ToList();
            int constSort(string l1, string l2) => l2.CompareTo(l1);
            constKeys.Sort(constSort);

            // On remplace les constantes par leur vraie valeur

            foreach (var constName in constKeys)
            {
                for (int i = 0; i < exports.Count; i++)
                {
                    var e = exports[i];

                    if (e.FinalNumLine >= 100)
                    {
                        var newLine = e.BasicLine.Replace(constName, consts[constName]);
                        e.BasicLine = newLine;                      
                    }
                }
            }

            // On remplace les définitions par leur vraie valeur

            foreach (var defName in defs.Keys)
            {
                for (int i = 0; i < exports.Count; i++)
                {
                    var e = exports[i];

                    if (e.FinalNumLine >= 100)
                    {
                        var newLine = defs[defName].UpdateLine(e.BasicLine);

                        if (newLine != null)
                        {
                            e.BasicLine = newLine;
                        }
                        else
                        {
                            return new(ResultStatusEnum.CallDefinitionError, e.SourceLineIndex + 1, e.SourceCode);
                        }
                    }
                }
            }

            // On trie les labels du plus grand au plus petit

            var labelKeys = labels.Keys.ToList();
            int labelSort(string l1, string l2) => l2.CompareTo(l1);
            labelKeys.Sort(labelSort);

            // On remplace les labels dans les lignes par leur numéro de ligne

            for (int i = 0; i < exports.Count; i++)
            {
                foreach (var key in labelKeys)
                {
                    exports[i].BasicLine = exports[i].BasicLine.Replace(key, labels[key].ToString());
                }
            }

            // On remplace les définitions ASCII par les caractères correspondants

            foreach (var line in exports)
            {
                for (int i = 0; i < 256; i++)
                {
                    var s = new string((char)i, 1);
                    line.BasicLine = line.BasicLine.Replace($"(${i})", s);
                }
            }

            // S'il reste à l'issue des @... présents dans le code c'est qu'on a utilisé des noms de labels erronés
            // Et ils n'ont pas été remplacés

            foreach (var line in exports)
            {
                if (line.BasicLine.Contains(" @")) return new GeneratorResult(ResultStatusEnum.LabelNotFound, line.SourceLineIndex + 1, line.SourceCode);
            }

            // On insère les lignes de titre du programme (lignes 10 et 20)

            exports.Insert(0, new ExportLine()
            {
                SourceCode = "** STUDIO HEADER **",
                SourceLineIndex = 0,
                FinalNumLine = 20,
                BasicLine = $"20 REM GENERATED WITH AMSTRAD CPC STUDIO"
            });

            var name = filename == null ? "SANS NOM" : Path.GetFileNameWithoutExtension(filename).ToUpper();

            exports.Insert(0, new ExportLine()
            {
                SourceCode = "** STUDIO HEADER **",
                SourceLineIndex = 0,
                FinalNumLine = 10,
                BasicLine = $"10 REM PROGRAM {name}"
            });
          
            var sb = new StringBuilder();
            foreach( var line in exports) sb.AppendLine(line.BasicLine);

            // On retourne le code généré

            return new GeneratorResult(sb.ToString());
        }

        private bool IsLabelValid(string line)
        {
            // Commence par un @
            // Composé des lettres A...Z, a...z, 0...9, _, -, . uniquement

            if (!line.StartsWith("@")) return false;

            var l = line.Substring(1).ToCharArray();
            
            foreach (var c in l)
            {
                if (!(
                    (c >= 'A' && c <= 'Z') || 
                    (c >= 'a' && c <= 'z') || 
                    (c >= '0' && c <= '9') ||
                    c == '_' || 
                    c == '-' ||
                    c == '.')
                    ) return false;
            }
            
            return true;
        }

        private bool IsDefinitionNameValid(string constName)
        {
            // Composé des lettres A...Z, a...z, 0...9, _, -, . uniquement
            // Se termine par () obligatoirement

            if (constName.Length < 3 || !constName.EndsWith("()")) return false;

            foreach (var c in constName.Substring(0, constName.Length - 2))
            {
                if (!(
                    (c >= 'A' && c <= 'Z') ||
                    (c >= 'a' && c <= 'z') ||
                    (c >= '0' && c <= '9') ||
                    c == '_' ||
                    c == '-' ||
                    c == '.')
                    ) return false;
            }

            return true;
        }

        private class ExportLine
        {
            public int SourceLineIndex { get; set; }

            public string SourceCode { get; set; } = "";

            public string BasicLine { get; set; } = "";

            public int FinalNumLine { get; set; }
        }

        public enum ResultStatusEnum
        {
            None,
            Success,
            LineNumberNotAllowed,
            LineTooLong,
            IllegalLabelDeclaration,
            DuplicateLabelDeclaration,
            LabelNotFound,
            UnabledToDefineCurrentDirectory,
            LibraryNotFound,
            LibraryLoadError,
            LibraryIsEmpty,
            VarDefinitionError,
            DuplicateVarDefinition,
            DefinitionError,
            DuplicateDefinition,
            CallDefinitionError,
            ConstantDefinitionError,
            DuplicateConstantDefinition
        }

        public class GeneratorResult
        {
            public ResultStatusEnum Status { get; set; } = ResultStatusEnum.None;

            public int ErrorLineNumber {  get; set; }

            public string? ErrorLineCode { get; set; }  

            public string? Code { get; set; }  
            
            public string? ErrorMessage
            {
                get
                {
                    var sb = new StringBuilder();

                    if (Status == ResultStatusEnum.Success)
                    {
                        sb.Append("OK");
                    }
                    else
                    {
                        var errorLabel = Status switch
                        {
                            ResultStatusEnum.Success => "Succès.",
                            ResultStatusEnum.LineNumberNotAllowed => "Les numéros de lignes ne sont pas autorisés.",
                            ResultStatusEnum.DuplicateLabelDeclaration => "Label déclaré plusieurs fois.",
                            ResultStatusEnum.IllegalLabelDeclaration => "Déclaration de label invalide.",
                            ResultStatusEnum.LabelNotFound => "Label introuvable.",
                            ResultStatusEnum.UnabledToDefineCurrentDirectory => "Impossible de déterminer le dossier de travail.",
                            ResultStatusEnum.LibraryNotFound => "Bibliothèque introuvable.",
                            ResultStatusEnum.LibraryIsEmpty => "Bibliothèque vide.",
                            ResultStatusEnum.LibraryLoadError => "Chargement de la bibliothèque impossible.",
                            ResultStatusEnum.LineTooLong => "Ligne trop longue.",
                            ResultStatusEnum.VarDefinitionError => "Erreur de définition de variable étendue.",
                            ResultStatusEnum.DuplicateVarDefinition => "Variable étendue déclarée plusieurs fois.",
                            ResultStatusEnum.DefinitionError => "Définition non valide.",
                            ResultStatusEnum.DuplicateDefinition => "Définition déclarée plusieurs fois.",
                            ResultStatusEnum.CallDefinitionError => "Appel incorrect d'une définition.",
                            ResultStatusEnum.ConstantDefinitionError => "Erreur de définition d'une constante.",
                            ResultStatusEnum.DuplicateConstantDefinition => "Constante déjà définie avec une valeur différente.",
                            ResultStatusEnum.None => "Aucun.",
                            _ => "Statut inconnu !"
                        };

                        sb.AppendLine(errorLabel);
                        sb.AppendLine($"à la ligne {ErrorLineNumber}");
                        sb.AppendLine("");
                        sb.AppendLine(ErrorLineCode);
                    }

                    return sb.ToString();
                }
            }

            public GeneratorResult(ResultStatusEnum status, int errorLineNumber, string errorLineCode)
            {
                Status = status;
                ErrorLineNumber = errorLineNumber;
                ErrorLineCode = errorLineCode;
            }

            public GeneratorResult(string code)
            {
                Status = ResultStatusEnum.Success;
                Code = code;
            }
        }

        public class Definition
        {
            public string? Name { get; set; }

            public List<string> Parameters { get; } = [];

            public string? Value { get; set; }

            public bool IsValid { get; private set; }

            private static bool IsValidName(string name)
            {
                // Composé des lettres A...Z, a...z, 0...9, _, -, . uniquement

                foreach (var c in name.Substring(0, name.Length - 2))
                {
                    if (!(
                        (c >= 'A' && c <= 'Z') ||
                        (c >= 'a' && c <= 'z') ||
                        (c >= '0' && c <= '9') ||
                        c == '_' ||
                        c == '-' ||
                        c == '.')
                        ) return false;
                }

                return true;
            }

            public static Definition Parse(string input)
            {
                // Format attendu 
                // #DEF name(v1,v2,v3,....,vn)=BASIC DEFINITION
                // #DEF display(m,x,y)=LOCATE {x},{y}:PRINT {m};

                var def = new Definition();

                if (input != null  && input.StartsWith("#DEF "))
                {
                    var k1 = input.IndexOf('[');

                    if (k1 > -1)
                    {
                        var name = input.Substring(5, k1 - 5);

                        if (IsValidName(name))
                        {
                            var k2 = input.IndexOf("=");

                            if (k2 > -1)
                            {
                                var basicInstructions = input.Substring(k2 + 1);

                                var p = input.Substring(5 + name.Length + 1, k2 - 5 - name.Length - 2);
                                var ps = p.Split(',');

                                def.Name = name;
                                def.Value = basicInstructions;

                                for (int i = 0; i < ps.Length; i++)
                                {
                                    if (ps[i].Length > 0) def.Parameters.Add(ps[i]);
                                }
                                
                                def.IsValid = true; 
                            }
                        }                        
                    }
                }

                return def;
            }

            public string? UpdateLine(string line)
            {
                string newLine = line;

                if (Name != null && Value != null && IsValid)
                {
                    while (true)
                    {
                        var k1 = newLine.IndexOf($"{Name}[");
                        var k2 = 0;
                        var parametersCount = 0;

                        if (k1 > -1)
                        {
                            var inString = false;
                            var index = k1 + Name.Length + 1;
                            var currentValue = "";
                            var values = new Dictionary<string, string>();

                            while (index < newLine.Length)
                            {
                                var c = newLine[index];

                                if (currentValue.Length == 0)
                                {
                                    // On commence une nouvelle valeur

                                    if (c == '\"')
                                    {
                                        // On commence une string

                                        inString = true;
                                    }
                                    else if (c == ']')
                                    {
                                        // Fin (pas de paramètre)

                                        k2 = index;
                                        break;
                                    }
                                    else
                                    {
                                        // On commence autre chose qu'une string
                                    }

                                    // On stocke le caractère

                                    currentValue += c;
                                }
                                else
                                {
                                    if (inString)
                                    {
                                        // On est en train de composer une string

                                        currentValue += c;

                                        // On attend un " pour la terminer

                                        if (c == '\"')
                                        {
                                            inString = false;
                                        }
                                    }
                                    else
                                    {
                                        // On est en train de composer autre chose qu'une string
                                        // Si on trouve une , on passe à la valeur suivante
                                        // Si on trouve une ) on arrête tout là et on stocke l'emplacement de )

                                        if (c == ',')
                                        {
                                            parametersCount += 1;

                                            if (Parameters.Count >= values.Count + 1)
                                            {
                                                var p = Parameters[values.Count];
                                                values[p] = currentValue;
                                            }
                                            currentValue = "";
                                        }
                                        else if (c == ']')
                                        {
                                            parametersCount += 1;

                                            if (Parameters.Count >= values.Count + 1)
                                            {
                                                var p = Parameters[values.Count];
                                                values[p] = currentValue;
                                            }
                                            k2 = index;
                                            break;
                                        }
                                        else
                                        {
                                            currentValue += c;
                                        }
                                    }
                                }

                                index += 1;
                            }

                            if (k2 == 0)
                            {
                                // Si k2=0 c'est qu'on n'a pas trouvé la ) de fin
                                // Erreur !!!

                                return null;
                            }
                            else if (parametersCount != Parameters.Count)
                            {
                                // Le nombre de paramètres n'est pas bon !

                                return null;
                            }
                            else
                            {
                                // On décompose la ligne en 2 parties
                                // Avant et après l'appel à la définition

                                var begin = newLine.Substring(0, k1);
                                var end = newLine.Substring(k2 + 1);

                                // On doit remplacer chaque paramètre {x} par sa valeur dans l'instruction basic

                                var basic = Value;

                                foreach (var key in values.Keys)
                                {
                                    var pn = $"[{key}]";
                                    basic = basic.Replace(pn, values[key]);
                                }

                                newLine = $"{begin}{basic}{end}";
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return newLine;
            }
        }
    }
}
