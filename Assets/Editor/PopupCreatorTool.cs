using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PopupCreatorTool : EditorWindow
    {
        private string popupName = "";

        [MenuItem("Tools/GUI Creator/Popup Creator")]
        public static void ShowWindow()
        {
            GetWindow<PopupCreatorTool>("Popup Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Popup Creator Tool", EditorStyles.boldLabel);
            GUILayout.Space(10);

            GUILayout.Label("Popup Name:");
            popupName = EditorGUILayout.TextField(popupName);

            GUILayout.Space(10);
            UnityEngine.GUI.enabled = !string.IsNullOrEmpty(popupName.Trim());
            if (GUILayout.Button("Create Popup", GUILayout.Height(30)))
            {
                CreatePopup();
            }

            UnityEngine.GUI.enabled = true;
        }

        private void CreatePopup()
        {
            string cleanName = popupName.Trim();
            if (string.IsNullOrEmpty(cleanName))
            {
                EditorUtility.DisplayDialog("Error", "Please enter a popup name.", "OK");
                return;
            }
            
            CreateViewScript(cleanName);
            CreateControllerScript(cleanName);
            UpdateRegistrator(cleanName);
            UpdatePopupIds(cleanName);

            AssetDatabase.Refresh();

            EditorApplication.delayCall += () =>
            {
                AddPopupViewComponentDelayed(cleanName);
                CreatePopupPrefab(cleanName);
            };

            EditorUtility.DisplayDialog("Success", $"Popup '{cleanName}' created successfully!", "OK");

            popupName = "";
        }
        private void CreateViewScript(string name)
        {
            string directoryPath = "Assets/Scripts/GUI/Popups/Views";
            CreateDirectoryIfNotExists(directoryPath);

            string fileName = $"{name}PopupView.cs";
            string filePath = Path.Combine(directoryPath, fileName);

            string scriptContent = GenerateViewScript(name);

            File.WriteAllText(filePath, scriptContent);
        }

        private void CreateControllerScript(string name)
        {
            string directoryPath = "Assets/Scripts/GUI/Popups/Controllers";
            CreateDirectoryIfNotExists(directoryPath);

            string fileName = $"{name}PopupController.cs";
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
                .AppendLine("namespace Gui.Popups.Views")
                .AppendLine("{")
                .AppendLine($"    public class {name}PopupView : PopupView")
                .AppendLine("    {")
                .AppendLine("        public override void Initialize()")
                .AppendLine("        {")
                .AppendLine("            base.Initialize();")
                .AppendLine($"            ID = PopupIds.{name}Popup;")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("}");

            return sb.ToString();
        }

        private string GenerateControllerScript(string name)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Gui.Popups.Views;")
                .AppendLine("using UnityEngine;")
                .AppendLine()
                .AppendLine("namespace Gui.Popups.Controllers")
                .AppendLine("{")
                .AppendLine($"    public class {name}PopupController : IPopupController")
                .AppendLine("    {")
                .AppendLine("        private IPopupManager _popupManager;")
                .AppendLine($"        private {name}PopupView _view;")
                .AppendLine($"        public string ID => PopupIds.{name}Popup;")
                .AppendLine()
                .AppendLine("        public void SetView(IPopupView view)")
                .AppendLine("        {")
                .AppendLine($"            _view = view as {name}PopupView;")
                .AppendLine("        }")
                .AppendLine()
                .AppendLine("        public void Initialize(IPopupManager popupManager)")
                .AppendLine("        {")
                .AppendLine($"           Debug.Log(\"Initializing {name} Popup\");")
                .AppendLine("            _popupManager = popupManager;")
                .AppendLine("            _view.OnShow = RegisterListeners;")
                .AppendLine("            _view.OnHidden = RemoveListeners;")
                .AppendLine("        }")
                .AppendLine()
                .AppendLine("        private void RegisterListeners()")
                .AppendLine("        {")
                .AppendLine("            _view.BackgroundButton.onClick.AddListener(HidePopup);")
                .AppendLine("        }")
                .AppendLine()
                .AppendLine("        private void RemoveListeners()")
                .AppendLine("        {")
                .AppendLine("            _view.BackgroundButton.onClick.RemoveListener(HidePopup);\n")
                .AppendLine("        }")
                .AppendLine()
                .AppendLine("        private void HidePopup()")
                .AppendLine("        {")
                .AppendLine("            _popupManager.HidePopup(ID);")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("}");

            return sb.ToString();
        }

        private void UpdateRegistrator(string name)
        {
            string[] searchPaths =
            {
                "Assets/Scripts",
                "Assets"
            };

            string registratorPath = null;

            foreach (var searchPath in searchPaths)
            {
                var files = Directory.GetFiles(searchPath, "PopupControllersRegistrator.cs",
                    SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    registratorPath = files[0];
                    break;
                }
            }

            if (registratorPath == null)
            {
                Debug.LogWarning(
                    "PopupControllersRegistrator.cs not found. Please manually add the registration line:");
                Debug.LogWarning(
                    $"builder.Register<{name}PopupController>(Lifetime.Singleton).As<IPopupController>();");
                return;
            }

            try
            {
                var content = File.ReadAllText(registratorPath);
                var registrationLine =
                    $"            builder.Register<{name}PopupController>(Lifetime.Singleton).As<IPopupController>();";

                if (content.Contains("builder.Register") && content.Contains("IPopupController"))
                {
                    var lastRegistration = content.LastIndexOf(".As<IPopupController>();");
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
                    Debug.LogWarning("Could not automatically add registration. Please manually add:");
                    Debug.LogWarning(registrationLine);
                    return;
                }

                File.WriteAllText(registratorPath, content);
                Debug.Log($"Successfully added {name}PopupController registration to PopupControllersRegistrator");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to update PopupControllersRegistrator: {e.Message}");
                Debug.LogWarning("Please manually add the registration line:");
                Debug.LogWarning(
                    $"builder.Register<{name}PopupController>(Lifetime.Singleton).As<IPopupController>();");
            }
        }

        private void AddPopupViewComponentDelayed(string name)
        {
            // Find the created GameObject
            var popupObject = GameObject.Find($"_Context_/_GUI_/_Popups_/{name}");
            if (popupObject != null)
            {
                AddPopupViewComponent(popupObject, name);
            }
        }

        private void AddPopupViewComponent(GameObject popupObject, string name)
        {
            System.Type viewType = null;

            string[] possibleTypeNames =
            {
                $"GUI.Popups.Views.{name}PopupView",
                $"{name}PopupView"
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

            if (viewType != null && popupObject.GetComponent(viewType) == null)
            {
                popupObject.AddComponent(viewType);
                Debug.Log($"Added {name}PopupView component to {popupObject.name}");
            }
            else if (viewType == null)
            {
                Debug.LogWarning(
                    $"Could not find {name}PopupView type. Please manually add the component to the GameObject.");
            }
        }

        private void CreatePopupPrefab(string name)
        {
            var popupObject = GameObject.Find($"_Context_/_GUI_/_Popups_/{name}");
            if (popupObject == null)
            {
                Debug.LogError($"Could not find popup GameObject '{name}' to create prefab.");
                return;
            }

            var prefabDirectory = "Assets/Prefabs/Popups";
            CreateDirectoryIfNotExists(prefabDirectory);

            // Create prefab path
            var prefabPath = Path.Combine(prefabDirectory, $"{name}.prefab");

            // Create the prefab
            var prefab = PrefabUtility.SaveAsPrefabAsset(popupObject, prefabPath);

            if (prefab != null)
            {
                Debug.Log($"Successfully created prefab: {prefabPath}");
                
                EditorGUIUtility.PingObject(prefab);
            }
            else
            {
                Debug.LogError($"Failed to create prefab at: {prefabPath}");
            }
        }

        private void UpdatePopupIds(string name)
        {
            string[] searchPaths =
            {
                "Assets/Scripts",
                "Assets"
            };

            string popupIdsPath = null;

            foreach (var searchPath in searchPaths)
            {
                var files = Directory.GetFiles(searchPath, "PopupIds.cs", SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    popupIdsPath = files[0];
                    break;
                }
            }

            if (popupIdsPath == null)
            {
                Debug.LogWarning("PopupIds.cs not found. Please manually add the popup ID:");
                Debug.LogWarning($"public const string {name}Popup = \"{name}Popup\";");
                return;
            }

            try
            {
                var content = File.ReadAllText(popupIdsPath);
                var newConstant = $"        public const string {name}Popup = \"{name}Popup\";";

                if (content.Contains($"{name}Popup = \"{name}Popup\""))
                {
                    Debug.Log($"{name}Popup constant already exists in PopupIds");
                    return;
                }
                
                var lastConstIndex = content.LastIndexOf("public const string");
                if (lastConstIndex != -1)
                {
                    var endOfLine = content.IndexOf('\n', lastConstIndex);
                    if (endOfLine != -1)
                        content = content.Insert(endOfLine + 1, newConstant + "\n");
                }
                else
                {
                    var closingBraceIndex = content.LastIndexOf("}");
                    if (closingBraceIndex != -1)
                    {
                        var prevClosingBrace = content.LastIndexOf("}", closingBraceIndex - 1);
                        if (prevClosingBrace != -1)
                        {
                            content = content.Insert(prevClosingBrace, "        " + newConstant + "\n        ");
                        }
                    }
                }

                File.WriteAllText(popupIdsPath, content);
                Debug.Log($"Successfully added {name}Popup constant to PopupIds");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to update PopupIds: {e.Message}");
                Debug.LogWarning("Please manually add the popup ID:");
                Debug.LogWarning($"public const string {name}Popup = \"{name}Popup\";");
            }
        }
    }
}