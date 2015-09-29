using UnityEngine;
using System.Collections;
namespace AT.Mechanics {
    [System.Serializable]
    public class Hull {
        public int maxHP;
        public int HP;
        public int armor;//damage protection
        public Hull(int maxHP, int armor) {
            this.maxHP = maxHP;
            this.HP = this.maxHP;
            this.armor = armor;
        }
        public bool applyDamage(Damage damage) {
            damage.value -= armor;
            if (damage.value > 0) {
                HP -= damage.value;
                if (HP <= 0) {
                    HP = 0;
                    return true;//Ship destroyed
                }
            }
            return false;//Still ok
        }
    }
}