using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class PlayerCharacterController : MonoBehaviour
{

    //Transform selected = null;
    int sexIndex = -1;

    public Transform 
		anteBracoEsq, anteBracoDir,
		bracoEsq, bracoDir,
		canelaEsq, canelaDir,
		pernaEsq, pernaDir, cabeca;
    
    List<MeshRenderer> partRend = new List<MeshRenderer>();
    List<Transform> partTransf = new List<Transform>();

    public BodySegments bs;
    public BodyComplete bc;

    List<Transform> filhos = new List<Transform>();
    
    public UDP udp;
    // Start is called before the first frame update


    bool human = false;
    void Start()
    {
        DontDestroyOnLoad(this.transform.gameObject);
        bs = GameObject.FindObjectOfType<BodySegments>();            
        bc = GameObject.FindObjectOfType<BodyComplete>();        
        udp = GameObject.FindObjectOfType<UDP>();

        for(int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {   
            filhos.Add(this.transform.GetChild(0).GetChild(i));
        }

        foreach (Transform filho in filhos)
        {
            foreach (Transform partes in filho.gameObject.GetComponentsInChildren<Transform>().ToList())
            {
                partes.gameObject.SetActive(false);                
            }            
        }

        Invoke("InitiateParts", 1);
        //InitiateParts();
        GetCharacterParts();
        
    }

    void GetCharacterParts()
    {
        anteBracoEsq =  filhos.Find(p => p.transform.name.Equals("AntebracoEsq"));
        anteBracoDir =  filhos.Find(p => p.transform.name.Equals("AntebracoDir"));
		bracoEsq =  filhos.Find(p => p.transform.name.Equals("BracoEsq"));
        bracoDir =  filhos.Find(p => p.transform.name.Equals("BracoDir"));
		canelaEsq =  filhos.Find(p => p.transform.name.Equals("PernaEsq"));
        canelaDir =  filhos.Find(p => p.transform.name.Equals("PernaDir"));
		pernaEsq =  filhos.Find(p => p.transform.name.Equals("CoxaEsq"));
        pernaDir =  filhos.Find(p => p.transform.name.Equals("CoxaDir"));
        cabeca =  filhos.Find(p => p.transform.name.Equals("Cabeca"));
    }

    
    void InitiateParts()
    {        
        GetPartRenders();
        GetPartTransf();

        foreach (Transform segment in partTransf)
        {
            segment.gameObject.SetActive(false);               
        }

        SetSex();
        SetHuman();
    }

    void GetPartRenders()
    {
        partRend.Add(bs.anteBracoEsq.GetComponent<MeshRenderer>());
        partRend.Add(bs.anteBracoDir.GetComponent<MeshRenderer>());
        partRend.Add(bs.bracoEsq.GetComponent<MeshRenderer>());
        partRend.Add(bs.bracoDir.GetComponent<MeshRenderer>());
        partRend.Add(bs.canelaEsq.GetComponent<MeshRenderer>());
        partRend.Add(bs.canelaDir.GetComponent<MeshRenderer>());
        partRend.Add(bs.pernaEsq.GetComponent<MeshRenderer>());
        partRend.Add(bs.pernaDir.GetComponent<MeshRenderer>());
        partRend.Add(bc.cabeca.GetComponent<MeshRenderer>());
        partRend.Add(bc.cinturaDir.GetComponent<MeshRenderer>());
        partRend.Add(bc.cinturaEsq.GetComponent<MeshRenderer>());
        partRend.Add(bc.cotoveloDir.GetComponent<MeshRenderer>());
        partRend.Add(bc.cotoveloEsq.GetComponent<MeshRenderer>());
        partRend.Add(bc.joelhoDir.GetComponent<MeshRenderer>());
        partRend.Add(bc.joelhoEsq.GetComponent<MeshRenderer>());
        partRend.Add(bc.maoDir.GetComponent<MeshRenderer>());
        partRend.Add(bc.maoEsq.GetComponent<MeshRenderer>());
        partRend.Add(bc.ombroDir.GetComponent<MeshRenderer>());
        partRend.Add(bc.ombroEsq.GetComponent<MeshRenderer>());
        partRend.Add(bc.peDir.GetComponent<MeshRenderer>());
        partRend.Add(bc.peEsq.GetComponent<MeshRenderer>());
    }

    void GetPartTransf()
    {
        partTransf.Add(anteBracoEsq);
        partTransf.Add(anteBracoDir);
        partTransf.Add(bracoEsq);
        partTransf.Add(bracoDir);
        partTransf.Add(canelaEsq);
        partTransf.Add(canelaDir);
        partTransf.Add(pernaEsq);
        partTransf.Add(pernaDir);
        partTransf.Add(cabeca);
    }


    // Update is called once per frame
    
	void LateUpdate () 
    {		

        UptadeSegments();

        if(Input.GetKeyDown(KeyCode.H))
        {
			SetHuman();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
			SetSex();
        }
		
	}

    void SetSex()
    {
        if(sexIndex >= 0)
        {            
            foreach (Transform filho in filhos)
            {
                filho.GetChild(sexIndex).gameObject.SetActive(false);               
            }
        }
        sexIndex = sexIndex+1 == filhos[0].childCount ? 0 : sexIndex+1;

        foreach (Transform filho in filhos)
        {
            filho.GetChild(sexIndex).gameObject.SetActive(true);               
        }        
    }

    void SetHuman()
    {
        human = !human;

        foreach(Transform p in partTransf)
            p.gameObject.SetActive(human);

        foreach(MeshRenderer p in partRend)
            p.enabled = !human;

    }
    
    
    void Initiate (Transform segment, Vector3 origem, Vector3 destino)
	{
        segment.transform.LookAt(destino, Vector3.forward);

		segment.position = new Vector3(origem.x + 0.5f * ( destino.x - origem.x), origem.y + 0.5f * 
			( destino.y - origem.y), origem.z + 0.5f * ( destino.z - origem.z));
		
		segment.transform.localScale = new Vector3 (80,  Vector3.Distance(origem,destino)/2, 80);

        segment.gameObject.SetActive(false);        
	}
	
    void UptadeSegments ()
	{   
        float scaleUp = 1.6f;
        cabeca.transform.position = bs.cabeca.transform.position;
        cabeca.transform.rotation = bs.cabeca.transform.rotation;    
        cabeca.transform.localScale = new Vector3(80,80,80);
        
        anteBracoEsq.transform.position  = bs.anteBracoEsq.transform.position;
        anteBracoEsq.transform.rotation  = bs.anteBracoEsq.transform.rotation;      
        anteBracoEsq.transform.localScale  = GetFinalScale(bs.anteBracoEsq.transform.localScale, scaleUp);   

        anteBracoDir.transform.position  = bs.anteBracoDir.transform.position;
        anteBracoDir.transform.rotation  = bs.anteBracoDir.transform.rotation;      
        anteBracoDir.transform.localScale  = GetFinalScale(bs.anteBracoDir.transform.localScale, scaleUp);   

        bracoEsq.transform.position  = bs.bracoEsq.transform.position;
        bracoEsq.transform.rotation  = bs.bracoEsq.transform.rotation;      
        bracoEsq.transform.localScale  = GetFinalScale(bs.bracoEsq.transform.localScale, scaleUp);   

        bracoDir.transform.position  = bs.bracoDir.transform.position;
        bracoDir.transform.rotation  = bs.bracoDir.transform.rotation;      
        bracoDir.transform.localScale  = GetFinalScale(bs.bracoDir.transform.localScale, scaleUp);   

        canelaEsq.transform.position  = bs.canelaEsq.transform.position;
        canelaEsq.transform.rotation  = bs.canelaEsq.transform.rotation;        
        canelaEsq.transform.localScale  = GetFinalScale(bs.canelaEsq.transform.localScale, scaleUp);   

        canelaDir.transform.position = bs.canelaDir.transform.position;
        canelaDir.transform.rotation = bs.canelaDir.transform.rotation;     
        canelaDir.transform.localScale = GetFinalScale(bs.canelaDir.transform.localScale, scaleUp);   

        pernaEsq.transform.position = bs.pernaEsq.transform.position;
        pernaEsq.transform.rotation = bs.pernaEsq.transform.rotation;       
        pernaEsq.transform.localScale = GetFinalScale(bs.pernaEsq.transform.localScale, scaleUp);   

        pernaDir.transform.position = bs.pernaDir.transform.position;
        pernaDir.transform.rotation = bs.pernaDir.transform.rotation;       
        pernaDir.transform.localScale = GetFinalScale(bs.pernaDir.transform.localScale, scaleUp);      
	}

    Vector3 GetFinalScale(Vector3 sca, float up)
    {
        return new Vector3(sca.z * up, sca.y, sca.z * up);        
    }
}
