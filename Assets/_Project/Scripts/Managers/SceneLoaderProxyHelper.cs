using UnityEngine;

namespace LittleExplorers.Managers
{
    /// <summary>
    /// Editor scriptleri için geçici buton proxy scripti. 
    /// Runtime'da butonların Load fonksiyonlarını tetikler (Persistent listenerlar için Target obje gerektiğinden).
    /// </summary>
    public class SceneLoaderProxyHelper : MonoBehaviour
    {
        public string methodName;

        public void LoadMethod()
        {
            var loader = LittleExplorers.Managers.SceneLoader.Instance;
            if (loader != null)
            {
                loader.Invoke(methodName, 0f); // Reflection mantığı olmadan MonoBehaviour.Invoke
            }
            else
            {
                Debug.LogError("SceneLoader bulunamadı! Lütfen oyunu SplashScreen'den başlatın.");
            }
        }
    }
}
