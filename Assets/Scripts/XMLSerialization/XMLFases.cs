using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Fase")]
public class PhaseInFiles
{
	[XmlAttribute("Indice")]
	public string Id {get;set;}
	[XmlElement("Inicio_do_Circuito")]
	public double CircuitBegin {get;set;}  // In percentage (0.0 to 1.0) where to begin the circuit
	[XmlElement("Termino_do_Circuito")]
	public double CircuitEnd {get;set;}
}

[XmlRoot("Arquivo_de_Fases")]
public class PhaseList
{
	
	[XmlArrayItem("Fase", typeof(PhaseInFiles))]
	[XmlArray("Lista_Fases")]
	public List<PhaseInFiles> phaseList;
	
	public PhaseList()
	{
		phaseList = new List<PhaseInFiles>();
	}
	
	public void Create() 
	{	
		PhaseList phases = new PhaseList();
		
		phases.phaseList.Add(new PhaseInFiles{ 
			Id = "A",
		});
		phases.phaseList.Add(new PhaseInFiles{ 
			Id = "B",
		});
		phases.phaseList.Add(new PhaseInFiles{ 
			Id = "C",
		});
		phases.phaseList.Add(new PhaseInFiles{ 
			Id = "D",
		});
		phases.phaseList.Add(new PhaseInFiles{ 
			Id = "E",
		});
		phases.phaseList.Add(new PhaseInFiles{ 
			Id = "F",
		});
		
		XmlSerializer serializer = new XmlSerializer(phases.GetType());
		using (StreamWriter writer = new StreamWriter("XML/Fases.xml"))
		{
		    serializer.Serialize(writer, phases);
		}
	}
	
	public PhaseList Load(){
		PhaseList fases = new PhaseList();
		XmlSerializer serializer = new XmlSerializer(fases.GetType());
		StreamReader reader = new StreamReader("XML/Fases.xml");
		fases = (PhaseList)serializer.Deserialize(reader);
		reader.Close();
		return fases;
	}
		
}
