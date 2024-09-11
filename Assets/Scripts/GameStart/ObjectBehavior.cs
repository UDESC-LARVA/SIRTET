using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectBehavior : MonoBehaviour {
	
	int behaviorAnimation = 1;
	
	public static bool tipoElementoVisual = true;
	
	public static bool mostrarSombras = true;

	// Chache classes e metodos
	BaseController controladora;
	Interface gui;
	ChallengeTracer tracer;
	Transform posicaoChao, posicaoTeto, posicaoParedeEsq, posicaoParedeDir;	
    GameObject sombraChao, sombraTeto, sombraEsq, sombraDir;
	GameObject sombraResource;
	
	
	// Datamodel dele mesmo...
	public Desafio repetir; 
	public Objeto objeto;
	
	public string type;	
	GameObject elementoGrafico;
	GameObject fantasma;
	LimitBehavior[] limits;


	//valores para a surpresa
	Vector2 xLimit, yLimit;
	Vector3 posRand;
	
	// Use this for initialization
	void Start () {
		controladora = GameObject.Find("Ambiente").GetComponent<BaseController>();
		gui = GameObject.FindGameObjectWithTag("Interface").GetComponent<Interface>();
		tracer = GameObject.Find("Objetos").GetComponent<ChallengeTracer>();
		
		limits = GameObject.FindObjectsOfType<LimitBehavior>();
		
		posicaoChao = GameObject.Find("Chao").GetComponent<Transform>().transform;
		posicaoTeto = GameObject.Find("Teto").GetComponent<Transform>().transform;
		posicaoParedeEsq = GameObject.Find("ParedeEsquerda").GetComponent<Transform>().transform;
		posicaoParedeDir = GameObject.Find("ParedeDireita").GetComponent<Transform>().transform;
		sombraResource = Resources.Load("Sombra") as GameObject;
		CreateShadows();

		elementoGrafico = this.transform.Find("elementoGrafico").gameObject;		
		fantasma = this.transform.Find("fantasma").gameObject;
		
		//fantasma.SetActive(false);
		//Debug.Log("APAGAR LINHA ACIMA");

		type = objeto.Type;

		if(this.type == "Surpresa")
		{
			//limites conforme a base sem a leitura de corpo, verificar o limite variado com a leitura e os dados do controlador
			xLimit = new Vector2(-900,900);
			yLimit = new Vector2(400,1600);
			posRand = new Vector3(0, 1000, this.transform.position.z);
			InvokeRepeating("RandomizaPosicao", 0, 1.5f);
		}

		if(!tipoElementoVisual)
			SetElement();

		if(!mostrarSombras)
			SetShadows();

	}

	void RandomizaPosicao()
	{
		float x = UnityEngine.Random.Range(xLimit.x, xLimit.y);
		float y = UnityEngine.Random.Range(yLimit.x, yLimit.y);
		posRand = new Vector3(x,y,this.transform.position.z);
	}

	
	void OnCollisionEnter (Collision Target)
	{
		this.GetComponent<Collider>().enabled = false;
		Invoke("DestroyObject", 2);
		//Debug.Log(objeto.Type + "|  |" + Target.transform.name);
		
		fantasma.SetActive(false);

		if(Target.gameObject.name == "Fim")
		{
			if(objeto.Type == "Alvo")
			{
				NegativeAnimation(Target.transform.name);				
			}
			else if(objeto.Type == "Obstaculo")
			{
				PositiveAnimation(Target.transform.name);
			}
			else if(objeto.Type == "Surpresa")
			{
				NegativeAnimation(Target.transform.name);
			}
		}
		else//ENCOSTOU NO JOGADOR
		{
			if(objeto.Type == "Alvo" )
			{				
				PositiveAnimation(Target.transform.name);
			}
			else if(objeto.Type == "Obstaculo")
			{
				NegativeAnimation(Target.transform.name);
			}	
			else if(objeto.Type == "Surpresa")
			{
				PositiveAnimation(Target.transform.name);
			}			
		}
	}
	
	void DestroyObject()
	{
		Destroy(this.transform.parent.gameObject);
	}
	
	#region Shadows
	
	void CreateShadows()
	{
		Vector3 posicaoSombra;
		Quaternion rotacao;
		Color color = GetComponent<Renderer>().material.color;
		
		// chao
		posicaoSombra = transform.position;
		posicaoSombra.y = posicaoChao.position.y + 0.5f;
		GameObject sombra = Instantiate(sombraResource, posicaoSombra, Quaternion.identity) as GameObject;
		sombra.transform.parent = transform;
		sombra.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		sombraChao = sombra;
		
		// alterando transparencia de acordo com a distancia do objeto
		float dist = Vector3.Distance(sombra.transform.position, transform.position);
		dist = ((dist - (controladora.height * 10)) / -(controladora.height * 10));
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;
		
		
		//paredeEsquerda
		posicaoSombra = transform.position;
		posicaoSombra.x =  posicaoParedeDir.position.x - 0.5f;
		rotacao = posicaoParedeDir.localRotation;
		sombra = Instantiate(sombraResource, posicaoSombra, rotacao) as GameObject;
		sombra.transform.parent = transform;
		sombra.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		sombraEsq = sombra;
		
		// alterando transparencia de acordo com a distancia do objeto
		dist = Vector3.Distance(sombra.transform.position, transform.position);
		dist = ( (dist - (controladora.height * 10)) / -(controladora.height * 10));
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;
		
		
		//paredeEsquerda
		posicaoSombra = transform.position;
		posicaoSombra.x =  posicaoParedeEsq.position.x + 0.5f;
		rotacao = posicaoParedeEsq.localRotation;
		sombra = Instantiate(sombraResource, posicaoSombra, rotacao) as GameObject;
		sombra.transform.parent = transform;
		sombra.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		sombraDir = sombra;
		
		// alterando transparencia de acordo com a distancia do objeto
		dist = Vector3.Distance(sombra.transform.position, transform.position);
		dist = ( (dist - (controladora.height * 10)) / -(controladora.height * 10));
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;
		
		
		// teto
		posicaoSombra = transform.position;
		posicaoSombra.y =  posicaoTeto.position.y - 0.5f;
		rotacao = posicaoTeto.localRotation;
		sombra = Instantiate(sombraResource, posicaoSombra, rotacao) as GameObject;
		sombra.transform.parent = transform;
		sombra.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		sombraTeto = sombra;

		// alterando transparencia de acordo com a distancia do objeto
		dist = Vector3.Distance(sombra.transform.position, transform.position);
		dist = ( (dist - (controladora.height * 10)) / -(controladora.height * 10));
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;
	}

	void UpdateShadows()
	{
		Vector3 posicaoMao = this.transform.position;
		float dist;
		GameObject sombra = null;
		
		Color color = GetComponent<Renderer>().material.color;
		
		// chao
        sombra = sombraChao;
		sombra.transform.position = new Vector3 (posicaoMao.x, posicaoChao.position.y + 0.5f, posicaoMao.z);
		
		// alterando transparencia de acordo com a distancia do objeto
		dist = Vector3.Distance(sombra.transform.position, transform.position);
		dist = ((dist - (controladora.height * 10)) / -(controladora.height * 10));
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;
        
        // paredeEsquerda
		sombra = sombraEsq;
		sombra.transform.position = new Vector3 (posicaoParedeDir.position.x-0.5f, posicaoMao.y, posicaoMao.z);

		
		// alterando transparencia de acordo com a distancia do objeto
		dist = Vector3.Distance(sombra.transform.position, transform.position);
		dist = ((dist - (controladora.height * 10)) / -(controladora.height * 10));
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;
	
		// paredeDireita
		sombra = sombraDir;
		sombra.transform.position = new Vector3 (posicaoParedeEsq.position.x+0.5f, posicaoMao.y, posicaoMao.z);
	
		
		// alterando transparencia de acordo com a distancia do objeto
		dist = Vector3.Distance(sombra.transform.position, transform.position);
		dist = ((dist - (controladora.height * 10)) / -(controladora.height * 10));
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;
						
		
	    // teto
		sombra = sombraTeto;
		sombra.transform.position = new Vector3 (posicaoMao.x, posicaoTeto.position.y - 0.5f, posicaoMao.z);
   
		
		// alterando transparencia de acordo com a distancia do objeto
		dist = Vector3.Distance(sombra.transform.position, transform.position);
		dist = ((dist - (controladora.height * 10)) / -(controladora.height * 10));
		color.a = dist;
		sombra.GetComponent<Renderer>().material.color = color;		
	}
	
	void DestroyShadows ()
	{
		List<GameObject> children = new List<GameObject>();
		foreach(Transform child in transform)
			children.Add(child.gameObject);
		children.ForEach(child => Destroy (child));
	}
	
	#endregion
	
	void PositiveAnimation(string hitName)
	{
		foreach(LimitBehavior limit in limits)
			limit.LimitColor(Color.green);

		controladora.PlaySoundPos();
		transform.GetComponent<Renderer>().material.color -= new Color(0,0,0,0.75f);
		tracer.HitOccured(objeto, true, this.transform.position, hitName);		
	}
	
	void NegativeAnimation(string hitName)
	{
		foreach(LimitBehavior limit in limits)
			limit.LimitColor(Color.red);

		controladora.PlaySoundNeg();
//		transform.renderer.material.color -= new Color(0,0,0,0.75f);
		objeto.Points *= -1;
		behaviorAnimation = 2;   // Negativo
		tracer.HitOccured(objeto, false, this.transform.position, hitName);
	}
	
	// Update is called once per frame
	void Update () {
		
		switch (behaviorAnimation) {
		case 1: // Move foward
			if(objeto.Type != "Surpresa")
			{
				transform.position = Vector3.Lerp(transform.position, transform.position - (Vector3.forward * (controladora.standardSize * 5)), Time.deltaTime * (controladora.standardObjVelocity));	
			}else
			{
				transform.position = Vector3.Lerp(transform.position, transform.position - (Vector3.forward * (controladora.standardSize * 5)), Time.deltaTime * (controladora.standardObjVelocity));	
							
				Vector3 posicaoFinal = Vector3.Lerp(transform.position, posRand, Time.deltaTime * (controladora.standardObjVelocity)*0.2f);
				posicaoFinal.z = this.transform.position.z;
				transform.position = posicaoFinal;
				
				UpdateShadows();
			}
			
			if(fantasma!=null)
			{
				//fantasma.transform.position = new Vector3(fantasma.transform.position.x, fantasma.transform.position.y, limits[0].transform.position.z);
				fantasma.transform.position = new Vector3(fantasma.transform.position.x, fantasma.transform.position.y, controladora.auxBase.transform.position.z);
			}
			
			break;
		case 2: // Explode -- Negativo -- vermelho
			if(transform.childCount > 0)
				DestroyShadows();
			transform.GetComponent<Renderer>().material.color = Color.Lerp(transform.GetComponent<Renderer>().material.color, transform.GetComponent<Renderer>().material.color - new Color(1,1,1,1), Time.deltaTime * 2);
			transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale + Vector3.one * 250, Time.deltaTime);
			break;
		case 3: // Implode  -- Postivo -- verde
			if(transform.childCount > 0)
				DestroyShadows();
			transform.GetComponent<Renderer>().material.color = Color.Lerp(transform.GetComponent<Renderer>().material.color, transform.GetComponent<Renderer>().material.color - new Color(-1,-1,-1,1), Time.deltaTime * 2);
			transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale - Vector3.one * 250, Time.deltaTime);
			break;
		}

		if(Input.GetKeyDown(KeyCode.T))
		{
			SetElement();
		}

		if(Input.GetKeyDown(KeyCode.W))
		{
			SetShadows();
		}
	}


	float aOriginal;
	
	void SetElement()
	{
		if(elementoGrafico == null)
			return;

		if(elementoGrafico.activeSelf)
		{
			elementoGrafico.SetActive(false);
			Color cor = this.GetComponent<Renderer>().material.color;
			aOriginal = cor.a;
			cor.a = 1;
			this.GetComponent<Renderer>().material.color = cor;

		}else
		{
			elementoGrafico.SetActive(true);
			Color cor = this.GetComponent<Renderer>().material.color;
			cor.a = aOriginal;
			this.GetComponent<Renderer>().material.color = cor;
		}

		tipoElementoVisual = elementoGrafico.activeSelf;
	}


	void SetShadows()
	{
		if(sombraChao == null || sombraDir == null || sombraEsq == null || sombraTeto == null)
			return;

		if(sombraChao.activeSelf)
		{
			sombraChao.SetActive(false);
			sombraDir.SetActive(false);
			sombraEsq.SetActive(false);
			sombraTeto.SetActive(false);
		}else
		{
			sombraChao.SetActive(true);
			sombraDir.SetActive(true);
			sombraEsq.SetActive(true);
			sombraTeto.SetActive(true);
		}	
		
		mostrarSombras = sombraChao.activeSelf;
	}
	
	
}
