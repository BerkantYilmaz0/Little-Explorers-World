using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Mini Oyun Merkezi (Game Selection) sahnesini otomatik olarak kurar.
/// </summary>
public class GameSelectionSetup : EditorWindow
{
    [MenuItem("Little Explorers/Oyun Seçim Ekranını Kur")]
    public static void SetupGameSelectionScene()
    {
        // GameSelection sahnesini aç
        string scenePath = "Assets/_Project/Scenes/GameSelection.unity";
        
        // Eğer sahne yoksa ProjectSetup ile oluşturulmalıydı ama yinede kontrol edelim:
        if (!AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath))
        {
            Debug.LogError("Oyun Seçim sahnesi bulunamadı. Önce 'Temel Sahneleri Kur' çalıştırılmalı.");
            return;
        }

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
            cam.backgroundColor = new Color(0.85f, 0.92f, 0.98f); // Açık mavi arka plan
            camObj.transform.position = new Vector3(0, 0, -10);

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

        // === BAŞLIK METNİ ===
        GameObject titleObj = CreateUIElement("TitleText", canvasObj.transform);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "OYNAMAK İSTEDİĞİN OYUNU SEÇ";
        titleText.fontSize = 60;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0.2f, 0.3f, 0.5f);
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.1f, 0.85f);
        titleRect.anchorMax = new Vector2(0.9f, 0.95f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        // === OYUN KARTLARI KONTEYNERİ (Horizontal Layout) ===
        GameObject containerObj = CreateUIElement("GamesContainer", canvasObj.transform);
        RectTransform containerRect = containerObj.GetComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.05f, 0.3f);
        containerRect.anchorMax = new Vector2(0.95f, 0.7f);
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;

        HorizontalLayoutGroup layoutGroup = containerObj.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.spacing = 50;

        // Hayvan görselini bul (kullanıcı "at" istendi)
        Sprite animalSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Sprites/Animals/at.jpg");
        if (animalSprite == null) animalSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Sprites/Animals/at.png"); // Fallback

        // === HAYVANLARI BİL BUTONU ===
        GameObject btn1Obj = CreateImageButton("Btn_AnimalQuiz", containerObj.transform, 
            "Hayvanları Bil", animalSprite);
            
        Button btn1 = btn1Obj.GetComponent<Button>();
        CreateSceneLoaderProxy(btn1Obj, "LoadAnimalQuizLevel");

        // Meyve görseli şimdilik yok, placeholder kullanıyoruz
        Sprite fruitSprite = null; // İleride eklenecek

        // === MEYVELERİ BİL BUTONU ===
        GameObject btn2Obj = CreateImageButton("Btn_FruitQuiz", containerObj.transform, 
            "Meyveleri Bil", fruitSprite);
            
        Button btn2 = btn2Obj.GetComponent<Button>();
        CreateSceneLoaderProxy(btn2Obj, "LoadFruitQuizLevel");

        // === ANA MENÜYE DÖN BUTONU ===
        GameObject backBtnObj = CreateButton("Btn_BackToMenu", canvasObj.transform, 
            "⬅ Ana Menüye Dön", new Color(0.8f, 0.8f, 0.8f), 
            new Vector2(0.2f, 0.05f), new Vector2(0.8f, 0.15f));
            
        backBtnObj.GetComponentInChildren<Text>().fontSize = 40;
        CreateSceneLoaderProxy(backBtnObj, "LoadMainMenu");

        // === EVENT SYSTEM ===
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

        Debug.Log("✅ Oyun Seçim (GameSelection) sahnesi kuruldu!");
    }

    private static GameObject CreateUIElement(string name, Transform parent)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform));
        obj.transform.SetParent(parent, false);
        return obj;
    }

    private static GameObject CreateButton(string name, Transform parent, string text, Color color, Vector2 anchorMin, Vector2 anchorMax)
    {
        GameObject btnObj = CreateUIElement(name, parent);
        Image bgImg = btnObj.AddComponent<Image>();
        bgImg.color = color;
        btnObj.AddComponent<Button>();

        // Outline
        Outline outline = btnObj.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.2f);
        outline.effectDistance = new Vector2(2, -2);

        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Text
        GameObject textObj = CreateUIElement("Text", btnObj.transform);
        Text t = textObj.AddComponent<Text>();
        t.text = text;
        t.fontSize = 60;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.black;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        RectTransform tRect = textObj.GetComponent<RectTransform>();
        tRect.anchorMin = Vector2.zero;
        tRect.anchorMax = Vector2.one;
        tRect.offsetMin = Vector2.zero;
        tRect.offsetMax = Vector2.zero;

        return btnObj;
    }

    private static GameObject CreateImageButton(string name, Transform parent, string text, Sprite iconSprite)
    {
        // Ana Konteyner Objesi
        GameObject containerObj = CreateUIElement(name, parent);
        LayoutElement le = containerObj.AddComponent<LayoutElement>();
        le.preferredWidth = 450;
        le.preferredHeight = 550;
        
        // İçerik için Vertical Layout Group
        VerticalLayoutGroup vLayout = containerObj.AddComponent<VerticalLayoutGroup>();
        vLayout.childAlignment = TextAnchor.MiddleCenter;
        vLayout.childControlHeight = true;
        vLayout.childControlWidth = true;
        vLayout.childForceExpandHeight = false;
        vLayout.childForceExpandWidth = false;
        vLayout.spacing = 20;

        // Görsel butonu kısmı
        GameObject imgObj = CreateUIElement("ImageButton", containerObj.transform);
        LayoutElement imgLe = imgObj.AddComponent<LayoutElement>();
        imgLe.preferredHeight = 400;
        imgLe.preferredWidth = 400;
        imgLe.flexibleHeight = 0;

        Image iconImg = imgObj.AddComponent<Image>();
        iconImg.preserveAspect = true;
        if (iconSprite != null)
        {
            iconImg.sprite = iconSprite;
        }
        else
        {
            iconImg.color = new Color(0.8f, 0.8f, 0.8f, 1f); // Görsel yoksa gri kutu
        }

        // Tıklanabilirlik için Button ekle
        Button btn = imgObj.AddComponent<Button>();
        // Efekt (buton gölgesi vs.)
        Outline outline = imgObj.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.3f);
        outline.effectDistance = new Vector2(5, -5);

        // Metin kısmı
        GameObject textObj = CreateUIElement("Text", containerObj.transform);
        LayoutElement textLe = textObj.AddComponent<LayoutElement>();
        textLe.preferredHeight = 100;
        textLe.flexibleHeight = 0;

        Text t = textObj.AddComponent<Text>();
        t.text = text;
        t.fontSize = 50;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = new Color(0.2f, 0.3f, 0.5f); // Başlık rengiyle uyumlu
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // Aksiyonların container üzerinden verilmesi için container'ı döndür, 
        // ama butonu container'a değil imgObj'ye koyduğumuz için container üstüne dummy bir buton atıp olayları oraya çekebiliriz.
        // Veya tıklaması kolay olsun diye container'ı buton yaparız.
        
        // Alternatif: Container'ı Button yapıp tıklanabilir alanını max yapalım
        DestroyImmediate(btn); // Yukarıdaki image butonunu sil
        DestroyImmediate(outline);
        
        Image containerBg = containerObj.AddComponent<Image>();
        containerBg.color = new Color(1,1,1,0); // Tamamen transparan arka plan (tıklama alanı için)
        containerObj.AddComponent<Button>(); // Asıl buton container

        return containerObj;
    }

    /// <summary>
    /// Buttonlara SceneLoader özelliklerini bağlamak için küçük bir komut tutucu script ekleriz
    /// </summary>
    private static void CreateSceneLoaderProxy(GameObject btnObj, string methodName)
    {
        // Unity'nin kendi Editor içinden SceneLoader Instance bulması zordur (Singletons runtime'da yaşar)
        // Bunun yerine butona tıklanınca SceneManager kullanacak bir UIEventProxy veya kod parçası gerekir.
        // Şimdilik LittleExplorers.Managers.SceneLoader referans almak yerine kolay bir Utility Script oluşturabiliriz
        
        var proxy = btnObj.AddComponent<LittleExplorers.Managers.SceneLoaderProxyHelper>();
        proxy.methodName = methodName;
        
        Button btn = btnObj.GetComponent<Button>();
        UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, proxy.LoadMethod);
    }
}
