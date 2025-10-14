using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ScreenCreatorTool : EditorWindow
{
    private string screenName = "";
    
    [MenuItem("Tools/GUI Creator/Screen Creator")]
    public static void ShowWindow()
    {
        GetWindow<ScreenCreatorTool>("Screen Creator");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Screen Creator Tool", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        GUILayout.Label("Screen Name:");
        screenName = EditorGUILayout.TextField(screenName);
        
        GUILayout.Space(10);
        UnityEngine.GUI.enabled = !string.IsNullOrEmpty(screenName.Trim());
        if (GUILayout.Button("Create Screen", GUILayout.Height(30)))
        {
            CreateScreen();
        }
        UnityEngine.GUI.enabled = true;
    }
    
    private void CreateScreen()
    {
        string cleanName = screenName.Trim();
        if (string.IsNullOrEmpty(cleanName))
        {
            EditorUtility.DisplayDialog("Error", "Please enter a screen name.", "OK");
            return;
        }
        CreateViewScript(cleanName);
        CreateControllerScript(cleanName);
        UpdateRegistrator(cleanName);
        UpdateGuiScreenIds(cleanName);
        
        AssetDatabase.Refresh();
        
        EditorUtility.DisplayDialog("Success", $"Screen '{cleanName}' created successfully!", "OK");
        
        screenName = "";
    }
    private void CreateViewScript(string name)
    {
        string directoryPath = "Assets/Scripts/Gui/Screens/Views";
        CreateDirectoryIfNotExists(directoryPath);
        
        string fileName = $"{name}ScreenView.cs";
        string filePath = Path.Combine(directoryPath, fileName);
        
        string scriptContent = GenerateViewScript(name);
        
        File.WriteAllText(filePath, scriptContent);
    }
    
    private void CreateControllerScript(string name)
    {
        string directoryPath = "Assets/Scripts/Gui/Screens/Controllers";
        CreateDirectoryIfNotExists(directoryPath);
        
        string fileName = $"{name}ScreenController.cs";
        string filePath = Path.Combine(directoryPath, fileName);
        
        string scriptContent = GenerateControllerScript(name);
        
        File.WriteAllText(filePath, scriptContent);
    }
    
    private void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    
    private string GenerateViewScript(string name)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using UnityEngine;")
            .AppendLine("using UnityEngine.UI;")
            .AppendLine()
            .AppendLine("namespace Gui.Screens.Views")
            .AppendLine("{")
            .AppendLine($"    public class {name}ScreenView : ScreenView")
            .AppendLine("    {")
            .AppendLine()
            .AppendLine("        public override void Initialize()")
            .AppendLine("        {")
            .AppendLine($"            ID = GuiScreenIds.{name}Screen;")
            .AppendLine("        }")
            .AppendLine("    }")
            .AppendLine("}");
        
        return sb.ToString();
    }
    
    private string GenerateControllerScript(string name)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using Gui.Screens.Views;")
            .AppendLine("using UnityEngine;")
            .AppendLine()
            .AppendLine("namespace Gui.Screens.Controllers")
            .AppendLine("{")
            .AppendLine($"    public class {name}ScreenController : IScreenController")
            .AppendLine("    {")
            .AppendLine($"        private {name}ScreenView _view;")
            .AppendLine("        private IScreenManager _screenManager;")
            .AppendLine($"        public string ID => GuiScreenIds.{name}Screen;")
            .AppendLine()
            .AppendLine("        public void SetView(IScreenView view)")
            .AppendLine("        {")
            .AppendLine($"            _view = view as {name}ScreenView;")
            .AppendLine("        }")
            .AppendLine()
            .AppendLine("        public void Initialize(IScreenManager screenManager)")
            .AppendLine("        {")
            .AppendLine($"           _screenManager = screenManager;")
            .AppendLine("            _view.OnShow = RegisterListeners;")
            .AppendLine("            _view.OnHidden = RemoveListeners;")
            .AppendLine("        }")
            .AppendLine()
            .AppendLine("        private void RegisterListeners()")
            .AppendLine("        {")
            .AppendLine("        }")
            .AppendLine()
            .AppendLine("        private void RemoveListeners()")
            .AppendLine("        {")
            .AppendLine("        }")
            .AppendLine("    }")
            .AppendLine("}");
        
        return sb.ToString();
    }
    
    private void UpdateRegistrator(string name)
    {
        string[] searchPaths = {
            "Assets/Scripts",
            "Assets"
        };
        
        string registratorPath = null;
        
        foreach (var searchPath in searchPaths)
        {
            var files = Directory.GetFiles(searchPath, "ScreenControllersRegistrator.cs", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                registratorPath = files[0];
                break;
            }
        }
        
        if (registratorPath == null)
        {
            Debug.LogWarning("ScreenControllersRegistrator.cs not found. Please manually add the registration line:");
            Debug.LogWarning($"builder.Register<{name}ScreenController>(Lifetime.Singleton).As<IScreenController>();");
            return;
        }
        
        try
        {
            var content = File.ReadAllText(registratorPath);
            var registrationLine = $"            builder.Register<{name}ScreenController>(Lifetime.Singleton).As<IScreenController>();";
            
            // Find a good place to insert the registration
            // Look for existing registrations or the end of a method
            if (content.Contains("builder.Register") && content.Contains("IScreenController"))
            {
                // Find the last IScreenController registration and add after it
                var lastRegistration = content.LastIndexOf(".As<IScreenController>();");
                if (lastRegistration != -1)
                {
                    var endOfLine = content.IndexOf('\n', lastRegistration);
                    if (endOfLine != -1)
                    {
                        content = content.Insert(endOfLine + 1, registrationLine + "\n");
                    }
                }
            }
            else
            {
                // If no existing registrations found, add a comment for manual addition
                Debug.LogWarning("Could not automatically add registration. Please manually add:");
                Debug.LogWarning(registrationLine);
                return;
            }
            
            File.WriteAllText(registratorPath, content);
            Debug.Log($"Successfully added {name}ScreenController registration to ScreenControllersRegistrator");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to update ScreenControllersRegistrator: {e.Message}");
            Debug.LogWarning("Please manually add the registration line:");
            Debug.LogWarning($"builder.Register<{name}ScreenController>(Lifetime.Singleton).As<IScreenController>();");
        }
    }
    
    private void AddScreenViewComponentDelayed(string name)
    {
        // Find the created GameObject
        var screenObject = GameObject.Find($"_Context_/_GUI_/_Screens_/{name}");
        if (screenObject != null)
        {
            AddScreenViewComponent(screenObject, name);
        }
    }
    
    private void AddScreenViewComponent(GameObject screenObject, string name)
    {
        // Try to get the ScreenView type after compilation
        System.Type viewType = null;
        
        // Search through all assemblies for the type with full namespace
        string[] possibleTypeNames = {
            $"Gui.Screens.Views.{name}ScreenView",
            $"{name}ScreenView"
        };
        
        foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var typeName in possibleTypeNames)
            {
                viewType = assembly.GetType(typeName);
                if (viewType != null)
                    break;
            }
            if (viewType != null)
                break;
        }
        
        if (viewType != null/* && screenObject.GetComponent(viewType) == null*/)
        {
            screenObject.AddComponent(viewType);
            Debug.Log($"Added {name}ScreenView component to {screenObject.name}");
        }
        else if (viewType == null)
        {
            Debug.LogWarning($"Could not find {name}ScreenView type. Please manually add the component to the GameObject.");
        }
    }
    
    private void CreateScreenPrefab(string name)
    {
        var screenObject = GameObject.Find($"_Context_/_GUI_/_Screens_/{name}");
        if (screenObject == null)
        {
            Debug.LogError($"Could not find screen GameObject '{name}' to create prefab.");
            return;
        }
        
        var prefabDirectory = "Assets/Prefabs/Screens";
        CreateDirectoryIfNotExists(prefabDirectory);
        
        // Create prefab path
        var prefabPath = Path.Combine(prefabDirectory, $"{name}.prefab");
        
        // Create the prefab
        var prefab = PrefabUtility.SaveAsPrefabAsset(screenObject, prefabPath);
        
        if (prefab != null)
        {
            Debug.Log($"Successfully created prefab: {prefabPath}");
            
            // Ping the prefab in the project window to highlight it
            EditorGUIUtility.PingObject(prefab);
        }
        else
        {
            Debug.LogError($"Failed to create prefab at: {prefabPath}");
        }
    }
    
    private void UpdateGuiScreenIds(string name)
    {
        string[] searchPaths = {
            "Assets/Scripts",
            "Assets"
        };
        
        string guiScreenIdsPath = null;
        
        foreach (var searchPath in searchPaths)
        {
            var files = Directory.GetFiles(searchPath, "GuiScreenIds.cs", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                guiScreenIdsPath = files[0];
                break;
            }
        }
        
        if (guiScreenIdsPath == null)
        {
            Debug.LogWarning("GuiScreenIds.cs not found. Please manually add the screen ID:");
            Debug.LogWarning($"public const string {name}Screen = \"{name}Screen\";");
            return;
        }
        
        try
        {
            var content = File.ReadAllText(guiScreenIdsPath);
            var newConstant = $"        public const string {name}Screen = \"{name}Screen\";";
            
            // Check if the constant already exists
            if (content.Contains($"{name}Screen = \"{name}Screen\""))
            {
                Debug.Log($"{name}Screen constant already exists in GuiScreenIds");
                return;
            }
            
            // Find the position to insert the new constant
            // Look for the last public const string line before the closing brace
            var lastConstIndex = content.LastIndexOf("public const string");
            if (lastConstIndex != -1)
            {
                // Find the end of that line
                var endOfLine = content.IndexOf('\n', lastConstIndex);
                if (endOfLine != -1)
                {
                    // Insert the new constant after the last one
                    content = content.Insert(endOfLine + 1, newConstant + "\n");
                }
            }
            else
            {
                // If no constants found, try to add before the closing brace of the class
                var closingBraceIndex = content.LastIndexOf("}");
                if (closingBraceIndex != -1)
                {
                    // Find the previous closing brace (end of namespace)
                    var prevClosingBrace = content.LastIndexOf("}", closingBraceIndex - 1);
                    if (prevClosingBrace != -1)
                    {
                        content = content.Insert(prevClosingBrace, "        " + newConstant + "\n        ");
                    }
                }
            }
            
            File.WriteAllText(guiScreenIdsPath, content);
            Debug.Log($"Successfully added {name}Screen constant to GuiScreenIds");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to update GuiScreenIds: {e.Message}");
            Debug.LogWarning("Please manually add the screen ID:");
            Debug.LogWarning($"public const string {name}Screen = \"{name}Screen\";");
        }
    }
}