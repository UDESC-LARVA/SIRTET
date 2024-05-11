using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRFollowHead : MonoBehaviour
{

    //GameObject cabeca;
    UDP datagrama;

    // Start is called before the first frame update
    void Start()
    {
        //cabeca = GameObject.Find ("Cabeca");
        datagrama = GameObject.Find("UDP").GetComponent<UDP>();
		
    }

    // Update is called once per frame
    void Update()
    {
        var myPos = new Vector3(0, 0, -100);
        transform.position = datagrama.cabeca + myPos;
    }

}
