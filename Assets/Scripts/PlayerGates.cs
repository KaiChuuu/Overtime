using UnityEngine;

public class PlayerGates : MonoBehaviour
{
    public GameObject[] gateBeams;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            for (int i = 0; i < gateBeams.Length; i++)
            {
                gateBeams[i].SetActive(false);
            }
        }
    }
    
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            for (int i = 0; i < gateBeams.Length; i++)
            {
                gateBeams[i].SetActive(true);
            }
        }
    }
}
