using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Interface : MonoBehaviour
{

	//controlls
	public BaseController controller;
	GameController game;
	ChallengeController challengeController;
	

	public float scoreGUI, scoreBarPos, scoreBarNeg, scoreWeight;
	public int starScore;
	public string starScoreStr = "___";
	public string texto;
	float seconds, minutes, performance = 5;
	public bool gameEnded = false;
	
	GUIStyle styleText = new GUIStyle ();
	GUIStyle styleAjuda = new GUIStyle ();
	public GUIStyle styleScore = new GUIStyle ();
	public GUIStyle styleScorePlus = new GUIStyle ();	
	public GUIStyle styleScoreMinus = new GUIStyle ();
	public float vScrollbarValue, curBarValue = 0;
	
	
	Texture2D textGreen, textRed;
	Texture2D auxBarra;// = new Texture2D(1,1);
	GUIStyle styleBarra;// = new GUIStyle();
	GUIStyle box;
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
		textGreen = new Texture2D(1,1);
		textRed = new Texture2D(1,1);
		styleBarra = new GUIStyle();		
			
		
	}
	void Start ()
	{
		controller = GameObject.Find ("Ambiente").GetComponent<BaseController> ();
		
		game = GameObject.Find ("Game Controller").GetComponent<GameController>();
		
		challengeController = GameObject.Find ("Objetos").GetComponent<ChallengeController>();
		
	
		int fontSize = 30;

		styleText.fontSize = fontSize;
		styleText.alignment = TextAnchor.MiddleCenter;
		styleText.normal.textColor = Color.white;
		
		styleScore.fontSize = fontSize;
		styleScore.alignment = TextAnchor.MiddleCenter;
		styleScore.normal.textColor = Color.white;

		styleScorePlus.fontSize = styleScore.fontSize;		
		styleScorePlus.alignment = styleScore.alignment;		
		styleScorePlus.normal.textColor = Color.green;
		
		styleScoreMinus.fontSize = styleScore.fontSize;		
		styleScoreMinus.alignment = styleScore.alignment;		
		styleScoreMinus.normal.textColor = Color.red;	
		
		styleAjuda.fontSize = fontSize;
		styleAjuda.alignment = TextAnchor.UpperLeft;
		styleAjuda.normal.textColor = Color.white;
		
		auxBarra.SetPixel(1,1,Color.gray);
		auxBarra.wrapMode = TextureWrapMode.Repeat;
		auxBarra.Apply();
		styleBarra.normal.background = auxBarra;


		textGreen.SetPixel(1,1,Color.green);
		textGreen.wrapMode = TextureWrapMode.Repeat;
		textGreen.Apply();

		textRed.SetPixel(1,1,Color.red);
		textRed.wrapMode = TextureWrapMode.Repeat;
		textRed.Apply();
		
		performance = controller.challengeSequence.Count / controller.levelQuantity;		

		Debug.Log("A performance é delimitada pelo numero de desafios ("+ controller.challengeSequence.Count +") dividido pelo numero de níveis ("+controller.levelQuantity+") = "+performance);
		
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

		
		curBarValue = Mathf.Lerp(curBarValue, vScrollbarValue,  1*Time.deltaTime);
	}
	

	void CreateScore ()
	{
		//Debug.Log(controller.challengeQuantity + " / " + scoreWeight);
		scoreWeight = (float) 100/controller.challengeQuantity;

	}
	
	void OnGUI ()
	{
		endTime = controller.gameTime - Time.timeSinceLevelLoad;
		seconds = (int)endTime % 60;
		minutes = (int)endTime / 60;
		
		if(endTime <= 0)
			controller.isPaused = true;			

		//////

		box = new GUIStyle(GUI.skin.box);	

		
		
		if(!controller.isHelp)
		{
			if(controller.isPaused)
			{
				if(endTime > 0)
					GUI.Label(new Rect(Screen.width*0.35f, Screen.height*0.25f, Screen.width*0.5f, Screen.height*0.5f), "Pause", styleText);
				else
					gameEnded = true;
					GUI.Label(new Rect(Screen.width*0.35f, Screen.height*0.25f, Screen.width*0.5f, Screen.height*0.5f), "Fim de Jogo", styleText);
			}
		}
		
		if(controller.isHelp)
		{
			float distancia = 0.12f;
			float x,y,w,h;

			x = Screen.width * 0.28f;
			y = Screen.height * 0.30f;

			w = Screen.width * 0.5f;
			h = Screen.height * 0.5f;
			
			GUI.Box(new Rect(Screen.width * 0.25f, 0, w/1.5f, h*(distancia*25)), " ", box);	
			GUI.Label(new Rect(x,y*(distancia*1 ),w,h), "F1 \t - Ajuda", styleAjuda);

			GUI.Label(new Rect(x,y*(distancia*3 ),w,h), "↑  \t - Aumentar Fase", styleAjuda);
			GUI.Label(new Rect(x,y*(distancia*4 ),w,h), "↓  \t - Diminuir Fase", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*5 ),w,h), "5  \t - Aumentar Nível", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*6 ),w,h), "4  \t - Diminuir Nível", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*7 ),w,h), "9  \t - Aumentar Tempo", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*8 ),w,h), "8  \t - Diminuir Tempo", styleAjuda);	

			GUI.Label(new Rect(x,y*(distancia*10 ),w,h), "V   \t - Aumentar Velocidade", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*11),w,h), "F    \t - Diminuir Velocidade", styleAjuda);						
			GUI.Label(new Rect(x,y*(distancia*12),w,h), "Pg Up  \t - Aumentar Intervalo", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*13),w,h), "Pg Down\t - Diminuir Intervalo", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*14),w,h), "→  \t - Aumentar Base", styleAjuda);
			GUI.Label(new Rect(x,y*(distancia*15),w,h), "←  \t - Diminuir Base", styleAjuda);	
			GUI.Label(new Rect(x,y*(distancia*16),w,h), "B  \t - Desligar Base", styleAjuda);		
			GUI.Label(new Rect(x,y*(distancia*17),w,h), "Space  \t - Pause", styleAjuda);

			GUI.Label(new Rect(x,y*(distancia*19),w,h), "T  \t - Tipo de Objeto", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*20),w,h), "S  \t - Sexo do Personagem", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*21),w,h), "H  \t - Humanoide", styleAjuda);
			GUI.Label(new Rect(x,y*(distancia*22),w,h), "Q  \t - Sombra da Mão", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*23),w,h), "W  \t - Projeção do Objeto", styleAjuda);			
			GUI.Label(new Rect(x,y*(distancia*24),w,h), "R  \t - Sombra do Desafio", styleAjuda);
		}		

		GUI.BeginGroup(new Rect(Screen.width - 250, 1, 150, 150));
		
		GUI.Box(new Rect(0, 0, 150, 150), " ", box);
		
		// Timer
		timerString = string.Format ("{0:00}:{1:00}", (minutes % 60).ToString ("00"), (seconds).ToString ("00"));
		GUI.Label (new Rect (1, 1, 150, 100), timerString, styleText);
		
		// Pontuacao
		styleScore.normal.textColor = Color.Lerp(styleScore.normal.textColor, Color.white, Time.deltaTime);
		GUI.Label (new Rect (1, 30, 150, 100), "" + (scoreGUI*100).ToString("0"), styleScore);
		GUI.Label (new Rect (1, 60, 150, 100), "" + starScoreStr, styleText);
		
		GUI.EndGroup();		

		///Feedback de níveis

		GUI.BeginGroup(new Rect(Screen.width - 450, Screen.height - 450, 400, 400));
		
		GUI.Box(new Rect(0, 0, 400, 400), " ", box);
		
		int dist = 30;
		GUI.Label (new Rect (1, dist * 0, 400, 100), "Jogador: " + game.file.player.Name, styleAjuda);			
		GUI.Label (new Rect (1, dist * 2, 350, 100), "Nível: " + game.file.player.CurrentLevel, styleAjuda);		
		GUI.Label (new Rect (1, dist * 3, 350, 100), "Velocidade: " + controller.standardObjVelocity, styleAjuda);
		GUI.Label (new Rect (1, dist * 4, 350, 100), "Intervalo: " + controller.standardChallengeInterval, styleAjuda);
		GUI.Label (new Rect (1, dist * 5, 350, 100), "Fase: " + game.file.player.CurrentPhase, styleAjuda);		
		GUI.Label (new Rect (1, dist * 7, 350, 100), "Lista Desafios: ", styleAjuda);
		GUI.Label (new Rect (1, dist * 8, 350, 100), ListaAO.nomeListaDesafios, styleAjuda);
		GUI.Label (new Rect (1, dist * 9, 350, 100), "Desafio Inicial: " + challengeController.desaIni, styleAjuda);		
		GUI.Label (new Rect (1, dist * 10, 350, 100), "Desafio Atual: " + challengeController.desaAtual, styleAjuda);
		GUI.Label (new Rect (1, dist * 11, 350, 100), "Desafio Final: " + challengeController.desaFin, styleAjuda);		
		
		GUI.EndGroup();	

		// Niveis			
		
		//GUI.Label (new Rect (50, 10, 11, Screen.height - 25)," ",styleBarra);
		int sliderWidth = 50;
		int sliderHeight = Screen.height - 25;
		GUI.BeginGroup(new Rect(50, 10, sliderWidth, sliderHeight));

		box.normal.background = textGreen;		
		GUI.Box(new Rect(0, 0, sliderWidth, sliderHeight), " ", box);	


		box.normal.background = textRed;
		GUI.Box(new Rect(0, 0, sliderWidth, sliderHeight * (1-((curBarValue+performance)/(performance*2)))), " ", box);
		GUI.EndGroup();	

		
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
				starScoreStr = starScoreStr + "X";
			}	
			else
			{
				starScoreStr = starScoreStr + "_";
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
					controller.AlterarNivel (1);
					curBarValue = -performance;
				}
			}
		} else {
			styleScore.normal.textColor = Color.red;
			if (vScrollbarValue > -performance) {	
				vScrollbarValue --;
				if (vScrollbarValue <= -performance) {
					controller.AlterarNivel (-1);
					curBarValue = -performance;
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
