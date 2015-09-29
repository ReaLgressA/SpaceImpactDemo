using UnityEngine;
using System.Collections;
namespace AT.Battle {
    [System.Serializable]
    public class SpawnerPack {
        public Object spawnPrefab;
        public float spawnDelay = 1;
        public int numberToSpawn = 0;
        public Rect spawnArea;
        private float cDelay = 0f;
        private Transform parent;
        public void setParent(Transform parent) {
            this.parent = parent;
        }
        public void update(float deltaTime) {
            if (numberToSpawn > 0) {
                if (cDelay <= 0f) {
                    --numberToSpawn;
                    cDelay = spawnDelay + cDelay;
                    GameObject go = GameObject.Instantiate(spawnPrefab) as GameObject;
                    go.transform.parent = parent;
                    Vector3 pos = new Vector3(Random.Range(BattleController.instance.BX_MAX + spawnArea.x, BattleController.instance.BX_MAX + spawnArea.xMax), Random.Range(spawnArea.y, spawnArea.yMax));
                    go.transform.position = pos;
                }
                cDelay -= Time.fixedDeltaTime;
            }
        }
    }
    [System.Serializable]
    public class SpawnerWave {
        public SpawnerPack[] packs;
        public void setParent(Transform parent) {
            for (int i = 0; i < packs.Length; ++i) {
                packs[i].setParent(parent);
            }
        }
        public void update(float deltaTime) {
            for (int i = 0; i < packs.Length; ++i) {
                packs[i].update(deltaTime);
            }
        }
        public bool isWaveFinished() {
            for (int i = 0; i < packs.Length; ++i) {
                if (packs[i].numberToSpawn > 0)
                    return false;
            }
            return true;
        }
    }
    public class Spawner : MonoBehaviour {
        public SpawnerWave[] waves;
        public int waveID;
        public SpawnerWave bossWawe;
        protected bool isBossFight;
        void Awake() {
            for (int i = 0; i < waves.Length; ++i) {
                waves[i].setParent(transform);
            }
            waveID = 0;
            isBossFight = false;
        }
        void FixedUpdate() {
            if (isBossFight) {
                bossWawe.update(Time.fixedDeltaTime);
            } else {
                waves[waveID].update(Time.fixedDeltaTime);
                if (waves[waveID].isWaveFinished()) {
                    ++waveID;
                    if (waveID >= waves.Length) {
                        //TODO: summon boss!
                        isBossFight = true;
                    }
                }
            }
        }
#if UNITY_EDITOR
        protected static Color[] areaColors = { Color.red, new Color(255, 165, 0), Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta, new Color(255, 182, 193), new Color(173, 230, 196), new Color(165, 42, 42) };
        void OnDrawGizmos() {
            if (waves == null)
                return;
            int colorID = 0;
            for (int i = 0; i < waves.Length; ++i) {
                for (int j = 0; j < waves[i].packs.Length; ++j, ++colorID) {
                    if (colorID == areaColors.Length) {
                        colorID = 0;
                    }
                    Gizmos.color = areaColors[colorID];
                    float xMin = Camera.main.transform.position.x * 2 + waves[i].packs[j].spawnArea.xMin;
                    float xMax = Camera.main.transform.position.x * 2 + waves[i].packs[j].spawnArea.xMax;
                    float yMin = waves[i].packs[j].spawnArea.yMin;
                    float yMax = waves[i].packs[j].spawnArea.yMax;
                    Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMin));
                    Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax));
                    Gizmos.DrawLine(new Vector3(xMax, yMax), new Vector3(xMin, yMax));
                    Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMin, yMin));
                }
            }
            for (int i = 0; i < bossWawe.packs.Length; ++i) {
                Gizmos.color = areaColors[i];
                float xMin = Camera.main.transform.position.x * 2 + bossWawe.packs[i].spawnArea.xMin;
                float xMax = Camera.main.transform.position.x * 2 + bossWawe.packs[i].spawnArea.xMax;
                float yMin = bossWawe.packs[i].spawnArea.yMin;
                float yMax = bossWawe.packs[i].spawnArea.yMax;
                Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMin));
                Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax));
                Gizmos.DrawLine(new Vector3(xMax, yMax), new Vector3(xMin, yMax));
                Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMin, yMin));
                Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMax));
                Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMin, yMax));
            }

        }
#endif
    }
}