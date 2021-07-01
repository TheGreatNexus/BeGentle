using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using TMPro;

public enum GameState { gameMenu, gamePlay, gamePause }

public class GameManager : Manager<GameManager>
{

    //Game State
    private GameState m_GameState;
    public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }

    // TIME SCALE
    private float m_TimeScale;
    public float TimeScale { get { return m_TimeScale; } }
    void SetTimeScale(float newTimeScale)
    {
        m_TimeScale = newTimeScale;
        Time.timeScale = m_TimeScale;
    }

    //Lives
    //[SerializeField] private int m_NStartLives;
    [SerializeField] private GameObject player;
    private GameObject[] gameObjects;
    [SerializeField] private List<Transform> bonusSpawnPoints = new List<Transform>();
    [SerializeField] private List<GameObject> bonusSpawnType = new List<GameObject>();
    private int checkPointNb;
    [SerializeField] private float m_Objectif;
    private float m_TimeLeft;
    private float m_MinutesLeft;
    private float m_SecondsLeft;
    [SerializeField] TextMeshProUGUI minutesFPS;
    [SerializeField] TextMeshProUGUI secondesFPS;
    [SerializeField] TextMeshProUGUI minutesMap;
    [SerializeField] TextMeshProUGUI secondesMap;
    [SerializeField] private float m_BonusCooldown;
    private float m_NextBonus;

    [SerializeField] AudioClip a_Music;
    [SerializeField] AudioClip a_Birds;
    [SerializeField] AudioSource a_Source;


    //Players
    //[SerializeField]
    //List<PlayerController> m_Players = new List<PlayerController>();

    #region Events' subscription
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();

        //MainMenuManager
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.AddListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);
        EventManager.Instance.AddListener<PlayerTookABonusEvent>(PlayerTookABonus);
        EventManager.Instance.AddListener<Player2HasSummonedEnemyEvent>(Player2SummonedEnemy);
        EventManager.Instance.AddListener<Player2WantToCheatEvent>(Player2WantToCheat);
        EventManager.Instance.AddListener<EnemyHasBeenHitEvent>(EnemyHasBeenHit);
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);
        EventManager.Instance.AddListener<PlayerHasKilledEnemyEvent>(PlayerHasKilledEnemy);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        //MainMenuManager
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.RemoveListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);
        EventManager.Instance.RemoveListener<PlayerTookABonusEvent>(PlayerTookABonus);
        EventManager.Instance.RemoveListener<Player2HasSummonedEnemyEvent>(Player2SummonedEnemy);
        EventManager.Instance.RemoveListener<Player2WantToCheatEvent>(Player2WantToCheat);
        EventManager.Instance.RemoveListener<EnemyHasBeenHitEvent>(EnemyHasBeenHit);
        EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);
        EventManager.Instance.RemoveListener<PlayerHasKilledEnemyEvent>(PlayerHasKilledEnemy);
    }
    #endregion

    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        Play();
        //EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = 0, eNLives = 0, eNEnemiesLeftBeforeVictory = 0 });
        yield break;
    }
    #endregion
    void Update()
    {
        m_MinutesLeft = Mathf.Floor(((m_TimeLeft - Time.time) / 60));
        m_SecondsLeft = Mathf.Floor((m_TimeLeft - Time.time) - (60 * m_MinutesLeft));
        minutesFPS.text = m_MinutesLeft.ToString();
        secondesFPS.text = m_SecondsLeft.ToString();
        minutesMap.text = m_MinutesLeft.ToString();
        secondesMap.text = m_SecondsLeft.ToString();
        if (Time.time > m_TimeLeft)
        {
            PlayerWin();
        }

        if (Time.time > m_NextBonus)
        {
            spawnBonus();
            m_NextBonus += m_BonusCooldown;
        }
        playMusic();
        EventManager.Instance.Raise(new GameStatisticsChangedEvent());

    }

    #region Callbacks to Events issued by MenuManager
    private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Cursor.lockState = CursorLockMode.None;
    }

    private void PlayButtonClicked(PlayButtonClickedEvent e)
    {
        Play();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ResumeButtonClicked(ResumeButtonClickedEvent e)
    {
        Resume();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void EscapeButtonClicked(EscapeButtonClickedEvent e)
    {
        if (IsPlaying)
            Cursor.lockState = CursorLockMode.None;
        Pause();
    }
    private void QuitButtonClicked(QuitButtonClickedEvent e)
    {
        Quit();
    }

    private void PlayerHasBeenHit(PlayerHasBeenHitEvent e)
    {
        player.GetComponent<Player>().isHit(e.eDamages);
        EventManager.Instance.Raise(new setPlayerHealthEvent());
        if(player.GetComponent<Player>().isDead()== true){
            PlayerLoose();
        }
        //EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eNLives = player.GetComponent<Player>().getPlayerHp() });
    }
    private void PlayerTookABonus(PlayerTookABonusEvent e)
    {
        player.GetComponent<Player>().takeBonus(e.eBonusName);
        EventManager.Instance.Raise(new setPlayerHealthEvent());
    }
    private void Player2WantToCheat(Player2WantToCheatEvent e)
    {
        //EventManager.Instance.Raise(new GameStatisticsChangedEvent());
        player.GetComponent<Player>().boostedStats();
        player.GetComponent<PlayerMovement>().superSpeed();
    }
    private void EnemyHasBeenHit(EnemyHasBeenHitEvent e)
    {
        e.eEnemy.GetComponentInChildren<Enemy>().Hit(e.eDamages);
        //EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eNLives = player.GetComponent<Player>().getPlayerHp() });
    }

    private void PlayerHasKilledEnemy(PlayerHasKilledEnemyEvent e)
    {
    }

    private void Player2SummonedEnemy(Player2HasSummonedEnemyEvent e)
    {
        Instantiate(e.eEnemyType, e.eSpawnPosition.position, Quaternion.identity);
    }
    #endregion

    private void PlayerHasAttacked(PlayerHasAttackedEvent e)
    {
        //Debug.Log("Has Attacked");
    }

    //EVENTS
    // private void Menu()
    // {
    //     Cursor.visible = true;
    //     SetTimeScale(0);
    //     m_GameState = GameState.gameMenu;
    //     EventManager.Instance.Raise(new GameMenuEvent());
    // }
    private void Quit()
    {
        EventManager.Instance.Raise(new GameQuitEvent());
    }

    private void Play()
    {
        //InitNewGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetTimeScale(1);
        m_GameState = GameState.gamePlay;
        EventManager.Instance.Raise(new GamePlayEvent());
        m_TimeLeft = Time.time + m_Objectif;
        m_NextBonus = Time.time + m_BonusCooldown;
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetTimeScale(0);
        m_GameState = GameState.gamePause;
        EventManager.Instance.Raise(new GamePauseEvent());
    }

    private void Resume()
    {
        SetTimeScale(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_GameState = GameState.gamePlay;
        EventManager.Instance.Raise(new GameResumeEvent());
    }
    private void PlayerLoose()
    {
        Cursor.lockState = CursorLockMode.None;
        SetTimeScale(0);
        Cursor.visible = true;
        m_GameState = GameState.gamePause;
        {
            EventManager.Instance.Raise(new GameOverEvent());
        }
    }

    private void PlayerWin()
    {
        Cursor.lockState = CursorLockMode.None;
        SetTimeScale(0);
        Cursor.visible = true;
        m_GameState = GameState.gamePause;
        EventManager.Instance.Raise(new GameOverEvent());
    }

    private void spawnBonus()
    {
        GameObject bonusType;
        Vector3 spawnPlace = new Vector3();
        int typeOfBonusChance = Mathf.RoundToInt(Random.Range(0, 11));
        int spawnPointOfBonusChance = Mathf.RoundToInt(Random.Range(0, 2));

        if (typeOfBonusChance < 7)
        {
            bonusType = bonusSpawnType[0];
        }
        else { bonusType = bonusSpawnType[1]; }

        switch (spawnPointOfBonusChance)
        {
            case 0:
                spawnPlace = bonusSpawnPoints[0].position;
                break;
            case 1:
                spawnPlace = bonusSpawnPoints[1].position;
                break;
        }

        Instantiate(bonusType, spawnPlace, bonusType.transform.rotation);

    }
    void playMusic(){
        if(!a_Source.isPlaying){
            a_Source.clip = a_Music;
            a_Source.Play();
        }
    }


}
