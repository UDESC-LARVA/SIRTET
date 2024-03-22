using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOutlineColor : MonoBehaviour
{
   
	// Para colisoes
	IEnumerator OnCollisionEnter (Collision Target)
    {	
		if(Target.transform.tag == "Obj" && this.tag != "Untagged")
		{
           
            PlayerCharacterController pcc = FindObjectOfType<PlayerCharacterController>();
         
            //Desta forma não é otimizado, pois a cada colisão é feito várias pesquisas o que acaba pesando o jogo

	 		Transform[] bodyPartsToChange = new Transform[2];
 
            switch(this.tag)
            {
                case "Left_Arm":
                    bodyPartsToChange[0] = pcc.anteBracoEsq.transform;                    
                    bodyPartsToChange[1] = pcc.bracoEsq.transform;
                break;
                case "Right_Arm":
                    bodyPartsToChange[0] = pcc.anteBracoDir.transform;                    
                    bodyPartsToChange[1] = pcc.bracoDir.transform;
                break;
                case "Left_Foot":
                    bodyPartsToChange[0] = pcc.canelaEsq.transform;                    
                    bodyPartsToChange[1] = pcc.pernaEsq.transform;
                break;
                case "Right_Foot":
                    bodyPartsToChange[0] = pcc.canelaDir.transform;                    
                    bodyPartsToChange[1] = pcc.pernaDir.transform;
                break;
                case "Head":
                    bodyPartsToChange[0] = pcc.cabeca.transform;                    
                    bodyPartsToChange[1] = pcc.cabeca.transform;
                break;
                               
            }
            

			Color color = new Color(0,0,1,0.5f);
			if(Target.gameObject.GetComponent<ObjectBehavior>().objeto.Type == "Obstaculo" || Target.gameObject.GetComponent<ObjectBehavior>().objeto.Type == "ObstaculoR")
				color = new Color(1,0,0,0.5f);
			
			
			foreach (Transform item in bodyPartsToChange) {
				item.GetComponentInChildren<Renderer>().materials[1].color = color;			}

			
			yield return new WaitForSeconds(3);
			foreach (Transform item in bodyPartsToChange) {
				item.GetComponentInChildren<Renderer>().materials[1].color = new Color(0,1,0,0.5f);
			}
			
		}
	}
}
