using Microsoft.VisualBasic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
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

            var lines = source.Split("\r\n").ToList();

            // On inclut les IMPORTS

            List<string> imports = new();

            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i].Trim();

                if (line.StartsWith("#IMPORT "))
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
                        return new GeneratorResult(ResultStatusEnum.UnabledToDefineCurrentDirectory, i + 1, line);
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

                            return new GeneratorResult(ResultStatusEnum.LibraryNotFound, i + 1, line);
                        }

                        // On charge le code de la lib

                        string? libCode = null;

                        try
                        {
                            using var reader = new StreamReader(libname);
                            libCode = reader.ReadToEnd();
                            reader.Close();
                        }
                        catch
                        {
                            // Impossible de charger le code de la lib !

                            return new GeneratorResult(ResultStatusEnum.LibraryLoadError, i + 1, line);
                        }

                        if (string.IsNullOrEmpty(libCode))
                        {
                            // lib vide !

                            return new GeneratorResult(ResultStatusEnum.LibraryIsEmpty, i + 1, line);
                        }

                        // Si c'est la 1ère lib qu'on importe on ajoute un END de sécurité juste avant

                        if (imports.Count == 0)
                        {
                            lines.Add("END");
                        }

                        lines.Add($"REM IMPORT {libname}");

                        // On prend le code de la lib et on ajoute les lines à la fin du code actuel

                        var l = libCode.Replace("\t", "").Split("\r\n");

                        for (int j = 0; j < l.Length; j++)
                        {
                            lines.Add(l[j]);
                        }

                        // On référence la lib pour ne pas l'utiliser plusieurs fois

                        imports.Add(libname);

                        // On supprime la ligne d'import qu'on vient de traiter

                        lines.RemoveAt(i);
                        i -= 1;
                    }
                }
            }

            // On gère les SUB...ENDSUB

            var SubDefinitions = new Dictionary<string, SubDefinition>();

            var r = UpdateForSub(lines, SubDefinitions);
            if (r.Status != ResultStatusEnum.Success) return r;

            // On gère les blocs IF THEN ELSE ENDIF artificiels
            // Modifications du code source avant traitements classiques

            r = UpdateForIfThenElseEndif(lines);
            if (r.Status != ResultStatusEnum.Success) return r;

            // On ajoute les lignes et on stocke tous les labels avec la ligne qui correspond

            var labels = new Dictionary<string, int>();
            var defs = new Dictionary<string, Definition>();
            var consts = new Dictionary<string, string>();
            int numLine;
            List<ExportLine> exports = new();           

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

        private bool IsSubNameValid(string constName)
        {
            // Composé des lettres A...Z, a...z, 0...9, _, -, . uniquement

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

        private GeneratorResult UpdateForSub(List<string> lines, Dictionary<string, SubDefinition> subDefinitions)
        {
            int subStartLine;
            int subEndLine;
            var subName = string.Empty;
            var subParams = new List<string>();
            var subBody = new List<string>();

            // On part à la recherche d'une ligne qui commence par SUB et qui se termine obligatoirement par un ]

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i].Trim();

                if (line.ToUpper().StartsWith("SUB "))
                {
                    // On a une SUB
                    // Le format doit être SUB NomDeLaSub[p1,p2,....,pn]
                    // Où p1...pn sont les paramètres de la SUB

                    if (!line.EndsWith("]")) return new GeneratorResult(ResultStatusEnum.SubDefinitionError, i + 1, line);

                    subStartLine = i;

                    // On par à la recherche du nom de la SUB

                    var sb = new StringBuilder();
                 
                    for (int j = 4; j < line.Length; j++)
                    {
                        var c = line[j];

                        if (c != '[')
                        {
                            sb.Append(c);
                        }
                        else
                        {
                            subName = sb.ToString();

                            // On vérifie que le nom est ok

                            if (!IsSubNameValid(subName)) return new GeneratorResult(ResultStatusEnum.SubDefinitionError, i + 1, line);

                            // On vérifie que la SUB n'existe pas déjà

                            if (subDefinitions.ContainsKey(subName)) return new GeneratorResult(ResultStatusEnum.DuplicateSubDefinition, i + 1, line);

                            // On récupère les paramètres

                            var sp = line.Substring(j + 1, line.Length - j - 2);

                            // On décompose les paramètres en une liste de noms de paramètres

                            subParams = DispatchSubParameters(sp);
                            if (subParams == null) return new GeneratorResult(ResultStatusEnum.SubDefinitionError, i, line);

                            // On a le nom et les paramètres

                            break;
                        }              
                    }

                    // On récupère le corps de la SUB
                    // Il commence après la ligne SUB et se termine à ENDSUB

                    var endSubFound = false;

                    for (int j = subStartLine + 1; j < lines.Count; j++)
                    {
                        var subLine = lines[j].Trim();

                        if (subLine.ToUpper() == "ENDSUB")
                        {
                            // Fin de la sub

                            endSubFound = true;
                            subEndLine = j;
                            break;
                        }
                        else
                        {
                            subBody.Add(lines[j]);
                        }
                    }

                    if (!endSubFound) return new GeneratorResult(ResultStatusEnum.EndSubStatementMissingInSub, subStartLine + 1, lines[subStartLine].Trim());

                    // On a tous les éléments pour prendre en compte cette SUB
                    // On l'ajoute aux SUB déjà prises en compte

                    var subDefinition = new SubDefinition(subName, subParams, subBody);
                    subDefinitions[subName] = subDefinition;

                    // On ajoute à la fin du code le label correspondant à cette SUB
                    // Le label = "@SUB." + subName
                    // On modifie le body pour que les paramètres possèdent leur nom final complet "SUB." + subName + "p." + paramName

                    lines.Add("END");
                    lines.Add($"REM SUB {subDefinition.Name}");
                    lines.Add(subDefinition.LabelName);

                    for (var b = 0; b < subDefinition.Body.Count; b++)
                    {
                        var bodyLine = subDefinition.Body[b];
                        
                        foreach (var param in subDefinition.Parameters)
                        {
                            bodyLine = bodyLine.Replace(param, $"{subDefinition.StartParameterName}{param}");
                        }

                        lines.Add(bodyLine);
                    }

                    lines.Add("RETURN");

                    // On enlève les lignes de définition de la SUB dans le code d'origine

                    for (int b = 0; b < subDefinition.Body.Count + 2; b++)
                    {
                        lines.RemoveAt(subStartLine);
                    }

                    // On se replace au début du code pour chercher la SUB suivante

                    i = subStartLine - 1;

                    subStartLine = 0;
                    subEndLine = 0;
                    subName = string.Empty;
                    subParams = new();
                    subBody = new();             
                }
            }

            // On a fait le tour de toutes les SUB présentes
            // Pour chacune on part à la recherche des appels pour les transformer 

            for (int i = 0; i < lines.Count; i++)
            {
                foreach (var sub in subDefinitions.Values)
                {
                    while (true)
                    {
                        var line = lines[i];

                        var k = line.IndexOf(sub.StartSubCallName);

                        if (k == -1)
                        {
                            // Aucun appel à cette SUB sur cette ligne
                            // On sort du while

                            break;
                        }
                        else
                        {
                            // On récupère les valeurs de paramètres et la position de la fin de l'appel "]"

                            var callCode = DispatchSubCallCode(sub, line, k);

                            // Si les valeurs retournées ne sont pas au bon nombre, erreur (trop ou pas assez de paramètres passés)

                            if (callCode.values.Count != sub.Parameters.Count) return new GeneratorResult(ResultStatusEnum.WrongNumberSubParameters, i + 1, line);
     
                            // On doit créer un bloc d'appel pour cet appel dans lequel les variables locales sont affectées et la SUB appelée

                            sub.CallCounter += 1;

                            lines.Add($"REM SUB {sub.Name} CALL {sub.CallCounter}");

                            lines.Add($"{sub.StartCallName}{sub.CallCounter}");
                            
                            for (int p = 0; p < sub.Parameters.Count; p++)
                            {
                                lines.Add($"{sub.StartParameterName}{sub.Parameters[p]}={callCode.values[p]}");
                            }

                            lines.Add($"gosub {sub.LabelName}");
                            lines.Add("return");

                            // On remplace l'appel SUB par l'appel GOSUB

                            var startLine = string.Empty;
                            if (k > 0) startLine = line.Substring(0, k - 1);

                            var endLine = line.Substring(callCode.endPosition + 1);

                            lines[i] = $"{startLine}gosub {sub.StartCallName}{sub.CallCounter}{endLine}";
                        }
                    }
                }
            }

            return new GeneratorResult(ResultStatusEnum.Success);
        }

        private List<string>? DispatchSubParameters(string p)
        {
            // Les paramètres sont des noms de variables avec leur type
            // Les valeurs (chaînes,nombres) sont interdits

            var parameters = p.Split(',');

            // On vérifie que chaque item est conforme
            // Types autorisés $ ! %

            foreach (var item in parameters)
            {
                // Le 1er caractère doit être une lettre A..Z ou a..z

                var c1 = item.ToUpper()[0];
                if (c1 < 'A' || c1 > 'Z') return null;

                // Les caractères suivants (sauf le dernier) peuvent être des lettres ou des chiffres

                for (var i =1; i < item.Length - 1; i++)
                {
                    var cn = item.ToUpper()[i];
                    var valid = (cn >= 'A' && cn <= 'Z' || cn >= '0' && cn <= '9');
                    if (!valid) return null;

                }

                // Le dernier caractère peut être une lettre ou un chiffre ou % ou ! ou $

                var cf = item.ToUpper()[item.Length - 1];
                if (!(cf >= 'A' && cf <= 'Z' || cf >= '0' && cf <= '9' || cf == '%' || cf == '!' || cf == '$')) return null;
            }

            return parameters.ToList();
        }

        private (List<string> values, int endPosition) DispatchSubCallCode(SubDefinition subDefinition, string  line, int startPosition)
        {
            var values = new List<string>();
            var expressionStep = 0;
            var inString = false;
            var currentValue = new StringBuilder();
            var endPosition = 0;

            var k = startPosition + subDefinition.Name.Length + 1;

            for (int i = k; i < line.Length; i++)
            {
                var c = line[i];

                if (c == '"')
                {
                    // Si valeur vide c'est le début d'une chaine
                    // Si valeur non vide et 1er caractère = " alors c'est la fin de la chaine
                    
                    if (currentValue.Length == 0)
                    {
                        // Début de chaine

                        inString = true;
                        currentValue.Append(c);
                    }
                    else if (currentValue[0] == '"')
                    {
                        // Fin de chaine

                        inString = false;
                        currentValue.Append(c);
                        values.Add(currentValue.ToString().Trim());
                        currentValue.Clear();
                    }
                    else
                    {
                        // Guillemet dans une expression

                        currentValue.Append(c);
                    }
                }
                else if (c == '(')
                {
                    // On entre dans une expression
                    // Si on rencontre une , dans une expression alors elle ne sera pas condidérée comme un séparateur de valeur mais faisant partie de l'expression

                    expressionStep += 1;
                    currentValue.Append(c); 
                }
                else if (c == ')')
                {
                    // On sort d'une expression

                    expressionStep -= 1;
                    currentValue.Append(c);
                }
                else if (c == ',')
                {
                    if (expressionStep == 0 && !inString)
                    {
                        // On n'est pas dans une expression
                        // Ni dans une chaine
                        // C'est un séparateur de paramètre

                        if (currentValue.Length > 0)
                        {
                            values.Add((string)currentValue.ToString().Trim());
                            currentValue.Clear();
                        }
                    }
                    else
                    {
                        // La virgule fait partie de la valeur

                        currentValue.Append(c);
                    }
                }
                else if (c == ']')
                {
                    // On est à la fin de la SUB
                    // Sauf si on est déjà dans une chaine

                    if (inString)
                    {
                        currentValue.Append((string)currentValue.ToString());   
                    }
                    else
                    {
                        // Fin finale !

                        if (currentValue.Length > 0)
                        {
                            values.Add((string)currentValue.ToString().Trim());    
                            currentValue.Clear();
                        }

                        endPosition = i;

                        break;
                    }
                }
                else
                {
                    // Rien de spécial
                    // On prend

                    currentValue.Append(c);
                }
            }

            return (values, endPosition);
        }

        private GeneratorResult UpdateForIfThenElseEndif(List<string> lines)
        {
            int counter = 1;

            var thenLines = new List<string>();
            var elseLines = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim().ToUpper();

                if (line.StartsWith("IF ") && line.EndsWith(" THEN"))
                {
                    // On a trouvé un IF qui répond au format des blocs
                    // On par à la recherche des morceaux
                    // avant et après ELSE et ENDIF

                    int bloc = 1;
                    bool elseZone = false;
                    bool endifFound = false;

                    for (int j = i + 1; j < lines.Count; j++)
                    {
                        var line2 = lines[j].Trim().ToUpper();

                        if (line2.StartsWith("IF ") && line2.EndsWith(" THEN"))
                        {
                            // On a un sous IF, on entre dedans

                            bloc++;

                            // On ajoute la ligne telle qu'elle

                            if (elseZone)
                            {
                                elseLines.Add(lines[j]);
                            }
                            else
                            {
                                thenLines.Add(lines[j]);
                            }
                        }
                        else if (line2 == "ELSE")
                        {
                            // On a trouvé un ELSE de bloc
                            // Si on est dans un sous bloc on n'en tient pas compte (on l'ajoute tel quel dans la zone actuelle)
                            // Sinon on passe en zone else

                            if (bloc == 1)
                            {
                                if (elseZone)
                                {
                                    // On est déjà en zone else
                                    // On a donc plusieurs else dans le bloc IF 
                                    // Erreur !!!!

                                    return new GeneratorResult(ResultStatusEnum.DuplicateElseStatementInIfPlus, i, lines[i]);
                                }
                                else
                                {
                                    // On passe en zone else officielle

                                    elseZone = true;
                                }
                            }
                            else
                            {
                                // On est dans un sous bloc on ajoute la ligne telle qu'elle

                                if (elseZone)
                                {
                                    elseLines.Add(lines[j]);
                                }
                                else
                                {
                                    thenLines.Add(lines[j]);
                                }
                            }
                        }
                        else if (line2 == "ENDIF")
                        {
                            // On a trouvé un ENDIF 
                            // Si on est dans un sous bloc on remonte d'un bloc
                            // Sinon on a terminé le scan des lignes

                            if (bloc > 1)
                            {
                                // On est dans un sous bloc
                                // On ajoute la ligne telle qu'elle
                                // On remonte d'un bloc

                                if (elseZone)
                                {
                                    elseLines.Add(lines[j]);
                                }
                                else
                                {
                                    thenLines.Add(lines[j]);
                                }

                                bloc -= 1;
                            }
                            else if (bloc == 1)
                            {
                                // On est au ENDIF final
                                // On a terminé le scan et on a tout pour manipuler le code d'origine
                                // j = position du ENDIF
                                // i = position du IF

                                endifFound = true;

                                // On compose les labels des blocs then et else

                                var thenLabel = $"@THEN{counter}";
                                var elseLabel = $"@ELSE{counter}";

                                // On compose la ligne unique IF avec un gosub vers les blocs then et else

                                var ifLine = $"{lines[i].Trim()} GOSUB {thenLabel}";
                                if (elseZone) ifLine += $" ELSE GOSUB {elseLabel}";

                                // Si c'est le 1er IF bloc qu'on ajoute au code on ajoute un END de sécurité à la fin du code
                                // Et on crée l'entête de la zone IF+

                                if (counter == 1)
                                {
                                    lines.Add("");
                                    lines.Add("END");
                                    lines.Add("");

                                    lines.Add("REM IF+ CODE");
                                    lines.Add("");
                                }

                                // On pose le code du then

                                lines.Add(thenLabel);
                                lines.AddRange(thenLines);
                                lines.Add("RETURN");

                                // On pose le code du else s'il existe

                                if (elseZone)
                                {
                                    lines.Add(elseLabel);
                                    lines.AddRange(elseLines);
                                    lines.Add("RETURN");
                                }

                                // On remplace le IF de base par le IF+

                                lines[i] = ifLine;

                                // On supprime toutes les lignes qui composent le IF+ (jusqu'au ENDIF inclus)

                                var size = j - i;

                                for (int a = 0; a < size; a++)
                                {
                                    lines.RemoveAt(i + 1);
                                }

                                // Fini
                                // On relance la machine pour traiter les autres cas de IF+
                                // On sort de la bouche interne par break
                                // On réactive la boucle externe en affectant à i la valeur zéro
                                // On n'oublie pas d'ajouter un au counter de IF+ traités
                                // On vide les lignes de then et else

                                thenLines.Clear();
                                elseLines.Clear();
                                counter += 1;
                                i = 0;

                                break;
                            }
                        }
                        else
                        {
                            // On est dans le code du bloc IF ELSE ENDIF

                            if (line2 == "RETURN" || line2.StartsWith("RETURN:") || line2.EndsWith(":RETURN"))
                            {
                                // On vérifie que RETURN n'est pas utilisé dans le bloc car cela foutrait en l'air la structure du IF+
                                // C'est une limitation de cette façon de faire

                                return new GeneratorResult(ResultStatusEnum.ReturnStatementNotAllowedInIfPlus, j + 1, lines[j]);
                            }

                            // Suivant la zone on ajoute la liste au bloc THEN ou au bloc ELSE

                            if (elseZone)
                            {
                                elseLines.Add(lines[j]);
                            }
                            else
                            {
                                thenLines.Add(lines[j]);
                            }
                        }
                    }

                    if (!endifFound)
                    {
                        // On n'a as trouvé le ENDIF du bloc IF+ en cours d'analyse
                        // Erreur !

                        return new GeneratorResult(ResultStatusEnum.EndIfStatementMissingInIfPlus, 0, "EOF");
                    }
                }
            }

            // Fini !

            return new GeneratorResult(ResultStatusEnum.Success);
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
            DuplicateConstantDefinition,
            EndIfStatementMissingInIfPlus,
            DuplicateElseStatementInIfPlus,
            ReturnStatementNotAllowedInIfPlus,
            DuplicateSubDefinition,
            SubDefinitionError,
            EndSubStatementMissingInSub,
            WrongNumberSubParameters
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
                            ResultStatusEnum.DuplicateElseStatementInIfPlus => "ELSE ne peut apparaitre qu'une seule fois dans un bloc IF+",
                            ResultStatusEnum.EndIfStatementMissingInIfPlus => "ENDIF non trouvé à la fin d'un bloc IF+",
                            ResultStatusEnum.ReturnStatementNotAllowedInIfPlus => "RETURN non autorisé dans un bloc IF+",
                            ResultStatusEnum.DuplicateSubDefinition => "SUB définie plusieurs fois",
                            ResultStatusEnum.SubDefinitionError => "Définition d'une SUB non valide",
                            ResultStatusEnum.EndSubStatementMissingInSub => "ENDSUB non trouvé à la fin d'une SUB",
                            ResultStatusEnum.WrongNumberSubParameters => "Nombre de paramètres incorrects lors de l'appel d'une SUB",
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

            public GeneratorResult(ResultStatusEnum status)
            {
                Status = status;
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

        public class SubDefinition
        {
            public string Name { get; init; }

            public List<string> Parameters { get; init; }    

            public List<string> Body { get; init; }

            public string LabelName => $"@SUB.{Name}";

            public string StartParameterName => $"SUB.{Name}.p.";

            public string StartCallName => $"{LabelName}.call.";

            public string StartSubCallName => $"{Name}[";

            public int CallCounter { get; set; }

            public SubDefinition(string name, List<string> parameters, List<string> body)
            {
                Name = name;    
                Parameters = parameters;
                Body = body;    
            }
        }
    }
}
