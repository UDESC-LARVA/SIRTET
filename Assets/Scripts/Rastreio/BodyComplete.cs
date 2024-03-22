using UnityEngine;
using System.Collections;

public class BodyComplete : MonoBehaviour {
	
	UDP datagrama;
	GameController game;

	public GameObject 
		cabeca, maoDir, maoEsq, 
		cotoveloEsq, cotoveloDir,
		ombroEsq, ombroDir,
		cinturaEsq, cinturaDir,
		joelhoDir, joelhoEsq, 
		peDir, peEsq;
	Vector3 position;
	
	GameObject sphere;

	string 
		left_arm = "Left_Arm", 
		right_arm = "Right_Arm",
		left_foot = "Left_Foot",
		right_foot = "Right_Foot",
		
		head = "Head";
	
	float standardSize, standardSizeHalf;
	
	void Awake()
	{
		this.enabled = false;
	}
	
	void Start () 
	{		
		datagrama = GameObject.Find("UDP").GetComponent<UDP>();
		game = GameObject.Find ("Game Controller").GetComponent<GameController>();
		
		//Resources
		sphere = Resources.Load("Sphere") as GameObject;
		
		standardSize = (float)game.file.parameters.EnvironmentParameters.Size;
		standardSizeHalf = standardSize / 2;
		
		CreateBody();
		AddShadowOnHands();
		
		GameObject.Find("UDP").GetComponent<BodySegments>().enabled = true;
		
	}

	void CreateBody()
	{
				
		// Braco Dir
		maoDir = CreateBodyPart("maoDir");
		cotoveloDir = CreateBodyPart("cotoveloDir");
		ombroDir = CreateBodyPart("ombroDir");
		//Join all the parts to form one
		maoDir.tag = right_arm;
		cotoveloDir.tag = right_arm;
		ombroDir.tag = right_arm;
		
		// Braco Esq
		maoEsq = CreateBodyPart("maoEsq");
		cotoveloEsq = CreateBodyPart("cotoveloEsq");
		ombroEsq = CreateBodyPart("ombroEsq");
		//Join all the parts to form one
		maoEsq.tag = left_arm;
		cotoveloEsq.tag = left_arm;
		ombroEsq.tag = left_arm;
		
		
		// Left foot
		cinturaEsq = CreateBodyPart("cinturaEsq");
		joelhoEsq = CreateBodyPart("joelhoEsq");
		peEsq = CreateBodyPart("peEsq");
		//Join all the parts to form one
		cinturaEsq.tag = left_foot;
		joelhoEsq.tag = left_foot;
		peEsq.tag = left_foot;
		
		// Right foot
		cinturaDir = CreateBodyPart("cinturaDir");
		joelhoDir = CreateBodyPart("joelhoDir");
		peDir = CreateBodyPart("peDir");
		//Join all the parts to form one
		cinturaDir.tag = right_foot;
		joelhoDir.tag = right_foot;
		peDir.tag = right_foot;
		
		
		cabeca = CreateBodyPart("cabeca");
		cabeca.transform.localScale *= 2.5f;
		cabeca.tag = head;
				
	}

	public void AddShadowOnHands()
	{
		maoDir.AddComponent<HandShadowController>();
		maoEsq.AddComponent<HandShadowController>();
	}


	GameObject CreateBodyPart (string PartName)
	{
		GameObject bodyPart;
		bodyPart = Instantiate(sphere, Vector3.zero, Quaternion.identity) as GameObject;
		bodyPart.transform.localScale = new Vector3(standardSizeHalf, standardSizeHalf , standardSizeHalf);
		bodyPart.transform.parent = transform;
		bodyPart.transform.name = PartName;
		
		return bodyPart;
		
	}

	// Update is called once per frame
	void Update () 
	{	
		cabeca.transform.position = datagrama.cabeca;
				
		maoDir.transform.position = datagrama.maoDir;
		maoEsq.transform.position = datagrama.maoEsq;
		
		cotoveloDir.transform.position = datagrama.cotoveloDir;
		cotoveloEsq.transform.position = datagrama.cotoveloEsq;
		
		ombroEsq.transform.position = datagrama.ombroEsq;
		ombroDir.transform.position = datagrama.ombroDir;

		cinturaEsq.transform.position = datagrama.cinturaEsq;
		cinturaDir.transform.position = datagrama.cinturaDir;
		
		joelhoDir.transform.position = datagrama.joelhoDir;
		joelhoEsq.transform.position = datagrama.joelhoEsq;
		
		peDir.transform.position = datagrama.peDir;
		peEsq.transform.position = datagrama.peEsq;
		
	}
	

}
