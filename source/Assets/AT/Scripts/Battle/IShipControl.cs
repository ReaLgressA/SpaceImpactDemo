using UnityEngine;
namespace AT.Battle {
    public interface IShipControl {
        void update(float deltaTime);
        void initialize(ShipController sc);
    }
}