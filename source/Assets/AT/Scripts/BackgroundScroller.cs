using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {
    public float scrollingTime;
    public Vector3 scrollingTarget;
    private Vector3 velocity = Vector3.zero;

	void Update () {
        if (scrollingTime > 0) { 
	        transform.position = Vector3.SmoothDamp(transform.position, scrollingTarget, ref velocity, scrollingTime);
            scrollingTime -= Time.deltaTime;
         }
	}
}
