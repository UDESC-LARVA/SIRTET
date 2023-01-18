using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour 
{
	public int nroItem = 0;
	
	void OnMouseEnter()
	{
		this.transform.position -= new Vector3(0,0,10f);
		this.GetComponent<Renderer>().material.color = Color.red;
	}
	
	void OnMouseExit()
	{
		this.transform.position += new Vector3(0,0,10f);
		this.GetComponent<Renderer>().material.color = Color.green;
	}
	
	void OnMouseDown()
	{
		switch (nroItem) {
		case 1:
			SceneManager.LoadScene("Game_Start");
			break;
		case 2:
			SceneManager.LoadScene("Menu_Parcial");
			break;
		case 3:
			Application.Quit();
			break;
		}
	}
	
}
