using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandShadowController : MonoBehaviour
{
   
	// Chache classes e metodos

    Transform chao, teto, esq, dir;
    GameObject sombraTeto, sombraEsq, sombraDir;
    GameObject sombraResource;
    Color color = Color.gray;
	
	public GameObject elementoGrafico;

	bool emJogo = false;
	
	// Use this for initialization
	void Start () 
    {
		sombraResource = Resources.Load("Sombra") as GameObject;
		CreateShadows();

		elementoGrafico = GameObject.Find("elementoGrafico");

	}

	public void BuscaLaterais()
	{		     		
		chao = GameObject.Find("Chao").GetComponent<Transform>();
		teto = GameObject.Find("Teto").GetComponent<Transform>();
		esq = GameObject.Find("ParedeEsquerda").GetComponent<Transform>();
		dir = GameObject.Find("ParedeDireita").GetComponent<Transform>();
		emJogo = true;
	}

    private void Update()
    {
        if(emJogo) ControlShadows();
    }

	
	void CreateShadows()
    {
        Vector3 posicaoMao = this.transform.position;
        Vector3 scalePad = new Vector3(0.3f, 0.3f, 0.3f);
		
		if(this.tag == "Left_Arm")
		{
			// paredeEsquerda
			sombraEsq = Instantiate(sombraResource, Vector3.zero, Quaternion.identity) as GameObject;	
			sombraEsq.transform.parent = transform;
			sombraEsq.transform.localScale = scalePad;	
			sombraEsq.transform.rotation = Quaternion.Euler(0,0,-90);
		}
		else if(this.tag == "Right_Arm")
		{ 
			// paredeDireita
			sombraDir = Instantiate(sombraResource, Vector3.zero, Quaternion.identity) as GameObject;
			sombraDir.transform.parent = transform;
			sombraDir.transform.localScale = scalePad;
	        sombraDir.transform.rotation = Quaternion.Euler(0,0,-270);
		}
	    
		// teto
		sombraTeto = Instantiate(sombraResource, Vector3.zero, Quaternion.identity) as GameObject;
		sombraTeto.transform.parent = transform;
		sombraTeto.transform.localScale = scalePad;		
        sombraTeto.transform.rotation = Quaternion.Euler(0,0,-180);
	}

    void ControlShadows()
    {

		if(!dir || !esq || !teto || !chao)
		{
			emJogo = false;
			return;
		} 


        Vector3 posicaoMao = this.transform.position;
		float dist;
		GameObject sombra = null;
        
		if(this.tag == "Left_Arm")
		{ // paredeEsquerda
			sombra = sombraEsq;
			sombra.transform.position = new Vector3 (esq.position.x+0.5f, posicaoMao.y, posicaoMao.z);
			
			// alterando transparencia de acordo com a distancia do objeto
			dist = Vector3.Distance(sombra.transform.position, transform.position);
			color.a = dist;
			sombra.GetComponent<Renderer>().material.color = color;
		}else if(this.tag == "Right_Arm")
		{
			// paredeDireita
			sombra = sombraDir;
			sombra.transform.position = new Vector3 (dir.position.x-0.5f, posicaoMao.y, posicaoMao.z);		
			
			// alterando transparencia de acordo com a distancia do objeto
			dist = Vector3.Distance(sombra.transform.position, transform.position);
			color.a = dist;
			sombra.GetComponent<Renderer>().material.color = color;
		}            
		
		
	    // teto
		sombra = sombraTeto;
		sombra.transform.position = new Vector3 (posicaoMao.x, teto.position.y - 0.5f, posicaoMao.z);
   
		
		// alterando transparencia de acordo com a distancia do objeto
		dist = Vector3.Distance(sombra.transform.position, transform.position);
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;	
    
    }
}
