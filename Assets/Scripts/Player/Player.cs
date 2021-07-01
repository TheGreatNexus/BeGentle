using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Player : MonoBehaviour
{
    private enum walkingState { STOP, WALKING, RUNNING, SPEEDBONUS };
    private enum BattleState { DEFAULT, ATTACKING, HITTING, MISSING, COOLDOWN };
    private BattleState m_BattleState;
    private walkingState m_WalkingState;

    //Player variables
    [SerializeField] private float m_playerHP;
    [SerializeField] private float m_playerMaxHP;
    [SerializeField] private float m_PlayerDamages;
    [SerializeField] private float m_PlayerCDAttack;
    [SerializeField] private float m_Range = 200;
    [SerializeField] private Animator anim;

    //Invicibility variables
    private float m_InvincibilityDuration = 1f;
    private float m_InvincibilityStarted;

    //Bonus Variables
    private float m_SpeedBonusTimer;
    [SerializeField] int m_SpeedBonusAmount;
    [SerializeField] float m_SpeedBonusTime;
    private bool isUnderBonusSpeed;

    //Audio
    [SerializeField] AudioClip a_Hit;
    [SerializeField] AudioClip a_Missed;
    [SerializeField] AudioClip a_Run;
    [SerializeField] AudioClip a_GotHit;
    [SerializeField] AudioClip a_Death;
    [SerializeField] AudioClip a_HealthBonus;
    [SerializeField] AudioClip a_SpeedBonus;
    [SerializeField] AudioSource a_Source;
    [SerializeField] AudioSource a_WalkSource;


    private bool hasBeenHitRecently;
    // Start is called before the first frame update
    void Start()
    {
        isUnderBonusSpeed = false;
        m_PlayerDamages = 10f;
        m_WalkingState = walkingState.STOP;
        m_BattleState = BattleState.DEFAULT;
    }

    // Update is called once per frame
    void Update()
    {
        //Attack management
        if (Input.GetButtonDown("Fire1"))
        {
            m_BattleState = BattleState.ATTACKING;
            anim.SetTrigger("HasAttacked");
        }
        //Sound management
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && m_WalkingState == walkingState.STOP)
        {
            EventManager.Instance.Raise(new PlayerWalkingAudioEvent());
            m_WalkingState = walkingState.RUNNING;
        }
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && m_WalkingState != walkingState.STOP)
        {
            a_WalkSource.Stop();
            EventManager.Instance.Raise(new PlayerStoppedWalkingAudioEvent());
            m_WalkingState = walkingState.STOP;
        }
        //hasBeenHitRecentlyManagement
        if (Time.time > m_InvincibilityStarted + m_InvincibilityDuration)
        {
            hasBeenHitRecently = false;
        }
        //SpeedBonus Expired
        if (Time.time > m_SpeedBonusTimer && isUnderBonusSpeed == true)
        {
            isUnderBonusSpeed = false;
            gameObject.GetComponent<PlayerMovement>().setSpeed(-m_SpeedBonusAmount);
        }
        if (m_WalkingState != walkingState.STOP)
        {
            playWalkingAudio();
        }

    }

    void FixedUpdate()
    {

    }

    public void boostedStats()
    {
        m_PlayerDamages = 1000;
        m_playerMaxHP = 1000;
        m_playerHP = 1000;
        m_Range = 400;
    }
    public void playerAttacked()
    {

        if (m_BattleState != BattleState.ATTACKING) { return; }
        RaycastHit r_Hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(new Vector3(transform.position.x, (transform.position.y) + 1, transform.position.z), transform.TransformDirection(Vector3.forward), out r_Hit, m_Range * 0.01f) && r_Hit.transform.tag == "Enemy")
        {
            Debug.DrawRay(new Vector3(transform.position.x, (transform.position.y) + 1, transform.position.z), transform.TransformDirection(Vector3.forward) * 200, Color.yellow);
            EventManager.Instance.Raise(new EnemyHasBeenHitEvent() { eDamages = m_PlayerDamages, eEnemy = r_Hit.collider.gameObject });
            m_BattleState = BattleState.HITTING;
            attackingAudio();
            m_BattleState = BattleState.DEFAULT;
        }
        else
        {
            Debug.DrawRay(new Vector3(transform.position.x, (transform.position.y) + 1, transform.position.z), transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            {
                m_BattleState = BattleState.MISSING;
                attackingAudio();
                m_BattleState = BattleState.DEFAULT;
            }
        }

    }


    private void attackingAudio()
    {
        switch (m_BattleState)
        {
            case BattleState.HITTING:
                a_Source.clip = a_Hit;
                a_Source.Play();
                break;
            case BattleState.MISSING:
                a_Source.clip = a_Missed;
                a_Source.Play();
                break;
            default:
                break;
        }

    }

    public void isHit(float dmg)
    {
        if (hasBeenHitRecently == false)
        {
            a_Source.clip = a_GotHit;
            a_Source.Play();
            m_playerHP -= dmg;
            hasBeenHitRecently = true;
            m_InvincibilityStarted = Time.time;
        }

    }

    public float getPlayerHp()
    {
        return m_playerHP;
    }

    public void takeBonus(string bonusName)
    {
        if (bonusName == "Health")
        {
            a_Source.clip = a_HealthBonus;
            a_Source.Play();
            m_playerHP += 25;
            if (m_playerHP > m_playerMaxHP)
            {
                m_playerHP = m_playerMaxHP;
            }
        }
        if (bonusName == "Speed")
        {
            a_Source.clip = a_SpeedBonus;
            a_Source.Play();
            m_SpeedBonusTimer = Time.time + m_SpeedBonusTime;
            if (isUnderBonusSpeed == false)
            {
                isUnderBonusSpeed = true;
                gameObject.GetComponent<PlayerMovement>().setSpeed(m_SpeedBonusAmount);
                m_WalkingState = walkingState.SPEEDBONUS;
            }
        }
    }
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.tag == "Enemy" && Time.time > m_NextHit)
    //     {
    //         m_NextHit = Time.time + m_CooldownHit;
    //         EventManager.Instance.Raise(new PlayerHasBeenHitEvent());
    //     }
    // }
    void playWalkingAudio()
    {
        if (!a_WalkSource.isPlaying)
        {
            a_WalkSource.clip = a_Run;
            a_WalkSource.Play();
        }
    }

    public bool isDead()
    {
        if (m_playerHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
