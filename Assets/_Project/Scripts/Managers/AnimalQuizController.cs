using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LittleExplorers.Managers
{
    /// <summary>
    /// Hayvan tahmin quizi kontrolcüsü.
    /// Bir hayvan sesi çalar, iki kart gösterir ve çocuğun doğru hayvanı seçmesini bekler.
    /// </summary>
    public class AnimalQuizController : MonoBehaviour
    {
        [Header("Hayvan Verileri")]
        public Data.AnimalData[] animals;

        [Header("UI Referansları")]
        [Tooltip("Soru metni (ör: 🔊 Bu hayvan hangisi?)")]
        public Text questionText;

        [Tooltip("Üst kart butonu")]
        public Button topCardButton;
        [Tooltip("Üst kart hayvan görseli")]
        public Image topCardImage;

        [Tooltip("Alt kart butonu")]
        public Button bottomCardButton;
        [Tooltip("Alt kart hayvan görseli")]
        public Image bottomCardImage;

        [Header("Geri Bildirim")]
        [Tooltip("Geri bildirim paneli (Tebrikler / Tekrar Dene)")]
        public GameObject feedbackPanel;
        [Tooltip("Geri bildirim metni")]
        public Text feedbackText;

        [Header("İlerleme")]
        [Tooltip("İlerleme metni (ör: 1/5)")]
        public Text progressText;

        [Header("Sesler")]
        [Tooltip("Doğru cevap ses efekti")]
        public AudioClip correctSFX;
        [Tooltip("Yanlış cevap ses efekti")]
        public AudioClip wrongSFX;
        [Tooltip("Oyun bitiş ses efekti")]
        public AudioClip winSFX;

        private int currentQuestionIndex = 0;
        private int correctAnimalIndex; // 0 = üst kart, 1 = alt kart
        private List<int> questionOrder;

        private void Start()
        {
            // Soruları karıştır
            questionOrder = new List<int>();
            for (int i = 0; i < animals.Length; i++)
                questionOrder.Add(i);
            ShuffleList(questionOrder);

            // Buton dinleyicilerini bağla
            topCardButton.onClick.AddListener(() => OnCardSelected(0));
            bottomCardButton.onClick.AddListener(() => OnCardSelected(1));

            // Geri bildirim panelini gizle
            feedbackPanel.SetActive(false);

            // İlk soruyu göster
            ShowNextQuestion();
        }

        /// <summary>Sıradaki soruyu gösterir.</summary>
        private void ShowNextQuestion()
        {
            if (currentQuestionIndex >= questionOrder.Count)
            {
                OnQuizComplete();
                return;
            }

            int correctIdx = questionOrder[currentQuestionIndex];
            Data.AnimalData correctAnimal = animals[correctIdx];

            // Yanlış cevap için rastgele farklı bir hayvan seç
            int wrongIdx = correctIdx;
            while (wrongIdx == correctIdx)
            {
                wrongIdx = Random.Range(0, animals.Length);
            }
            Data.AnimalData wrongAnimal = animals[wrongIdx];

            // Doğru kartı rastgele üste veya alta yerleştir
            correctAnimalIndex = Random.Range(0, 2);

            if (correctAnimalIndex == 0)
            {
                // Doğru cevap üstte
                topCardImage.sprite = correctAnimal.animalSprite;
                bottomCardImage.sprite = wrongAnimal.animalSprite;
            }
            else
            {
                // Doğru cevap altta
                topCardImage.sprite = wrongAnimal.animalSprite;
                bottomCardImage.sprite = correctAnimal.animalSprite;
            }

            // Kartları göster, boyutlarını koru
            topCardImage.preserveAspect = true;
            bottomCardImage.preserveAspect = true;

            // Soru metnini güncelle
            questionText.text = "🔊 Bu hayvan hangisi?";

            // İlerleme metnini güncelle
            if (progressText != null)
            {
                progressText.text = $"{currentQuestionIndex + 1} / {questionOrder.Count}";
            }

            // Butonları aktif yap
            topCardButton.interactable = true;
            bottomCardButton.interactable = true;

            // Geri bildirim panelini gizle
            feedbackPanel.SetActive(false);

            // Hayvan sesini çal
            if (correctAnimal.touchSFX != null && Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlaySFX(correctAnimal.touchSFX);
            }
        }

        /// <summary>Kart seçildiğinde çağrılır. cardIndex: 0=üst, 1=alt</summary>
        private void OnCardSelected(int cardIndex)
        {
            // Butonları devre dışı bırak (çift tıklamayı önle)
            topCardButton.interactable = false;
            bottomCardButton.interactable = false;

            if (cardIndex == correctAnimalIndex)
            {
                // Doğru cevap
                StartCoroutine(ShowFeedback(true));
            }
            else
            {
                // Yanlış cevap
                StartCoroutine(ShowFeedback(false));
            }
        }

        private IEnumerator ShowFeedback(bool isCorrect)
        {
            feedbackPanel.SetActive(true);

            if (isCorrect)
            {
                feedbackText.text = "🎉 Tebrikler!";
                feedbackText.color = new Color(0.2f, 0.8f, 0.2f); // Yeşil

                if (correctSFX != null && Audio.AudioManager.Instance != null)
                    Audio.AudioManager.Instance.PlaySFX(correctSFX);

                // 1.5 saniye bekle, sonraki soruya geç
                yield return new WaitForSeconds(1.5f);

                currentQuestionIndex++;
                ShowNextQuestion();
            }
            else
            {
                feedbackText.text = "❌ Tekrar Dene!";
                feedbackText.color = new Color(0.9f, 0.2f, 0.2f); // Kırmızı

                if (wrongSFX != null && Audio.AudioManager.Instance != null)
                    Audio.AudioManager.Instance.PlaySFX(wrongSFX);

                // 1 saniye bekle, butonları tekrar aç
                yield return new WaitForSeconds(1f);

                feedbackPanel.SetActive(false);
                topCardButton.interactable = true;
                bottomCardButton.interactable = true;

                // Hayvan sesini tekrar çal
                Data.AnimalData correctAnimal = animals[questionOrder[currentQuestionIndex]];
                if (correctAnimal.touchSFX != null && Audio.AudioManager.Instance != null)
                {
                    Audio.AudioManager.Instance.PlaySFX(correctAnimal.touchSFX);
                }
            }
        }

        /// <summary>Tüm sorular bitiğinde çağrılır.</summary>
        private void OnQuizComplete()
        {
            questionText.text = "🏆 Harika! Hepsini Bildin!";
            topCardButton.gameObject.SetActive(false);
            bottomCardButton.gameObject.SetActive(false);
            feedbackPanel.SetActive(false);

            if (progressText != null)
                progressText.text = "Tamamlandı!";

            if (winSFX != null && Audio.AudioManager.Instance != null)
                Audio.AudioManager.Instance.PlaySFX(winSFX);

            Debug.Log("[AnimalQuiz] Quiz tamamlandı!");

            // Ödül sistemini tetikle
            RewardManager reward = FindFirstObjectByType<RewardManager>();
            if (reward != null)
            {
                reward.ShowReward();
            }
        }

        /// <summary>Listeyi rastgele karıştırır (Fisher-Yates).</summary>
        private void ShuffleList(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
