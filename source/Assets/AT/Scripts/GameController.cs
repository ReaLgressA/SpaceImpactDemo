using UnityEngine;
namespace AT {
    public class GameController : MonoBehaviour {
        void FixedUpdate() {
            if (Input.GetKey(KeyCode.F2))
                Application.LoadLevel(Application.loadedLevel);
        }
    }
}