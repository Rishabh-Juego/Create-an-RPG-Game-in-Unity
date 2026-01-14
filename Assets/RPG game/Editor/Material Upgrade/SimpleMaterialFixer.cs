using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Used to update materials stuck with invalid shader keywords without causing Unity to freeze.
/// </summary>
public class SimpleMaterialFixer : EditorWindow
{
    private Vector2 scrollPosition;
    private List<string> materialsToFix = new List<string>();

    private static readonly string[] TargetShaders = {
        "Custom/Cutout_2Sided_URP",
        "Custom/Cutout_Wind_URP",
        "Fantasy Forest/StandardNoCulling_URP",
        "Custom/UnlitShadow_URP"
    };

    [MenuItem("Tools/Material Cleaner/Safe Deep Fixer")]
    public static void ShowWindow() => GetWindow<SimpleMaterialFixer>("Safe Fixer");

    private void OnGUI()
    {
        GUILayout.Label("Final Attempt: Nuclear Keyword Fixer", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This version avoids all 'Material.shader' calls to prevent Unity from freezing.", MessageType.Warning);
        
        if (GUILayout.Button("1. Find Materials", GUILayout.Height(30))) FindMaterials();

        if (materialsToFix.Count > 0)
        {
            if (GUILayout.Button($"2. Fix All {materialsToFix.Count} Materials", GUILayout.Height(30))) FixAll();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var path in materialsToFix)
            {
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat == null) continue;
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField(mat.name, GUILayout.Width(150));
                if (GUILayout.Button("Fix", GUILayout.Width(50))) FixMaterial(mat, path);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void FindMaterials()
    {
        materialsToFix.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null && mat.shader != null)
            {
                foreach (string target in TargetShaders)
                {
                    if (mat.shader.name == target) {
                        materialsToFix.Add(path);
                        break;
                    }
                }
            }
        }
    }

    private void FixAll()
    {
        try {
            for (int i = 0; i < materialsToFix.Count; i++)
            {
                if (EditorUtility.DisplayCancelableProgressBar("Fixing", materialsToFix[i], (float)i / materialsToFix.Count)) break;
                FixMaterial(AssetDatabase.LoadAssetAtPath<Material>(materialsToFix[i]), materialsToFix[i]);
            }
        } finally {
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
        }
    }

    private void FixMaterial(Material material, string path)
    {
        if (material == null) return;

        // 1. STAGE: RAW SERIALIZED RESET
        // We use SerializedObject to wipe the 'internal' state without calling C++ Material functions
        SerializedObject so = new SerializedObject(material);
        
        // Wipe keywords and invalid spaces
        SerializedProperty keywords = so.FindProperty("m_ShaderKeywords");
        if (keywords != null) keywords.stringValue = "";

        SerializedProperty invalidKeywords = so.FindProperty("m_InvalidKeywords");
        if (invalidKeywords != null) invalidKeywords.ClearArray();

        // 2. STAGE: SAVE THE TEXTURE RAW
        Texture savedTexture = GetTextureFromSerialized(so, "_MainTex") ?? GetTextureFromSerialized(so, "_BaseMap");

        // Force apply the empty keyword state to the asset file
        so.ApplyModifiedProperties();

        // 3. STAGE: RE-IMPORT TO FLUSH CACHE
        // This forces Unity to reload the material from the disk where we just wiped the keywords
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        // 4. STAGE: FINAL DATA SYNC
        // Now that the material is 'fresh', we can safely use standard calls
        Material freshMat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (savedTexture != null)
        {
            // Use low-level set to avoid property validation crashes
            freshMat.SetTexture("_MainTex", savedTexture);
            // Check for _BaseMap presence safely
            if (ShaderUtil.GetPropertyCount(freshMat.shader) > 0)
            {
                for(int i=0; i < ShaderUtil.GetPropertyCount(freshMat.shader); i++)
                {
                    if (ShaderUtil.GetPropertyName(freshMat.shader, i) == "_BaseMap")
                        freshMat.SetTexture("_BaseMap", savedTexture);
                }
            }
        }

        EditorUtility.SetDirty(freshMat);
        Debug.Log($"<color=green>Cleaned and Synced:</color> {freshMat.name}");
    }

    private Texture GetTextureFromSerialized(SerializedObject so, string propName)
    {
        SerializedProperty texEnvs = so.FindProperty("m_SavedProperties.m_TexEnvs");
        if (texEnvs == null) return null;
        for (int i = 0; i < texEnvs.arraySize; i++)
        {
            SerializedProperty entry = texEnvs.GetArrayElementAtIndex(i);
            SerializedProperty first = entry.FindPropertyRelative("first");
            if (first != null && first.stringValue == propName)
            {
                SerializedProperty texPtr = entry.FindPropertyRelative("second.m_Texture");
                return texPtr != null ? texPtr.objectReferenceValue as Texture : null;
            }
        }
        return null;
    }
}