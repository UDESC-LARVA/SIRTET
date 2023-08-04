using UnityEngine;
using System.Collections;
 
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;



using UnityEditor;
 
public class UDP: MonoBehaviour
{
	
	public float naturalHeight, naturalFeetDistance, naturalWingspan;
	bool showSizes = false;
	
	//http://www.roymech.co.uk/Useful_Tables/Human/Human_sizes.html
	public float 
		neck, shoulderBreadth, sittingShoulder,
		leftArm, leftForearm, rightArm, rightForearm,
		leftLowerLeg, leftLeg, rightLowerLeg, rightLeg;
	bool sizeCaught = false;
	public bool status;
	public double kinectAngle;
	public Vector3 
		cabeca,cabeca_aux, pescoso,
		ombroEsq, ombroDir,
		cotoveloEsq, cotoveloDir,
		maoEsq, maoDir,
		cinturaEsq, cinturaDir,
		joelhoEsq, joelhoDir,
		peEsq, peDir;
	Thread receiveThread;
	UdpClient client;
	
	public void Awake ()
	{
		init ();
		DontDestroyOnLoad (transform.gameObject);

		//*
		status = !status;
		Debug.LogWarning("APAGAR LINHAS ACIMA");
		//*/
	}
	
	public void Start ()
	{
		kinectAngle = 5.0;
		
		#region setar posicao inicial -- para teste sem o kinect
		
		cabeca = new Vector3 (0f, 500f, -2500f);
		cabeca_aux = new Vector3 (0f, 500f, -2500f);
		maoEsq = new Vector3 (-300f, -200f, -2500f);
		maoDir = new Vector3 (300f, -200f, -2500f);
		peEsq = new Vector3 (-200f, -1000f, -2500f);
		peDir = new Vector3 (200f, -1000f, -2500f);
		
		cotoveloEsq = new Vector3 (-300f, 0f, -2500f);
		cotoveloDir = new Vector3 (300f, 0f, -2500f);
		joelhoEsq = new Vector3 (-200f, -600f, -2500f);
		joelhoDir = new Vector3 (200f, -600f, -2500f);
		
		pescoso = new Vector3 (0f, 200f, -2500f);
		ombroEsq = new Vector3 (-200f, 200f, -2500f);
		ombroDir = new Vector3 (200f, 200f, -2500f);
		cinturaEsq = new Vector3 (-120f, -400f, -2500f);
		cinturaDir = new Vector3 (120f, -400f, -2500f);
	
		#region Ajustes para testar em casa
		//cabeca.z = KinectDistance (kinectAngle, cabeca);
		cabeca.y = escalarY (cabeca.y);
		cabeca.z = escalarZ (cabeca.z);
		cabeca_aux = cabeca;
		
		pescoso.y = escalarY (pescoso.y);
		pescoso.z = escalarZ (pescoso.z);
		
		maoEsq.y = escalarY (maoEsq.y);
		maoEsq.z = escalarZ (maoEsq.z);
		
		maoDir.y = escalarY (maoDir.y);
		maoDir.z = escalarZ (maoDir.z);
		
		cotoveloDir.y = escalarY (cotoveloDir.y);
		cotoveloDir.z = escalarZ (cotoveloDir.z);
		
		cotoveloEsq.y = escalarY (cotoveloEsq.y);
		cotoveloEsq.z = escalarZ (cotoveloEsq.z);
		
		ombroDir.y = escalarY (ombroDir.y);
		ombroDir.z = escalarZ (ombroDir.z);
		
		ombroEsq.y = escalarY (ombroEsq.y);
		ombroEsq.z = escalarZ (ombroEsq.z);
			
		cinturaDir.y = escalarY (cinturaDir.y);
		cinturaDir.z = escalarZ (cinturaDir.z);
		
		cinturaEsq.y = escalarY (cinturaEsq.y);
		cinturaEsq.z = escalarZ (cinturaEsq.z);
				
		joelhoDir.y = escalarY (joelhoDir.y);
		joelhoDir.z = escalarZ (joelhoDir.z);
		
		joelhoEsq.y = escalarY (joelhoEsq.y);
		joelhoEsq.z = escalarZ (joelhoEsq.z);
		
		peEsq.y = escalarY (peEsq.y);
		peEsq.z = escalarZ (peEsq.z);
		
		peDir.y = escalarY (peDir.y);
		peDir.z = escalarZ (peDir.z);
		
		#endregion	
		#endregion
		
	}
	
	public void Update ()
	{
		if (Input.GetKeyDown("n"))
			showSizes = !showSizes;
	
		if (Input.GetKeyDown ("k"))
			status = !status;
	}


	private void init ()
	{
		receiveThread = new Thread (
            new ThreadStart (ReceiveData));
 
		receiveThread.IsBackground = true;
		receiveThread.Start ();
 
		print ("Start"); 
	}

