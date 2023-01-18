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
	[XmlElement("Fase")]
	public string Phase {get;set;}
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
			Phase = "A",
			Velocity = "MIN",
			IntermissionTime = "MIN"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 2,
			Phase = "A",
			Velocity = "MED",
			IntermissionTime = "MIN"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 3,
			Phase = "A",
			Velocity = "MAX",
			IntermissionTime = "MIN"
		});
		// A - ItermissinTime = MED
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 4,
			Phase = "A",
			Velocity = "MIN",
			IntermissionTime = "MED"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 5,
			Phase = "A",
			Velocity = "MED",
			IntermissionTime = "MED"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 6,
			Phase = "A",
			Velocity = "MAX",
			IntermissionTime = "MED"
		});
		// A - ItermissinTime = MAX
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 7,
			Phase = "A",
			Velocity = "MIN",
			IntermissionTime = "MAX"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 8,
			Phase = "A",
			Velocity = "MED",
			IntermissionTime = "MAX"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 9,
			Phase = "A",
			Velocity = "MAX",
			IntermissionTime = "MAX"
		});
		// B - ItermissinTime = MIN
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 1,
			Phase = "B",
			Velocity = "MIN",
			IntermissionTime = "MIN"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 2,
			Phase = "B",
			Velocity = "MED",
			IntermissionTime = "MIN"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 3,
			Phase = "B",
			Velocity = "MAX",
			IntermissionTime = "MIN"
		});
		// B - ItermissinTime = MED
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 4,
			Phase = "B",
			Velocity = "MIN",
			IntermissionTime = "MED"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 5,
			Phase = "B",
			Velocity = "MED",
			IntermissionTime = "MED"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 6,
			Phase = "B",
			Velocity = "MAX",
			IntermissionTime = "MED"
		});
		// B - ItermissinTime = MAX
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 7,
			Phase = "B",
			Velocity = "MIN",
			IntermissionTime = "MAX"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 8,
			Phase = "B",
			Velocity = "MED",
			IntermissionTime = "MAX"
		});
		niveis.listaNiveis.Add(new LevelInFile{ 
			Id = 9,
			Phase = "B",
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