using UnityEngine;
using System.Collections;

public class VFXController : MonoBehaviour {
    public float duration = 1f;
    protected float cDuration = 0f;
	void OnEnable() {
	    cDuration = duration;
	}
	void FixedUpdate () {
        if (cDuration <= 0f) {
            gameObject.SetActive(false);
        } else {
            cDuration -= Time.fixedDeltaTime;
        }
	}
}
