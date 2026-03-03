using UnityEngine;
using UnityEngine.Events;

namespace LittleExplorers.Interactions
{
    /// <summary>
    /// Dokunulabilir 2D obje. Dokunulduğunda ses çalar, seslendirme kuyruğa eklenir ve görsel tepki verir.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class InteractableObject : MonoBehaviour
    {
        [Header("Ses")]
        public AudioClip onTouchSFX;
        public AudioClip voiceoverClip;

        [Header("Olaylar")]
        public UnityEvent onTouch;

        private void OnMouseDown()
        {
            Debug.Log($"[InteractableObject] Dokunuldu: {gameObject.name}");

            if (onTouchSFX != null && Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.PlaySFX(onTouchSFX);
            }

            if (voiceoverClip != null && Audio.AudioManager.Instance != null)
            {
                Audio.AudioManager.Instance.QueueVoiceover(voiceoverClip);
            }

            // Görsel geri bildirim: küçük zıplama efekti
            transform.localScale = Vector3.one * 0.9f;
            Invoke(nameof(ResetScale), 0.1f);

            onTouch?.Invoke();
        }

        private void ResetScale()
        {
            transform.localScale = Vector3.one;
        }
    }
}
