using UnityEngine;

namespace LittleExplorers.Managers
{
    /// <summary>
    /// Çiftlik Hayvanları seviyesini kontrol eder.
    /// Hayvanları sahneye yerleştirir ve etkileşimleri yönetir.
    /// </summary>
    public class FarmLevelController : MonoBehaviour
    {
        [Header("Hayvan Verileri")]
        [Tooltip("Sahnede gösterilecek hayvan verileri listesi")]
        public Data.AnimalData[] animals;

        [Header("Yerleşim Ayarları")]
        [Tooltip("Hayvanların yerleştirileceği konumlar")]
        public Transform[] spawnPoints;

        [Header("Prefab")]
        [Tooltip("Hayvan prefab'ı (InteractableObject bileşeni ve SpriteRenderer içermeli)")]
        public GameObject animalPrefab;

        [Header("Ölçek")]
        [Tooltip("Hayvanların ekrandaki boyutu")]
        public float animalScale = 0.5f;

        private int discoveredCount = 0;
        private bool[] discovered;

        private void Start()
        {
            discovered = new bool[animals.Length];
            SpawnAnimals();
        }

        /// <summary>Hayvanları belirlenen konumlara yerleştirir.</summary>
        private void SpawnAnimals()
        {
            for (int i = 0; i < animals.Length; i++)
            {
                if (animals[i] == null) continue;

                if (i >= spawnPoints.Length)
                {
                    Debug.LogWarning($"Yetersiz spawn noktası! {animals[i].animalName} için yer yok.");
                    break;
                }

                GameObject animalObj = Instantiate(animalPrefab, spawnPoints[i].position, Quaternion.identity);
                animalObj.name = animals[i].animalName;

                // Ölçeği ayarla
                animalObj.transform.localScale = Vector3.one * animalScale;

                // Sprite'ı ata
                SpriteRenderer sr = animalObj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = animals[i].animalSprite;
                }

                // Çarpıştırıcıyı sprite boyutuna göre ayarla
                BoxCollider2D col = animalObj.GetComponent<BoxCollider2D>();
                if (col != null && sr != null && sr.sprite != null)
                {
                    col.size = sr.sprite.bounds.size / animalScale;
                }

                // Etkileşim bileşenini yapılandır
                int index = i; // Lambda için yakalanacak değer
                Interactions.InteractableObject interactable = animalObj.GetComponent<Interactions.InteractableObject>();
                if (interactable != null)
                {
                    interactable.onTouchSFX = animals[i].touchSFX;
                    interactable.voiceoverClip = animals[i].voiceoverClip;

                    // Dokunulduğunda keşif sayacını artır
                    interactable.onTouch.AddListener(() => OnAnimalDiscovered(index, animalObj));
                }
            }
        }

        /// <summary>Bir hayvan keşfedildiğinde çağrılır.</summary>
        private void OnAnimalDiscovered(int index, GameObject animalObj)
        {
            // Aynı hayvan zaten keşfedildiyse atla
            if (discovered[index]) return;
            discovered[index] = true;

            discoveredCount++;
            Debug.Log($"[FarmLevel] Keşfedilen hayvan: {animalObj.name} ({discoveredCount}/{animals.Length})");

            // Tüm hayvanlar keşfedildiyse seviyeyi tamamla
            if (discoveredCount >= animals.Length)
            {
                OnLevelComplete();
            }
        }

        /// <summary>Seviye tamamlandığında ödül sistemini tetikler.</summary>
        private void OnLevelComplete()
        {
            Debug.Log("[FarmLevel] Seviye tamamlandı! Tüm hayvanlar keşfedildi.");

            RewardManager reward = FindFirstObjectByType<RewardManager>();
            if (reward != null)
            {
                reward.ShowReward();
            }
        }
    }
}
