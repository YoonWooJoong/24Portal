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
        achievementDic.Add("FirstClear", new Achievement("ù����", "ù��° ���� Ŭ���� �ߴ�", false));
        achievementDic.Add("GetItem", new Achievement("����ġ", "ó������ ������ ������", false));
        achievementDic.Add("Teleport", new Achievement("�����̵�", "��Ż�� �̿��Ͽ� �����̵��� �ߴ�", false));
    }

    /// <summary>
    /// ���� �ε�
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
    /// ���� ����� ȣ��
    /// </summary>
    public void SaveAchievement()
    {
        foreach (var achievement in achievementDic)
        {
            PlayerPrefs.SetInt(achievement.Key, achievement.Value.isCleared ? 1 : 0);
        }
    }

    /// <summary>
    /// ���� �ر� �� ����
    /// </summary>
    public void UnLockAchievement(string name)
    {
        if(achievementDic.ContainsKey(name) && !achievementDic[name].isCleared)
        {
            achievementDic[name].isCleared = true;
            SaveAchievement();
            //StartCoroutine(UpUI);

            //UI ȭ�� ���� ���� �߰�
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
