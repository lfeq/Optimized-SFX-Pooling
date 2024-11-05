using UnityEngine;

namespace AudioSystem {
    public static class GameObjectExtensions {
        public static T getOrAdd<T>(this GameObject t_gameObject) where T : Component {
            T component = t_gameObject.GetComponent<T>();
            if (!component) {
                component = t_gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}