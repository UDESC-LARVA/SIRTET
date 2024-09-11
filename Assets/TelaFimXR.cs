using TMPro;
using UnityEngine;

public class TelaFimXR : MonoBehaviour
{
    Interface gui;
    public GameObject telaFim;
    public GameObject telaPausa;

    // References to the TextMeshProUGUI components
    public TextMeshProUGUI scoreText;

    public string scoreGUI;


    void Start()
    {
        gui = GameObject.Find ("Interface").GetComponent<Interface> ();

        telaFim.SetActive(false);
        telaPausa.SetActive(false);

        // Initialize the UI
        Update();
    }

    void Update()
    {   
        if (gui.gameEnded)
        {
            telaFim.SetActive(true);
        }
        if(gui.controller.isPaused)
        {
            if(!gui.gameEnded)
            {
                telaPausa.SetActive(true);
            }
        }
        else
        {
            telaPausa.SetActive(false);
        }
        scoreGUI = (gui.scoreGUI*100).ToString("0");
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = scoreGUI;
    }
}
