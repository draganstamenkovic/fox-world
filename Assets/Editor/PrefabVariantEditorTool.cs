using UnityEditor;
using UnityEngine;

public class PrefabVariantEditorTool : EditorWindow
{
    private string prefabPath = "Assets/Prefabs/BaseUI/Popup/UI_Popup.prefab";
    private string variantName = "UI_Popup_Variant";
    private Transform parentTransform;
    private GameObject loadedPrefab;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Prefab Variant Creator")]
    public static void ShowWindow()
    {
        PrefabVariantEditorTool window = GetWindow<PrefabVariantEditorTool>("Prefab Variant Creator");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Variant Creator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Prefab Path Section
        GUILayout.Label("Prefab Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Prefab Path:");
        prefabPath = EditorGUILayout.TextField(prefabPath);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Browse", GUILayout.Width(60)))
        {
            string selectedPath = EditorUtility.OpenFilePanel("Select Prefab", "Assets", "prefab");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                // Convert absolute path to relative path
                if (selectedPath.StartsWith(Application.dataPath))
                {
                    prefabPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                }
            }
        }

        if (GUILayout.Button("Load Prefab", GUILayout.Width(100)))
        {
            LoadPrefab();
        }

        EditorGUILayout.EndHorizontal();

        // Show prefab info if loaded
        if (loadedPrefab != null)
        {
            EditorGUILayout.HelpBox($"Loaded: {loadedPrefab.name}", MessageType.Info);
        }
        else if (!string.IsNullOrEmpty(prefabPath))
        {
            GameObject testLoad = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (testLoad == null)
            {
                EditorGUILayout.HelpBox("Prefab not found at specified path!", MessageType.Warning);
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // Variant Settings Section
        GUILayout.Label("Variant Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Variant Name:");
        variantName = EditorGUILayout.TextField(variantName);

        EditorGUILayout.LabelField("Parent Transform (Optional):");
        parentTransform = (Transform)EditorGUILayout.ObjectField(parentTransform, typeof(Transform), true);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // Action Buttons
        GUILayout.Label("Actions", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        UnityEngine.GUI.enabled = !string.IsNullOrEmpty(prefabPath);

        if (GUILayout.Button("Create Variant in Scene", GUILayout.Height(30)))
        {
            CreateVariantInScene();
        }

        EditorGUILayout.Space();
        /*
            if (GUILayout.Button("Create Variant and Save as New Prefab", GUILayout.Height(30)))
            {
                CreateAndSaveVariant();
            }

            UnityEngine.GUI.enabled = true;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // Quick Actions Section
            GUILayout.Label("Quick Actions", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");

            if (GUILayout.Button("Create UI_Popup Variant (Default Path)"))
            {
                CreateDefaultUIPopupVariant();
            }
            */
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    private void LoadPrefab()
    {
        loadedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (loadedPrefab == null)
        {
            Debug.LogError($"Could not load prefab at path: {prefabPath}");
            EditorUtility.DisplayDialog("Error", $"Could not load prefab at path:\n{prefabPath}", "OK");
        }
        else
        {
            Debug.Log($"Successfully loaded prefab: {loadedPrefab.name}");
        }
    }

    private void CreateVariantInScene()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab == null)
        {
            EditorUtility.DisplayDialog("Error", $"Could not load prefab at path:\n{prefabPath}", "OK");
            return;
        }

        // Create the variant
        GameObject variant = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        // Set name
        if (!string.IsNullOrEmpty(variantName))
        {
            variant.name = variantName;
        }

        // Set parent
        if (parentTransform != null)
        {
            variant.transform.SetParent(parentTransform);
            
            var rectTransform = variant.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.localScale = Vector2.one;
        }

        // Select the created variant
        Selection.activeGameObject = variant;

        // Focus on the created object in hierarchy
        EditorGUIUtility.PingObject(variant);

        Debug.Log($"Created prefab variant '{variant.name}' in scene hierarchy");

        // Show success message
        EditorUtility.DisplayDialog("Success", $"Created prefab variant '{variant.name}' in scene hierarchy", "OK");
    }
}