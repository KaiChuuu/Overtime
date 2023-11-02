using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameManager gameManager;
    [Tooltip("0: Start Panel, 1: Game Panel, 2: End Panel")]
    public GameObject[] scenePanels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        scenePanels[0].SetActive(false);
        scenePanels[1].SetActive(true);

        gameManager.GameStart();
    }
}
