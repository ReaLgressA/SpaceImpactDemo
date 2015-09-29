using UnityEngine;
namespace AT.Battle {
    public class BattleController : MonoBehaviour {
        public static BattleController instance = null;
        public float BX_MIN = 0, BX_MAX, BY_MIN = 0, BY_MAX;//Battle rectangle borders
        public Object playerPrefab;
        protected GameObject goPlayer;

        void Awake() {
            if(instance != null) {
                Destroy(gameObject);
            }
            instance = this;
            BX_MIN = 0f;
            BY_MIN = 0f;
            BX_MAX = BattleCamera.sWidth;
            BY_MAX = BattleCamera.sHeight;
            goPlayer = Instantiate(playerPrefab) as GameObject;
            goPlayer.transform.position = new Vector3(BX_MIN, BY_MAX / 2.0f + 0.32f);
            
        }
        public static Vector3 CropShipPosition(Vector3 pos, Vector2 shipSize) {
            float hw = shipSize.x / 2f, hh = shipSize.y / 2f;
            float bxMax = instance.BX_MAX - hw, bxMin = instance.BX_MIN + hw;
            float byMax = instance.BY_MAX - hh, byMin = instance.BY_MIN + hh;
            if (pos.x > bxMax) {
                pos.x = bxMax;
            } else if (pos.x < bxMin) {
                pos.x = bxMin;
            }
            if (pos.y > byMax) {
                pos.y = byMax;
            } else if (pos.y < byMin) {
                pos.y = byMin;
            }
            return pos;
        }
        #if UNITY_EDITOR
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(BX_MIN, BY_MIN), new Vector3(BX_MAX, BY_MIN));
            Gizmos.DrawLine(new Vector3(BX_MAX, BY_MIN), new Vector3(BX_MAX, BY_MAX));
            Gizmos.DrawLine(new Vector3(BX_MAX, BY_MAX), new Vector3(BX_MIN, BY_MAX));
            Gizmos.DrawLine(new Vector3(BX_MIN, BY_MAX), new Vector3(BX_MIN, BY_MIN));
        }
        #endif
    }
}