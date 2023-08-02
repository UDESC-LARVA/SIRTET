using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Linq;

public struct CustomObjectSize 
{
	public float Height;
	public float Weight;
	public int Quantity;
}

[XmlRoot("Objeto")]
public class Objeto
{
	[XmlElement("Tipo")]
	public string Type { get; set; }
	[XmlElement("Posicao")]
	public Vector2 Position { get; set; }
	[XmlElement("Tamanho")]
	public CustomObjectSize Size { get; set; }
 	[XmlIgnore]
	public float Points { get; set; }
}

[XmlRoot("Desafio")]
public class Desafio
{
	[XmlArrayItem("Objeto", typeof(Objeto))]
	[XmlArray("Objetos")]
	public List<Objeto> ListaObjeto;
	
	[XmlElement("Dificuldade")]
	public int Dificulty { get; set; }
	[XmlElement("Numero")]
	public int SequenceNumber { get; set; }
	
	public Desafio()
	{
		ListaObjeto = new List<Objeto>();
	}
}

public class ListaAO
{
	
	[XmlArrayItem("Desafio", typeof(Desafio))]
	[XmlArray("Desafios")]
	private static List<Desafio> _listaDesafios;
	public List<Desafio> ListaDesafios;
	
	public ListaAO()
	{
		ListaDesafios = new List<Desafio>();
	}
	
	public void Create ()
	{
		ListaAO listaAO = new ListaAO ();
		
		// Auxiliares
		Vector2 posicao;
		CustomObjectSize tamanho;
		List<Objeto> listaObj;
		
		for (int i = 1; i <= 10; i++) 
		{
			listaObj = new List<Objeto>();
			Objeto obj = new Objeto();
			if (i % 2 == 0) 
			{
				posicao.x = i * 10;
				posicao.y = i * 10;
				obj.Position = posicao;
				
				tamanho.Height = 300;
				tamanho.Weight = 300;
				tamanho.Quantity = Random.Range(1,8);
				obj.Size = tamanho;
				
				obj.Type = "Alvo";
				
				listaObj.Add(obj);
				listaAO.ListaDesafios.Add( new Desafio { Dificulty = 1, SequenceNumber = i, ListaObjeto = listaObj });
			} 
			else 
			{
				posicao.x = i * 10;
				posicao.y = i * 10;
				obj.Position = posicao;
				
				tamanho.Height = 300;
				tamanho.Weight = 300;
				tamanho.Quantity = Random.Range(1,8);
				obj.Size = tamanho;
				
				obj.Type = "Obstaculo";
				
				listaObj.Add(obj);
				listaAO.ListaDesafios.Add( new Desafio { Dificulty = 1, SequenceNumber = i, ListaObjeto = listaObj });
			}
		}
		
		XmlSerializer serializer = new XmlSerializer(listaAO.GetType());
		using (StreamWriter writer = new StreamWriter(@"XML/ListaAO.xml"))
		{
		    serializer.Serialize(writer, listaAO);
		}
	}

	public ListaAO Load()
	{
		ListaAO listaAO = new ListaAO ();
		XmlSerializer serializer = new XmlSerializer(listaAO.GetType());
		StreamReader reader = new StreamReader("XML/ListaAO.xml");
		listaAO = (ListaAO)serializer.Deserialize(reader);
		reader.Close();


		/*
		Debug.Log(listaAO.ListaDesafios.Count);
		for(int i = 0; i<listaAO.ListaDesafios.Count;i++)
		{Debug.Log("I = " + i + "| Id =" + listaAO.ListaDesafios[i].SequenceNumber);}
		Debug.Log(string.Join(", ", listaAO.ListaDesafios.Select(c => c.SequenceNumber).ToList()));
		*/

		return listaAO;
	}
}