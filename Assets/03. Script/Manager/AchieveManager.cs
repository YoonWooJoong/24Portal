using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    private Dictionary<string, Achievement> achievementDic;
    public GameObject AchievePopUpPrefab;

    public GameObject AchieveContainer;
    public RectTransform AchieveUI;
    public TextMeshProUGUI achieveName;
    public TextMeshProUGUI achieveDescription;

    private void Awake()
    {
        achievementDic = new Dictionary<string, Achievement>();
        
        InitAchievement();
        LoadAchievement();
    }

    private void Start()
    {
        ClearButton();
    }

    private void InitAchievement()
    {
        achievementDic.Clear();
        achievementDic.Add("FirstClear", new Achievement("첫걸음", "첫번째 맵을 클리어 했다", false));
        achievementDic.Add("GetItem", new Achievement("돌잔치", "처음으로 물건을 집었다", false));
        achievementDic.Add("Teleport", new Achievement("순간이동", "포탈을 이용하여 순간이동을 했다", false));
        achievementDic.Add("Die", new Achievement("첫죽음", "죽었다!", false));
    }

    /// <summary>
    /// 업적 로드
    /// </summary>
    public void LoadAchievement()
    {
        foreach (var achieve in achievementDic) 
        {
            if (PlayerPrefs.HasKey(achieve.Key) && PlayerPrefs.GetInt(achieve.Key) == 1)
            {
                achievementDic[achieve.Key].isCleared = true; 
            }
        }
    }

    /// <summary>
    /// 업적 저장시 호출
    /// </summary>
    public void SaveAchievement()
    {
        foreach (var achievement in achievementDic)
        {
            PlayerPrefs.SetInt(achievement.Key, achievement.Value.isCleared ? 1 : 0);
        }
    }

    /// <summary>
    /// 업적 해금 시 실행
    /// </summary>
    public void UnLockAchievement(string name)
    {
        if (AchieveContainer == null)
        {
            AchieveContainer = Instantiate(AchievePopUpPrefab);
            AchieveUI = AchieveContainer.transform.GetChild(0).GetComponentInChildren<RectTransform>();
            achieveName = AchieveUI.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
            achieveDescription = AchieveUI.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        }

        if (achievementDic.ContainsKey(name) && !achievementDic[name].isCleared)
        {
            achievementDic[name].isCleared = true;
            SaveAchievement();
            achieveName.text = name;
            achieveDescription.text = achievementDic[name]._description;
            Debug.Log(achieveDescription.text);
            GameManager.Instance.soundManager.PlaySFX(3);
            StartCoroutine(UpUI());
        }
    }

    private IEnumerator UpUI()
    {
        for(int i = 0; i < 100; i++)
        {
            AchieveUI.transform.position = AchieveUI.transform.position + new Vector3(0, 1, 0);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(DownUI());
    }

    private IEnumerator DownUI()
    {
        for (int i = 0; i < 100; i++)
        {
            if(AchieveUI != null)
                AchieveUI.transform.position = AchieveUI.transform.position + new Vector3(0, -1, 0);
            yield return null;
        }

        achieveName.text = null;
        achieveDescription.text = null;
    }

    public void TestButton()
    {
        UnLockAchievement("FirstClear");
    }

    public void ClearButton()
    {
        foreach (var achieve in achievementDic)
        {
            if (PlayerPrefs.HasKey(achieve.Key) )
            {
                achievementDic[achieve.Key].isCleared = false;
            }
        }
    }
}
