using UnityEngine;

public class PlayerGates : MonoBehaviour
{
    public GameObject[] gateBeams;
    private AudioSource gateNoise;

    public AudioClip gateDisable;
    public AudioClip gateActive;

    void Awake()
    {
        gateNoise = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            for (int i = 0; i < gateBeams.Length; i++)
            {
                gateBeams[i].SetActive(false);
            }
            gateNoise.loop = false;
            gateNoise.Stop();
            gateNoise.clip = gateDisable;
            gateNoise.Play();
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
            gateNoise.Stop();
            gateNoise.clip = gateActive;
            gateNoise.loop = true;
            gateNoise.Play();
        }
    }
}
