using System.Collections;
using UnityEngine;

namespace LittleExplorers.Managers
{
    /// <summary>
    /// Seviye tamamlandığında konfeti/yıldız efekti ve kutlama paneli gösteren ödül sistemi.
    /// </summary>
    public class RewardManager : MonoBehaviour
    {
        [Header("Ödül Paneli")]
        [Tooltip("Kutlama paneli (varsayılan olarak kapalı)")]
        public GameObject rewardPanel;

        [Header("Parçacık Efektleri")]
        [Tooltip("Konfeti parçacık sistemi")]
        public ParticleSystem confettiEffect;

        [Header("Ses")]
        [Tooltip("Kutlama ses efekti")]
        public AudioClip celebrationSFX;

        [Header("Yıldız Animasyonu")]
        [Tooltip("Yıldız objeleri (saklanacak ve tek tek gösterilecek)")]
        public GameObject[] stars;

        /// <summary>Ödül gösterimini başlatır.</summary>
        public void ShowReward()
        {
            StartCoroutine(RewardSequence());
        }

        private IEnumerator RewardSequence()
        {
            // Kutlama sesi çal
            if (celebrationSFX != null && Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlaySFX(celebrationSFX);
            }

            // Konfeti efektini başlat
            if (confettiEffect != null)
            {
                confettiEffect.Play();
            }

            // Paneli göster
            if (rewardPanel != null)
            {
                rewardPanel.SetActive(true);
            }

            // Yıldızları sırayla göster (her biri küçülerek açılan animasyonla)
            if (stars != null)
            {
                for (int i = 0; i < stars.Length; i++)
                {
                    if (stars[i] != null)
                    {
                        stars[i].SetActive(true);
                        stars[i].transform.localScale = Vector3.zero;

                        // Basit ölçekleme animasyonu
                        float elapsed = 0f;
                        float duration = 0.3f;
                        while (elapsed < duration)
                        {
                            elapsed += Time.deltaTime;
                            float t = elapsed / duration;
                            // Zıplama efekti: hedef boyutun biraz üstüne çıkıp geri gelir
                            float scale = t < 0.7f
                                ? Mathf.Lerp(0f, 1.2f, t / 0.7f)
                                : Mathf.Lerp(1.2f, 1f, (t - 0.7f) / 0.3f);
                            stars[i].transform.localScale = Vector3.one * scale;
                            yield return null;
                        }
                        stars[i].transform.localScale = Vector3.one;

                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }

            Debug.Log("[RewardManager] Ödül gösterimi tamamlandı!");
        }

        /// <summary>Ödül panelini kapatır ve ana menüye döner.</summary>
        public void OnContinueButtonPressed()
        {
            if (rewardPanel != null)
            {
                rewardPanel.SetActive(false);
            }

            // Ana menüye dön
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadLevelSelection();
            }
        }
    }
}
