using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;


public class HUDManager : Manager<HUDManager>
{
	[Header("HudManager")]
	[Header("Texts")]
	[SerializeField] private Text m_TxtNLives;
    [SerializeField] private Text m_TxtNObjective;
	//[SerializeField] private Text m_TxtScore;

	[SerializeField] private GameObject m_Life1;
	[SerializeField] private GameObject m_Life2;
	[SerializeField] private GameObject m_Life3;
	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	#region Callbacks to GameManager events
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		//m_TxtBestScore.text = e.eBestScore.ToString();
		//m_TxtScore.text = e.eScore.ToString();
		if (e.eNLives == 2)
		{
			m_Life3.SetActive(false);
		}
		if (e.eNLives == 1)
		{
			m_Life2.SetActive(false);
		}
        if (e.eNLives == 0)
        {
            m_Life1.SetActive(false);
        }
	}

    protected override void GameObjectiveChanged(GameObjectiveChangedEvent e)
    {
		m_TxtNObjective.text = "Objective : " + e.eNObjective.ToString();
    }
	#endregion
}
