using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Hayvan Tahmin Quizi sahnesini otomatik olarak kurar.
/// Canvas, kartlar, soru metni ve geri bildirim panelini oluşturur.
/// </summary>
public class AnimalQuizSetup : EditorWindow
{
    [MenuItem("Little Explorers/Hayvan Quizi Sahnesini Kur")]
    public static void SetupQuizScene()
    {
        // GamePlay sahnesini aç
        string scenePath = "Assets/_Project/Scenes/GamePlay.unity";
        EditorSceneManager.OpenScene(scenePath);
        Scene currentScene = SceneManager.GetActiveScene();

        // Eski objeleri temizle (kamerayı koru)
        GameObject[] rootObjects = currentScene.GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (obj.name != "Main Camera")
            {
                Object.DestroyImmediate(obj);
            }
        }

        // --- Kamerayı Ayarla ---
        GameObject camObj = GameObject.FindWithTag("MainCamera");
        if (camObj != null)
        {
            Camera cam = camObj.GetComponent<Camera>();
            cam.orthographicSize = 5f;
            cam.backgroundColor = new Color(0.18f, 0.22f, 0.35f); // Koyu lacivert arka plan
            camObj.transform.position = new Vector3(0, 0, -10);

            // Ses çalabilmek için AudioListener gerekli
            if (camObj.GetComponent<AudioListener>() == null)
            {
                camObj.AddComponent<AudioListener>();
            }
        }

        // === ANA CANVAS ===
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0.5f;
        canvasObj.AddComponent<GraphicRaycaster>();

        // === ARKA PLAN PANELİ ===
        GameObject bgPanel = CreateUIElement("Background", canvasObj.transform);
        Image bgImage = bgPanel.AddComponent<Image>();
        bgImage.color = new Color(0.15f, 0.2f, 0.35f); // Koyu lacivert
        RectTransform bgRect = bgPanel.GetComponent<RectTransform>();
        StretchFull(bgRect);

        // === SORU METNİ ===
        GameObject questionObj = CreateUIElement("QuestionText", canvasObj.transform);
        Text questionText = questionObj.AddComponent<Text>();
        questionText.text = "🔊 Bu hayvan hangisi?";
        questionText.fontSize = 64;
        questionText.alignment = TextAnchor.MiddleCenter;
        questionText.color = Color.white;
        questionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        RectTransform questionRect = questionObj.GetComponent<RectTransform>();
        questionRect.anchorMin = new Vector2(0, 0.82f);
        questionRect.anchorMax = new Vector2(1, 0.95f);
        questionRect.offsetMin = Vector2.zero;
        questionRect.offsetMax = Vector2.zero;

        // === İLERLEME METNİ ===
        GameObject progressObj = CreateUIElement("ProgressText", canvasObj.transform);
        Text progressText = progressObj.AddComponent<Text>();
        progressText.text = "1 / 5";
        progressText.fontSize = 36;
        progressText.alignment = TextAnchor.MiddleCenter;
        progressText.color = new Color(0.7f, 0.7f, 0.8f);
        progressText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        RectTransform progressRect = progressObj.GetComponent<RectTransform>();
        progressRect.anchorMin = new Vector2(0.3f, 0.76f);
        progressRect.anchorMax = new Vector2(0.7f, 0.82f);
        progressRect.offsetMin = Vector2.zero;
        progressRect.offsetMax = Vector2.zero;

        // === ÜST KART ===
        GameObject topCard = CreateCard("TopCard", canvasObj.transform,
            new Vector2(0.1f, 0.42f), new Vector2(0.9f, 0.75f),
            new Color(0.95f, 0.85f, 0.55f)); // Sıcak sarı

        Button topBtn = topCard.GetComponent<Button>();
        Image topCardImg = topCard.transform.Find("AnimalImage").GetComponent<Image>();

        // === ALT KART ===
        GameObject bottomCard = CreateCard("BottomCard", canvasObj.transform,
            new Vector2(0.1f, 0.05f), new Vector2(0.9f, 0.38f),
            new Color(0.55f, 0.85f, 0.75f)); // Pastel yeşil

        Button bottomBtn = bottomCard.GetComponent<Button>();
        Image bottomCardImg = bottomCard.transform.Find("AnimalImage").GetComponent<Image>();

        // === GERİ BİLDİRİM PANELİ ===
        GameObject feedbackPanel = CreateUIElement("FeedbackPanel", canvasObj.transform);
        Image feedbackBg = feedbackPanel.AddComponent<Image>();
        feedbackBg.color = new Color(0, 0, 0, 0.6f); // Yarı saydam siyah
        RectTransform feedbackRect = feedbackPanel.GetComponent<RectTransform>();
        StretchFull(feedbackRect);

        GameObject feedbackTextObj = CreateUIElement("FeedbackText", feedbackPanel.transform);
        Text feedbackText = feedbackTextObj.AddComponent<Text>();
        feedbackText.text = "🎉 Tebrikler!";
        feedbackText.fontSize = 80;
        feedbackText.alignment = TextAnchor.MiddleCenter;
        feedbackText.color = Color.white;
        feedbackText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        RectTransform feedbackTextRect = feedbackTextObj.GetComponent<RectTransform>();
        StretchFull(feedbackTextRect);

        feedbackPanel.SetActive(false);

        // === HAYVAN VERİLERİNİ YÜKLE ===
        string dataDir = "Assets/_Project/Data";
        string[] animalFiles = { "aslan", "at", "domuz", "kedi", "kopek" };

