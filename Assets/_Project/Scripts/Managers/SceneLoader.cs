using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleExplorers.Managers
{
    /// <summary>
    /// Sahneler arası geçişi yöneten tekil (singleton) yönetici.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadMainMenu()
        {
            LoadScene("MainMenu");
        }

        public void LoadLevelSelection()
        {
            LoadScene("LevelSelection");
        }

        public void LoadGameSelection()
        {
            LoadScene("GameSelection");
        }

        public void LoadAnimalQuizLevel()
        {
            LoadScene("AnimalQuizLevel");
        }

        public void LoadFruitQuizLevel()
        {
            LoadScene("FruitQuizLevel");
        }

        // Geriye dönük uyumluluk (Eski Play butonu bozulmasın diye tutuyoruz)
        public void LoadGamePlay()
        {
            LoadScene("GameSelection");
        }
    }
}
