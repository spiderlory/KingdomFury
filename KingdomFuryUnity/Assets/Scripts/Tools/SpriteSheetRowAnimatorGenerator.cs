using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class SpriteSheetRowAnimatorGenerator : EditorWindow
{
    Texture2D spriteSheet;
    float frameRate = 12f;
    string outputFolder = "Assets/Animations";
    float epsilon = 2f; // tolleranza verticale in pixel

    string characterName = "Hero";
    string animationNamesCSV = "Idle,Walk,Attack,Jump";

    [MenuItem("Tools/Generate Animations by Row")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SpriteSheetRowAnimatorGenerator));
    }

    void OnGUI()
    {
        GUILayout.Label("Sprite Sheet → Generatore Animazioni per Riga", EditorStyles.boldLabel);

        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("Sprite Sheet", spriteSheet, typeof(Texture2D), false);
        characterName = EditorGUILayout.TextField("Nome Personaggio", characterName);
        animationNamesCSV = EditorGUILayout.TextField("Nomi Animazioni (in ordine)", animationNamesCSV);

        frameRate = EditorGUILayout.FloatField("Frame Rate", frameRate);
        outputFolder = EditorGUILayout.TextField("Cartella Output", outputFolder);
        epsilon = EditorGUILayout.FloatField("Tolleranza Y (pixel)", epsilon);

        if (GUILayout.Button("Genera Animazioni"))
        {
            GenerateAnimations();
        }
    }

    void GenerateAnimations()
    {
        if (spriteSheet == null)
        {
            Debug.LogError("❌ Nessuno sprite sheet selezionato!");
            return;
        }

        string path = AssetDatabase.GetAssetPath(spriteSheet);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer == null || importer.spritesheet == null || importer.spritesheet.Length == 0)
        {
            Debug.LogError("❌ Sprite sheet non tagliato! Apri lo Sprite Editor e fai lo slicing prima.");
            return;
        }

        // Legge e normalizza la lista dei nomi
        var animationNames = animationNamesCSV
            .Split(',')
            .Select(a => a.Trim())
            .Where(a => !string.IsNullOrEmpty(a))
            .ToList();

        // 🔧 Raggruppa i frame per riga (coordinata Y con tolleranza)
        var rows = new Dictionary<float, List<SpriteMetaData>>();

        foreach (var meta in importer.spritesheet)
        {
            float matchedY = float.NaN;

            foreach (float key in rows.Keys)
            {
                if (Mathf.Abs(key - meta.rect.y) < epsilon)
                {
                    matchedY = key;
                    break;
                }
            }

            if (float.IsNaN(matchedY))
            {
                rows[meta.rect.y] = new List<SpriteMetaData> { meta };
            }
            else
            {
                rows[matchedY].Add(meta);
            }
        }

        // Ordina righe dall'alto verso il basso
        var orderedRows = rows.OrderByDescending(kv => kv.Key).ToList();

        // Carica gli sprite reali
        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(path)
            .Where(o => o is Sprite)
            .ToArray();

        // Ordina per nome
        System.Array.Sort(sprites, (a, b) => a.name.CompareTo(b.name));

        int rowIndex = 0;
        foreach (var kv in orderedRows)
        {
            var metas = kv.Value.OrderBy(m => m.rect.x).ToList(); // da sinistra a destra
            List<Sprite> frameList = new List<Sprite>();

            foreach (var meta in metas)
            {
                Sprite s = sprites.FirstOrDefault(sp => sp.name == meta.name) as Sprite;
                if (s != null) frameList.Add(s);
            }

            if (frameList.Count > 0)
            {
                // Determina il nome dell'animazione in base alla riga
                string animSuffix = (rowIndex < animationNames.Count) ? animationNames[rowIndex] : $"Row_{rowIndex}";
                string animName = $"{characterName}_{animSuffix}";

                CreateAnimation(frameList, animName, frameRate, outputFolder);
                Debug.Log($"✅ Creata animazione '{animName}' ({frameList.Count} frame)");
            }

            rowIndex++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"🎬 Finito! Generate {rowIndex} animazioni in {outputFolder}");
    }

    void CreateAnimation(List<Sprite> frames, string animName, float frameRate, string outputFolder)
    {
        string path = $"{outputFolder}/{animName}.anim";
        Directory.CreateDirectory(outputFolder);

        var clip = new AnimationClip();
        clip.frameRate = frameRate;

        var binding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        var keyframes = new ObjectReferenceKeyframe[frames.Count];
        for (int i = 0; i < frames.Count; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i / frameRate,
                value = frames[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
        AssetDatabase.CreateAsset(clip, path);
    }
}
