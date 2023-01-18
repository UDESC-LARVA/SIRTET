using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interface : MonoBehaviour
{
	
	public float scoreGUI, scoreBarPos, scoreBarNeg, scoreWeight;
	public int starScore;
	public string starScoreStr = "☆☆☆";
	public string texto;
	float seconds, minutes, performance = 5;
	GUIStyle styleText = new GUIStyle ();
	GUIStyle styleAjuda = new GUIStyle ();
	public GUIStyle styleScore = new GUIStyle ();
	
	public GUIStyle styleScorePlus = new GUIStyle ();	
	public GUIStyle styleScoreMinus = new GUIStyle ();
	public float vScrollbarValue;
	BaseController controller;
	Texture2D auxBarra;// = new Texture2D(1,1);
	GUIStyle styleBarra;// = new GUIStyle();
	int corBarra = 0;
	
	public string timerString;
	public float endTime;

	//interface controllers/flags
	public bool showVariables = false;
	// Use this for initialization


	///feedback score
	public class ScoreFeedback
	{
		public float valor;
		public Rect retangulo;		
	}

	List<ScoreFeedback> scores = new List<ScoreFeedback>();

	private void Awake()
	{
		auxBarra = new Texture2D(1,1);
		styleBarra = new GUIStyle();
	}
	void Start ()
	{
		controller = GameObject.Find ("Ambiente").GetComponent<BaseController> ();
	
		
		styleText.fontSize = 20;
		styleText.alignment = TextAnchor.UpperLeft;
		styleText.normal.textColor = Color.white;
		
		styleScore.fontSize = 20;
		styleScore.alignment = TextAnchor.UpperLeft;
		styleScore.normal.textColor = Color.white;

		styleScorePlus.fontSize = styleScore.fontSize;		
		styleScorePlus.alignment = styleScore.alignment;		
		styleScorePlus.normal.textColor = Color.green;
		
		styleScoreMinus.fontSize = styleScore.fontSize;		
		styleScoreMinus.alignment = styleScore.alignment;		
		styleScoreMinus.normal.textColor = Color.red;
	
		
		styleAjuda.fontSize = 20;
		styleAjuda.alignment = TextAnchor.UpperLeft;
		styleAjuda.normal.textColor = Color.white;
		
		auxBarra.SetPixel(1,1,Color.gray);
		auxBarra.wrapMode = TextureWrapMode.Repeat;
		auxBarra.Apply();
		styleBarra.normal.background = auxBarra;
		
		performance = controller.challengeSequence.Count / controller.levelQuantity;
		
		
		CreateScore();
		
		timerString = controller.gameTime.ToString();
		
	}
	
	void Update ()
	{	
		if (Input.GetKeyDown("v"))
			showVariables = !showVariables;
		if(corBarra==0){
			barraCinza();
		}else{
			corBarra--;
		}
	}
	

	void CreateScore ()
	{
		scoreWeight = (float) 100/controller.challengeQuantity;
	}
	
	void OnGUI ()
	{
		endTime = controller.gameTime - Time.timeSinceLevelLoad;
		seconds = (int)endTime % 60;
		minutes = (int)endTime / 60;
		
		if(endTime <= 0)
			controller.isPaused = true;
		
		GUI.BeginGroup(new Rect(Screen.width - 250, 1, 500, 500));
		
		// Timer
		timerString = string.Format ("{0:00}:{1:00}", (minutes % 60).ToString ("00"), (seconds).ToString ("00"));
		GUI.Label (new Rect (1, 1, 100, 100), timerString, styleText);
		
		// Pontuacao
		styleScore.normal.textColor = Color.Lerp(styleScore.normal.textColor, Color.white, Time.deltaTime);
		GUI.Label (new Rect (1, 30, 100, 100), "" + (scoreGUI*100).ToString("0"), styleScore);
		GUI.Label (new Rect (1, 60, 100, 100), "" + starScoreStr, styleText);
		
		GUI.EndGroup();		
		// Niveis
		
		
		GUI.Label (new Rect (50, 10, 11, Screen.height - 25)," ",styleBarra);
		GUI.VerticalSlider (new Rect (50, 10, 500, Screen.height - 25), vScrollbarValue, performance, -performance);
		
		// Mensagem
		GUIStyle FeedBack = new GUIStyle ();
		FeedBack.fontSize = 50;
		FeedBack.alignment = TextAnchor.UpperLeft;
		FeedBack.normal.textColor = Color.white;
		Rect posicao = new Rect (Screen.width / 2, Screen.height - 50, 20, 20);
		GUI.Label (posicao, texto, FeedBack);
		
		
		if(showVariables)
		{
			float width = Screen.width * 0.25f, height = Screen.height * 0.25f;
			GUI.BeginGroup(new Rect(width, 0, width, height));
			
			GUI.Label(new Rect(0,0,width, height*.2f),"scrollbarvalue: "+ vScrollbarValue);
			GUI.Label(new Rect(0,height*.1f, width, height*.2f), "perform: " + performance);			
			
			
			GUI.EndGroup();
		}
		
		
		if(!controller.isHelp){
			if(controller.isPaused){
				if(endTime > 0)
					GUI.Label(new Rect(Screen.width*0.35f, Screen.height*0.25f, Screen.width*0.5f, Screen.height*0.5f), "Pause", styleText);
				else
					GUI.Label(new Rect(Screen.width*0.35f, Screen.height*0.25f, Screen.width*0.5f, Screen.height*0.5f), "Fim de Jogo", styleText);
			}
		}
		if(controller.isHelp){
				GUI.Label(new Rect(Screen.width*0.4f, Screen.height*0.1f, Screen.width*0.5f, Screen.height*0.5f), "Ajuda", styleText);
				GUI.Label(new Rect(Screen.width*0.3f, Screen.height*0.3f, Screen.width*0.5f, Screen.height*0.5f), "up - Aumentar Tempo", styleAjuda);
				GUI.Label(new Rect(Screen.width*0.3f, Screen.height*0.4f, Screen.width*0.5f, Screen.height*0.5f), "down - Diminuir Tempo", styleAjuda);
				GUI.Label(new Rect(Screen.width*0.3f, Screen.height*0.5f, Screen.width*0.5f, Screen.height*0.5f), "right - Aumentar Velocidade", styleAjuda);
				GUI.Label(new Rect(Screen.width*0.3f, Screen.height*0.6f, Screen.width*0.5f, Screen.height*0.5f), "left - Diminuir Velocidade", styleAjuda);
				GUI.Label(new Rect(Screen.width*0.3f, Screen.height*0.7f, Screen.width*0.5f, Screen.height*0.5f), "Pg up - Aumentar Itervalo de Desafios", styleAjuda);
				GUI.Label(new Rect(Screen.width*0.3f, Screen.height*0.8f, Screen.width*0.5f, Screen.height*0.5f), "Pg Down - Diminuir Intervalo de Desafios", styleAjuda);
				GUI.Label(new Rect(Screen.width*0.3f, Screen.height*0.9f, Screen.width*0.5f, Screen.height*0.5f), "Space - Pause", styleAjuda);
			
		}		




		//////////////score feedback
		//foreach(ScoreFeedback s in scores)
		for(int i=0; i<scores.Count; i++)
		{
			ScoreFeedback s = scores[i];

			string valor = s.valor.ToString("0");
			float yMod = 10 * Time.deltaTime;
		
			if(s.valor>0)
			{				
				s.retangulo.y -= yMod;
				valor = "+" + valor;				
				GUI.Label(s.retangulo, valor, styleScorePlus);	
			}else if(s.valor <0)
			{
				s.retangulo.y += yMod;				
				GUI.Label(s.retangulo, valor, styleScoreMinus);	
			}
			
		}			

		//////////////////
	}
	public void barraCinza(){
		auxBarra.SetPixel(1,1,Color.gray);
		auxBarra.wrapMode = TextureWrapMode.Repeat;
		auxBarra.Apply();
	}
	
	public void barraVerde(){
		auxBarra.SetPixel(1,1,Color.green);
		auxBarra.wrapMode = TextureWrapMode.Repeat;
		auxBarra.Apply();
		corBarra = 15;
	
	}
	
	public void barraVermelha(){
		auxBarra.SetPixel(1,1,Color.red);
		auxBarra.wrapMode = TextureWrapMode.Repeat;
		auxBarra.Apply();
		corBarra = 15;
	
	}
	
	public void StarScore ()
	{
		starScore+=1;
		starScoreStr = "";
		
		for(int i = 1; i<4;i++)
		{
			if(i <= starScore)			
			{
				starScoreStr = starScoreStr + "★";
			}	
			else
			{
				starScoreStr = starScoreStr + "☆";
			}
		}
	}
	public void score (float points)
	{
		scoreGUI += points;
		if (points > 0) {
			styleScore.normal.textColor = Color.green;
			if (vScrollbarValue < performance) {
				vScrollbarValue ++;
				if (vScrollbarValue >= performance) {
					controller.SubirNivel ();
				}
			}
		} else {
			styleScore.normal.textColor = Color.red;
			if (vScrollbarValue > -performance) {	
				vScrollbarValue --;
				if (vScrollbarValue <= -performance) {
					controller.DescerNivel ();
				}
			}
		}		
	}

	public void feedbackScore(float value, Vector3 position)
	{
		ScoreFeedback sco = new ScoreFeedback();
		sco.valor = value;
		float largura = 0;
		float altura = 0;
		//no caso do GUI a altura 0 não atrapalha para este feedback
		sco.retangulo = new Rect(position.x-(largura/2), Screen.height-(position.y-(altura/2)), largura, altura);

		//Debug.Log(position);
		
		scores.Add(sco);
		StartCoroutine(ApagarFeedback(3));
	}


	IEnumerator ApagarFeedback(int seconds)
 	{       
        yield return new WaitForSeconds(seconds);    
		scores.RemoveAt(0);
 	}
}
