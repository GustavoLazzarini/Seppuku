//Made by Galactspace Studios

using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Tools.ScriptCreator
{
    public class ScriptableCreatorTool : EditorWindow
    {
        public string assetPath = "Scripts/Scriptable/";
        public string assetName = "ScriptName";
        public string menuName = "Game/";

        public string derivated = "ScriptableObject";

        public string[] references = new [] { "UnityEngine" };
        public ScVariable[] variables = new ScVariable[0];
        public ScMethod[] methods = new ScMethod[0];

        private void ResetDefaults() 
        {
            menuName = "Game/";
            assetName = "ScriptName";
            assetPath = "Scripts/Scriptable/";

            derivated = "ScriptableObject";
            
            references = new[] { "UnityEngine" };
            variables = new ScVariable[0];
            methods = new ScMethod[0];
        }

        [MenuItem("Tools/Scriptable Creator")]
        static void Init()
        {
            ScriptableCreatorTool window = (ScriptableCreatorTool)EditorWindow.GetWindow(typeof(ScriptableCreatorTool), false, "Scriptable Creator", true);
            window.minSize = new Vector2(620, 420);
            window.Show();
        }

        private void OnGUI() 
        {
            SerializedObject serializedObject = new SerializedObject((ScriptableObject)this);
            serializedObject.Update();

            EditorGUILayout.Space();
            
            CreateProperty("assetName", serializedObject, new GUIContent("Asset Name", "The final name of the asset (Visible on project window)"));
            CreateProperty("menuName", serializedObject, new GUIContent("Menu Name", "The path that shows on the Create menu"));
            CreateProperty("assetPath", serializedObject, new GUIContent("Asset Path", "The final path of the asset"));
            
            EditorGUILayout.Space();
            
            CreateProperty("derivated", serializedObject, new GUIContent("Mother Class", "The class that this script will derivate from"));
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                CreateProperty("references", serializedObject, new GUIContent("References", "The references that will be created inside the Scriptable"), GUILayout.MinWidth(1200), GUILayout.MaxWidth(1200));
                GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                CreateProperty("variables", serializedObject, new GUIContent("Variables", "The variables that will be created inside the Scriptable"), GUILayout.MinWidth(600), GUILayout.MaxWidth(600));
                CreateProperty("methods", serializedObject, new GUIContent("Methods", "The methods that will be created inside the Scriptable"), GUILayout.MinWidth(600), GUILayout.MaxWidth(600));
                GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();            
            
            EditorGUILayout.BeginHorizontal();
            
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Create Asset", GUILayout.Width(120), GUILayout.Height(30)))
                    CreateScript();
                GUILayout.FlexibleSpace();
            
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateProperty(string name, SerializedObject serializedObject, GUIContent label = null, params GUILayoutOption[] options)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(name), label, options);
        }

        private void CreateScript()
        {            
            string _namespace = assetPath;
            _namespace = _namespace.Replace("Scripts/", "");
            _namespace = _namespace.Substring(0, _namespace.Length - 1);
            _namespace = _namespace.Replace("/", ".");

            string script = "";
            WriteStringLine(ref script, "//Copyright Galactspace Studios 2022");
            WriteStringLine(ref script, "");
            script += CreateReferences();
            WriteStringLine(ref script, "");
            WriteStringLine(ref script, $"namespace {_namespace}");
            WriteStringLine(ref script, "{");
                WriteStringLine(ref script, $"\t[CreateAssetMenu(menuName = \"{menuName}\")]");
                WriteStringLine(ref script, $"\tpublic class {assetName} : {derivated}");
                WriteStringLine(ref script, "\t{");
                    script += CreateVars("\t\t");
                    script += CreateMethods("\t\t");
                WriteStringLine(ref script, "\t}");
            WriteStringLine(ref script, "}");

            string path = $"Assets/{assetPath}{assetName}.cs";
            
            Directory.CreateDirectory($"{Application.dataPath}/{assetPath.Substring(0, assetPath.Length - 1)}");

            StreamWriter sw = new StreamWriter(path);
            sw.Write(script);
            sw.Close();

            AssetDatabase.ImportAsset(path);
            EditorUtility.FocusProjectWindow();
        }

        private string CreateReferences(string offset = "")
        {
            string returnText = "";
            if (references.Length == 0) return returnText;
            
            returnText += $"{offset}//References\n";            

            for (int i = 0; i < references.Length; i++)
                returnText += $"using {references[i]};\n";
            
            return returnText;
        }

        private void WriteStringLine(ref string arg, string message)
        {
            arg += $"{message}\n";
        }

        private string FirstLetterLowerCase(string arg) => $"{char.ToLower(arg[0])}{arg.Substring(1)}";
        private string FirstLetterUpperCase(string arg) => $"{char.ToUpper(arg[0])}{arg.Substring(1)}";

        private string CreateVars(string offset = "")
        {
            string returnText = "";
            if (variables.Length == 0) return returnText;

            WriteStringLine(ref returnText, $"{offset}//Variables");

            for (int i = 0; i < variables.Length; i++)
            {
                ScVariable current = variables[i];

                if (current.variableType == ScType.Space)
                {
                    WriteStringLine(ref returnText, $"{offset}[Space]");
                    continue;
                }

                string varType = variables[i].variableType == ScType.Custom
                                    ? variables[i].customTypeName
                                    : FirstLetterLowerCase(variables[i].variableType.ToString());

                WriteStringLine(ref returnText, $"{offset}[SerializeField] private {varType} _{FirstLetterLowerCase(current.propertyName)};");
                if (current.createProperty) WriteStringLine(ref returnText, $"{offset}public {varType} {FirstLetterUpperCase(current.propertyName)} => _{FirstLetterLowerCase(current.propertyName)};");
            
                if (i < variables.Length - 1) WriteStringLine(ref returnText, "");
            }

            return returnText;
        }

        private string CreateMethods(string offset = "")
        {
            string returnText = "";
            if (methods.Length == 0) return returnText;

            WriteStringLine(ref returnText, $"\n{offset}//Methods");

            for (int i = 0; i < methods.Length; i++)
            {
                ScMethod current = methods[i];

                if (current.returnType == ScType.Space) continue;

                string varType = methods[i].returnType == ScType.Custom
                                    ? methods[i].customReturnType
                                    : FirstLetterLowerCase(methods[i].returnType.ToString());

                returnText += $"{offset}public {varType} {FirstLetterUpperCase(current.methodName)}(";
                
                for (int j = 0; j < current.parametersLength; j++)
                {
                    if (j < current.parametersLength - 1) returnText += $"object arg{j}, ";
                    if (j == current.parametersLength - 1) returnText += $"object arg{j}";
                }

                returnText += ")\n";

                WriteStringLine(ref returnText, $"{offset}" + "{");
                WriteStringLine(ref returnText, $"{offset}\t{(current.returnType != ScType.Void ? "return default;" : "")}");
                WriteStringLine(ref returnText, $"{offset}" + "}");
                
                if (i < methods.Length - 1) WriteStringLine(ref returnText, "");
            }

            return returnText;            
        }

        // private string CreateVariables(string offset = "")
        // {
        //     string returnText = "";
        //     if (variables.Length == 0) return returnText;

        //     returnText += $"{offset}//Variables\n";
        //     returnText += $"{offset}[Space]\n";

        //     for (int i = 0; i < variables.Length; i++)
        //     {
        //         string type = $"{variables[i].Substring(0, variables[i].IndexOf(' '))}";
                
        //         string varNameWithoutLowercase = $"{variables[i].Substring(variables[i].LastIndexOf(' ') + 1)}";
        //         string varName = $"_{char.ToLower(varNameWithoutLowercase[0])}{varNameWithoutLowercase.Substring(1)}";
                
        //         string propName = $"{char.ToUpper(varName[1])}{varName.Substring(2)}";

        //         returnText += $"{offset}[SerializeField] private {type} {varName};\n";
        //         returnText += $"{offset}public {type} {propName} => {varName};\n";

        //         if (i < variables.Length - 1) returnText += "\n";
        //     }

        //     return returnText;
        // }

        // private string CreateMethods(string offset = "")
        // {   
        //     string returnText = "";
        //     if (methods.Length == 0) return returnText;

        //     returnText += $"\n{offset}//Methods\n";

        //     for (int i = 0; i < methods.Length; i++)
        //     {
        //         returnText += $"{offset}public void {methods[i]}\n";
        //         returnText += offset + "{\n\n" + offset + "}\n";

        //         if (i < methods.Length - 1) returnText += "\n";
        //     }

        //     return returnText;
        // }
    }
}
