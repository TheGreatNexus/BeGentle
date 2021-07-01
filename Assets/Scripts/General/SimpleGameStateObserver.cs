using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public abstract class SimpleGameStateObserver : MonoBehaviour, IEventHandler
{

	public virtual void SubscribeEvents()
	{
		EventManager.Instance.AddListener<GameMenuEvent>(GameMenu);
		EventManager.Instance.AddListener<GamePlayEvent>(GamePlay);
		EventManager.Instance.AddListener<GameOverEvent>(GameOver);
        EventManager.Instance.AddListener<WinEvent>(Win);
		EventManager.Instance.AddListener<GamePauseEvent>(GamePause);
		EventManager.Instance.AddListener<GameResumeEvent>(GameResume);
        EventManager.Instance.AddListener<GameQuitEvent>(GameQuit);
        EventManager.Instance.AddListener<setPlayerHealthEvent>(setPlayerHealth);
		EventManager.Instance.AddListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
        EventManager.Instance.AddListener<GameObjectiveChangedEvent>(GameObjectiveChanged);

	}

	public virtual void UnsubscribeEvents()
	{
		EventManager.Instance.RemoveListener<GameMenuEvent>(GameMenu);
		EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
		EventManager.Instance.RemoveListener<GameOverEvent>(GameOver);
        EventManager.Instance.RemoveListener<WinEvent>(Win);
		EventManager.Instance.RemoveListener<GamePauseEvent>(GamePause);
		EventManager.Instance.RemoveListener<GameResumeEvent>(GameResume);
        EventManager.Instance.RemoveListener<GameQuitEvent>(GameQuit);
        EventManager.Instance.RemoveListener<setPlayerHealthEvent>(setPlayerHealth);
		EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
        EventManager.Instance.RemoveListener<GameObjectiveChangedEvent>(GameObjectiveChanged);

	}

	protected virtual void Awake()
	{
		SubscribeEvents();
	}

	protected virtual void OnDestroy()
	{
		UnsubscribeEvents();
	}

	protected virtual void GameMenu(GameMenuEvent e)
	{
	}

	protected virtual void GamePlay(GamePlayEvent e)
	{
	}

	protected virtual void GameOver(GameOverEvent e)
	{
	}

    protected virtual void Win(WinEvent e)
    {
    }


    protected virtual void GamePause(GamePauseEvent e)
	{
	}

	protected virtual void GameResume(GameResumeEvent e)
	{
	}
    protected virtual void GameQuit(GameQuitEvent e)
    {
    }
    protected virtual void setPlayerHealth(setPlayerHealthEvent e)
    {
    }
	protected virtual void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
	}
    protected virtual void GameObjectiveChanged(GameObjectiveChangedEvent e)
    {
    }
}
