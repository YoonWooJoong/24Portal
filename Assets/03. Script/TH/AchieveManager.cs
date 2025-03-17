using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public static AchieveManager Instance;

    private Dictionary<string, Achievement> achievementDic;

    public RectTransform AchieveUI;
    public TextMeshProUGUI achieveName;
    public TextMeshProUGUI achieveDescription;

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
        if (achievementDic.ContainsKey(name) && !achievementDic[name].isCleared)
        {
            achievementDic[name].isCleared = true;
            SaveAchievement();
            achieveName.text = name;
            achieveDescription.text = achievementDic[name]._description;


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
