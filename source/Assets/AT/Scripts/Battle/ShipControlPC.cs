using UnityEngine;
using AT.Mechanics;
namespace AT.Battle {
    public class ShipControlPC : MonoBehaviour, IShipControl {
        public ShipController sc = null;
        public void update(float deltaTime) {
            sc.accDirectionChanged(new Vector2(-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")));
            if (Input.GetKey(KeyCode.Space)) {
                sc.makeWeaponShot(MissileSource.PLAYER);
            }
        }
        public void initialize(ShipController sc) {
            this.sc = sc;
        }
    }
}