using UnityEngine;

namespace LittleExplorers.Data
{
    /// <summary>
    /// Her bir hayvanın verilerini tutan ScriptableObject.
    /// İsim, sprite, ses efekti ve seslendirme bilgilerini içerir.
    /// </summary>
    [CreateAssetMenu(fileName = "YeniHayvan", menuName = "Little Explorers/Hayvan Verisi")]
    public class AnimalData : ScriptableObject
    {
        [Header("Temel Bilgiler")]
        public string animalName;
        public Sprite animalSprite;

        [Header("Sesler")]
        [Tooltip("Hayvana dokunulduğunda çalacak ses efekti")]
        public AudioClip touchSFX;

        [Tooltip("Hayvanın adını söyleyen seslendirme klibi")]
        public AudioClip voiceoverClip;
    }
}
