using UnityEngine;
using UnityEngine.UI;
namespace AT.Battle {
    public class PlayerUI : MonoBehaviour {
        public Text hullText;
        public Text energyText;
        protected ShipController sp;
        void Start() {
            this.sp = ShipController.player;
        }
        void FixedUpdate() {
            hullText.text = string.Format("Hull: {0}/{1}", sp.hull.HP, sp.hull.maxHP);
            //energyText = string.Format("Hull: {0}/{1}", sc.hull.HP, sc.hull.maxHP);
        }
    }
}