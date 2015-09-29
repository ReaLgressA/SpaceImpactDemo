using UnityEngine;
using AT.Mechanics;
namespace AT.Battle {
    public class ShipController : MonoBehaviour {
        public static BattleController bc = null;
        public static ShipController player = null;
        public const float UI_HEIGHT = 2;
        public float cSpeed = 4.0f;//Current speed
        public Vector2 accDir = Vector2.zero;//Acceleration direction
        public IShipControl sControl = null;
        public Transform tForm = null;
        public Hull hull;
        public Shield shield;
        public Weapon[] weapons;
        public SpriteRenderer shieldSpriteRenderer;
        public Vector2 shipSize;
        protected bool cropShipPosition;

        void Start() {
            if (bc == null) {
                bc = BattleController.instance;
            }
            this.tForm = transform;
            this.sControl = GetComponent<ShipControlPC>();
            if (sControl == null) {
                this.sControl = GetComponent<EnemyShipControl>();
                if (sControl == null) {
                    this.sControl = GetComponent<ShipControlTouch>();//TODO: touch control
                    if (sControl == null) {
                        this.sControl = GetComponent<EnemyBossControl>();
                    }
                }
            }
            sControl.initialize(this);
            if (gameObject.tag == "Player") {
                player = this;
                this.cropShipPosition = true;
                tForm.position = BattleController.CropShipPosition(tForm.position, shipSize);
            } else {
                this.cropShipPosition = false;
            }
        }
        protected void updateMovement() {
            if (accDir != Vector2.zero) {
                Vector3 shift = (Vector3)accDir * cSpeed * Time.deltaTime;
                tForm.Translate(shift);
                if (cropShipPosition) {
                    tForm.position = BattleController.CropShipPosition(tForm.position, shipSize);
                }
            }
        }
        void Update() {
            if (hull.HP > 0) {
                sControl.update(Time.deltaTime);
                for (int i = 0; i < weapons.Length; ++i) { 
                    weapons[i].update(Time.deltaTime);
                }
                if (shield.maxSP > 0) {
                    shield.update(Time.deltaTime);
                    Color shColor = shieldSpriteRenderer.color;
                    shColor.a = shield.SP / (float)shield.maxSP;
                    shieldSpriteRenderer.color = shColor;
                }
                updateMovement();
            }
        }
        public void makeWeaponShot(MissileSource source) {
            for (int i = 0; i < weapons.Length; ++i) {
                if (weapons[i].makeShot()) {
                    MissilePool.FireMissile(weapons[i].damage, transform.position + weapons[i].shotPositionOffset, weapons[i].missileSpeed, weapons[i].shotDirection, source, weapons[i].missileType);
                }
            }
            
        }
        public void accDirectionChanged(Vector2 dir) {
            this.accDir = dir;
        }
        public void ApplyDamage(Damage damage) {
            if (shield.applyDamage(ref damage)) {
                if (hull.applyDamage(damage)) {
                    //TODO: BOOM sound
                    VFXPool.PlayVFX(tForm.position, VFXType.EXPLOSION);
                    Destroy(gameObject, 0.25f);
                }
            }
        }
        #if UNITY_EDITOR
        protected static Color[] weaponColors = { Color.red, new Color(255, 165, 0), Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta, new Color(255, 182, 193), new Color(173, 230, 196), new Color(165, 42, 42) };
        void OnDrawGizmosSelected() {
            if(weapons == null)
                return;
            for (int i = 0; i < weapons.Length; ++i) {
                Gizmos.color  = weaponColors[i];
                Gizmos.DrawCube(transform.position + weapons[i].shotPositionOffset, new Vector3(0.08f, 0.08f, -1f));
            }
        }
        #endif
    }
}