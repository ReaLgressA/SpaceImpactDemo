using UnityEngine;
using AT.Mechanics;
namespace AT.Battle {
    public class EnemyShipControl : MonoBehaviour, IShipControl {
        public ShipController sc = null;
        public float startDelay = 3.0f;//delay after respawn 
        public bool isAIRunning = false;
        public void update(float deltaTime) {
            if (isAIRunning) {
                for (int i = 0; i < sc.weapons.Length; ++i) {
                    if (sc.weapons[i].cMissilies > 0 && sc.weapons[i].csd <= 0) {
                        sc.makeWeaponShot(MissileSource.ENEMY);
                        break;
                    }
                }
            } else {
                startDelay -= deltaTime;
                if (startDelay < 0f) {
                    isAIRunning = true;
                    startDelay = 0f;
                }
            }
        }
        public void initialize(ShipController sc) {
            this.sc = sc;
            sc.accDirectionChanged(new Vector2(0, 1));
        }
    }
}