	private void ReceiveData ()
	{
		//int somePort = 8051;//porta original do sistet, pega as informações do kinekt
		int somePort = 8051;

		//int portTestMediaPipe = 15656;
		//somePort = portTestMediaPipe;

		string verifyTextUDP = null;

		try {
 
		 	client = new UdpClient (somePort);
			
			while (Thread.CurrentThread.IsAlive) {

				IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
				byte[] data = new byte[1024];
				
				data = client.Receive (ref anyIP);
				
					
				string text = Encoding.UTF8.GetString (data);				
				//Debug.Log("dados recebidos " +text);				

				//x = float.Parse(text);
				string[] words = text.Split (';');
				
				/*==int cont = 0; foreach (var w in words) {Debug.Log("["+ cont+"] - "+w); cont++;}*/
				
				Vector3 coord3D;
				#region Membros do corpo
				#region Mao Esquerda
				coord3D.x = float.Parse (words [0]);
				coord3D.y = escalarY (float.Parse (words [1]));
				coord3D.z = escalarZ (-(float.Parse (words [2])));
								
				
				maoEsq = coord3D;
				//print ("Mao Esquerda: "+ maoEsq);
				#endregion
				
				#region Mao Direita
				coord3D.x = float.Parse (words [3]);
				coord3D.y = escalarY (float.Parse (words [4]));
				coord3D.z = escalarZ (-(float.Parse (words [5])));
				
				maoDir = coord3D;
				//print ("Mao Direita: "+ maoDir);
				#endregion
				
				#region Cabeca
				coord3D.x = float.Parse (words [6]);
				coord3D.y = escalarY (float.Parse (words [7]));
				coord3D.z = escalarZ (-(float.Parse (words [8])));
				
				cabeca = coord3D;
				cabeca_aux = cabeca;
				cabeca_aux.y = cabeca_aux.y - 1;
				#endregion

				#region Pe Esquerdo
				coord3D.x = float.Parse (words [9]);
				coord3D.y = escalarY (float.Parse (words [10]));
				coord3D.z = escalarZ (-(float.Parse (words [11])));
				
				peEsq = coord3D;
				//print ("Pe Esquerdo: "+ peEsq);
				#endregion

				#region Pe Direito
				coord3D.x = float.Parse (words [12]);
				coord3D.y = escalarY (float.Parse (words [13]));
				coord3D.z = escalarZ (-(float.Parse (words [14])));
				
				peDir = coord3D;
				//print ("Pe Direito: "+ peDir);
				#endregion
				
				#region Joelho Esquerdo
				coord3D.x = float.Parse (words [15]);
				coord3D.y = escalarY (float.Parse (words [16]));
				coord3D.z = escalarZ (-(float.Parse (words [17])));
				
				joelhoEsq = coord3D;
//				print ("Joelho Esquerdo: "+ joelhoEsq);
				#endregion
				
				#region Joelho Direito
				coord3D.x = float.Parse (words [18]);
				coord3D.y = escalarY (float.Parse (words [19]));
				coord3D.z = escalarZ (-(float.Parse (words [20])));
				
				joelhoDir = coord3D;
//				print ("Joelho Dir: "+ joelhoDir);
				#endregion
				
				#region Cintura Direita
				coord3D.x = float.Parse (words [21]);
				coord3D.y = escalarY (float.Parse (words [22]));
				coord3D.z = escalarZ (-(float.Parse (words [23])));
				
				cinturaDir = coord3D;
//				print ("Cintura Dir: "+ cinturaDir);
				#endregion
				
				#region Cintura Esquerda
				coord3D.x = float.Parse (words [24]);
				coord3D.y = escalarY (float.Parse (words [25]));
				coord3D.z = escalarZ (-(float.Parse (words [26])));
				
				
				cinturaEsq = coord3D;
//				print ("Cintura Esquerdo: "+ cinturaEsq);
				#endregion
				
				#region Ombro Esquerdo
				coord3D.x = float.Parse (words [27]);
				coord3D.y = escalarY (float.Parse (words [28]));
				coord3D.z = escalarZ (-(float.Parse (words [29])));
				
				ombroEsq = coord3D;
//				print ("Ombro Esquerdo: "+ ombroEsq);
				#endregion
				
				#region Ombro Direito
				coord3D.x = float.Parse (words [30]);
				coord3D.y = escalarY (float.Parse (words [31]));
				coord3D.z = escalarZ (-(float.Parse (words [32])));
				
				ombroDir = coord3D;
//				print ("Ombro Dir: "+ ombroDir);
				#endregion
				
				#region Cotovelo Esquerdo
				coord3D.x = float.Parse (words [33]);
				coord3D.y = escalarY (float.Parse (words [34]));
				coord3D.z = escalarZ (-(float.Parse (words [35])));
				
				cotoveloEsq = coord3D;
//				print ("Cotovelo Esquerdo: "+ cotoveloEsq);
				#endregion
				
				#region Cotovelo Direito
				coord3D.x = float.Parse (words [36]);
				coord3D.y = escalarY (float.Parse (words [37]));
				coord3D.z = escalarZ (-(float.Parse (words [38])));
				
				cotoveloDir = coord3D;
//				print ("Cotovelo Dir: "+ cotoveloDir);
				#endregion
				
				#region pescoso
				coord3D.x = float.Parse (words [39]);
				coord3D.y = escalarY (float.Parse (words [40]));
				coord3D.z = escalarZ (-(float.Parse (words [41])));
				
				pescoso = coord3D;
//				print ("pescoso: "+ pescoso);

				

				#endregion
				#endregion
			
				if (!sizeCaught) {
					#region Funçoes antropometricas com valores 'reais' 
					
					
					neck = Vector3.Distance (
						new Vector3 (float.Parse (words [6]), float.Parse (words [7]), float.Parse (words [8])),
						new Vector3 (float.Parse (words [39]), float.Parse (words [40]), float.Parse (words [41])));
					
					shoulderBreadth = Vector3.Distance (
						new Vector3 (float.Parse (words [27]), float.Parse (words [28]), float.Parse (words [29])),
						new Vector3 (float.Parse (words [30]), float.Parse (words [31]), float.Parse (words [32])));
					
					sittingShoulder = Vector3.Distance (
						new Vector3 (float.Parse (words [27]), float.Parse (words [28]), float.Parse (words [29])),
						new Vector3 (float.Parse (words [24]), float.Parse (words [25]), float.Parse (words [26])));	
					
					leftArm = Vector3.Distance (
						new Vector3 (float.Parse (words [0]), float.Parse (words [1]), float.Parse (words [2])),
						new Vector3 (float.Parse (words [33]), float.Parse (words [34]), float.Parse (words [35])));
					
					leftForearm = Vector3.Distance (
						new Vector3 (float.Parse (words [33]), float.Parse (words [34]), float.Parse (words [35])),
						new Vector3 (float.Parse (words [27]), float.Parse (words [28]), float.Parse (words [29])));
					
					rightArm = Vector3.Distance (
						new Vector3 (float.Parse (words [3]), float.Parse (words [4]), float.Parse (words [5])),
						new Vector3 (float.Parse (words [36]), float.Parse (words [37]), float.Parse (words [38])));
					
					rightForearm = Vector3.Distance (
						new Vector3 (float.Parse (words [36]), float.Parse (words [37]), float.Parse (words [38])),
						new Vector3 (float.Parse (words [30]), float.Parse (words [31]), float.Parse (words [32])));
					
					leftLowerLeg = Vector3.Distance (
						new Vector3 (float.Parse (words [9]), float.Parse (words [10]), float.Parse (words [11])),
						new Vector3 (float.Parse (words [15]), float.Parse (words [16]), float.Parse (words [17])));
					
					leftLeg = Vector3.Distance (
						new Vector3 (float.Parse (words [15]), float.Parse (words [16]), float.Parse (words [17])),
						new Vector3 (float.Parse (words [24]), float.Parse (words [25]), float.Parse (words [26])));
					
					rightLowerLeg = Vector3.Distance (
						new Vector3 (float.Parse (words [12]), float.Parse (words [13]), float.Parse (words [14])),
						new Vector3 (float.Parse (words [18]), float.Parse (words [19]), float.Parse (words [20])));
					
					rightLeg = Vector3.Distance (
						new Vector3 (float.Parse (words [18]), float.Parse (words [19]), float.Parse (words [20])),
						new Vector3 (float.Parse (words [21]), float.Parse (words [22]), float.Parse (words [32])));
					
					naturalHeight = (neck + sittingShoulder + rightLeg + rightLowerLeg + 150) * 0.001f;
					naturalWingspan = (leftArm + leftForearm + shoulderBreadth + rightForearm + rightArm) * 0.001f;					
					
					#endregion
					
//					sizeCaught = true;
				}
					
				naturalFeetDistance = NaturalFeetDistance (
					float.Parse (words [9]), 
					float.Parse (words [10]),
					float.Parse (words [11]),
					float.Parse (words [12]),
					float.Parse (words [13]),
					float.Parse (words [14])
					);
				
				
				if (verifyTextUDP == text)
					status = false;
				else
					status = true;
				
				verifyTextUDP = text;


			}
 
		} catch (Exception e) {
			status = false;			
			print (e);
		}
	} 
	
