using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Oculus.Interaction;
using UnityEngine.UI;

public class MainMenuXR : MonoBehaviour
{
    BaseController controller;
    XMLReader file;
    Player player;
    GameObject sound;
    GameController game;

    public bool modoAleatorio = false;
    string stringToEdit = "Nome do Jogador";

    public GameObject menu;
    public GameObject opcoes;
    public GameObject creditos;

    public TextMeshProUGUI volumeMusica;
    public TextMeshProUGUI volumeSom;
    public TextMeshProUGUI numMusica;
    [SerializeField] private TMP_Text _listaDesafiosText;

    Material skinColorMat;
    public Image corPelePanel;
    public Color32 corPele = new Color32(27, 25, 24, 255);

    string listaDeDesafios = ListaAO.nomeListaDesafios;

    public static class PassThrough
    {
        public static bool _isPassThroughOn = true;
    }

    [Tooltip("Objects that shouldn't be rendered during passthrough")]
    [Header("Passthrough Objects To Remove")]
    [SerializeField] private GameObject[] _objects;
    [Tooltip("These are UI objects that should be toggled ON/OFF during passthrough")]
    [Header("UI GameObjects to toggle ON/OFF")]
    [SerializeField] private TMP_Text _passThroughText;
    //[SerializeField] private PokeInteractable _locomotionInteractable;
    [SerializeField] private PokeInteractable _passThroughInteractable;

    private OVRPassthroughLayer _layer;    
    private Camera _camera;

    void Start ()
	{
        file = GameObject.Find("XML").GetComponent<XMLReader>();
        sound = GameObject.Find("Audio");
        game = GameObject.Find("Game Controller").GetComponent<GameController>();
        skinColorMat = Resources.Load<Material>("Objs/Characters/CharactersMaterial/CharacterSkin");

        if(file.player.Name != null)
			stringToEdit = file.player.Name;

        _layer = FindObjectOfType<OVRPassthroughLayer>();
        _camera = OVRManager.FindMainCamera();

        
        
        if (PassThrough._isPassThroughOn)
        {
            TurnPassThroughOn();
        }
        else
        {
            TurnPassThroughOff();
        }
        

    }

    void Update()
    {   
        if(SceneManager.GetActiveScene().name == "Game_Start")
        {
            controller = GameObject.Find("Ambiente").GetComponent<BaseController> ();
        }
        if(SceneManager.GetActiveScene().name == "Menu_Total")
        {
            volumeMusica.text = (sound.GetComponent<AudioSource>().volume * 100).ToString("0");
            volumeSom.text = (game.feedbackVolume * 100).ToString("0");
            numMusica.text = "Musica: " + sound.GetComponent<SoundBehavior>().musicIndex;
            corPelePanel.color = corPele;
            _listaDesafiosText.text = ListaAO.nomeListaDesafios;
        }
    }

    // Start is called before the first frame update
    public void Iniciar()
    {
        SceneManager.LoadScene("Game_Start");
    }

    public void MainMenu()
    {
        controller.EndSession();
    }

    public void Encerrar()
    {
        Application.Quit();
    }
    
    public void ModoAleatorio()
    {
        stringToEdit = "ModoAleatorio";
			player = new Player();
			stringToEdit = stringToEdit.ToUpper();
			player = file.GetPlayerByName(stringToEdit);	
			if(player == null)
			{
				player = new Player {
					Name = stringToEdit,
					CurrentPhase = "A",
					CurrentLevel = 7,
					Session = 1
				};
				file.playerList.SaveOrUpdate(player);
				file.playerList = file.playerList.Load();
				file.GetPlayerByName(player.Name); //forçar pegar jogador novo
			}

			modoAleatorio = true;
			SceneManager.LoadScene("Game_Start");
    }

    public void Creditos()
    {
        menu.SetActive(false);
        creditos.SetActive(true);
    }

    public void Opcoes()
    {
        menu.SetActive(false);
        opcoes.SetActive(true);
    }

    public void Voltar()
    {
        creditos.SetActive(false);
        opcoes.SetActive(false);

        menu.SetActive(true);
    }

    public void AumentarVolumeMusica()
    {
        if (game.feedbackVolume < 0.1)
        {
            sound.GetComponent<AudioSource>().volume = sound.GetComponent<AudioSource>().volume + 0.01f;
        }
    }

    public void DiminuirVolumeMusica()
    {
        if (game.feedbackVolume > 0)
        {
            sound.GetComponent<AudioSource>().volume = sound.GetComponent<AudioSource>().volume - 0.01f;
        }
    }

    public void AumentarVolumeSom()
    {
        if (game.feedbackVolume < 0.1)
        {
            game.feedbackVolume = game.feedbackVolume + 0.01f;
        }
    }

    public void DiminuirVolumeSom()
    {
        if (game.feedbackVolume > 0)
        {
            game.feedbackVolume = game.feedbackVolume - 0.01f;
        }
    }

    public void SetCorPele01()
    {
        corPele = new Color32(27, 25, 24, 255);
        skinColorMat.color = new Color32(27, 25, 24, 255);
    }

    public void SetCorPele02()
    {
        corPele = new Color32(46, 40, 42, 255);
        skinColorMat.color = new Color32(46, 40, 42, 255);
    }

    public void SetCorPele03()
    {
        corPele = new Color32(117, 91, 75, 255);
        skinColorMat.color = new Color32(117, 91, 75, 255);
    }

    public void SetCorPele04()
    {
        corPele = new Color32(183, 139, 97, 255);
        skinColorMat.color = new Color32(183, 139, 97, 255);
    }

    public void SetCorPele05()
    {
        corPele = new Color32(225, 196, 163, 255);
        skinColorMat.color = new Color32(225, 196, 163, 255);
    }

    public void SetCorPele06()
    {
        corPele = new Color32(255, 224, 192, 255);
        skinColorMat.color = new Color32(255, 224, 192, 255);
    }

    public void SetCorPele07()
    {
        corPele = new Color32(255, 239, 213, 255);
        skinColorMat.color = new Color32(255, 239, 213, 255);
    }

    public void AlteraListaDesafios()
    {
        for(int i=0; i<ListaAO.nomesListas.Count; i++)
		{
			string nome = ListaAO.nomesListas[i];
			if(nome == ListaAO.nomeListaDesafios)
                if(i == (ListaAO.nomesListas.Count - 1))
                {
                    ListaAO.nomeListaDesafios = ListaAO.nomesListas[0];
                }
                else
                {
                    ListaAO.nomeListaDesafios = ListaAO.nomesListas[i+1];
                    break;
                }
		}
    }

    public void TogglePassThrough()
    {
        if (PassThrough._isPassThroughOn)
        {
            TurnPassThroughOff();
        }
        else
        {
            TurnPassThroughOn();
        }
    }

    private void TurnPassThroughOn()
    {
        PassThrough._isPassThroughOn = true;
        _layer.textureOpacity = 1;
        _passThroughText.text = "Passthrough: ON";
        _camera.clearFlags = CameraClearFlags.SolidColor;
        foreach ( GameObject obj in _objects)
        {
            obj.SetActive(false);
        }
    }

    private void TurnPassThroughOff()
    {
        PassThrough._isPassThroughOn = false;
        _layer.textureOpacity = 0;
        _passThroughText.text = "Passthrough: OFF";
        _camera.clearFlags = CameraClearFlags.Skybox;
        foreach (GameObject obj in _objects)
        {
            obj.SetActive(true);
        }
    }
}
