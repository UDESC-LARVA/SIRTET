using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections.Generic;

public class MenuInterface : MonoBehaviour {
	
	

	XMLReader file;
	SoundBehavior sound;
	GameController game;
	public BaseController baseCont;
	
		
	
	GUIStyle styleText = new GUIStyle ();
	GUIStyle button, smallbutton, opt ,others, small, skinColor;
	
	string stringToEdit = "Nome do Jogador";

	public Texture2D logoUdesc, logoLarva;
	
	bool showStats = false, showTitle = true, showOpt = false, showCred = false;
	Player player;
	
	// dinamic sizes
	float width, height;
	
	float indexMusic;

	public bool modoAleatorio = false;
	
	string listaDeDesafios = ListaAO.nomeListaDesafios;

	Material skinColorMat;
	
	// Use this for initialization
	void Start ()
	{
		file = GameObject.Find ("XML").GetComponent<XMLReader>();
		sound = GameObject.Find ("Audio").GetComponent<SoundBehavior>();
		game = GameObject.Find ("Game Controller").GetComponent<GameController>();
		baseCont = FindObjectOfType<BaseController>();
		
		logoUdesc = Resources.Load<Texture2D>("Imagens/LogoUdesc");
		logoLarva = Resources.Load<Texture2D>("Imagens/LogoLarva");
		
		skinColorMat = Resources.Load<Material>("Objs/Characters/CharactersMaterial/CharacterSkin");
		
		styleText.fontSize = 50;
		styleText.alignment = TextAnchor.UpperCenter;
		styleText.normal.textColor = Color.green;

		if(file.player.Name != null)
			stringToEdit = file.player.Name;

		if(GameController.testMode) Invoke("Inicia", 0.2f);
	}

	void Inicia()
	{		
		SceneManager.LoadScene("Game_Start");	
	}
	
	void OnGUI()
	{		
		width = Screen.width * 0.45f;
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
		{
			SceneManager.LoadScene("Game_Start");
		}
		if(GUI.Button(new Rect(0, height, width, height), "Opções", button))
			showOpt = !showOpt;
		if(GUI.Button(new Rect(0, height * 2, width, height), "Encerrar", button))
			Application.Quit();
		if(GUI.Button(new Rect(width/3*2, height * 3, width/3, height/2), "Créditos", smallbutton))
			showCred = !showCred;

		if(GUI.Button(new Rect(0, height * 3, width/3, height/2), "Modo Aleatório", smallbutton))
		{
			stringToEdit = "ModoAleatorio";
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

			modoAleatorio = true;
			SceneManager.LoadScene("Game_Start");
		}
				
		GUI.EndGroup();


		// Imagens
		float logoX, logoY, logoHeight, logoRatio;
		logoX = Screen.width*0.1f;		
		logoY = Screen.height*0.1f;
		logoHeight = Screen.height*0.15f;

		logoRatio = logoHeight/logoUdesc.height;
		GUI.DrawTexture(new Rect(logoX, logoY*8, logoUdesc.width * logoRatio, logoUdesc.height * logoRatio), logoUdesc);

		logoRatio = logoHeight/logoLarva.height;
		GUI.DrawTexture(new Rect(logoX+25+(logoLarva.width * logoRatio * 2), logoY*8, logoLarva.width * logoRatio, logoLarva.height * logoRatio), logoLarva);
        
		
		// Carregar Jogador
        GUI.BeginGroup(new Rect(Screen.width*0.25f, Screen.height*0.65f, Screen.width*.5f, height*2));
		others = new GUIStyle(GUI.skin.textField);
		others.fontSize = button.fontSize/3;
		others.alignment = TextAnchor.MiddleCenter;
		stringToEdit = GUI.TextField(new Rect(0,0,Screen.width*.22f,height*.4f), stringToEdit , 25, others);
		others = button;
		others.fontSize = others.fontSize/2;
		if (GUI.Button(new Rect(0,height*.4f,Screen.width*.22f,height*.85f), "Carregar Jogador", others))
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
				new Rect(Screen.width*.23f,0,Screen.width*.22f,height*1.25f),
				"Jogador: " + player.Name + "\n" +
				"Fase: " +    player.CurrentPhase + "\t" +
				"Nivel: " +   player.CurrentLevel + "\n" +
				"Sessao: " +  player.Session, small);
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

		skinColor = new GUIStyle(GUIStyle.none);

