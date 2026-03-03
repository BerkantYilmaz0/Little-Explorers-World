using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectSetup : EditorWindow
{
    [MenuItem("Little Explorers/Temel Sahneleri Kur")]
    public static void SetupScenes()
    {
        string[] sceneNames = { "SplashScreen", "MainMenu", "LevelSelection", "GameSelection", "AnimalQuizLevel", "FruitQuizLevel" };
        string scenesPath = "Assets/_Project/Scenes";
        
        if (!AssetDatabase.IsValidFolder(scenesPath))
        {
            Debug.LogError("Hata: Assets/_Project/Scenes klasörü bulunamadı.");
            return;
        }

        EditorBuildSettingsScene[] buildScenes = new EditorBuildSettingsScene[sceneNames.Length];

        for (int i = 0; i < sceneNames.Length; i++)
        {
            string scenePath = $"{scenesPath}/{sceneNames[i]}.unity";
            
            // Yeni boş sahne oluştur
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            
            // 2D için ortografik kamera ekle
            GameObject cameraObj = new GameObject("Main Camera");
            Camera cam = cameraObj.AddComponent<Camera>();
            cam.orthographic = true;
            cameraObj.tag = "MainCamera";

            // Splash sahnesine kalıcı yönetici objeleri ekle
            if (sceneNames[i] == "SplashScreen")
            {
                GameObject managersObj = new GameObject("[PersistentManagers]");
                managersObj.AddComponent<LittleExplorers.Managers.SceneLoader>();
                managersObj.AddComponent<LittleExplorers.Audio.AudioManager>();
            }
            
            // Sahneyi kaydet
            EditorSceneManager.SaveScene(newScene, scenePath);
            Debug.Log($"Sahne oluşturuldu: {scenePath}");

            // Build Settings dizisine ekle
            buildScenes[i] = new EditorBuildSettingsScene(scenePath, true);
        }

        // Build Settings'e uygula
        EditorBuildSettings.scenes = buildScenes;
        Debug.Log("Sahneler Build Settings'e başarıyla eklendi.");
    }
}
