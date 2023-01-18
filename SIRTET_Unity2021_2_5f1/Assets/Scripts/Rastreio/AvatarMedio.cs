using UnityEngine;
using System.Collections;

public class AvatarMedio : MonoBehaviour {
	
	UDP datagrama;
	BaseController padrao;

	GameObject aux, cabeca, maoDir, maoEsq, cotoveloEsq, cotoveloDir, joelhoDir, joelhoEsq, peDir, peEsq;
	Vector3 position;
	
	GameObject sphere;
	
	void Awake()
	{
		this.enabled = false;
	}
	
	
	// Use this for initialization
	void Start () {
		datagrama = GameObject.Find("UDP").GetComponent<UDP>();
		padrao = GameObject.Find("Ambiente").GetComponent<BaseController>();
		sphere = Resources.Load("Sphere") as GameObject;
		CreateBody();
		
	}

	void CreateBody()
	{
		
		float metadePadrao = padrao.standardSize / 1.5f;
		
		cabeca = CreateBodyPart();
		maoDir = CreateBodyPart();
		maoEsq = CreateBodyPart();
		
		
		cotoveloEsq = CreateBodyPart();
		cotoveloEsq.transform.localScale = new Vector3(metadePadrao, metadePadrao, metadePadrao);
		cotoveloDir = CreateBodyPart();
		cotoveloDir.transform.localScale = new Vector3(metadePadrao, metadePadrao, metadePadrao);
		
		//joelhoDir = CreateBodyPart();
		//joelhoDir.transform.localScale = new Vector3(metadePadrao, metadePadrao, metadePadrao);
		//joelhoEsq = CreateBodyPart();
		//joelhoEsq.transform.localScale = new Vector3(metadePadrao, metadePadrao, metadePadrao);
		
		//peDir = CreateBodyPart();
		//peEsq = CreateBodyPart();
		
	}

	GameObject CreateBodyPart ()
	{
		GameObject bodyPart;
		bodyPart = Instantiate(sphere, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		bodyPart.transform.localScale = new Vector3(padrao.standardSize, padrao.standardSize , padrao.standardSize);
		bodyPart.transform.parent = transform;
		
		return bodyPart;
		
	}

	// Update is called once per frame
	void Update () {
		
		cabeca.transform.position = datagrama.cabeca;
		maoDir.transform.position = datagrama.maoDir;
		maoEsq.transform.position = datagrama.maoEsq;
		cotoveloDir.transform.position = datagrama.cotoveloDir;
		cotoveloEsq.transform.position = datagrama.cotoveloEsq;
		//joelhoDir.transform.position = datagrama.joelhoDir;
		//joelhoEsq.transform.position = datagrama.joelhoEsq;
		//peDir.transform.position = datagrama.peDir;
		//peEsq.transform.position = datagrama.peEsq;
		
	}

}
