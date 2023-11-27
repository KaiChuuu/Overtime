using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

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
    public Color gameTimeDefault;
    public Color gameTimeEnd;
    public Color gamePredictedHighscore;
    public TMPro.TMP_Text gameTime;
    public TMPro.TMP_Text gameScore;
    public TMPro.TMP_Text leftTime;
    public TMPro.TMP_Text rightTime;

    public Slider healthSlider;
    public Image healthBar;
    [Tooltip("0: Full Health, 1: Zero Health")]
    public Color[] healthColors;
    public SpriteRenderer[] damageIndicators;
    public Color damageIndicatorColor;
    private bool indicatorActive = false;

    public TMPro.TMP_Text currentAmmo;
    public TMPro.TMP_Text maxAmmo;
    public Sprite weapon2DModel;
    public TMPro.TMP_Text weaponName;
    public Slider reloadSlider;

    public RectTransform playerRadar;

    /// End Scene
    public GameObject highscorePanel;
    public TMPro.TMP_InputField nameInputField;
    public TMPro.TMP_Text finalTime;
    public TMPro.TMP_Text finalScore;
    public TMPro.TMP_Text finalKillTotal;
    public TMPro.TMP_Text finalRank;
    public TMPro.TMP_Text finalPlayerName;

    /// Leaderboard Scene
    public LeaderboardDisplay leaderboardDisplay;

    void Start()
    {
        nameInputField.onValidateInput += delegate (string input, int charIndex, char addedChar) {
            return NameInputValidator(addedChar);
        };

        scenePanels[0].SetActive(true);
        scenePanels[1].SetActive(false);
        scenePanels[2].SetActive(false);
        scenePanels[3].SetActive(false);

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
        audioManager.UnmuteEnvironmentAudio();

        scenePanels[0].SetActive(false);

        gameManager.GameIntro();
        gameIntroPlaying = true;
        earlyDisplay = true;
    }

    public void GameEnd()
    {
        gameManager.GameEnd();

        audioManager.GameOverAudio();

        scenePanels[1].SetActive(false);

        if (!muteAudio)
        {
            audioManager.StopBgMusic();
        }
        audioManager.MuteEnvironmentAudio();

        //Update endsceen UI
        finalTime.text = gameTime.text;

        StartCoroutine(GameOverDelay());
    }

    public void GameEndScores(int score, int kills, bool isHighscore, int rank)
    {
        //Update endsceen UI
        finalScore.text = score.ToString();
        finalKillTotal.text = kills.ToString();

        if (isHighscore)
        {
            highscorePanel.SetActive(true);
            finalRank.text = "#" + rank.ToString();
        }
        else
        {
            highscorePanel.SetActive(false);
        }
    }

    public char NameInputValidator(char addedChar)
    {
        if(addedChar >= 'A' &&  addedChar <= 'Z')
        {
            return addedChar;
        }else if(addedChar >= 'a' && addedChar <= 'z')
        {
            return Char.ToUpper(addedChar);
        }
        return '\0';
    }

    public void UpdateHighscoreStatus(string name)
    {
        gameManager.leaderboardmanager.UpdateCurrentHighscoreName(name);
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(3f);

        scenePanels[2].SetActive(true);

        gameManager.EndScreen();
    }

    public async void GameOutroScene()
    {
        await gameManager.leaderboardmanager.AddScore();

        audioManager.StartEndButtonAudio();

        gameManager.GameOutro();
        gameOutroPlaying = true;

        scenePanels[2].SetActive(false);
    }

    public void GameMenu()
    {
        scenePanels[0].SetActive(true);
    }

    public void LeaderboardScene()
    {
        scenePanels[0].SetActive(false);
        scenePanels[3].SetActive(true);

        leaderboardDisplay.DisplayLeaderboard();
    }

    public void ToMenuFromLeaderboard()
    {
        scenePanels[0].SetActive(true);
        scenePanels[3].SetActive(false);
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
        if(!indicatorActive && health < healthSlider.value)
        {
            //Flash damage Indicator
            indicatorActive = true;
            StartCoroutine(DamageIndication());
        }

        healthSlider.value = health;

        healthBar.color = Color.Lerp(healthColors[1], healthColors[0], health / healthSlider.maxValue);
    }

    IEnumerator DamageIndication()
    {
        //Display red
        for (int i = 0; i < 4; i++)
        {
            damageIndicatorColor.a += 0.25f;
            foreach (SpriteRenderer border in damageIndicators)
            {
                border.color = damageIndicatorColor;
            }
            yield return new WaitForSeconds(0.05f);
        }

        //Remove red
        for (int i = 0; i < 4; i++)
        {
            damageIndicatorColor.a -= 0.25f;
            foreach (SpriteRenderer border in damageIndicators)
            {
                border.color = damageIndicatorColor;
            }
            yield return new WaitForSeconds(0.05f);
        }

        indicatorActive = false;
    }

    public void SetWeaponUI(string name, int clip, int ammo, float reloadDelay, Sprite gunModel)
    {
        weaponName.text = name.ToString();
        currentAmmo.text = clip.ToString();
        maxAmmo.text = "/" + ammo.ToString();
        reloadSlider.maxValue = reloadDelay;
        reloadSlider.value = reloadDelay;
        weapon2DModel = gunModel;
    }

    public void UpdateWeaponAmmo(int ammo)
    {
        currentAmmo.text = ammo.ToString();
    }

    public void UpdateWeaponMaxAmmo(int ammo)
    {
        maxAmmo.text = "/" + ammo.ToString();
    }

    public void UpdateReloadBar(float duration)
    {
        reloadSlider.value = duration;
    }

    public void DisableReloadBar()
    {
        reloadSlider.gameObject.SetActive(false);
    }

    public void EnableReloadBar()
    {
        reloadSlider.gameObject.SetActive(true);
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

    public void UpdateGameTimeColor(string color)
    {
        switch (color)
        {
            case "Red":
                gameTime.color = gameTimeEnd;
                break;
            case "White":
                gameTime.color = gameTimeDefault;
                break;
        }
    }

    public void UpdateGameScore(int score)
    {
        gameScore.text = score.ToString();
    }

    public void UpdateGameScoreColor(string color)
    {
        switch (color)
        {
            case "Blue":
                gameScore.color = gamePredictedHighscore;
                break;
            case "White":
                gameScore.color = gameTimeDefault;
                break;
        }
    }

    public void UpdateLeftTimer(float time, bool display)
    {
        leftTime.text = time.ToString();
        UpdateLeftUIColor(time);

        if (display)
        {
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
        UpdateRightUIColor(time);

        if (display)
        {
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