	private void OnDrawGizmos() 		
    {

		
		float radius = 50;
        Gizmos.color = Color.blue;
		Gizmos.DrawSphere(cabeca, radius); //ok
		
        Gizmos.color = Color.green;
		Gizmos.DrawSphere(cabeca_aux, radius);
		
        Gizmos.color = Color.gray;
		Gizmos.DrawSphere(pescoso, radius);

        Gizmos.color = Color.red;
		Gizmos.DrawSphere(ombroEsq, radius);
		Gizmos.DrawSphere(ombroDir, radius);
		Gizmos.DrawSphere(cotoveloEsq, radius);
		Gizmos.DrawSphere(cotoveloDir, radius);
		Gizmos.DrawSphere(maoEsq, radius);
		Gizmos.DrawSphere(maoDir, radius);
		Gizmos.DrawSphere(cinturaEsq, radius);
		Gizmos.DrawSphere(cinturaDir, radius);
		Gizmos.DrawSphere(joelhoEsq, radius);
		Gizmos.DrawSphere(joelhoDir, radius);
		Gizmos.DrawSphere(peEsq, radius);
		Gizmos.DrawSphere(peDir, radius);

    }


	#region OnAppExit
	public void OnApplicationQuit ()
	{
		// end of application
		if (receiveThread != null) { 
			receiveThread.Abort (); 
		}
		status = false;
		print ("Stop"); 
	}
	
