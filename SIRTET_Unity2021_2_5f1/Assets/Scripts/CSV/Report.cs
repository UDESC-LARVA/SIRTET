using System.IO;
using System;

public class Report
{
	string filePath;
	
	public bool FileCreate ()
	{
		return true;
	}
	
	public void FileRead (Player jogador)
	{
		
	}
	
	public bool FileWrite (Player playerData, int score, string time)
	{	
		try
		{
			filePath = "Relatorios" + playerData.Name.ToString() + "Relatório";
			string text =
				playerData.Session + "; " +
				System.DateTime.Now.Date + "; " + 
				time + "; " + 
				score.ToString() + "; " + 
				playerData.CurrentPhase.ToString()  + "; " + 
				playerData.CurrentLevel.ToString();
			File.AppendAllText(filePath, text);
			return true;
		}
		catch(Exception e)
		{
			System.Console.WriteLine(e);
			return false;
		}
	}
	
	
}
