using UnityEngine;
namespace AT.Battle {
    public class BattleCamera : MonoBehaviour {
        public static BattleCamera instance = null;
        public static float sWidth, sHeight;
        public float ortographicSize = 6.4f;
        public Transform background;
        void Awake() {
            if(instance != null)
                return;
            instance = this;
            Camera camera = GetComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = ortographicSize;
            sWidth = ortographicSize * camera.aspect;
            transform.position = new Vector3(sWidth, ortographicSize, -1f);
            background.position = new Vector3(background.position.x, ortographicSize, background.position.z);
            sWidth *= 2f;
            sHeight = ortographicSize * 2f;
        }
    }
}