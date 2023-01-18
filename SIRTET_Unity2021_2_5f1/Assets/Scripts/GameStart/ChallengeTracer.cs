using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChallengeTracer : MonoBehaviour {
	
	// Para cada desafio lançado, verificar o acerto de cada objeto dentro deste...
	// Avaliar se este obete um retorno positivo ou nao para ser adicionado a fila de repetiçao.
	
	public List<Desafio> challengeInAction = new List<Desafio>();
	
	public int tamanhoDaLista;
	Desafio challengeCurrent;
	public int challengeSize = 0, totalHits = 0;
	float challengeTotalPoints;
	
	BaseController controller;
	Interface gui;
	Camera cam;
	
	void Start()
	{
		controller = GameObject.Find("Ambiente").GetComponent<BaseController>();
		gui = GameObject.FindGameObjectWithTag("Interface").GetComponent<Interface>();

		tamanhoDaLista = challengeInAction.Count;
		cam = GameObject.FindObjectOfType<Camera>();
	}
	
	void Update () 
	{
		if(challengeInAction.Count != 0 && challengeSize <= 0)
			GetNextChallengeInAction();

		if(Input.GetKeyDown(KeyCode.A))
		{
			gui.StarScore();
		}
	}
	
	public void GetNextChallengeInAction()
	{
		challengeCurrent = challengeInAction[0];
		challengeInAction.Remove(challengeCurrent);
		challengeSize = 0;
		foreach(Objeto obj in challengeCurrent.ListaObjeto)
			challengeSize += obj.Size.Quantity;
		
		challengeTotalPoints = challengeCurrent.Dificulty * gui.scoreWeight;
	}
	public void HitOccured(Objeto obj, bool goodHit, Vector3 position, string hitName)
	{		
		if(hitName != "Fim")
		{
			StopCoroutine(ShakeCam(0.5f,25));
			StartCoroutine(ShakeCam(0.5f,25));
		}

		if(obj.Type == "Surpresa")
		{
			if(hitName != "Fim")
			{
				gui.StarScore();
			}
			Debug.Log("passou ESTRELA");
			return;
		}


		challengeSize --;
		if(goodHit)
			totalHits ++;		
		else{
			if(controller.challengeSequence.IndexOf(challengeCurrent)  != controller.challengeSequence.Count-1)
				controller.challengeSequence.Add(challengeCurrent);
			if(obj.Type == "Alvo")
				obj.Type = "AlvoR";
			else
				obj.Type = "ObstaculoR";
		}				
		
		if(challengeSize == 0)
		{
			float score = 0;			
			float scoreMod = 0;

			if(hitName == "maoDir" ||hitName == "maoEsq" ||hitName == "peEsq" ||hitName == "peDir")
				scoreMod=1;
			else
				scoreMod=0.5f;

			
			if(totalHits > challengeCurrent.ListaObjeto.Count * (controller.challengePercent*0.01f)) // acertou 50% dos objetos no desafio?
			{	
				score = challengeCurrent.Dificulty * gui.scoreWeight * scoreMod;
				gui.score(score);
				print ("Challenge overcomed");
			}
			else
			{		
				score = -challengeCurrent.Dificulty * gui.scoreWeight * scoreMod;
				gui.score(score);
			}
			
			gui.feedbackScore(score*100, cam.WorldToScreenPoint(position));

			totalHits = 0;
		}


		/*Debug.Log("tamanhoDaLista" + tamanhoDaLista +
				"\nchallengeSize" + challengeSize +
				"\ntotalHits" + totalHits +
				"\nchallengeTotalPoints" + challengeTotalPoints);*/
	
	}

	public IEnumerator ShakeCam(float duration, float magnitude)
	{
		Vector3 originalPos = cam.transform.localPosition;
		float elapsed = 0.0f;
		while(elapsed<duration)
		{
			float x = Random.Range(-1f,1f) * magnitude;
			float y = Random.Range(-1f,1f) * magnitude;

			cam.transform.localPosition = new Vector3(cam.transform.localPosition.x+x, cam.transform.localPosition.y+y, originalPos.z);


			elapsed += Time.deltaTime;

			yield return null;
		}

		cam.transform.localPosition = originalPos;
	}
}
