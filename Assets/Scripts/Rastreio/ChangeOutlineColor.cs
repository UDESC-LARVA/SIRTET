using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeOutlineColor : MonoBehaviour
{
    

    PlayerCharacterController pcc;
    GameObject[] bodyPartsToChange = new GameObject[2];
    public Material outlineMat;
    bool started = false;

    void Start () 
    {
		pcc = FindObjectOfType<PlayerCharacterController>();

        switch(this.tag)
        {
            case "Left_Arm":
                bodyPartsToChange[0] = pcc.anteBracoEsq.GetChild(0).gameObject;                    
                bodyPartsToChange[1] = pcc.bracoEsq.GetChild(0).gameObject;
            break;
            case "Right_Arm":
                bodyPartsToChange[0] = pcc.anteBracoDir.GetChild(0).gameObject;                    
                bodyPartsToChange[1] = pcc.bracoDir.GetChild(0).gameObject;
            break;
            case "Left_Foot":
                bodyPartsToChange[0] = pcc.canelaEsq.GetChild(0).gameObject;                    
                bodyPartsToChange[1] = pcc.pernaEsq.GetChild(0).gameObject;
            break;
            case "Right_Foot":
                bodyPartsToChange[0] = pcc.canelaDir.GetChild(0).gameObject;                    
                bodyPartsToChange[1] = pcc.pernaDir.GetChild(0).gameObject;
            break;
            case "Head":
                bodyPartsToChange[0] = pcc.cabeca.GetChild(0).gameObject;                    
                bodyPartsToChange[1] = pcc.cabeca.GetChild(0).gameObject;
            break;                           
        }

        outlineMat = Resources.Load("Material/MatOutlineHit") as Material;
	}


	// Para colisoes
	IEnumerator OnCollisionEnter (Collision Target)
    {	
		if(Target.transform.tag == "Obj" && this.tag != "Untagged")
		{
            string tipoObjeto =  Target.gameObject.GetComponent<ObjectBehavior>().objeto.Type;

            outlineMat.color = new Color(0,0,1,0.5f);

			if(tipoObjeto == "Obstaculo" || tipoObjeto == "ObstaculoR")
				outlineMat.color = new Color(1,0,0,0.5f);
			
			foreach (GameObject part in bodyPartsToChange) 
            {
			    part.gameObject.layer = 8;
            }

			
			yield return new WaitForSeconds(3);
            foreach (GameObject part in bodyPartsToChange) 
            {
			    part.gameObject.layer = 7;
            }

            
            outlineMat.color  = new Color(0,1,0,0.5f);	
			
		}
	}

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Game_Start")
        {
            if(!started)
            {
                started = true;
                foreach (GameObject part in bodyPartsToChange) 
                {
			        part.gameObject.layer = 7;
                }
                outlineMat.color  = new Color(0,1,0,0.5f);
            }
        }
    }
}