		if(showOpt)
		{
			GUI.BeginGroup(new Rect(Screen.width*0.75f, 0, Screen.width*.25f, Screen.height));
			GUI.Box(new Rect(0, 0, Screen.width*.25f, Screen.height), "Opcões:", others);
			
			// Musica
			float w = Screen.width*.22f, h = height;
			GUI.BeginGroup(new Rect(10, height, w, h));
			GUI.Label(new Rect(10, 0, width, h/2), "Volume Musica:   " + (sound.GetComponent<AudioSource>().volume * 10).ToString("0.0"), opt); 
			sound.GetComponent<AudioSource>().volume = GUI.HorizontalSlider(new Rect (0, h/2, w, h/2), sound.GetComponent<AudioSource>().volume, 0f, 1f);
			GUI.EndGroup();
			
			// Sons
			GUI.BeginGroup(new Rect(10, height*2f, w, h));
			GUI.Label(new Rect(10, 0, width, height/2), "Sons:   " + (game.feedbackVolume * 10).ToString("0.0"), opt); 
			game.feedbackVolume = GUI.HorizontalSlider(new Rect (0, h/2, w, h/2), game.feedbackVolume, 0, 1);
			GUI.EndGroup();
			
			//Numero Musica
			GUI.BeginGroup(new Rect(10, height*3f, w, h));
			GUI.Label(new Rect(10,0, width, height/2), "Musica:   " + sound.musicIndex, opt); 
			GUI.EndGroup();

			//Cor da pele
			
			
			GUI.BeginGroup(new Rect(10, height*4f, w, h));
			GUI.Label(new Rect(10, 0, (w/2)-10, height/2.0f), "Cor da pele:   ", opt); 

			skinColor.normal.background = MakeTex(skinColorMat.color);
			GUI.Box(new Rect(w/2, 0, (w/2)-10, height/2.2f)," ",skinColor);
			
			skinColor.normal.background = MakeTex(new Color32(27, 25, 24, 255));
			if(GUI.Button(new Rect(5*0+(w/8)*0, h/2, w/8, h/2), " ", skinColor))
				skinColorMat.color = skinColor.normal.background.GetPixel(1,1);

			skinColor.normal.background = MakeTex(new Color32(46, 40, 42, 255));
			if(GUI.Button(new Rect(5*1+(w/8)*1, h/2, w/8, h/2), " ", skinColor))
				skinColorMat.color = skinColor.normal.background.GetPixel(1,1);

			skinColor.normal.background = MakeTex(new Color32(117, 91, 75, 255));
			if(GUI.Button(new Rect(5*2+(w/8)*2, h/2, w/8, h/2), " ", skinColor))
				skinColorMat.color = skinColor.normal.background.GetPixel(1,1);

			skinColor.normal.background = MakeTex(new Color32(183, 139, 97, 255));
			if(GUI.Button(new Rect(5*3+(w/8)*3, h/2, w/8, h/2), " ", skinColor))
				skinColorMat.color = skinColor.normal.background.GetPixel(1,1);

			skinColor.normal.background = MakeTex(new Color32(225, 196, 163, 255));
			if(GUI.Button(new Rect(5*4+(w/8)*4, h/2, w/8, h/2), " ", skinColor))
				skinColorMat.color = skinColor.normal.background.GetPixel(1,1);

			skinColor.normal.background = MakeTex(new Color32(255, 224, 192, 255));
			if(GUI.Button(new Rect(5*5+(w/8)*5, h/2, w/8, h/2), " ", skinColor))
				skinColorMat.color = skinColor.normal.background.GetPixel(1,1);

			skinColor.normal.background = MakeTex(new Color32(255, 239, 213, 255));
			if(GUI.Button(new Rect(5*6+(w/8)*6, h/2, w/8, h/2), " ", skinColor))
				skinColorMat.color = skinColor.normal.background.GetPixel(1,1);


			GUI.EndGroup();

			//Lista de desafios
			GUI.BeginGroup(new Rect(10, height*5f, w, h*ListaAO.nomesListas.Count));
			GUI.Label(new Rect(10,0, w, height/2), "Lista de Desafios:", opt); 					


			for(int i=0; i<ListaAO.nomesListas.Count; i++)
			{
				string nome = ListaAO.nomesListas[i];
				if(nome == ListaAO.nomeListaDesafios)
					nome = ">>"+nome+"<<";
				
				if(GUI.Button(new Rect(10, h/2*(i+1), w, h/2), nome, button))
					ListaAO.nomeListaDesafios = ListaAO.nomesListas[i];
			}

			//listaDeDesafios = GUI.TextField(new Rect(0, h/2, w, h/2), listaDeDesafios , 25, others);
			GUI.EndGroup();
						
			
			GUI.EndGroup();
		}	

		if(showCred)
		{			
			GUI.BeginGroup(new Rect(0, 0, Screen.width*.25f, Screen.height/3));

			string texto = "Game Design: \nGabriel Mesquita Rossito, \nMarcelo da Silva Hounsell, \nAntonio Vinicius Soares, \nDiego Fellipe Tondorf"+
			"\n\nGame Dev, v1.0: \nGabriel Mesquita Rossito"+
			"\n\nGame Dev, v2.0 (MediaPipe): \nDiego Fellipe Tondorf";
		    	    
			GUI.Box(new Rect(0, 0, Screen.width*.25f, Screen.height), texto, small);
			
			GUI.EndGroup();
		}		






		if(GameController.testMode)
		{
			player = new Player();
			stringToEdit = "JOGADOR_TESTE";
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
	}
	
	private Texture2D MakeTex(Color col )
	{
		Texture2D result = new Texture2D(1, 1, TextureFormat.RGBAFloat, false); 
    	result.SetPixel(0, 0, col);
    	result.Apply(); // not sure if this is necessary
		return result;
	}


	
}
