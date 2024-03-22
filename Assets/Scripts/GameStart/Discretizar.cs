using UnityEngine;
using System.Collections;

public class Discretizar : MonoBehaviour {
	
	BaseController padrao;
	
	// Use this for initialization
	void Start () {
		padrao = GameObject.Find("Ambiente").GetComponent<BaseController>();
		float diferenca = padrao.depth / padrao.width;
		int aux = (int)diferenca;
		diferenca = (float) aux;
		int discretizar = (int)padrao.discretize;
		GetComponent<Renderer>().material.mainTextureScale = new Vector2 (discretizar, (discretizar * diferenca));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
