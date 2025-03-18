using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Camera _camera;
    private Vector3 _originCameraPos;
    private Coroutine _coroutine;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);

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
