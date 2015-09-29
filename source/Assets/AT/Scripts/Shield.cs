using UnityEngine;
using System.Collections;
namespace AT.Mechanics {
    [System.Serializable]
    public class Shield {
        public int maxSP;
        public float SP;
        public float rechargeRate = 0.0f;//% per second
        public float rebuildDelay = 0.0f;//seconds
        public bool isDestroyed = true;
        public float curRebuildTime = 0.0f;
        public Shield(int maxSP, float rechargeRate, float rebuildDelay) {
            this.maxSP = maxSP;
            this.SP = this.maxSP;
            this.rechargeRate = rechargeRate;
            this.rebuildDelay = rebuildDelay;
        }
        protected void updateRebuilding(float deltaTime) {
            if (isDestroyed) {
                curRebuildTime += deltaTime;
                if (curRebuildTime > rebuildDelay) {
                    isDestroyed = false;
                }
            }
        }
        protected void updateRecharging(float deltaTime) {
            if (!isDestroyed && SP < maxSP) {
                SP += maxSP * rechargeRate * deltaTime;
                if (SP >= maxSP) {
                    SP = maxSP;
                }
            }
        }
        public void update(float deltaTime) {
            updateRebuilding(deltaTime);
            updateRecharging(deltaTime);
        }
        public bool applyDamage(ref Damage damage) {
            if(maxSP == 0) 
                return true;//No shields
            if (damage.value > 0) {
                SP -= damage.value;
                if (SP < 0f) {
                    damage.value += (int)SP;
                    SP = 0f;
                    isDestroyed = true;
                    return true;//Shield passed through
                }
            }
            return false;//Shield ok
        }
    }
}