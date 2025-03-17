using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public static AchieveManager Instance;

    private Dictionary<string, Achievement> achievementDic;

    public RectTransform AchieveUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        achievementDic = new Dictionary<string, Achievement>();

        InitAchievement();
        LoadAchievement();
    }

    private void InitAchievement()
    {
        achievementDic.Clear();
        achievementDic.Add("FirstClear", new Achievement("첫걸음", "첫번째 맵을 클리어 했다", false));
        achievementDic.Add("GetItem", new Achievement("돌잔치", "처음으로 물건을 집었다", false));
        achievementDic.Add("Teleport", new Achievement("순간이동", "포탈을 이용하여 순간이동을 했다", false));
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
        if(achievementDic.ContainsKey(name) && !achievementDic[name].isCleared)
        {
            achievementDic[name].isCleared = true;
            SaveAchievement();
            //StartCoroutine(UpUI);

            //UI 화면 띄우는 연출 추가
        } 
    }

    private IEnumerator UpUI()
    {
        yield return null;
    }

    private void DownUI()
    {

    }
}
