using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public float curHP;
    public float maxHp;
    public float recoveryRate;

    public float currentHitTime;
    public float hitGap;

    public Image hitImage;

    private void Awake()
    {
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
            if(curHP <= maxHp)
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
        if (curHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //죽었을 때 스테이지 초기화 등...
    }

    private void HitImageColorChange()
    {
        hitImage.color = new Color(1, 0, 0, (1 - curHP / maxHp) * 0.5f);
    }
}
