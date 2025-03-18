using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public UIManager uiManager;
    public SoundManager soundManager;
    public AchieveManager achieveManager;
    public int sceneLoadIndex;

    private float mouseSensitivty;
    public float MouseSensitivty
    {
        get { return mouseSensitivty; }
        set { mouseSensitivty = Mathf.Clamp01(value); }
    }

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

        playerPrefab = Resources.Load<GameObject>("Player/Player");
        StageChanger changerObject = new GameObject("StageChanger").AddComponent<StageChanger>();
        UIManager uIObject = new GameObject("UIManager").AddComponent<UIManager>();
        SoundManager soundObject = new GameObject("SoundManager").AddComponent<SoundManager>();
        changerObject.transform.SetParent(transform);
        uIObject.transform.SetParent(transform);
        soundObject.transform.SetParent(transform);
        stageChanger = GetComponentInChildren<StageChanger>();
        uiManager = GetComponentInChildren<UIManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        achieveManager = GetComponentInChildren<AchieveManager>();
    }

    private void Start()
    {
        soundManager.Init();

        soundManager.PlayBGM(0);
    }

    public void PlayerCreate()
    {
        if (playerPrefab != null)
            Instantiate(playerPrefab, startPosition.transform.position, Quaternion.identity);
        else
            Debug.Log("플레이어 프리팹이 없습니다.");
    }

}
