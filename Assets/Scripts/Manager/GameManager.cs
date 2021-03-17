using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

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
	private GameObject[] gameObjects;
	private int checkPointNb;

	private int m_StartLives = 3;
	public int NLives { get { return m_StartLives; } }
	void DecrementNLives(int decrement)
	{
		SetNLives(m_StartLives - decrement);
	}

	void SetNLives(int nLives)
	{
		m_StartLives = nLives;
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eNLives = m_StartLives });
	}
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
		EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
		EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
		EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		//MainMenuManager
		EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
		EventManager.Instance.RemoveListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);
		EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
		EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
		EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);
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
		Menu();
	}

	private void PlayButtonClicked(PlayButtonClickedEvent e)
	{
		Play();
	}

	private void ResumeButtonClicked(ResumeButtonClickedEvent e)
	{
		Resume();
	}

	private void EscapeButtonClicked(EscapeButtonClickedEvent e)
	{
		if (IsPlaying)
			Pause();
	}
    private void QuitButtonClicked(QuitButtonClickedEvent e)
    {
        Quit();
    }

	private void PlayerHasBeenHit(PlayerHasBeenHitEvent e)
	{
		//EventManager.Instance.Raise(new PlayerHasBeenHitAudioEvent());
		DecrementNLives(1);
		//Debug.Log(m_StartLives);
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eNLives = m_StartLives });
		if (m_StartLives == 0)
		{
            PlayerLoose();
		}
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
        GameObject.Find("NLives").transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = m_StartLives.ToString();
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
        SetTimeScale(0);
        Cursor.visible = true;
        m_GameState = GameState.gamePause;
        {
                EventManager.Instance.Raise(new GameOverEvent());
        }
    }
}
