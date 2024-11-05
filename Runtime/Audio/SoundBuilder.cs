using UnityEngine;

namespace AudioSystem {
    public class SoundBuilder {
        private readonly SoundManager m_soundManager;
        private Vector3 m_position = Vector3.zero;
        private bool m_randomPitch;

        public SoundBuilder(SoundManager t_soundManager) {
            this.m_soundManager = t_soundManager;
        }

        public SoundBuilder withPosition(Vector3 t_position) {
            this.m_position = t_position;
            return this;
        }

        public SoundBuilder withRandomPitch() {
            m_randomPitch = true;
            return this;
        }

        public void play(SoundData t_soundData) {
            if (t_soundData == null) {
                Debug.LogError("SoundData is null");
                return;
            }
            if (!m_soundManager.canPlaySound(t_soundData)) {
                return;
            }
            SoundEmitter soundEmitter = m_soundManager.get();
            soundEmitter.initialize(t_soundData);
            soundEmitter.transform.position = m_position;
            soundEmitter.transform.parent = m_soundManager.transform;
            if (m_randomPitch) {
                soundEmitter.withRandomPitch(t_soundData.minRandomPitch, t_soundData.maxRandomPitch);
            }
            if (t_soundData.frequentSound) {
                soundEmitter.Node = m_soundManager.frequentSoundEmitters.AddLast(soundEmitter);
            }
            soundEmitter.play();
        }
    }
}