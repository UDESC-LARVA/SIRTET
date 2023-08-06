using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class BaseController : MonoBehaviour
{
	
	#region Variaveis
	
//	XMLReader game.file;
	Interface gui;
//	UDP game.kinect;
//	Report report;
//	game.soundBehavior game.sound;
	public GameController game;
	AudioSource feedbackAudioSource;

	// Variavel para identificar quanto o ambiente sera discreto
	public float
		discretize,
		width, 
		height, 
		depth;
	
	// Variaveis para funcoes de GamePlay
	public int 
		challengeQuantity, 
		levelQuantity,
		challengePercent;
	public string
		phaseCurrentId = "A";
	public float gameTime;
	public float incrementoTempo;//guarda o valor do incremento/decremento de tempo (Sempre 20 por cento do tempo inicial)
	public bool isPaused = false;
	public bool isHelp = false;
	public float baseL = 400, baseD = 80;//dimensoes da base | l->x | d->z
	public float incrementoBaseL, incrementoBaseD;
	public Transform auxBase;	
	MeshCollider col;
	public UDP datagrama;
	public Vector2 baseX, baseZ;
	bool blink = false;
	
	// Niveis e Fases
	public Phase phaseCurrent;
	List<Level> levelSequence;
	public Level levelCurrent;
	
	// Variavel para definir padroes para demais objetos (corpo e objetos)
	public float standardSize, standardObjSize, standardObjVelocity, standardChallengeInterval = 2;
	public List<Desafio> challengeSequence;

	
	#endregion
	
	#region Start - Update
	
	void Awake ()
	{			
		//Evitar carregar mutas vezes
		//game.file = GameObject.Find ("XML").GetComponent<XMLReader> ();
		
		gui = GameObject.Find ("Interface").GetComponent<Interface> ();
		game = GameObject.Find ("Game Controller").GetComponent<GameController>();

		feedbackAudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
	
		datagrama = GameObject.Find("UDP").GetComponent<UDP>();	


		foreach(HandShadowController hsc in GameObject.FindObjectsOfType<HandShadowController>())
		{
			hsc.BuscaLaterais();
		}

		
		//Centralizar variaveis estaticas Globais
		
		//Ambiente
		gameTime = game.file.parameters.GameplayParameters.TotalTime * 60;
		incrementoTempo = gameTime/5;//incremento de 20 % do tempo inicial
		//incrementoTempo = 20;//incremento de 20 % do tempo inicial
		discretize = game.file.parameters.EnvironmentParameters.Discretize;
		depth = game.file.parameters.EnvironmentParameters.Depth;
		width = LarguraGoniometricaAmbiente ();
		height = AlturaGoniometricaAmbiente ();
		
		//base
		baseD = ((float)game.file.parameters.EnvironmentParameters.BaseD)/1000;
		baseL = ((float)game.file.parameters.EnvironmentParameters.BaseL)/1000;
		incrementoBaseD = baseD/20;
		incrementoBaseL = baseL/20;
		auxBase = GameObject.Find("Base").GetComponent<Transform>().transform;
		
		//Objeto
		standardSize = (float)game.file.parameters.EnvironmentParameters.Size;
		standardObjSize = standardSize + (standardSize * 0.5f);
		
		//Calcular percurso de objetos e variaveis relacionadas
		challengeSequence = new List<Desafio> (game.file.circuito.Load().ListaDesafios);
		foreach (Desafio challenge in challengeSequence) {
			challenge.ListaObjeto.ForEach (obj => obj = escalonarPosicaoObj (obj));
		}
		
		if (game.file.player == null || game.file.player.Name == null) { // Chuncho para quando nao carregar jogador
			game.file.player = new Player {
			
					Name = "DEFAULT",
					CurrentPhase = "A",
					CurrentLevel = 7,
					Session = 1
			};
		}
		
		// Leitura e atualizacao do jogador
		game.file.player.Session ++;
		game.file.player.Height = float.Parse(game.kinect.naturalHeight.ToString("0.00"));
		game.file.player.Wingspan = float.Parse(game.kinect.naturalWingspan.ToString("0.00"));
		
		//Niveis e Fases
		
		//Separando Fase corrente
		phaseCurrent = game.file.GetFaseByIndice (phaseCurrentId);
		//Pegando apenas os niveis da fase corrente
		levelSequence = new List<Level> (game.file.gameLevels);
		levelQuantity = levelSequence.Count;
		levelCurrent = game.file.GetNivelByIndice (game.file.player.CurrentLevel);
		if (levelCurrent == null)
		{
			print ("Nivel ou Fase nao cadastrado em Arquivo.");
			SceneManager.LoadScene("Menu_Partial");
		}
				
		//Setando Variaveis dinamicas
		
		//Fases
		challengeQuantity = 0;
		challengeSequence.ForEach(d => challengeQuantity += d.Dificulty) ; //phaseCurrent.quantidadeAlvos + phaseCurrent.quantidadeObstaculos;
		challengePercent = (game.file.parameters.ChallengeParameters.HitPercentage != 0) ? game.file.parameters.ChallengeParameters.HitPercentage : 50;
		
		//Setando tamanho do ambiente
		
		transform.localScale = new Vector3 (width, height, depth);
		transform.position = new Vector3 (transform.position.x, transform.position.y, depth * 4);
		
		//Niveis
		standardObjVelocity = levelCurrent.Velocity;
		standardChallengeInterval = levelCurrent.IntermissionTime;

		col = auxBase.GetComponent<MeshCollider>();

		

		//
		/*SOMENTE TESTES
		standardObjVelocity = 15;
		standardChallengeInterval = 1f;
		Debug.LogWarning("APAGAR LINHAS ACIMA");
		
		//*/
		/////////
	}

	void Update ()
	{
	
		if(Input.GetKeyDown(KeyCode.Keypad7))
			AlterarFase(1);
		if(Input.GetKeyDown(KeyCode.Keypad4))
			AlterarFase(-1);
		if(Input.GetKeyDown(KeyCode.Keypad8))
			AlterarNivel (1);
		if(Input.GetKeyDown(KeyCode.Keypad5))
			AlterarNivel (-1);

		//TEMPO
		if(Input.GetKeyDown(KeyCode.Keypad9))
			AlterarTempo(1);
		if(Input.GetKeyDown(KeyCode.Keypad6))
			AlterarTempo(-1);
				
		//O trecho a seguir chama as funçoes de alteraçao de niveis
		if(Input.GetKeyDown(KeyCode.Home))
			AlterarVelocidade(1);
		if(Input.GetKeyDown(KeyCode.End))
			AlterarVelocidade(-1);
		if(Input.GetKeyDown(KeyCode.PageUp))
			AlterarIntervalo(1);
		if(Input.GetKeyDown(KeyCode.PageDown))
			AlterarIntervalo(-1);
		
		

		
		//AJUDA		
		if(Input.GetKeyDown(KeyCode.F1) && isPaused==false)
		{
			isHelp = !isHelp;
			isPaused = !isPaused;
		}else if(Input.GetKeyDown(KeyCode.F1) && isHelp==true)
		{
			isHelp = !isHelp;
			isPaused = !isPaused;
		}

		// Pause
		if (Input.GetKeyDown("space")&&isHelp==false)
			isPaused = !isPaused;
		
		
		if(Input.GetKeyDown(KeyCode.D)){
			if(baseD - (float)5/100 > 0 && baseL - (float)5/100 > 0 ){
				baseD = baseD - incrementoBaseD;
				baseL = baseL - incrementoBaseL;
			}
			auxBase.transform.localScale = new Vector3 (baseL, 0, baseD);
		}
		if(Input.GetKeyDown(KeyCode.L)){
			baseD = baseD + incrementoBaseD;
			baseL = baseL + incrementoBaseL;
			auxBase.transform.localScale = new Vector3 (baseL, 0, baseD);
		}
		if (Input.GetKeyDown (KeyCode.Backspace) && game.kinect.status)
			isPaused = !isPaused;



		if(!blink && StepOut())
		{
			StartCoroutine(StepBase());
			blink = true;
		}


		if (isPaused)
		{
			Time.timeScale = 0;
			game.sound.pause = true;
		}else
		{
			Time.timeScale = 1;
			game.sound.pause = false;
		}

		if (!game.kinect.status)
			isPaused = true; 
		
		if (Input.GetKeyDown (KeyCode.Escape))
			EndSession ();
		
		
		GetComponent<AudioSource>().volume = game.feedbackVolume;
		
	}
	
	#endregion
	
	#region Audio FeedBack
	// Funcoes de audio
	public void PlaySoundPos ()
	{
		feedbackAudioSource.clip = (AudioClip)Resources.Load ("Sons/Pos2");
		feedbackAudioSource.Play();
	}
	
	public void PlaySoundNeg ()
	{
		feedbackAudioSource.clip = (AudioClip)Resources.Load ("Sons/Neg2");
		feedbackAudioSource.Play();
	}
	#endregion	
	
	#region GamePlay
	// Funcoes de Game Play
	// Variaveis de GameDesign
	// Controle de nivel: Velocidade, Intervalo, Permanencia
	// subirNivel = arrendondadoParaCima (quantidadePecas/quantidadeNiveis)
	// descerNivel = arrendondadoParaCima (subirNivel/2);
	public IEnumerator StepBase(){
		
		auxBase.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(0.8f);
		auxBase.GetComponent<Renderer>().enabled = true;
		blink = false;
	}
	
	private bool StepOut(){
		Vector3 positionBase = auxBase.transform.position;
		
		float peDirX, peDirZ, peEsqX, peEsqZ;
		
		peDirX = datagrama.peDir.x;
		peDirZ = datagrama.peDir.z;
		peEsqX = datagrama.peDir.x;
		peEsqZ = datagrama.peDir.z;

		if(!col.bounds.Contains(new Vector3(peDirX, auxBase.transform.position.y, peDirZ)))
			return true;

		if(!col.bounds.Contains(new Vector3(peEsqX, auxBase.transform.position.y, peEsqZ)))
			return true;
		
		return false;
	}

	public void AlterarFase (int value)
	{
		if(value < 0 && game.file.player.CurrentPhase=="A")
			return;

		char fase = game.file.player.CurrentPhase.ToCharArray()[0];
		fase = (char)(fase + value);
		game.file.player.CurrentPhase = fase.ToString();
	}

	public void AlterarNivel (int value)
	{
		if(value > 0)
		{
			if(game.file.player.CurrentLevel == levelQuantity)
			{ 
				game.file.player.CurrentLevel = 1;
				AlterarFase(1);				
			}
			gui.barraVerde();
			print ("Subiu Nivel " + game.file.player.CurrentLevel);
		}else if(value < 0)
		{
			if(game.file.player.CurrentLevel == 1)
			{ 
				game.file.player.CurrentLevel = (int)(levelQuantity/2)+1;
				AlterarFase(-1);				
			}
			gui.barraVermelha();
			print ("Desceu Nivel " + game.file.player.CurrentLevel);
		}		

		game.file.player.CurrentLevel += value;

		gui.vScrollbarValue = 0;

		levelCurrent = game.file.GetNivelByIndice (game.file.player.CurrentLevel);
		standardObjVelocity = levelCurrent.Velocity;
		standardChallengeInterval = levelCurrent.IntermissionTime;
	}
	
	
	public void AlterarVelocidade(int value)
	{
		if(standardObjVelocity+value < 1)
			return;

		standardObjVelocity += value;
	}

	public void AlterarIntervalo(int value)
	{
		if(standardChallengeInterval+value < 1)
			return;

		standardChallengeInterval += value;
	}

	public void AlterarTempo(int value)
	{
		gameTime += (incrementoTempo * value);
	}




	#endregion
	
	#region Medidas do Ambiente
	public float LarguraGoniometricaAmbiente ()
	{
		UDP membros = GameObject.Find ("UDP").GetComponent<UDP> ();
		float total = 0f;
		
		total += Vector3.Distance (membros.maoDir, membros.cotoveloDir) / 10;
		total += Vector3.Distance (membros.cotoveloDir, membros.ombroDir) / 10;
		total += Vector3.Distance (membros.ombroDir, membros.ombroEsq) / 10;
		total += Vector3.Distance (membros.ombroEsq, membros.cotoveloEsq) / 10;
		total += Vector3.Distance (membros.cotoveloEsq, membros.maoEsq) / 10;
		
		return total + total * 0.5f;
	}
	
	public float AlturaGoniometricaAmbiente ()
	{
		UDP membros = GameObject.Find ("UDP").GetComponent<UDP> ();
		float total = 0f;
		
		//total += Vector3.Distance (membros.cabeca, membros.pescoso) / 10;
		total += Vector3.Distance (membros.cotoveloEsq, membros.maoEsq) / 10;
		total += Vector3.Distance (membros.ombroEsq, membros.cotoveloEsq) / 10;
		
		total += Vector3.Distance (membros.ombroEsq, membros.cinturaEsq) / 10;
		total += Vector3.Distance (membros.cinturaEsq, membros.joelhoEsq) / 10;
		total += Vector3.Distance (membros.joelhoEsq, membros.peEsq) / 10;
		
//		total += Vector3.Distance(membros.cabeca, membros.peDir)/10;
		
		return total + total * 0.2f;
	}
	#endregion
	
	#region Escalando Posição dos objetos
	// Escalar as posições que vem do arquivo so será possível apos ideintificar o tamanho do ambiente, escala de 0 a 10
	public Objeto escalonarPosicaoObj (Objeto obj)
	{
		float l = width - standardObjSize / 20;
		l = l / 20; // metade para cada lado....
		float a = height - standardObjSize / 20;
		a = a / 10;
		
		Vector2 updatePos = obj.Position;
		updatePos.x = (int)(l * obj.Position.x);
		updatePos.y = (int)(a * obj.Position.y);
		obj.Position = updatePos;
		
		return obj;
	}
	#endregion
	
	public void EndSession ()
	{
		/*if(report.game.fileWrite(game.file.player, gui.scoreGUI, gui.timer))
			print("Report reported");*/
		// Atualizar dados na lista de Jogador
		// Gravar Relatorio
		// Voltar para Menu com nome do Jogador
		
		Player playerData = game.file.player;
		string time = gui.timerString;
		float score = gui.scoreGUI;
		
		// Verify Current phase and sees if the playe can go to next phase
		
		game.file.playerList.SaveOrUpdate(playerData);
		
		string filePath = "Relatorios/" + playerData.Name.ToString () + "_Relatorio.csv";
		
		string text =
			playerData.Session + "; " +
				System.DateTime.Now + "; " + 
				time.ToString () + "; " + 
				score.ToString () + "; " + 
				playerData.CurrentPhase.ToString () + "; " + 
				playerData.CurrentLevel.ToString () + "\n";
		
		if(!File.Exists(filePath))
			File.AppendAllText(filePath, "Sessao; Data; Tempo Final; Pontos; Fase Final; Nivel Final \n");	
		
		File.AppendAllText (filePath, text);
		
		SceneManager.LoadScene("Menu_Total");
		
	}
}
