using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace AudioSystem {
    public class SoundManager : PersistentSingleton<SoundManager> {
        [SerializeField] private SoundEmitter soundEmitterPrefab;
        [SerializeField] private bool collectionCheck = true;
        [SerializeField] private int defaultCapacity = 10;
        [SerializeField] private int maxPoolSize = 100;
        [SerializeField] private int maxSoundInstances = 30;
        private readonly List<SoundEmitter> m_activeSoundEmitters = new();
        public readonly LinkedList<SoundEmitter> frequentSoundEmitters = new();
        private IObjectPool<SoundEmitter> m_soundEmitterPool;

        private void Start() {
            initializePool();
        }

        public SoundBuilder createSoundBuilder() {
            return new SoundBuilder(this);
        }

        public bool canPlaySound(SoundData t_data) {
            if (!t_data.frequentSound) {
                return true;
            }
            if (frequentSoundEmitters.Count >= maxSoundInstances) {
                try {
                    frequentSoundEmitters.First.Value.stop();
                    return true;
                }
                catch {
                    Debug.Log("SoundEmitter is already released");
                }
                return false;
            }
            return true;
        }

        public SoundEmitter get() {
            return m_soundEmitterPool.Get();
        }

        public void returnToPool(SoundEmitter t_soundEmitter) {
            m_soundEmitterPool.Release(t_soundEmitter);
        }

        public void stopAll() {
            foreach (SoundEmitter soundEmitter in m_activeSoundEmitters) {
                soundEmitter.stop();
            }
            frequentSoundEmitters.Clear();
        }

        private void initializePool() {
            m_soundEmitterPool = new ObjectPool<SoundEmitter>(
                createSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize);
        }

        private SoundEmitter createSoundEmitter() {
            SoundEmitter soundEmitter = Instantiate(soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        private void OnTakeFromPool(SoundEmitter t_soundEmitter) {
            t_soundEmitter.gameObject.SetActive(true);
            m_activeSoundEmitters.Add(t_soundEmitter);
        }

        private void OnReturnedToPool(SoundEmitter t_soundEmitter) {
            if (t_soundEmitter.Node != null) {
                frequentSoundEmitters.Remove(t_soundEmitter.Node);
                t_soundEmitter.Node = null;
            }
            t_soundEmitter.gameObject.SetActive(false);
            m_activeSoundEmitters.Remove(t_soundEmitter);
        }

        private void OnDestroyPoolObject(SoundEmitter t_soundEmitter) {
            Destroy(t_soundEmitter.gameObject);
        }
    }
}