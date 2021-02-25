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
		EventManager.Instance.AddListener<GamePauseEvent>(GamePause);
		EventManager.Instance.AddListener<GameResumeEvent>(GameResume);

	}

	public virtual void UnsubscribeEvents()
	{
		EventManager.Instance.RemoveListener<GameMenuEvent>(GameMenu);
		EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
		EventManager.Instance.RemoveListener<GamePauseEvent>(GamePause);
		EventManager.Instance.RemoveListener<GameResumeEvent>(GameResume);

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

	protected virtual void GamePause(GamePauseEvent e)
	{
	}

	protected virtual void GameResume(GameResumeEvent e)
	{
	}
}
