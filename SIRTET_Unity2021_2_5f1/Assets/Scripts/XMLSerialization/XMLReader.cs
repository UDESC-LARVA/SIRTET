using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level
{
	public int Id {get;set;}
	public int Velocity {get;set;}
	public int IntermissionTime {get;set;}
	public int StayTime {get;set;}
}

public class Phase
{
	public string Id {get;set;}
	public double CircuitBegin {get;set;}
	public double CircuitEnd {get;set;}
}


public class XMLReader : MonoBehaviour {
	
	public Player player = new Player();
	
	public PlayerList playerList = new PlayerList();
	PhaseList filePhases = new PhaseList();
	ListaNiveis fileLevels = new ListaNiveis();
	
	public ListaAO circuito = new ListaAO();
	public Parameters parameters = new Parameters();
	public List<Phase> gamePhases;
	public List<Level> gameLevels;
	
	public void Awake ()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void Start()
	{
//		CreateAllFiles();
		LoadAllFiles();
	}

	void CreateAllFiles ()
	{
//		circuito.Create(); // it's been loaded in enviroment controller
		parameters.Create();
//		filePhases.Create();
//		fileLevels.Create();
//		playerList.Create();
	}

	public void LoadAllFiles ()
	{
		circuito = circuito.Load();
		parameters = parameters.Load();
		filePhases = filePhases.Load();
		gamePhases = new List<Phase>(TraduzirFases(filePhases.phaseList));
		fileLevels = fileLevels.Load();
		gameLevels = new List<Level>(TraduzirNiveis(fileLevels.listaNiveis));
		playerList = playerList.Load();
	}
	
	#region Tradu��o Fases
	List<Phase> TraduzirFases(List<PhaseInFiles> listaArquivo)
	{
		List<Phase> fasesTraduzidas = new List<Phase>();
		foreach(PhaseInFiles n in listaArquivo)
		{
			fasesTraduzidas.Add(new Phase{ 
				Id = n.Id,
			});
		}
		return fasesTraduzidas;
	}

	#endregion
	
	#region Tradu��o Niveis
	List<Level> TraduzirNiveis(List<LevelInFile> listaArquivo)
	{
		List<Level> niveisTraduzidos = new List<Level>();
		foreach(LevelInFile n in listaArquivo)
		{
			niveisTraduzidos.Add(new Level{ 
				Id = n.Id,
				Velocity = traduzirVelocidade(n.Velocity),
				IntermissionTime = traduzirIntervalo(n.IntermissionTime)
			});
		}
		return niveisTraduzidos;
	}
	
	int traduzirVelocidade(string velo)
	{
 		switch(velo)
		{
			case "MIN":
				return parameters.ObjectsParameters.MinVelocity;
			case "MED":
				return (parameters.ObjectsParameters.MinVelocity + parameters.ObjectsParameters.MaxVelocity)/2;
			case "MAX":
				return parameters.ObjectsParameters.MaxVelocity;
		}
		return parameters.ObjectsParameters.MinVelocity;
	}

	int traduzirIntervalo(string inter)
	{
 		switch(inter)
		{
			case "MIN":
				return parameters.ChallengeParameters.MinIntermission;
			case "MED":
				return (parameters.ChallengeParameters.MinIntermission + parameters.ChallengeParameters.MaxIntermission)/2;
			case "MAX":
				return parameters.ChallengeParameters.MaxIntermission;
		}
		return parameters.ChallengeParameters.MinIntermission;
	}
	#endregion
	
	#region Metodos Gerais
	public Phase GetFaseByIndice(string indice)
	{
		foreach(Phase f in gamePhases)
			if(f.Id == indice)
				return f;
		return null;
	}
	
	public Level GetNivelByIndice(int indice)
	{
		foreach(Level n in gameLevels)
			if(n.Id == indice)
				return n;
		return null;
	}
	
	public Player GetPlayerByName(string name)
	{
		player = null;
		foreach(Player j in playerList.playerList)
			if(j.Name == name)
			{
				player = j;
				break;
			}
		return player;
	}
	#endregion
}