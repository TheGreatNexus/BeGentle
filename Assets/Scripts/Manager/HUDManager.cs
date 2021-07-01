using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;
using TMPro;


public class HUDManager : Manager<HUDManager>
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject player;
	[SerializeField] GameObject mapPlayer;
	[SerializeField] TextMeshProUGUI nextIncomeTime;
    [SerializeField] TextMeshProUGUI nextIncome;
    [SerializeField] TextMeshProUGUI actualMoney;

    //[SerializeField] private Text m_TxtScore;
    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }
    #endregion

    protected override void setPlayerHealth(setPlayerHealthEvent e)
    {
        slider.value = player.GetComponent<Player>().getPlayerHp();
    }


    #region Callbacks to GameManager events
    protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
    {
		nextIncome.text= mapPlayer.GetComponent<MapPlayer>().getIncomeValue().ToString();
        nextIncomeTime.text = mapPlayer.GetComponent<MapPlayer>().getNextIncome().ToString();
        actualMoney.text = mapPlayer.GetComponent<MapPlayer>().getMoney().ToString();
		//mapPlayer.GetComponent<MapPlayer>().get
    }

    protected override void GameObjectiveChanged(GameObjectiveChangedEvent e)
    {

    }
    #endregion
}
