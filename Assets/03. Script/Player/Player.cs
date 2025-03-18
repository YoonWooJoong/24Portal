using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public float curHP;
    public float maxHp;
    public float recoveryRate;

    private float currentHitTime;
    public float hitGap;

    private Camera _camera;
    private Vector3 _originCameraPos;
    private Coroutine _coroutine;

    public bool isDie = false;

    private void Awake()
    {
        curHP = maxHp;
        currentHitTime = Time.time;

        _camera = Camera.main;
        _originCameraPos = _camera.transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (isDie)
            return;

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
    
    /// <summary>
    /// 캐릭터 피격시 호출
    /// </summary>
    /// <param name="damage">입힐 데미지</param>
    public void TakeDamage(float damage)
    {
        if (isDie) return;

        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(HitEffect());

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
        if(isDie) { return; }
        isDie = true;
        GameManager.Instance.achieveManager.UnLockAchievement("Die");
        _camera.transform.DOMove(_camera.transform.position + new Vector3(0, -1.0f), 1.5f).SetEase(Ease.OutElastic); // 중간에 한 번 튕기기
        _camera.transform.DORotate(new Vector3(0, 0, 90), 1.0f).SetEase(Ease.OutBounce); // 끝날 때 튕기기

        Invoke("RestartGame", 2f); //2초 뒤 재시작
    }

    private void RestartGame()
    {
        GameManager.Instance.stageChanger.RestartScene();
    }

    //피격시 카메라 흔들림
    private IEnumerator HitEffect()
    {
        float duration = 0f;

        while(true)
        {
            if (duration > 0.5f)
                break;

            float x = Random.Range(-0.2f, 0.2f);
            float y = Random.Range(-0.2f, 0.2f);

            _camera.transform.localPosition = _originCameraPos + new Vector3(x, y);

            duration += Time.deltaTime;

            yield return null;
        }

        _camera.transform.localPosition = _originCameraPos;
    }

}
