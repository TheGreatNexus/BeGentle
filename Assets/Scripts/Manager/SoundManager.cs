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
        EventManager.Instance.AddListener<PlayerHasTouchAudioEvent>(PlayerHasTouch);
        EventManager.Instance.AddListener<PlayerWalkingAudioEvent>(PlayerWalking);
        // EventManager.Instance.AddListener<EnemyHasBeenHitEvent>(EnemyHasBeenHit);
    }
    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
    //    EventManager.Instance.RemoveListener<PlayerHasBeenHitAudioEvent>(PlayerHasBeenHit);
        EventManager.Instance.RemoveListener<PlayerHasTouchAudioEvent>(PlayerHasTouch);
        EventManager.Instance.RemoveListener<PlayerHasMissHitAudioEvent>(PlayerHasMissHit);
        EventManager.Instance.RemoveListener<PlayerWalkingAudioEvent>(PlayerWalking);
        // EventManager.Instance.RemoveListener<EnemyHasBeenHitEvent>(EnemyHasBeenHit);
    }
    [SerializeField] AudioClip PlayerHasTouchAudio;
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

    private void PlayerHasTouch(PlayerHasTouchAudioEvent e)
    {
        GameObject PlayerHasTouchGO = GameObject.Find("PlayerHasTouch");
        AudioSource audioSource = PlayerHasTouchGO.GetComponent<AudioSource>();
        audioSource.PlayOneShot(PlayerHasTouchAudio, 0.2f);
    }
}
