using UnityEngine;
using System.Collections.Generic;
using AT.Mechanics;
namespace AT.Battle {
    [System.Serializable]
    public class MissilePoolDescription{
        public MissileType type;
        public Object prefabMissile;
        public int maxMissiles;
        public bool isExtendAllowed = true;
        [System.NonSerialized]
        public List<GameObject> goList = null;
        [System.NonSerialized]
        public List<Missile> missiles = null;
        public void Initialize(Transform parent) {
            goList = new List<GameObject>(maxMissiles);
            missiles = new List<Missile>(maxMissiles);
            for (int i = 0; i < maxMissiles; ++i) {
                goList.Add(GameObject.Instantiate(prefabMissile) as GameObject);
                goList[i].transform.parent = parent;
                goList[i].SetActive(false);
                missiles.Add(goList[i].GetComponent<Missile>());
            }
        }
        public void fireMissile(Damage damage, Vector3 pos, float speed, Vector2 direction, MissileSource source, MissileType type) {
            for (int i = 0; i < missiles.Count; ++i) {
                if (!goList[i].activeSelf) {
                    goList[i].SetActive(true);
                    missiles[i].Initialize(damage, pos, speed, direction, source, type);
                    return;
                }
            }
            if (isExtendAllowed) { //extend the pool
                goList.Add(GameObject.Instantiate(prefabMissile) as GameObject);
                int idx = goList.Count - 1;
                missiles.Add(goList[idx].GetComponent<Missile>());
                goList[idx].transform.parent = goList[0].transform.parent ;
                goList[idx].SetActive(true);
                missiles[idx].Initialize(damage, pos, speed, direction, source, type);
            } 
        }
    }

    public class MissilePool : MonoBehaviour {
        public static MissilePool instance = null;
        public MissilePoolDescription[] pools;
        protected Dictionary<MissileType, int> poolMap;
        void Awake() {
            if(instance != null)
                return;
            instance = this;
            poolMap = new Dictionary<MissileType, int>(pools.Length);
            for (int i = 0; i < pools.Length; ++i) {
                pools[i].Initialize(transform);
                poolMap.Add(pools[i].type, i);
            }
        }
        public static void FireMissile(Damage damage, Vector3 pos, float speed, Vector2 direction, MissileSource source, MissileType type) {
            int idx = -1;
            instance.poolMap.TryGetValue(type, out idx);
            instance.pools[idx].fireMissile(damage, pos, speed, direction, source, type);
        }
    }
}