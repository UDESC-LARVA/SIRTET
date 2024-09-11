using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    BaseController controller;
	GameController game;
	ChallengeController challengeController;

    // References to the TextMeshProUGUI components
    public TextMeshProUGUI jogadorText;
    public TextMeshProUGUI nivelText;
    public TextMeshProUGUI velocidadeText;
    public TextMeshProUGUI intervaloText;
    public TextMeshProUGUI faseText;
    public TextMeshProUGUI listaDesafiosText;
    public TextMeshProUGUI desafioInicialText;
    public TextMeshProUGUI desafioAtualText;
    public TextMeshProUGUI desafioFinalText;

    // Example variables to update the UI with
    private string jogador = "Default";
    private int nivel = 0;
    private float velocidade = 0.0f;
    private float intervalo = 0.0f;
    private string fase = "A";
    private string listaDesafios = "Original";
    private int desafioInicial = 0;
    private int desafioAtual = 0;
    private int desafioFinal = 0;

    void Start()
    {
        controller = GameObject.Find ("Ambiente").GetComponent<BaseController> ();
		
		game = GameObject.Find ("Game Controller").GetComponent<GameController>();
		
		challengeController = GameObject.Find ("Objetos").GetComponent<ChallengeController>();

        // Initialize the UI
        UpdateUI();
    }

    void Update()
    {
        jogador = game.file.player.Name;
        nivel = game.file.player.CurrentLevel;
        velocidade = controller.standardObjVelocity;
        intervalo = controller.standardChallengeInterval;
        fase = game.file.player.CurrentPhase;
        listaDesafios = ListaAO.nomeListaDesafios;
        desafioInicial = challengeController.desaIni;
        desafioAtual = challengeController.desaAtual;
        desafioFinal = challengeController.desaFin;

        UpdateUI();
        
    }

    void UpdateUI()
    {
        jogadorText.text = "Jogador: " + jogador;
        nivelText.text = "Nivel: " + nivel;
        velocidadeText.text = "Velocidade: " + velocidade;
        intervaloText.text = "Intervalo: " + intervalo;
        faseText.text = "Fase: " + fase;
        listaDesafiosText.text = "Lista Desafios: " + listaDesafios;
        desafioInicialText.text = "Desafio Inicial: " + desafioInicial;
        desafioAtualText.text = "Desafio Atual: " + desafioAtual;
        desafioFinalText.text = "Desafio Final: " + desafioFinal;
    }
}
