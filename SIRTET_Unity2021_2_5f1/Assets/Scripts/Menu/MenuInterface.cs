using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuInterface : MonoBehaviour {
	
	XMLReader file;
	SoundBehavior sound;
	GameController game;
	
	GUIStyle styleText = new GUIStyle ();
	GUIStyle button, smallbutton, opt ,others, small;
	
	string stringToEdit = "Nome do Jogador";
	
	bool showStats = false, showTitle = true, showOpt = false, showCred = false;
	Player player;
	
	// dinamic sizes
	float width, height;
	
	float indexMusic;
	
	
	// Use this for initialization
	void Start ()
	{
		file = GameObject.Find ("XML").GetComponent<XMLReader>();
		sound = GameObject.Find ("Audio").GetComponent<SoundBehavior>();
		game = GameObject.Find ("Game Controller").GetComponent<GameController>();
		
		styleText.fontSize = 50;
		styleText.alignment = TextAnchor.UpperCenter;
		styleText.normal.textColor = Color.green;

		if(file.player.Name != null)
			stringToEdit = file.player.Name;
	}
	
	void OnGUI()
	{		
		width = Screen.width * 0.5f;
		height = Screen.height * 0.1f;
		
		
		button = new GUIStyle(GUI.skin.button);
		button.fontSize = (int)height - 25;
		button.normal.textColor = Color.white;
		button.hover.textColor = Color.green;

		smallbutton = new GUIStyle(GUI.skin.button);
		smallbutton.fontSize = (int)(height - 25)/2;
		smallbutton.normal.textColor = Color.white;
		smallbutton.hover.textColor = Color.green;

		small = new GUIStyle(GUI.skin.box);
		small.fontSize = (int)height/4;
		small.alignment = TextAnchor.UpperCenter;
		small.normal.textColor = Color.white;
		
		styleText.fontSize = (int)height;
		
		GUI.BeginGroup(new Rect(Screen.width*0.25f, Screen.height*0.10f, width, Screen.height*0.5f));
		
		if(showTitle)
			GUI.Label(new Rect(0,0, width, height), "SIRTET", styleText);
		else
			GUI.Label(new Rect(0,0, width, height), "Fim de Jogo", styleText);
		
		
		GUI.EndGroup();
		
		
		GUI.BeginGroup(new Rect(Screen.width*0.25f, Screen.height*0.25f, width, Screen.height*0.5f));
		
		// Carregar nova interface para mostrar segundo menu e outros dados, como subiu/desceu nivel/fase		
		
		if(GUI.Button(new Rect(0, 0, width, height), "Iniciar", button))
			SceneManager.LoadScene("Game_Start");
		if(GUI.Button(new Rect(0, height, width, height), "Opções", button))
			showOpt = !showOpt;
		if(GUI.Button(new Rect(0, height * 2, width, height), "Encerrar", button))
			Application.Quit();
		if(GUI.Button(new Rect(width/3*2, height * 3, width/3, height/2), "Créditos", smallbutton))
			showCred = !showCred;
				
		GUI.EndGroup();

		
        
		
		// Carregar Jogador

        GUI.BeginGroup(new Rect(Screen.width*0.25f, Screen.height*0.6f, Screen.width*.5f, height*2));
		others = new GUIStyle(GUI.skin.textField);
		others.fontSize = button.fontSize/3;
		others.alignment = TextAnchor.MiddleCenter;
		stringToEdit = GUI.TextField(new Rect(0,0,Screen.width*.25f,height*.4f), stringToEdit , 25, others);
		others = button;
		others.fontSize = others.fontSize/2;
		if (GUI.Button(new Rect(0,height*.4f,Screen.width*.25f,height*.85f), "Carregar Jogador", others))
		{	
			player = new Player();
			stringToEdit = stringToEdit.ToUpper();
			player = file.GetPlayerByName(stringToEdit);
			if(player == null)
			{
				player = new Player {
					Name = stringToEdit,
					CurrentPhase = "A",
					CurrentLevel = 7,
					Session = 1
				};
				file.playerList.SaveOrUpdate(player);
				file.playerList = file.playerList.Load();
				file.GetPlayerByName(player.Name); //forçar pegar jogador novo
			}
			showStats = true;
		}
		if(showStats)
			GUI.Box(
				new Rect(Screen.width*.25f,0,Screen.width*.25f,height*1.25f),
				"Jogador: " + player.Name + "\n" +
				"Fase: " +    player.CurrentPhase + "\n" +
				"Nivel: " +   player.CurrentLevel + "\n" +
				"Sessao: " +  player.Session);
		GUI.EndGroup();
        
		
		// Options
		
		
		opt = new GUIStyle(GUI.skin.label);
		opt.fontSize = (int)height/3;
		opt.alignment = TextAnchor.MiddleLeft;
		opt.normal.textColor = Color.white;
		
		others = new GUIStyle(GUI.skin.box);
		others.fontSize = (int)height/3;
		others.alignment = TextAnchor.UpperCenter;
		others.normal.textColor = Color.white;
		
		if(showOpt)
		{
			GUI.BeginGroup(new Rect(Screen.width*0.75f, 0, Screen.width*.25f, Screen.height));
			GUI.Box(new Rect(0, 0, Screen.width*.25f, Screen.height), "Opcoes:", others);
			
			// Musica
			float w = Screen.width*.20f, h = height;
			GUI.BeginGroup(new Rect(0, height, w, h));
			GUI.Label(new Rect(0, 0, width, h/2), "Volume Musica:   " + (sound.GetComponent<AudioSource>().volume * 10).ToString("0.0"), opt); 
			sound.GetComponent<AudioSource>().volume = GUI.HorizontalSlider(new Rect (0, h/2, w, h/2), sound.GetComponent<AudioSource>().volume, 0f, 1f);
			GUI.EndGroup();
			
			// Sons
			GUI.BeginGroup(new Rect(0, height*2f, w, h));
			GUI.Label(new Rect(0,0, width, height/2), "Sons:   " + (game.feedbackVolume * 10).ToString("0.0"), opt); 
			game.feedbackVolume = GUI.HorizontalSlider(new Rect (0, h/2, w, h/2), game.feedbackVolume, 0, 1);
			GUI.EndGroup();
			
			//Numero Musica
			GUI.BeginGroup(new Rect(0, height*3f, w, h));
			GUI.Label(new Rect(0,0, width, height/2), "Musica:   " + sound.musicIndex, opt); 
			GUI.EndGroup();
						
			
			GUI.EndGroup();
		}	

		if(showCred)
		{			
			GUI.BeginGroup(new Rect(0, 0, Screen.width*.25f, Screen.height/3));

			string texto = "Game Design: \nGabriel Mesquita Rossito, \nMarcelo da Silva Hounsell, \nAntonio Vinicius Soares"+
			"\n\nGame Dev, v1.0: \nGabriel Mesquita Rossito"+
			"\n\nGame Dev, v2.0 (MediaPipe): \nDiego Filipe Tondorf";
		    	    
			GUI.Box(new Rect(0, 0, Screen.width*.25f, Screen.height), texto, small);
			
			GUI.EndGroup();
		}		
		
	}
}
