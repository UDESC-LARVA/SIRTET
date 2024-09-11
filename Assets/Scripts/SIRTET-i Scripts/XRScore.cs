using TMPro;
using UnityEngine;

public class XRScore : MonoBehaviour
{
    Interface gui;

    // References to the TextMeshProUGUI components
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI starText;

    private string timerString ;

    public string scoreGUI;
	public string starScoreStr = "___";


    void Start()
    {
        gui = GameObject.Find ("Interface").GetComponent<Interface> ();

        // Initialize the UI
        Update();

    }

    void Update()
    {
        

        timerString = gui.timerString;
        scoreGUI = (gui.scoreGUI*100).ToString("0");
        starScoreStr = gui.starScoreStr;

        UpdateUI();
        
    }

    void UpdateUI()
    {
        timerText.text = timerString;
        scoreText.text = scoreGUI;
        starText.text = starScoreStr;
    }

/*
    public void score (float points)
	{
		scoreGUI += points;
		if (points > 0) {
			styleScore.normal.textColor = Color.green;
			if (vScrollbarValue < performance) {
				vScrollbarValue ++;
				if (vScrollbarValue >= performance) {
					controller.AlterarNivel (1);
					curBarValue = -performance;
				}
			}
		} else {
			styleScore.normal.textColor = Color.red;
			if (vScrollbarValue > -performance) {	
				vScrollbarValue --;
				if (vScrollbarValue <= -performance) {
					controller.AlterarNivel (-1);
					curBarValue = -performance;
				}
			}
		}		
	}
    */
}
