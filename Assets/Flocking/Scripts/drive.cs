using UnityEngine;
using System.Collections;

public class drive : MonoBehaviour {
    public float speed = 5.0F;
    void Update() {
        float translationY = Input.GetAxis("Vertical") * speed;
        float translationX = Input.GetAxis("Horizontal") * speed;
        translationY *= Time.deltaTime;
        translationX *= Time.deltaTime;
        transform.Translate(0, translationY, 0);
        transform.Translate(translationX, 0, 0);
    }
}

