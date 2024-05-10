using UnityEngine;
using System.Collections;

public class LimitBehavior : MonoBehaviour {

	void Update () 
	{
		this.GetComponent<Renderer>().material.color = Color.Lerp(this.GetComponent<Renderer>().material.color, Color.black, Time.deltaTime);
	}
	
	public void LimitColor(Color color)
	{
		this.GetComponent<Renderer>().material.color = color;
	}
}
