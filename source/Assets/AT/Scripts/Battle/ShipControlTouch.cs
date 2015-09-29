using UnityEngine;
using AT.Mechanics;
namespace AT.Battle {
    public class ShipControlTouch : MonoBehaviour, IShipControl {
        public ShipController sc = null;

        public void update(float deltaTime) {
            Vector3 acc = Input.gyro.userAcceleration;
            sc.accDirectionChanged(new Vector2(-acc.y, acc.x));
            if (Input.touchCount > 0) {
                sc.makeWeaponShot(MissileSource.PLAYER);
            }
        }
        public void initialize(ShipController sc) {
            this.sc = sc;
        }
    }
}