        // Önce sprite'ların import ayarlarını kontrol et
        string spritesPath = "Assets/_Project/Sprites/Animals";
        string[] extensions = { ".jpg", ".png" };
        foreach (string animalFile in animalFiles)
        {
            foreach (string ext in extensions)
            {
                string path = $"{spritesPath}/{animalFile}{ext}";
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer != null && importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spritePixelsPerUnit = 100;
                    importer.SaveAndReimport();
                }
            }
        }

        // ScriptableObject verilerini oluştur veya yükle
        if (!AssetDatabase.IsValidFolder(dataDir))
        {
            AssetDatabase.CreateFolder("Assets/_Project", "Data");
        }

        string audioDir = "Assets/_Project/Audio/SFX";
        string[] animalDisplayNames = { "Aslan", "At", "Domuz", "Kedi", "Köpek" };
        LittleExplorers.Data.AnimalData[] animalDataList = new LittleExplorers.Data.AnimalData[animalFiles.Length];

        for (int i = 0; i < animalFiles.Length; i++)
        {
            // Ses dosyasını yükle
            string audioPath = $"{audioDir}/{animalFiles[i]}.mp3";
            AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            if (audioClip != null)
                Debug.Log($"Ses dosyası yüklendi: {audioPath}");
            else
                Debug.LogWarning($"Ses dosyası bulunamadı: {audioPath}");

            string dataPath = $"{dataDir}/{animalFiles[i]}_data.asset";
            LittleExplorers.Data.AnimalData existingData = AssetDatabase.LoadAssetAtPath<LittleExplorers.Data.AnimalData>(dataPath);

            if (existingData != null)
            {
                // Mevcut veriye ses ata ve kaydet
                existingData.touchSFX = audioClip;
                EditorUtility.SetDirty(existingData);
                animalDataList[i] = existingData;
            }
            else
            {
                // Yeni oluştur
                Sprite sprite = null;
                foreach (string ext in extensions)
                {
                    sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{spritesPath}/{animalFiles[i]}{ext}");
                    if (sprite != null) break;
                }

                LittleExplorers.Data.AnimalData data = ScriptableObject.CreateInstance<LittleExplorers.Data.AnimalData>();
                data.animalName = animalDisplayNames[i];
                data.animalSprite = sprite;
                data.touchSFX = audioClip;
                AssetDatabase.CreateAsset(data, dataPath);
                animalDataList[i] = data;
            }
        }

        // === QUIZ CONTROLLER EKLE ===
        GameObject controllerObj = new GameObject("[AnimalQuizController]");
        LittleExplorers.Managers.AnimalQuizController controller =
            controllerObj.AddComponent<LittleExplorers.Managers.AnimalQuizController>();

        controller.animals = animalDataList;
        controller.questionText = questionText;
        controller.topCardButton = topBtn;
        controller.topCardImage = topCardImg;
        controller.bottomCardButton = bottomBtn;
        controller.bottomCardImage = bottomCardImg;
        controller.feedbackPanel = feedbackPanel;
        controller.feedbackText = feedbackText;
        controller.progressText = progressText;

        // === EFEKT SESLERINİ YÜKLE VE ATA ===
        controller.correctSFX = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/SFX/correct.mp3");
        controller.wrongSFX = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/SFX/wrong.mp3");
        controller.winSFX = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/SFX/win.mp3");

        // === REWARD MANAGER EKLE ===
        GameObject rewardObj = new GameObject("[RewardManager]");
        rewardObj.AddComponent<LittleExplorers.Managers.RewardManager>();

        // === AUDIO MANAGER EKLE (Ses çalması için gerekli) ===
        GameObject audioObj = new GameObject("[AudioManager]");
        audioObj.AddComponent<LittleExplorers.Audio.AudioManager>();

        // === EVENT SYSTEM (Yeni Input System ile uyumlu) ===
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>();
        }

        // === SAHNEYI KAYDET ===
        EditorSceneManager.SaveScene(currentScene);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("✅ Hayvan Tahmin Quizi sahnesi başarıyla kuruldu!");
    }

    /// <summary>Kart UI elemanı oluşturur (arka plan + hayvan görseli).</summary>
    private static GameObject CreateCard(string name, Transform parent,
        Vector2 anchorMin, Vector2 anchorMax, Color cardColor)
    {
        // Kart arka planı
        GameObject card = CreateUIElement(name, parent);
        Image cardBg = card.AddComponent<Image>();
        cardBg.color = cardColor;
        card.AddComponent<Button>();

        // Köşeleri yuvarlatma efekti için Outline ekle
        Outline outline = card.AddComponent<Outline>();
        outline.effectColor = new Color(1, 1, 1, 0.3f);
        outline.effectDistance = new Vector2(3, 3);

        RectTransform cardRect = card.GetComponent<RectTransform>();
        cardRect.anchorMin = anchorMin;
        cardRect.anchorMax = anchorMax;
        cardRect.offsetMin = Vector2.zero;
        cardRect.offsetMax = Vector2.zero;

        // Hayvan görseli
        GameObject imageObj = CreateUIElement("AnimalImage", card.transform);
        Image animalImg = imageObj.AddComponent<Image>();
        animalImg.preserveAspect = true;
        animalImg.raycastTarget = false;
        RectTransform imgRect = imageObj.GetComponent<RectTransform>();
        // Kart içinde biraz boşluk bırak
        imgRect.anchorMin = new Vector2(0.05f, 0.05f);
        imgRect.anchorMax = new Vector2(0.95f, 0.95f);
        imgRect.offsetMin = Vector2.zero;
        imgRect.offsetMax = Vector2.zero;

        return card;
    }

    /// <summary>Temel UI elemanı oluşturur.</summary>
    private static GameObject CreateUIElement(string name, Transform parent)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform));
        obj.transform.SetParent(parent, false);
        return obj;
    }

    /// <summary>RectTransform'u tam ekran yapar.</summary>
    private static void StretchFull(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
