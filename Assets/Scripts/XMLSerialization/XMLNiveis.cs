using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Nivel")]
public class LevelInFile
{
	[XmlAttribute("Indice")]
	public int Id {get;set;}
	[XmlElement("Velocidade")]
	public string Velocity {get;set;}
	[XmlElement("Tempo_Intervalo")]
	public string IntermissionTime {get;set;}
	[XmlElement("Tempo_Permanencia")]
	public string StayTime {get;set;}
}

[XmlRoot("Arquivo_de_Niveis")]
public class ListaNiveis
{

	[XmlArrayItem("Nivel", typeof(LevelInFile))]
	[XmlArray("Lista_Niveis")]
	public List<LevelInFile> listaNiveis;
	
	public ListaNiveis()
	{
		listaNiveis = new List<LevelInFile>();
	}
	
	public void Create()
	{
		ListaNiveis niveis = new ListaNiveis();
		
		/*A lista de niveis foi implementada de maneira que para de incrementar a velocidade, o nivels deve ser incrementado em 1, 
		* e para o intervalo de desafio, para aumenta-lo e necessario um incremento de 3 unidades*/
		
		// A - ItermissinTime = MIN
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 1,		
			Velocity = "MIN",
			IntermissionTime = "MIN"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 2,		
			Velocity = "MED",
			IntermissionTime = "MIN"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 3,		
			Velocity = "MAX",
			IntermissionTime = "MIN"
		});
		// A - ItermissinTime = MED
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 4,		
			Velocity = "MIN",
			IntermissionTime = "MED"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 5,		
			Velocity = "MED",
			IntermissionTime = "MED"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 6,		
			Velocity = "MAX",
			IntermissionTime = "MED"
		});
		// A - ItermissinTime = MAX
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 7,		
			Velocity = "MIN",
			IntermissionTime = "MAX"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 8,		
			Velocity = "MED",
			IntermissionTime = "MAX"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 9,		
			Velocity = "MAX",
			IntermissionTime = "MAX"
		});
		
		
		XmlSerializer serializer = new XmlSerializer(niveis.GetType());
		using (StreamWriter writer = new StreamWriter(@"XML/Niveis.xml"))
		{
		    serializer.Serialize(writer, niveis);
		}	
	}
	
	public ListaNiveis Load()
	{
		ListaNiveis niveis = new ListaNiveis();
		XmlSerializer serializer = new XmlSerializer(niveis.GetType());
		StreamReader reader = new StreamReader("XML/Niveis.xml");
		niveis = (ListaNiveis)serializer.Deserialize(reader);
		reader.Close();
		return niveis;
	}
	
}