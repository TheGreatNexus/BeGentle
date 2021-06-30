using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.SceneManagement;

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
	private int checkPointNb;
	private int m_Objectif = 10;
	private int m_CurrentKillCount=0;

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
        EventManager.Instance.AddListener<Player2HasSummonedEnemyEvent>(Player2SummonedEnemy);
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
        EventManager.Instance.RemoveListener<Player2HasSummonedEnemyEvent>(Player2SummonedEnemy);
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

	#region Callbacks to Events issued by MenuManager
	private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
	{
		Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
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
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eNLives = player.GetComponent<Player>().getPlayerHp()});
	}
    private void EnemyHasBeenHit(EnemyHasBeenHitEvent e)
    {
        e.eEnemy.GetComponentInChildren<Enemy>().Hit(e.eDamages);
        //EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eNLives = player.GetComponent<Player>().getPlayerHp() });
    }

    private void PlayerHasKilledEnemy(PlayerHasKilledEnemyEvent e)
    {
        m_CurrentKillCount +=1;
        EventManager.Instance.Raise(new GameObjectiveChangedEvent() { eNObjective = m_Objectif - m_CurrentKillCount });
        if (m_CurrentKillCount == m_Objectif)
        {
            PlayerWin();
        }
    }

    private void Player2SummonedEnemy(Player2HasSummonedEnemyEvent e)
    {
		Debug.Log(e.eEnemyType);
        Instantiate(e.eEnemyType,e.eSpawnPosition.position,Quaternion.identity);
    }
	#endregion

	private void PlayerHasAttacked(PlayerHasAttackedEvent e)
    {
        //Debug.Log("Has Attacked");
    }

	//EVENTS
	private void Menu()
	{
        Cursor.visible = true;
		SetTimeScale(0);
		m_GameState = GameState.gameMenu;
		EventManager.Instance.Raise(new GameMenuEvent());
	}
    private void Quit()
    {
        EventManager.Instance.Raise(new GameQuitEvent());
    }

	private void Play()
	{
        //InitNewGame();
        Cursor.visible = false;
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
        //GameObject.Find("NbLife").transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = m_StartLives.ToString();
        EventManager.Instance.Raise(new GamePlayEvent());
	}

	private void Pause()
	{
        Debug.Log("pause clicked");
        Cursor.visible = true;
		SetTimeScale(0);
		m_GameState = GameState.gamePause;
		EventManager.Instance.Raise(new GamePauseEvent());
	}

	private void Resume()
	{
		SetTimeScale(1);
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
        EventManager.Instance.Raise(new WinEvent());
    }
	
	
}
