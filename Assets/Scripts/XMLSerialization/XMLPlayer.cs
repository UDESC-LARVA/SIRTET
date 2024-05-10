using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Jogador")]
public class Player
{
	[XmlAttribute("Nome")]
	public string Name { get; set; }
	[XmlElement("Fase_Atual")]
	public string CurrentPhase { get; set; }
	[XmlElement("Nivel_Atual")]
	public int CurrentLevel { get; set; }
	[XmlElement("Sessao")]
	public int Session { get; set; }
	[XmlAttribute("Envergadura")]
	public float Wingspan { get; set; }
	[XmlAttribute("Altura")]
	public float Height { get; set; }
}

[XmlRoot("Arquivo_de_Jogadores")]
public class PlayerList
{
	[XmlArrayItem("Jogador", typeof(Player))]
	[XmlArray("Lista_Jogadores")]
	public List<Player> playerList;
	
	public PlayerList()
	{
		playerList = new List<Player>();
	}

	string arquivo = "XML/Jogador.xml";
	
	
	public PlayerList Load()
	{
		PlayerList jogador = new PlayerList();
		XmlSerializer serializer = new XmlSerializer(jogador.GetType());
		StreamReader reader = new StreamReader(arquivo);
		jogador = (PlayerList)serializer.Deserialize(reader);
		reader.Close();
		return jogador;
	}
	
	public bool SaveOrUpdate (Player player)
	{
		bool updated = false;
		
		try
		{
			// Load
			PlayerList xmlPlayerList = new PlayerList();
			XmlSerializer serializer = new XmlSerializer(xmlPlayerList.GetType());
			StreamReader reader = new StreamReader(arquivo);
			xmlPlayerList = (PlayerList)serializer.Deserialize(reader);
			reader.Close();
			
			// Change data values, from game to real
//			player.Wingspan = 
			
			
			// Update
			foreach(Player j in xmlPlayerList.playerList)
			{
				if(j.Name == player.Name)
				{
					j.Height = player.Height;
					j.Wingspan = player.Wingspan;
					j.CurrentPhase = player.CurrentPhase;
					j.CurrentLevel = player.CurrentLevel;
					j.Session = player.Session;
					updated = true;
					break;
				}
			}
			
			// Add
			if(!updated)
				xmlPlayerList.playerList.Add(player);

			//SaveOrUpdate
			using (StreamWriter writer = new StreamWriter(arquivo))
			{
			    serializer.Serialize(writer, xmlPlayerList);
			}
			
			return true;
		}
		catch(XmlException)
		{
			return false;
		}
	}
}