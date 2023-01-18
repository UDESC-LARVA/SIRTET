using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float x = 0, y = 0, z = 0;
    public float speed = 300;
    void Update()
    {
        float rot = Time.deltaTime * speed;
        this.gameObject.transform.Rotate(rot * x, rot * y, rot * z);
    }
}
