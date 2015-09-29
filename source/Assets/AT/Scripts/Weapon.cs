using UnityEngine;
using System.Collections;
namespace AT.Mechanics {
    public enum DamageType {
        ENERGY
    }
    [System.Serializable]
    public struct Damage {
        public DamageType type;
        public int value;
        public int armorPenetration;
        public int shieldPenetration;
        public Damage(DamageType type, int value) {
            this.type = type;
            this.value = value;
            this.armorPenetration = 0;
            this.shieldPenetration = 0;
        }
        public Damage(DamageType type, int value, int armorPenetration, int shieldPenetration) {
            this.type = type;
            this.value = value;
            this.armorPenetration = armorPenetration;
            this.shieldPenetration = shieldPenetration;
        }
    }

    [System.Serializable]
    public class Weapon {
        public Damage damage;
        public float cooldown;//seconds
        public float ccd;//current cooldown
        public float shotDelay;
        public float csd;//current shot delay
        public MissileType missileType;
        public int maxMissiles;
        public int cMissilies = 0;   
        public float missileSpeed = 8.0f;
        public Vector3 shotPositionOffset;
        public Vector2 shotDirection;     
        public Weapon(Damage damage, float cooldown, int maxMissiles, float shotDelay) {
            this.damage = damage;
            this.cooldown = cooldown;
            this.ccd = 0.0f;
            this.maxMissiles = maxMissiles;
            this.cMissilies = this.maxMissiles;
            this.shotDelay = shotDelay;
            this.csd = 0.0f;
        }
        protected void updateCooldown(float deltaTime) {
            if (cMissilies < maxMissiles) {
                ccd += deltaTime;
                if (ccd >= cooldown) {
                    ccd = (++cMissilies == maxMissiles) ? (ccd - cooldown) : (0);
                }
            }
        }
        protected void updateShotDelay(float deltatTime) {
            if (csd > 0.0f) {
                csd -= deltatTime;
            }
        }
        public void update(float deltaTime) {
            updateCooldown(deltaTime);
            updateShotDelay(deltaTime);
        }
        public bool makeShot() {
            if (cMissilies > 0 && csd <= 0.0f) {
                --cMissilies;
                csd = shotDelay;
                return true;//Shot made
            }
            return false;//ongoing delay or not enough missiles
        }
    }
}