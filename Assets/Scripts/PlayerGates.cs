using UnityEngine;
using UnityEngine.AI;

public class PlayerGates : MonoBehaviour
{
    public GameObject[] gateBeams;
    private AudioSource gateNoise;

    public AudioClip gateDisable;
    public AudioClip gateActive;

    private NavMeshObstacle obstacle;
    private BoxCollider colldier;

    void Awake()
    {
        gateNoise = GetComponent<AudioSource>();
        obstacle = GetComponent<NavMeshObstacle>();
        colldier = GetComponent<BoxCollider>();
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

    public void DeactivateGate()
    {
        for (int i = 0; i < gateBeams.Length; i++)
        {
            gateBeams[i].SetActive(false);
        }
        gateNoise.loop = false;
        gateNoise.Stop();
        gateNoise.clip = gateDisable;
        gateNoise.Play();

        obstacle.enabled = false;
        colldier.enabled = false;
    }

    public void ReactivateGate()
    {
        for (int i = 0; i < gateBeams.Length; i++)
        {
            gateBeams[i].SetActive(true);
        }
        gateNoise.Stop();
        gateNoise.clip = gateActive;
        gateNoise.loop = true;
        gateNoise.Play();

        obstacle.enabled = true;
        colldier.enabled = true;
    }
}
