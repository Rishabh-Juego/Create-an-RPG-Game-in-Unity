using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class MaterialCleaner : Editor
{
    // Mapping Old Shader Name -> New Shader Name
    private static readonly Dictionary<string, string> ShaderMapping = new Dictionary<string, string>
    {
        { "Custom/cutout", "Custom/Cutout_Wind_URP" },
        { "Custom/UnlitShadow", "Custom/UnlitShadow_URP" },
        { "Custom/Cutout_2Sided", "Custom/Cutout_2Sided_URP" },
        { "Fantasy Forest/StandardNoCulling", "Fantasy Forest/StandardNoCulling_URP" }
    };

    private static readonly List<string> NewURPShaders = new List<string>(ShaderMapping.Values);

    [MenuItem("Tools/Shader Utilities/Clean and Audit Materials")]
    public static void CleanAndAudit()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");
        int cleanedCount = 0;
        int upgradedCount = 0;
        int legacyErrorCount = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null || mat.shader == null) continue;

            string currentShaderName = mat.shader.name;

            // 1. Check if it's one of the NEW shaders that needs keyword cleaning
            if (NewURPShaders.Contains(currentShaderName))
            {
                CleanMaterialKeywords(mat);
                cleanedCount++;
            }
            // 2. Check if it's one of the OLD shaders we know about
            else if (ShaderMapping.ContainsKey(currentShaderName))
            {
                Debug.LogWarning($"[MaterialCleaner] Material '{mat.name}' is using an old known shader: {currentShaderName}. Swapping to URP version...");
                UpgradeShader(mat, ShaderMapping[currentShaderName]);
                upgradedCount++;
            }
            // 3. General check: Does the shader source contain "CGPROGRAM"?
            else if (IsLegacyCGShader(mat.shader))
            {
                Debug.LogError($"[MaterialCleaner] LEGACY DETECTED: Material '{mat.name}' uses CGPROGRAM at path: {path}. Shader Name: {currentShaderName}");
                legacyErrorCount++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Audit Complete",
            $"Cleaned Keywords: {cleanedCount}\nAuto-Upgraded: {upgradedCount}\nLegacy CG Errors: {legacyErrorCount}\n\nCheck Console for details.", "OK");
    }

    private static void CleanMaterialKeywords(Material mat)
    {
        Undo.RecordObject(mat, "Clean Keywords");
        mat.shaderKeywords = new string[0]; // Wipe stale keywords
        EditorUtility.SetDirty(mat);
    }

    private static void UpgradeShader(Material mat, string newShaderName)
    {
        Shader newShader = Shader.Find(newShaderName);
        if (newShader != null)
        {
            Undo.RecordObject(mat, "Upgrade Shader");
            mat.shader = newShader;
            mat.shaderKeywords = new string[0]; // Clean keywords during swap
            EditorUtility.SetDirty(mat);
        }
        else
        {
            Debug.LogError($"[MaterialCleaner] Could not find shader: {newShaderName}");
        }
    }

    private static bool IsLegacyCGShader(Shader shader)
    {
        string path = AssetDatabase.GetAssetPath(shader);
        if (string.IsNullOrEmpty(path) || !path.EndsWith(".shader")) return false;

        try
        {
            // We only need to check the first few hundred lines usually
            string content = File.ReadAllText(path);
            return content.Contains("CGPROGRAM");
        }
        catch
        {
            return false;
        }
    }
}