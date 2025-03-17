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
        GameManager.Instance.sceneLoadIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// �������̵�
    /// �������̾����� �α����
    /// </summary>
    public void NextScene()
    {
        if (Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
        {
            GameManager.Instance.sceneLoadIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(0);
        }
        else { Debug.LogWarning("�������� �����ϴ�."); }
    }


    /// <summary>
    /// ����� �� �ҷ�����
    /// </summary>
    /// <param name="buildindex">����Ⱦ� �ε���</param>
    public void LoadScene(int buildindex)
    {
        GameManager.Instance.sceneLoadIndex = buildindex;
        SceneManager.LoadScene(0);

    }

}
