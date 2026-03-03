using System.Collections.Generic;
using UnityEngine;

namespace LittleExplorers.Audio
{
    /// <summary>
    /// Ses efektleri, arka plan müziği ve sıralı seslendirme (voiceover) sistemini yöneten tekil yönetici.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource voiceoverSource;
        private AudioSource sfxSource;
        private AudioSource bgmSource;

        private Queue<AudioClip> voiceoverQueue = new Queue<AudioClip>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeAudioSources()
        {
            voiceoverSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            bgmSource = gameObject.AddComponent<AudioSource>();

            bgmSource.loop = true;
        }

        private void Update()
        {
            // Sırada ses varsa ve şu an çalmıyorsa, sıradaki sesi başlat
            if (voiceoverQueue.Count > 0 && !voiceoverSource.isPlaying)
            {
                PlayNextVoiceover();
            }
        }

        /// <summary>Arka plan müziğini başlatır. Aynı klip zaten çalıyorsa yeniden başlatmaz.</summary>
        public void PlayBGM(AudioClip clip)
        {
            if (bgmSource.clip == clip) return;
            bgmSource.clip = clip;
            bgmSource.Play();
        }

        /// <summary>Tek seferlik ses efekti çalar.</summary>
        public void PlaySFX(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        /// <summary>Seslendirmeyi kuyruğa ekler, üst üste binmeden sırayla çalar.</summary>
        public void QueueVoiceover(AudioClip clip)
        {
            voiceoverQueue.Enqueue(clip);
        }

        /// <summary>Tüm seslendirmeleri durdurur ve kuyruğu temizler.</summary>
        public void StopVoiceovers()
        {
            voiceoverQueue.Clear();
            if (voiceoverSource.isPlaying)
            {
                voiceoverSource.Stop();
            }
        }

        private void PlayNextVoiceover()
        {
            AudioClip clip = voiceoverQueue.Dequeue();
            voiceoverSource.clip = clip;
            voiceoverSource.Play();
        }
    }
}
