using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Ambiente")]
public class Environment
{
	[XmlElement("Profundidade")]
	public int Depth { get; set; }
	[XmlElement("Discretizar")]
	public int Discretize { get; set; } 
	[XmlElement("Tamanhos_Padrao_Modelos")]
	public int Size { get; set; }
	[XmlElement("XML_Base_L")]
	public int BaseL { get; set; }
	[XmlElement("XML_Base_D")]
	public int BaseD { get; set; }
}

[XmlRoot("Objetos")]
public class Objects
{
	[XmlElement("Velocidade")]
	public float Velocity { get; set; }
	[XmlElement("Velocidade_Maxima")]
	public int MaxVelocity {get;set;}
	[XmlElement("Velocidade_Minima")]
	public int MinVelocity {get;set;}
}

[XmlRoot("Desafios")]
public class Challenges
{
	[XmlElement("Porcentagem_Acerto")]
	public int HitPercentage { get;	set; }
	[XmlElement("Tempo_Intervalo_Maximo")]
	public int MaxIntermission {get;set;}
	[XmlElement("Tempo_Intervalo_Minimo")]
	public int MinIntermission {get;set;}
}

[XmlRoot("Jogabilidade")]
public class Gameplay
{
	[XmlElement("Quantidade_Objetos")]
	public int ObjectsQuantity { get; set; }
	[XmlElement("Quantidade_Niveis")]
	public int LevelsQuantity { get; set; }
	[XmlElement("Tempo_Total")]
	public int TotalTime { get; set; }
}

[XmlRoot("Arquivo_Parametros_Gerais")]
public class Parameters
{
	public Environment EnvironmentParameters { get; set; }
	public Objects ObjectsParameters { get; set; }
	public Gameplay GameplayParameters { get; set; }
	public Challenges ChallengeParameters { get; set; }
	
	string arquivo = "XML/ParametrosGerais.xml";
	
	public void Create()
	{
		Environment innerAmb = new Environment ();
		innerAmb.Depth = 1000;
		innerAmb.Discretize = 5;
		innerAmb.Size = 100;
		innerAmb.BaseL = 400;
		innerAmb.BaseD = 80;
		
		Objects innerObj = new Objects ();
		innerObj.Velocity = 4.0f;
		innerObj.MaxVelocity = 8;
		innerObj.MinVelocity = 4;
		
		Gameplay innerJoga = new Gameplay();
		innerJoga.ObjectsQuantity = 20;
		innerJoga.LevelsQuantity = 3;
		innerJoga.TotalTime = 5;
		
		Challenges innerDesa = new Challenges();
		innerDesa.HitPercentage = 50;
		innerDesa.MaxIntermission = 10;
		innerDesa.MinIntermission = 2;
		
		
		Parameters xmlToSerialize = new Parameters();
		xmlToSerialize.EnvironmentParameters = innerAmb;
		xmlToSerialize.ObjectsParameters = innerObj;
		xmlToSerialize.GameplayParameters = innerJoga;
		xmlToSerialize.ChallengeParameters = innerDesa;
		
		var serializer = new XmlSerializer(typeof(Parameters));
		var stream = new FileStream(arquivo, FileMode.Create);
		serializer.Serialize(stream, xmlToSerialize);
		stream.Close();
	}
	public Parameters Load()
	{
		Parameters parametros = new Parameters();
		XmlSerializer serializer = new XmlSerializer(parametros.GetType());
		StreamReader reader = new StreamReader(arquivo);
		parametros = (Parameters)serializer.Deserialize(reader);
		reader.Close();
		return parametros;
	}
}