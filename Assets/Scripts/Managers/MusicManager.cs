using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem {
    [RequireComponent(typeof(MusicManager))]
    public class MusicManager : PersistentSingleton<MonoBehaviour> {
        private const float CrossFadeTime = 1.0f;

        [SerializeField] private List<AudioClip> initialPlaylist;
        [SerializeField] private AudioMixerGroup musicMixerGroup;
        private readonly Queue<AudioClip> m_playlist = new();
        private AudioSource m_current;
        private float m_fading;
        private AudioSource m_previous;

        private void Start() {
            foreach (AudioClip clip in initialPlaylist) {
                addToPlaylist(clip);
            }
        }

        private void Update() {
            handleCrossFade();
            if (m_current && !m_current.isPlaying && m_playlist.Count > 0) {
                playNextTrack();
            }
        }

        public void addToPlaylist(AudioClip t_clip) {
            m_playlist.Enqueue(t_clip);
            if (m_current == null && m_previous == null) {
                playNextTrack();
            }
        }

        public void clear() {
            m_playlist.Clear();
        }

        public void playNextTrack() {
            if (m_playlist.TryDequeue(out AudioClip nextTrack)) {
                play(nextTrack);
            }
        }

        public void play(AudioClip t_clip) {
            if (m_current && m_current.clip == t_clip) {
                return;
            }
            if (m_previous) {
                Destroy(m_previous);
                m_previous = null;
            }
            m_previous = m_current;
            m_current = gameObject.getOrAdd<AudioSource>();
            m_current.clip = t_clip;
            m_current.outputAudioMixerGroup = musicMixerGroup; // Set mixer group
            m_current.loop = false; // For playlist functionality, we want tracks to play once
            m_current.volume = 0;
            m_current.bypassListenerEffects = true;
            m_current.Play();
            m_fading = 0.001f;
        }

        private void handleCrossFade() {
            if (m_fading <= 0f) {
                return;
            }
            m_fading += Time.deltaTime;
            float fraction = Mathf.Clamp01(m_fading / CrossFadeTime);

            // Logarithmic fade
            float logFraction = fraction.toLogarithmicFraction();
            if (m_previous) {
                m_previous.volume = 1.0f - logFraction;
            }
            if (m_current) {
                m_current.volume = logFraction;
            }
            if (fraction >= 1) {
                m_fading = 0.0f;
                if (m_previous) {
                    Destroy(m_previous);
                    m_previous = null;
                }
            }
        }
    }
}