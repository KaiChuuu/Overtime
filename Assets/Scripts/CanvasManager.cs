using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    /// Scene Controller
    public GameManager gameManager;
    public AudioManager audioManager;
    [Tooltip("0: Start Panel, 1: Game Panel, 2: End Panel")]
    public GameObject[] scenePanels;

    /// Menu Scene
    public GameObject muttedAudio;
    public GameObject unmuttedAudio;
    public bool muteAudio = false;

    /// Game Scene
    public float gameIntroOutroDelay = 5f;
    private float timer = 0f;
    private bool gameIntroPlaying = false;
    private bool earlyDisplay = false;
    private bool gameOutroPlaying = false;

    private int clockTimes = 60; //Assuming clocks start 60
    public Image leftTimerBg;
    public Image rightTimerBg;
    public TMPro.TMP_Text gameTime;
    public TMPro.TMP_Text leftTime;
    public TMPro.TMP_Text rightTime;

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

    /// End Scene
    public TMPro.TMP_Text finalTime;

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

        //Display UI slightly earlier
        if(earlyDisplay && timer > gameIntroOutroDelay - 1f)
        {
            earlyDisplay = false;
            scenePanels[1].SetActive(true);
        }

        if(timer > gameIntroOutroDelay)
        {
            timer = 0f;
            gameIntroPlaying = false;
            gameManager.GameStart();
            if (!muteAudio) {
                audioManager.PlayBgMusic();
            }
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
        audioManager.StartEndButtonAudio();

        scenePanels[0].SetActive(false);

        gameManager.GameIntro();
        gameIntroPlaying = true;
        earlyDisplay = true;
    }

    public void GameEnd()
    {
        audioManager.GameOverAudio();

        scenePanels[1].SetActive(false);
        scenePanels[2].SetActive(true);

        if (!muteAudio)
        {
            audioManager.StopBgMusic();
        }

        //Update endsceen UI
        finalTime.text = gameTime.text;

        gameManager.GameEnd();
    }

    public void GameOutroScene()
    {
        audioManager.StartEndButtonAudio();

        gameManager.GameOutro();
        gameOutroPlaying = true;

        scenePanels[2].SetActive(false);
    }

    public void GameMenu()
    {
        scenePanels[0].SetActive(true);
    }

    ////////////////////////////////////////////////////
    /// Menu Scene
    ////////////////////////////////////////////////////
    public void MuteAudio()
    {
        audioManager.DefaultButtonAudio();

        unmuttedAudio.SetActive(false);
        muttedAudio.SetActive(true);

        muteAudio = true;
    }

    public void UnmuteAudio()
    {
        audioManager.DefaultButtonAudio();

        muttedAudio.SetActive(false);
        unmuttedAudio.SetActive(true);

        muteAudio = false;
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

    public void UpdateKillCount(int enemyScore)
    {
        gameManager.UpdateKillCount(enemyScore);
    }

    public void UpdateGameTime(string time)
    {
        gameTime.text = time;
    }

    public void UpdateLeftTimer(float time, bool display)
    {
        leftTime.text = time.ToString();

        if (display)
        {
            UpdateLeftUIColor(time);
            gameManager.UpdateLeftGameColor(time);
        }
    }

    void UpdateLeftUIColor(float time)
    {
        //UI change color
        leftTimerBg.color = Color.Lerp(healthColors[1], healthColors[0], time / clockTimes);
    }

    public void UpdateRightTimer(float time, bool display)
    {
        rightTime.text = time.ToString();

        if (display)
        {
            UpdateRightUIColor(time);
            gameManager.UpdateRightGameColor(time);
        }
    }

    void UpdateRightUIColor(float time)
    {
        //UI change color
        rightTimerBg.color = Color.Lerp(healthColors[1], healthColors[0], time / clockTimes);
    }

    public void UpdateFreezeColor()
    {
        //UI change color

        gameManager.UpdateBothGameColor("freeze");
    }

    public void Unfreeze() 
    {
        gameManager.Unfreeze();
    }

    ////////////////////////////////////////////////////
    /// End Scene
    ////////////////////////////////////////////////////

}
