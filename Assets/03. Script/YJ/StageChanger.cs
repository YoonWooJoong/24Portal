using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    /// <summary>
    /// �� �����
    /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// �������̵�
    /// �������̾����� �α����
    /// </summary>
    public void NextScene()
    {
        if (Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else { Debug.LogWarning("�������� �����ϴ�."); }
    }

    /// <summary>
    /// ����� �� �ҷ�����
    /// </summary>
    /// <param name="buildindex">����Ⱦ� �ε���</param>
    public void LoadScene(int buildindex)
    {
        SceneManager.LoadScene(buildindex);
    }

    /// <summary>
    /// �� ó�� ������ �̵�
    /// </summary>
    public void GoFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// ������ ���� �ҷ����� ����
    /// </summary>
    /// <param name="index"></param>
    public void ChoiceScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