	void OnDisable ()
	{ 
		if (receiveThread != null) 
			receiveThread.Abort (); 
		status = false;
		client.Close (); 
	}
	#endregion
	
	#region Normalizações
	// Funcoes de Normalizacao das dos dados Kinect -> Ambiente3D
	//scaledValue = (rawValue - min) / (max - min); --> formula original: (x - min1) / (max1 - min1) = (x2 - min2) / (max2 - min2)
	public float escalarY (float y)
	{
		float yentrada = y;
		int min1 = -800, // min1 e max1 e o valor desejado
			max1 = 800,
			min2 = -800, //min2 e max 2 e o valor original do kinect
			max2 = 800;
		
		int regular = 1050;
		min1 += regular;
		max1 += regular;
		
		y = (((y - min2) / (max2 - min2)) * (max1 - min1)) + min1;

		return y;
	}
	
	public float escalarZ (float z)
	{
		int min1 = -2200, // min1 e max1 e o valor desejado
			max1 = 0,
			min2 = -2200, //min2 e max 2 e o valor original do kinect
			max2 = 0;
		
		int regular = 1400;
		min1 += regular;
		max1 += regular;
		
		float z2 = z;

		z = (((z - min2) / (max2 - min2)) * (max1 - min1)) + min1;

		//Debug.Log("original: "+z2+"   |   Normalizado: " +z);
		
		return z;
	}
	
	
	// used to fix the position of the Kinect, if is 5 degrees up, the head position will be diferent than the foot position
	public float KinectDistance (double kinectAngle, Vector3 position)
	{
		float kinectDistance = 0f;
		
		//float hypotenuse = (Vector3.Distance(Vector3.zero, position)) / (float)Math.Sin(kinectAngle);
		float hypotenuse = kinectDistance / (float)Math.Cos (kinectAngle);
		
		return hypotenuse;
	}
	
	#endregion	
	
	public Texture2D kinectReceiveSucess, kinectReceiveFail;
	
	void OnGUI ()
	{
		Texture2D kinectStatus;
		if (status)
			kinectStatus = kinectReceiveSucess;
		else
			kinectStatus = kinectReceiveFail;
		GUI.Label (new Rect (Screen.width - 50, Screen.height - 50, 50, 50), kinectStatus);
		
		if (showSizes) {
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height - 150, 50, 50), "Height: " + naturalHeight.ToString ("0.00") + "m", new GUIStyle () {fontSize = 50});
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height - 100, 50, 50), "Wingspan: " + naturalWingspan.ToString ("0.00") + "m", new GUIStyle () {fontSize = 50});
			GUI.Label (new Rect (Screen.width * 0.40f, Screen.height - 50, 50, 50), "Feet: " + naturalFeetDistance.ToString ("0.0") + "cm", new GUIStyle () {fontSize = 50});
		}
	}
	
	#region Funcore antropometricas
	
	float NaturalFeetDistance (float leftX, float leftY, float leftZ, float rightX, float rightY, float rightZ)
	{
		return Vector3.Distance (
			new Vector3 (leftX, leftY, leftZ),
			new Vector3 (rightX, rightY, rightZ)
		) * 0.1f;
	}
	
	float NaturalDistanceFootFromCenter (float footX, float footY, float footZ)
	{
		return Vector3.Distance (
			new Vector3 (0, footY, footZ), 
			new Vector3 (footX, footY, footZ)
		);
	}
	#endregion
	
}