using UnityEngine;
using System.Collections;

public class EndBehavior : MonoBehaviour {
	
	Vector3 pos1,pos2;
	bool flag = true;
	
	void Start ()
	{
		pos1 = transform.localPosition;
		pos2 = transform.localPosition - new Vector3(0,0.1f,0);
	}
	
	void Update () 
	{
//		transform.RotateAround(new Vector3(0,0,1), Mathf.PI);
		if(flag)
			transform.localPosition = pos1;
		else
			transform.localPosition = pos2;
		
		flag = !flag;
	}
}
