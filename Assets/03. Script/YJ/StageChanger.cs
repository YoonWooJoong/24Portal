using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{

    /// <summary>
    /// 씬 재시작
    /// </summary>
    public void RestartScene()
    {
        GameManager.Instance.sceneLoadIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 다음씬이동
    /// 다음씬이없으면 로그출력
    /// </summary>
    public void NextScene()
    {
        if (Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
        {
            GameManager.Instance.sceneLoadIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(0);
        }
        else { Debug.LogWarning("다음씬이 없습니다."); }
    }


    /// <summary>
    /// 저장된 씬 불러오기
    /// </summary>
    /// <param name="buildindex">저장된씬 인덱스</param>
    public void LoadScene(int buildindex)
    {
        GameManager.Instance.sceneLoadIndex = buildindex;
        SceneManager.LoadScene(0);

    }

}
