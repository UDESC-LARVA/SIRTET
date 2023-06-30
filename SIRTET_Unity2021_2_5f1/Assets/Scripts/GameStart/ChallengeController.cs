using UnityEngine;
using System.Collections;

public class ChallengeController : MonoBehaviour
{
	
	// variavel para atribuir objeto (prefab)
	GameObject prefabObject;
	[SerializeField]
	BaseController controladora;
	Interface gui;
	ChallengeTracer tracer;
	//int contadorDesafios = 0;

	GameObject alvoPrefab;
	GameObject obstaculoPrefab;
	GameObject surpresaPrefab;
	GameObject ghostPrefab;

	
	// Use this for initialization
	void Start ()
	{
		controladora = GameObject.Find("Ambiente").GetComponent<BaseController>();
		gui = GameObject.Find("Interface").GetComponent<Interface>();
		tracer = gameObject.GetComponent<ChallengeTracer>();
		
		prefabObject = Resources.Load ("Obj") as GameObject;

		alvoPrefab = Resources.Load ("Objs/Coin") as GameObject;		
		obstaculoPrefab = Resources.Load ("Objs/Fireball") as GameObject;		
		surpresaPrefab = Resources.Load ("Objs/Star") as GameObject;
		ghostPrefab = Resources.Load ("Objs/Ghost") as GameObject;
		
		//InvokeRepeating("CriarObjeto", padrao.intervaloObj, padrao.intervaloObj);  // comecando depois de 2 seg
		StartCoroutine (CreateSecretChallenge());
		StartCoroutine (CreateChallenge());
	}

	IEnumerator CreateChallenge ()
	{
		while (true) {			
			if (controladora.challengeSequence.Count > 0) {
				
				// Pega desafio e lança invokeRepeating para cada objeto dentro dele
				Desafio nextChallenge = controladora.challengeSequence [0];
				controladora.challengeSequence.Remove(nextChallenge);
				controladora.challengeSequence.Add(nextChallenge);//correção temporária para a v2
				tracer.challengeInAction.Add(new Desafio(){ Dificulty = nextChallenge.Dificulty, SequenceNumber = nextChallenge.SequenceNumber , ListaObjeto = nextChallenge.ListaObjeto });
				foreach (Objeto obj in nextChallenge.ListaObjeto) {
					StartCoroutine (CreateObject (nextChallenge, obj, 0.15f));
				}
			}
			yield return new WaitForSeconds(controladora.standardChallengeInterval);
		}
	}
	IEnumerator CreateSecretChallenge ()
	{
		int cont = 0;
		while (cont < 3) 
		{	
			int rand = Random.Range(30,90);
			yield return new WaitForSeconds(rand);
			
			Objeto obj = new Objeto();
			obj.Position = new Vector2(0,1000);
			obj.Type = "Surpresa";

			CustomObjectSize size = new CustomObjectSize();
			size.Height = 120;
			size.Weight = 120;
			size.Quantity = 1;
			obj.Size = size;

			StartCoroutine (CreateObject (null, obj, 0.15f));

			cont++;
		}
	}
	
	IEnumerator CreateObject (Desafio challenge, Objeto obj, float timer)
	{
		GameObject challengeGroup = new GameObject ();
		
		challengeGroup.transform.parent = transform;
		challengeGroup.name = obj.Position.ToString ();
		float times = obj.Size.Quantity;
		while (times > 0) {
			
			Vector3 pos = new Vector3 (obj.Position.x, obj.Position.y, controladora.depth * 10);
			GameObject newObject = Instantiate (prefabObject, pos, Quaternion.identity) as GameObject;
			newObject.transform.localScale = new Vector3 (obj.Size.Weight, obj.Size.Height, controladora.standardObjSize);
			newObject.GetComponent<ObjectBehavior>().objeto = obj;
			newObject.GetComponent<ObjectBehavior>().objeto.Points = (gui.scoreWeight / obj.Size.Quantity);
			newObject.transform.parent = challengeGroup.transform;
			newObject.name = times.ToString ();
					
			newObject.GetComponent<Renderer>().material.color = ChooseColor(obj.Type);
			
			if(obj.Type == "AlvoR")
				obj.Type = "Alvo";
			else if(obj.Type == "ObstaculoR")
				obj.Type = "Obstaculo";


			if(obj.Type == "Alvo")//moedas
			{
				GameObject testObj = Instantiate (alvoPrefab, pos, Quaternion.identity, newObject.transform) as GameObject;
				testObj.transform.name = "elementoGrafico";
			}
			else if(obj.Type == "Obstaculo")//Fogo
			{
				GameObject testObj = Instantiate (obstaculoPrefab, pos, Quaternion.identity, newObject.transform) as GameObject;
				testObj.transform.name = "elementoGrafico";
			}else if(obj.Type == "Surpresa")//Surpresa
			{
				GameObject testObj = Instantiate (surpresaPrefab, pos, Quaternion.identity, newObject.transform) as GameObject;
				testObj.transform.name = "elementoGrafico";
			}

			
			GameObject fantasma = Instantiate (ghostPrefab, pos, Quaternion.identity, newObject.transform) as GameObject;
			fantasma.transform.name = "fantasma";
			
			
			
			times --;
			yield return new WaitForSeconds(timer);
		}
	}
	private Color ChooseColor(string tipo)
	{
		float aPadrao = 0.05f;
		
		Color color = new Color(0,0,0,aPadrao);
		if(tipo == "AlvoR")
				color = new Color (0, 40, 0, aPadrao); //verdeescuro			
		else if(tipo == "Alvo")
				color = new Color (0, 0, 1, aPadrao); //azul
		else if(tipo == "ObstaculoR")
				color = new Color (255, 0, 127, aPadrao); //rosa
		else if(tipo == "Obstaculo")
				color = new Color (1, 0, 0, aPadrao); //Vermelho
		else if(tipo == "Surpresa")
				color = new Color (1, 1, 0, aPadrao); //Amarelo
		
		return color;
	}
	
	// funcao para randomizar a posicao no ambiente, variando de acordo com seus limites
	Vector3 RandomizarPosicao ()
	{
		
		// Randomizar uma posicao que fique na area dinamica
		float x, y;
		x = Random.Range (-((controladora.width * 5) - controladora.standardObjSize / 2), ((controladora.width) * 5) - controladora.standardObjSize / 2); // ajustando distancia do centro do objeto
		y = Random.Range (controladora.standardObjSize / 2, ((controladora.height) * 5) * 2 - controladora.standardObjSize / 2); // ajustando distancia do centro do objeto
		int aux;
		
		// qubrando o valor float para pegar apenas inteiro
		aux = (int)x;
		x = (float)aux;
		
		// qubrando o valor float para pegar apenas inteiro
		aux = (int)y;
		y = (float)aux;
		
		return  new Vector3 (x, y, (controladora.depth * 5) * 2);
	}
	
}
