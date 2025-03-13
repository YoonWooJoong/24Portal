using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }
    public StageChanger stageChanger;
    public GameObject playerPrefab;
    public GameObject startPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        playerPrefab = Resources.Load<GameObject>("Player");
        StageChanger changerObject = new GameObject("StageChanger").AddComponent<StageChanger>();
        changerObject.transform.SetParent(transform);
        stageChanger = GetComponentInChildren<StageChanger>();
        PlayerCreate();
    }

    public void PlayerCreate()
    {
        Debug.Log(playerPrefab);
        Debug.Log("캐릭터 생성");
        Debug.Log(startPosition.gameObject.transform.position);
        if (playerPrefab != null)
            Instantiate(playerPrefab, startPosition.transform.position, Quaternion.identity);
        else
            Debug.Log("플레이어 프리팹이 없습니다.");
    }

}
