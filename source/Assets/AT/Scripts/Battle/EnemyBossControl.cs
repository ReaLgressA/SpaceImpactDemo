using UnityEngine;
using AT.Mechanics;
namespace AT.Battle {
    public class EnemyBossControl : MonoBehaviour, IShipControl {
        public ShipController sc = null;
        public float startDelay = 3.0f;//delay after respawn 
        public bool isAIRunning = false;
        protected float targetPositionX;
        public void update(float deltaTime) {
            if (isAIRunning) {
                float distance = Mathf.Abs(ShipController.player.tForm.position.y - sc.tForm.position.y);
                float dirX = 0;
                if(distance > 0.32f) {
                    dirX = (ShipController.player.tForm.position.y < sc.tForm.position.y) ? (-1f) : 1f;
                }
                float dirY = (sc.tForm.position.x > targetPositionX) ? 1f : 0;
                sc.accDirectionChanged(new Vector2(dirX, dirY));
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
            targetPositionX = BattleCamera.sWidth - sc.shipSize.x*2;
            sc.accDirectionChanged(new Vector2(0, 1));
        }
    }
}