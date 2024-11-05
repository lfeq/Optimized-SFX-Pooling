using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioSystem {
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour {
        private AudioSource m_audioSource;
        private Coroutine m_playingCoroutine;
        public SoundData Data { get; private set; }
        public LinkedListNode<SoundEmitter> Node { get; set; }

        private void Awake() {
            m_audioSource = gameObject.getOrAdd<AudioSource>();
        }

        public void initialize(SoundData t_data) {
            Data = t_data;
            m_audioSource.clip = t_data.clip;
            m_audioSource.outputAudioMixerGroup = t_data.mixerGroup;
            m_audioSource.loop = t_data.loop;
            m_audioSource.playOnAwake = t_data.playOnAwake;
            m_audioSource.mute = t_data.mute;
            m_audioSource.bypassEffects = t_data.bypassEffects;
            m_audioSource.bypassListenerEffects = t_data.bypassListenerEffects;
            m_audioSource.bypassReverbZones = t_data.bypassReverbZones;
            m_audioSource.priority = t_data.priority;
            m_audioSource.volume = t_data.volume;
            m_audioSource.pitch = t_data.pitch;
            m_audioSource.panStereo = t_data.panStereo;
            m_audioSource.spatialBlend = t_data.spatialBlend;
            m_audioSource.reverbZoneMix = t_data.reverbZoneMix;
            m_audioSource.dopplerLevel = t_data.dopplerLevel;
            m_audioSource.spread = t_data.spread;
            m_audioSource.minDistance = t_data.minDistance;
            m_audioSource.maxDistance = t_data.maxDistance;
            m_audioSource.ignoreListenerVolume = t_data.ignoreListenerVolume;
            m_audioSource.ignoreListenerPause = t_data.ignoreListenerPause;
            m_audioSource.rolloffMode = t_data.rolloffMode;
        }

        public void play() {
            if (m_playingCoroutine != null) {
                StopCoroutine(m_playingCoroutine);
            }
            m_audioSource.Play();
            m_playingCoroutine = StartCoroutine(waitForSoundToEnd());
        }

        private IEnumerator waitForSoundToEnd() {
            yield return new WaitWhile(() => m_audioSource.isPlaying);
            stop();
        }

        public void stop() {
            if (m_playingCoroutine != null) {
                StopCoroutine(m_playingCoroutine);
                m_playingCoroutine = null;
            }
            m_audioSource.Stop();
            SoundManager.Instance.returnToPool(this);
        }

        public void withRandomPitch(float t_min = -0.05f, float t_max = 0.05f) {
            m_audioSource.pitch += Random.Range(t_min, t_max);
        }
    }
}