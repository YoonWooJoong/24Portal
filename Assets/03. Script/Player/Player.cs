using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("Player").AddComponent<Player>();
            }
            return instance;
        }
    }
    public PlayerController controller;
    public float curHP;
    public float maxHp;
    public float recoveryRate;

    private float currentHitTime;
    public float hitGap;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);

        curHP = maxHp;
        currentHitTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (hitGap < Time.time - currentHitTime)
        {
            if(curHP < maxHp)
            {
                curHP += recoveryRate * Time.deltaTime;
            }
            else
            {
                curHP = maxHp;
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        curHP -= damage;
        currentHitTime = Time.time;
        if (curHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //죽었을 때 스테이지 초기화 등...
    }

}
