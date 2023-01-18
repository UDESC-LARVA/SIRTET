using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ChangeColor : MonoBehaviour {
	
	
	
	// Para colisoes
	IEnumerator OnCollisionEnter (Collision Target){
		
		//if(Target.transform.tag != this.tag && Target.transform.tag != "Untagged")
		if(Target.transform.tag == "Obj")
		{
	 		GameObject[] bodyPartsToChange = GameObject.FindGameObjectsWithTag(this.tag);
	//		bodyPartsToChange.ForEach(p => p.transform.renderer.material.color = new Color(0,0,0,0.60f));
			
			Color color = new Color(0,0,1,0.5f);
			if(Target.gameObject.GetComponent<ObjectBehavior>().objeto.Type == "Obstaculo" || Target.gameObject.GetComponent<ObjectBehavior>().objeto.Type == "ObstaculoR")
				color = new Color(1,0,0,0.5f);
			
			
			foreach (GameObject item in bodyPartsToChange) {
				item.transform.GetComponent<Renderer>().material.color = color;
			}
	//		renderer.material.color = new Color(0,0,0,0.60f);
			
			
			ReportCollision(Target.gameObject.GetComponent<ObjectBehavior>().objeto.Type,Target.gameObject.GetComponent<ObjectBehavior>().objeto.Points);
			yield return new WaitForSeconds(3);
			foreach (GameObject item in bodyPartsToChange) {
				item.transform.GetComponent<Renderer>().material.color = new Color(0,1,0,0.5f);
			}
			
			
	//		bodyPartsToChange.ForEach(p => p.transform.renderer.material.color = new Color(0,0,1,0.60f));
//		renderer.material.color = new Color(0,0,1,0.60f);
		}
	}
	private void ReportCollision(string tipo, float pontos){
		GameController game;
		
				
		
		if(tipo=="AlvoR")
			tipo = "Alvo";
		else if(tipo == "ObstaculoR")
			tipo = "Obstaculo";
		
		
		
		game = GameObject.Find ("Game Controller").GetComponent<GameController>();
		Player playerData = game.file.player;
		
			
		string filePath = "Relatorios/" + playerData.Name.ToString () + "_Toques.csv";
		
		string date = System.DateTime.Now.ToString();
		string text = playerData.Session+ ";"
			+ date + ";"
			+ tipo+ ";"
			+pontos +";"
			+this.tag+"\n" ;
		
		if(!File.Exists(filePath))
			File.AppendAllText(filePath, "Sessao; Data; Tipo do Objeto; Pontos; Membro \n"+text);
		else{
			if(!ExistLine(date,this.tag,tipo))
				File.AppendAllText (filePath, text);
		}
	}
	//Funçao que verifica se a nova linha ja existe no csv
	private bool ExistLine(string data, string membro,string tipo)
	{
		GameController game = GameObject.Find ("Game Controller").GetComponent<GameController>();
		ChallengeTracer ct = gameObject.GetComponent<ChallengeTracer>();
		Player playerData = game.file.player;
    	
   	 	using(var reader = new StreamReader("Relatorios/" + playerData.Name.ToString () + "_Toques.csv")){
			
			while (!reader.EndOfStream)
    		{
        		var line = reader.ReadLine();
        		var values = line.Split(';');
    			if(values[1] == data && values[2] == tipo && values[4]==membro)
					return true;
	    	}
			return false;
		}
	}
	
//	void OnCollisionExit (Collision Target){
//		renderer.material.color = Color.blue;
//	}
}
