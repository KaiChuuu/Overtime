using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    /// Scene Controller
    public GameManager gameManager;
    [Tooltip("0: Start Panel, 1: Game Panel, 2: End Panel")]
    public GameObject[] scenePanels;

    /// Game Scene
    public float gameIntroOutroDelay = 3f;
    private float timer = 0f;
    private bool gameIntroPlaying = false;
    private bool gameOutroPlaying = false;

    public Slider healthSlider;
    public Image healthBar;
    [Tooltip("0: Full Health, 1: Zero Health")]
    public Color[] healthColors;

    public TMPro.TMP_Text currentAmmo;
    public TMPro.TMP_Text maxAmmo;
    public Sprite weapon2DModel; //or 3d model
    public TMPro.TMP_Text weaponName;
    public Slider reloadSlider;

    public RectTransform playerRadar;

    void Start()
    {
        scenePanels[0].SetActive(true);
        scenePanels[1].SetActive(false);
        scenePanels[2].SetActive(false);

        gameManager.GameSetup();
    }

    void Update()
    {
        if (gameIntroPlaying)
        {
            IntroDelay();
        }
        if (gameOutroPlaying)
        {
            OutroDelay();
        }
    }

    void IntroDelay()
    {
        timer += Time.deltaTime;

        if(timer > gameIntroOutroDelay)
        {
            timer = 0f;
            gameIntroPlaying = false;
            gameManager.GameStart();
            scenePanels[1].SetActive(true);
        }
    }

    void OutroDelay()
    {
        timer += Time.deltaTime;

        if (timer > gameIntroOutroDelay)
        {
            timer = 0f;
            gameOutroPlaying = false;
            GameMenu();
        }
    }

    public void GameStart()
    {
        scenePanels[0].SetActive(false);

        gameManager.GameIntro();
        gameIntroPlaying = true;
    }

    public void GameEnd()
    {
        scenePanels[1].SetActive(false);
        scenePanels[2].SetActive(true);

        gameManager.GameEnd();
    }

    public void GameOutroScene()
    {
        gameManager.GameOutro();
        gameOutroPlaying = true;

        scenePanels[2].SetActive(false);
    }

    public void GameMenu()
    {
        scenePanels[0].SetActive(true);
    }

    ////////////////////////////////////////////////////
    /// Game Scene
    ////////////////////////////////////////////////////
    public void SetHealthSlider(float startingHealth)
    {
        healthSlider.maxValue = startingHealth;
        healthSlider.value = startingHealth;
        healthBar.color = healthColors[0];
    }

    public void UpdatePlayerHealth(float health)
    {
        healthSlider.value = health;

        healthBar.color = Color.Lerp(healthColors[1], healthColors[0], health / healthSlider.maxValue);
    }

    public void SetWeaponUI(string name, int ammo, float reloadDelay)
    {
        weaponName.text = name.ToString();
        currentAmmo.text = ammo.ToString();
        maxAmmo.text = "/" + ammo.ToString();
        reloadSlider.maxValue = reloadDelay;
        reloadSlider.value = reloadDelay;
    }

    public void UpdateWeaponAmmo(int ammo)
    {
        currentAmmo.text = ammo.ToString();
    }

    public void UpdateReloadBar(float duration)
    {
        reloadSlider.value = duration;
    }

    public void UpdatePlayerRadar(float playerRotation)
    {
        playerRadar.localEulerAngles = new Vector3(0, 0, -playerRotation);
    }
}
