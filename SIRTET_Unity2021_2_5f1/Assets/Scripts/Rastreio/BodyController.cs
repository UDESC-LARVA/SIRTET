using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BodyController : MonoBehaviour {

	public int bodyType;
	bool gameStart = false;
	
	void Update () {
		
		//if(Application.loadedLevel == 1)
		if(SceneManager.GetActiveScene().buildIndex == 1)
			gameStart = true;
		else
			gameStart = false;
		
		if(gameStart)
			enable(bodyType);
		
		if(Input.GetKeyDown("1"))
		{
			destroyAll();
			disableAll();
			enable(1);
		}
		if(Input.GetKeyDown("2"))
		{
			destroyAll();
			disableAll();
			enable(2);
		}
		if(Input.GetKeyDown("3"))
		{
			destroyAll();
			disableAll();
			enable(3);
		}
		
	}
	
	void destroyAll()
	{
		List<Transform> children = new List<Transform>();
		foreach(Transform child in transform)
			children.Add(child.transform);
		
		children.ForEach(c => Destroy(c.gameObject));
	}

	void disableAll ()
	{
		GameObject.Find("UDP").GetComponent<AvatarBasico>().enabled = false;
		GameObject.Find("UDP").GetComponent<AvatarMedio>().enabled = false;
		GameObject.Find("UDP").GetComponent<BodyComplete>().enabled = false;
		GameObject.Find("UDP").GetComponent<BodySegments>().enabled = false;
	}

	void enable (int nro)
	{
		switch (nro) {
		case 1:
			GameObject.Find("UDP").GetComponent<AvatarBasico>().enabled = true;
			break;
		case 2:
			GameObject.Find("UDP").GetComponent<AvatarMedio>().enabled = true;
			break;
		case 3:
			GameObject.Find("UDP").GetComponent<BodyComplete>().enabled = true;
			break;
		}
	}
}
