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
        EventManager.Instance.AddListener<PlayerHasMissHitAudioEvent>(PlayerHasMissHit);
        EventManager.Instance.AddListener<PlayerHasHitAudioEvent>(PlayerHasHit);
        EventManager.Instance.AddListener<PlayerWalkingAudioEvent>(PlayerWalking);
        EventManager.Instance.AddListener<PlayerStoppedWalkingAudioEvent>(PlayerStoppedWalking);
        // EventManager.Instance.AddListener<EnemyHasBeenHitEvent>(EnemyHasBeenHit);
    }
    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        //    EventManager.Instance.RemoveListener<PlayerHasBeenHitAudioEvent>(PlayerHasBeenHit);
        EventManager.Instance.RemoveListener<PlayerHasHitAudioEvent>(PlayerHasHit);
        EventManager.Instance.RemoveListener<PlayerHasMissHitAudioEvent>(PlayerHasMissHit);
        EventManager.Instance.RemoveListener<PlayerWalkingAudioEvent>(PlayerWalking);
        EventManager.Instance.RemoveListener<PlayerStoppedWalkingAudioEvent>(PlayerStoppedWalking);
        // EventManager.Instance.RemoveListener<EnemyHasBeenHitEvent>(EnemyHasBeenHit);
    }
    [SerializeField] AudioClip PlayerHasHitAudio;
    [SerializeField] AudioClip PlayerHasMissHitAudio;
    [SerializeField] AudioClip PlayerWalkingAudio;

    /*   private void PlayerHasBeenHit(PlayerHasBeenHitAudioEvent e)
       {
           GameObject PlayerHasBeenHitGO = GameObject.Find("PlayerHasBeenHit");
           AudioSource audioSource = PlayerHasBeenHitGO.GetComponent<AudioSource>();
           audioSource.PlayOneShot(PlayerHitAudio, 0.2f);
       }*/

    private void PlayerHasMissHit(PlayerHasMissHitAudioEvent e)
    {
        GameObject PlayerHasMissHitGO = GameObject.Find("PlayerHasMissHit");
        AudioSource audioSource = PlayerHasMissHitGO.GetComponent<AudioSource>();
        audioSource.PlayOneShot(PlayerHasMissHitAudio, 0.2f);
    }

    private void PlayerWalking(PlayerWalkingAudioEvent e)
    {
        GameObject PlayerWalkingGO = GameObject.Find("PlayerWalking");
        AudioSource audioSource = PlayerWalkingGO.GetComponent<AudioSource>();
        audioSource.PlayOneShot(PlayerWalkingAudio, 0.2f);
    }

    private void PlayerStoppedWalking(PlayerStoppedWalkingAudioEvent e)
    {

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
        {   GameObject PlayerHasHitGO = GameObject.Find("PlayerHasHitGO");
            AudioSource audioSource = PlayerHasHitGO.GetComponent<AudioSource>();
            if (audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(PlayerHasHitAudio, 1f);
            }
        }
    }
}
