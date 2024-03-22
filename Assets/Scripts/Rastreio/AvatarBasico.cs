using UnityEngine;
using System.Collections;

public class AvatarBasico : MonoBehaviour {
	
	UDP datagrama;
	BaseController padrao;

	GameObject cabeca, maoDir, maoEsq, cotoveloEsq, cotoveloDir, joelhoDir, joelhoEsq, peDir, peEsq;
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
		cabeca = CreateBodyPart();
		maoDir = CreateBodyPart();
		maoEsq = CreateBodyPart();
		peDir = CreateBodyPart();
		peEsq = CreateBodyPart();
		
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
		peDir.transform.position = datagrama.peDir;
		peEsq.transform.position = datagrama.peEsq;
		
	}

}
