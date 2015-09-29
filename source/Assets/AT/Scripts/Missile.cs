using UnityEngine;
namespace AT.Mechanics {
    public enum MissileSource {
        ENEMY,
        PLAYER
    }
    public enum MissileType {
        LASER_SHOT_RED,
        LASER_SHOT_BLUE
    }
    [RequireComponent(typeof(AudioSource))]
    public class Missile : MonoBehaviour {
        public const float DEFAULT_LIFETIME = 7.0f;
        public float speed;
        public Vector2 direction;
        public AudioClip shot_sound;
        public AudioClip hit_sound;
        public float lifetime;
        public MissileSource source;
        public MissileType type;
        public Damage damage;
        protected AudioSource audioSource;
        protected SpriteRenderer spriteRenderer;
        protected Collider2D collider;
        public void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            collider = GetComponent<Collider2D>();
        }
        public void Initialize(Damage damage, Vector3 pos, float speed, Vector2 dir, MissileSource source, MissileType type) {
            transform.position = pos;
            this.speed = speed;
            this.direction = dir;
            this.lifetime = 0.0f;
            this.source = source;
            this.type = type;
            this.damage = damage;
            gameObject.SetActive(true);
            spriteRenderer.enabled = true;
            collider.enabled = true;
            audioSource.PlayOneShot(shot_sound);
        }

        void Update() {
            if (spriteRenderer.enabled) {
                transform.Translate(direction * speed * Time.deltaTime);
                lifetime += Time.deltaTime;
                if (lifetime > DEFAULT_LIFETIME) {
                    gameObject.SetActive(false);
                }
            } else if(!audioSource.isPlaying) {
                gameObject.SetActive(false);
            }
        }

        void OnTriggerEnter2D(Collider2D other) {
            GameObject go = other.gameObject;
            if (go.tag == "Player" && source == MissileSource.ENEMY ||
                go.tag == "Enemy" && source == MissileSource.PLAYER) {
                go.SendMessage("ApplyDamage", damage);
                audioSource.PlayOneShot(hit_sound);
                spriteRenderer.enabled = false;
                collider.enabled = false;
            }
        }
    }
}