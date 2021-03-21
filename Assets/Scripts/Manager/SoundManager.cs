using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class SoundManager : Manager<SoundManager>
{
    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        // EventManager.Instance.AddListener<PlayerHasBeenHitAudioEvent>(PlayerHasBeenHit);
        // EventManager.Instance.AddListener<PlayerHasMissHitAudioEvent>(PlayerHasMissHit);
        // EventManager.Instance.AddListener<EnemyHasBeenHitEvent>(EnemyHasBeenHit);
        EventManager.Instance.AddListener<PlayerHasHitAudioEvent>(PlayerHasHit);
        EventManager.Instance.AddListener<PlayerWalkingAudioEvent>(PlayerWalking);
        EventManager.Instance.AddListener<PlayerStoppedWalkingAudioEvent>(PlayerStoppedWalking);
        
    }
    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        // EventManager.Instance.RemoveListener<PlayerHasBeenHitAudioEvent>(PlayerHasBeenHit);
        // EventManager.Instance.RemoveListener<PlayerHasMissHitAudioEvent>(PlayerHasMissHit);
        // EventManager.Instance.RemoveListener<EnemyHasBeenHitEvent>(EnemyHasBeenHit);
        EventManager.Instance.RemoveListener<PlayerHasHitAudioEvent>(PlayerHasHit);
      
        EventManager.Instance.RemoveListener<PlayerWalkingAudioEvent>(PlayerWalking);
        EventManager.Instance.RemoveListener<PlayerStoppedWalkingAudioEvent>(PlayerStoppedWalking);
        
    }
    [SerializeField] AudioClip PlayerHasHitAudio;
    [SerializeField] AudioClip PlayerHasMissHitAudio;
    [SerializeField] AudioClip PlayerWalkingAudio;


  private void PlayerHasMissHit(PlayerHasMissHitAudioEvent e)
    {
        if (GameObject.Find("PlayerHasMissHitGO") == null)
        {
            GameObject PlayerHasMissHitGO = new GameObject("PlayerHasMissHitGO");
            PlayerHasMissHitGO.transform.parent = GameObject.Find("AudioSources").transform;
            AudioSource audioSource = PlayerHasMissHitGO.AddComponent<AudioSource>();
            audioSource.PlayOneShot(PlayerHasMissHitAudio, 1f);
        }
        else
        {
            GameObject PlayerHasMissHitGO = GameObject.Find("PlayerHasMissHitGO");
            AudioSource audioSource = PlayerHasMissHitGO.GetComponent<AudioSource>();
            if (audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(PlayerHasMissHitAudio, 1f);
            }
        }
    } 

    private void PlayerWalking(PlayerWalkingAudioEvent e)
    {
        if (GameObject.Find("PlayerWalkingGO") == null)
        {
            GameObject PlayerWalkingGO = new GameObject("PlayerWalkingGO");
            Debug.Log(PlayerWalkingGO);
            PlayerWalkingGO.transform.parent = GameObject.Find("AudioSources").transform;
            AudioSource audioSource = PlayerWalkingGO.AddComponent<AudioSource>();
            audioSource.PlayOneShot(PlayerWalkingAudio, 1f);
        }
        else
        {
            GameObject PlayerWalkingGO = GameObject.Find("PlayerWalkingGO");
            AudioSource audioSource = PlayerWalkingGO.GetComponent<AudioSource>();
            if (audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(PlayerWalkingAudio, 1f);
            }
        }   
    }

    private void PlayerStoppedWalking(PlayerStoppedWalkingAudioEvent e)
    {
        if (GameObject.Find("PlayerWalkingGO") == null)
        {
            GameObject PlayerWalkingGO = new GameObject("PlayerWalkingGO");
            Debug.Log(PlayerWalkingGO);
            PlayerWalkingGO.transform.parent = GameObject.Find("AudioSources").transform;
            AudioSource audioSource = PlayerWalkingGO.AddComponent<AudioSource>();
        }
        else
        {
            GameObject PlayerWalkingGO = GameObject.Find("PlayerWalkingGO");
            AudioSource audioSource = PlayerWalkingGO.GetComponent<AudioSource>();
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }
    }

    private void PlayerHasHit(PlayerHasHitAudioEvent e)
    {

        if (GameObject.Find("PlayerHasHitGO") == null)
        {
            GameObject PlayerHasHitGO = new GameObject("PlayerHasHitGO");
            PlayerHasHitGO.transform.parent = GameObject.Find("AudioSources").transform;
            //GameObject PlayerHasHitGO = GameObject.Find("PlayerHasHit");
            AudioSource audioSource = PlayerHasHitGO.AddComponent<AudioSource>();
            audioSource.PlayOneShot(PlayerHasHitAudio, 1f);
        }
        else
        {  
            GameObject PlayerHasHitGO = GameObject.Find("PlayerHasHitGO");
            AudioSource audioSource = PlayerHasHitGO.GetComponent<AudioSource>();
            if (audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(PlayerHasHitAudio, 1f);
            }
        }
    }
}
