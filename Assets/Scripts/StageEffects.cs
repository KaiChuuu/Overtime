using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEffects : MonoBehaviour
{
    public int stage;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlayerMovement targetMovement = collider.gameObject.GetComponentInChildren<PlayerMovement>();
            if (targetMovement != null)
            {
                switch (stage)
                {
                    case 2:                    
                        targetMovement.UpdatePlayerPhysics(3);       
                        break;
                    case 3:         
                        targetMovement.UpdatePlayerPhysics(1); 
                        break;
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            PlayerMovement targetMovement = collider.gameObject.GetComponentInChildren<PlayerMovement>();
            if (targetMovement != null)
            {
                targetMovement.UpdatePlayerPhysics(2);
            }
        }
    }
}
