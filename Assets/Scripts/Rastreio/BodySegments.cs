using UnityEngine;
using System.Collections;

public class BodySegments : MonoBehaviour {
	
	public GameObject 
		cylinder,
		anteBracoEsq, anteBracoDir,
		bracoEsq, bracoDir,
		canelaEsq, canelaDir,
		pernaEsq, pernaDir, cabeca;
	UDP datagrama;
	
	string 
		left_arm = "Left_Arm", 
		right_arm = "Right_Arm",
		left_foot = "Left_Foot",
		right_foot = "Right_Foot",
		head = "Head";
	
	void Awake()
	{
		//this.enabled = false;
	}
	
	void Start () 
	{		
		datagrama = GameObject.Find("UDP").GetComponent<UDP>();
		cylinder = Resources.Load("Cylinder") as GameObject;
		
		Vector3 aux = new Vector3();
		aux = datagrama.cabeca;
		aux.y -= (float)0.1;
		cabeca = CreateSegmentBodyPart(datagrama.cabeca,aux, "cabeca");
		cabeca.tag = head;		
				
		anteBracoEsq = CreateSegmentBodyPart(datagrama.maoEsq, datagrama.cotoveloEsq, "anteBracoEsq");
		bracoEsq = CreateSegmentBodyPart(datagrama.cotoveloEsq, datagrama.ombroEsq, "bracoEsq");
		anteBracoEsq.tag = left_arm;
		bracoEsq.tag = left_arm;
		
		anteBracoDir = CreateSegmentBodyPart(datagrama.maoDir, datagrama.cotoveloDir, "anteBracoDir");
		bracoDir = CreateSegmentBodyPart(datagrama.cotoveloDir, datagrama.ombroDir, "bracoDir");
		anteBracoDir.tag = right_arm;
		bracoDir.tag = right_arm;
		
		canelaDir = CreateSegmentBodyPart(datagrama.peDir, datagrama.joelhoDir, "canelaDir");
		pernaDir = CreateSegmentBodyPart(datagrama.joelhoDir, datagrama.cinturaDir, "pernaDir");
		canelaDir.tag = pernaDir.tag = right_foot;
		
		canelaEsq = CreateSegmentBodyPart(datagrama.peEsq, datagrama.joelhoEsq, "canelaEsq");
		pernaEsq = CreateSegmentBodyPart(datagrama.joelhoEsq, datagrama.cinturaEsq, "pernaEsq");
		canelaEsq.tag = pernaEsq.tag = left_foot;


		cabeca.transform.eulerAngles = new Vector3(0,0,0);
		cabeca.transform.localScale = new Vector3(50,50,50);
		
	}

	GameObject CreateSegmentBodyPart (Vector3 origem, Vector3 destino, string name)
	{
		GameObject segment;
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, destino-origem);
		Vector3 pos = new Vector3(origem.x + 0.5f * ( destino.x - origem.x), origem.y + 0.5f * 
			( destino.y - origem.y), origem.z + 0.5f * ( destino.z - origem.z));
		
		segment = Instantiate(cylinder, pos, rot) as GameObject;
		segment.transform.localScale = new Vector3 (50, Vector3.Distance(origem,destino)/2, 50);

		segment.transform.name = name;
				
		segment.transform.parent = transform;
		
		return segment;
	}
	
	
	private void Update() 
	{		
		cabeca.transform.position = datagrama.cabeca;
		cabeca.transform.eulerAngles = new Vector3(0,0,0);
		
		UptadeSegment(anteBracoEsq, datagrama.maoEsq, datagrama.cotoveloEsq);
		UptadeSegment(anteBracoDir, datagrama.maoDir, datagrama.cotoveloDir);
		UptadeSegment(bracoEsq, datagrama.cotoveloEsq, datagrama.ombroEsq);
		UptadeSegment(bracoDir, datagrama.cotoveloDir, datagrama.ombroDir);
		UptadeSegment(canelaEsq, datagrama.peEsq, datagrama.joelhoEsq);
		UptadeSegment(canelaDir, datagrama.peDir, datagrama.joelhoDir);
		UptadeSegment(pernaEsq, datagrama.joelhoEsq, datagrama.cinturaEsq);
		UptadeSegment(pernaDir, datagrama.joelhoDir, datagrama.cinturaDir);		
	}

	void UptadeSegment (GameObject segmentPart, Vector3 origem, Vector3 destino)
	{
		//Quaternion desRot = Quaternion.FromToRotation(Vector3.up, origem - destino);

		Quaternion desRot = Quaternion.FromToRotation(Vector3.up, destino - origem);

		Vector3 desPos = new Vector3(origem.x + 0.5f * ( destino.x - origem.x), origem.y + 0.5f * 
			( destino.y - origem.y), origem.z + 0.5f * ( destino.z - origem.z));

		//segmentPart.transform.rotation = Quaternion.Lerp(segmentPart.transform.rotation, desRot, 0.1f);
		//segmentPart.transform.position = Vector3.Lerp(segmentPart.transform.position, desPos, 0.1f);		

		segmentPart.transform.rotation = desRot;
		segmentPart.transform.position = desPos;	
		
		
		segmentPart.transform.localScale = new Vector3 (50, Vector3.Distance(origem, destino)/2, 50);
	}
}
