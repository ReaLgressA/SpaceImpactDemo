using UnityEngine;
using System.Collections.Generic;
namespace AT.Battle {
    public enum VFXType {
        EXPLOSION
    }

    [System.Serializable]
    public class VFXPoolDescription {
        public VFXType type;
        public Object prefabVFX;
        public int maxNumber;
        public bool isExtendAllowed = true;
        [System.NonSerialized]
        public List<GameObject> goList = null;
        public void Initialize(Transform parent) {
            goList = new List<GameObject>(maxNumber);
            for (int i = 0; i < maxNumber; ++i) {
                goList.Add(GameObject.Instantiate(prefabVFX) as GameObject);
                goList[i].transform.parent = parent;
                goList[i].SetActive(false);
            }
        }
        public void playVFX(Vector3 pos) {
            for (int i = 0; i < goList.Count; ++i) {
                if (!goList[i].activeSelf) {
                    goList[i].SetActive(true);
                    goList[i].transform.position = pos;
                    return;
                }
            }
            if (isExtendAllowed) { //extend the pool
                goList.Add(GameObject.Instantiate(prefabVFX) as GameObject);
                int idx = goList.Count - 1;
                goList[idx].transform.parent = goList[0].transform.parent;
                goList[idx].SetActive(true);
                goList[idx].transform.position = pos;
            }
        }
    }
    public class VFXPool : MonoBehaviour {
        public static VFXPool instance = null;
        public VFXPoolDescription[] pools;
        protected Dictionary<VFXType, int> poolMap;
        void Awake() {
            if (instance != null)
                return;
            instance = this;
            poolMap = new Dictionary<VFXType, int>(pools.Length);
            for (int i = 0; i < pools.Length; ++i) {
                pools[i].Initialize(transform);
                poolMap.Add(pools[i].type, i);
            }
        }
        public static void PlayVFX(Vector3 pos, VFXType type) {
            int idx = -1;
            instance.poolMap.TryGetValue(type, out idx);
            instance.pools[idx].playVFX(pos);
        }

    }
}