using UnityEngine;
using UnityEngine.Serialization;

namespace AudioSystem {
    public class PersistentSingleton<T> : MonoBehaviour where T : Component {
        protected static T s_instance;
        [FormerlySerializedAs("AutoUnparentOnAwake")] public bool autoUnparentOnAwake = true;

        public static bool HasInstance => s_instance != null;

        public static T Instance {
            get {
                if (s_instance == null) {
                    s_instance = FindAnyObjectByType<T>();
                    if (s_instance == null) {
                        GameObject go = new GameObject(typeof(T).Name + " Auto-Generated");
                        s_instance = go.AddComponent<T>();
                    }
                }
                return s_instance;
            }
        }

        /// <summary>
        ///     Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake() {
            initializeSingleton();
        }

        public static T tryGetInstance() {
            return HasInstance ? s_instance : null;
        }

        protected virtual void initializeSingleton() {
            if (!Application.isPlaying) {
                return;
            }
            if (autoUnparentOnAwake) {
                transform.SetParent(null);
            }
            if (s_instance == null) {
                s_instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else {
                if (s_instance != this) {
                    Destroy(gameObject);
                }
            }
        }
    }
}