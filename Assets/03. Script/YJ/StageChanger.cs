using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    /// <summary>
    /// ¾À Àç½ÃÀÛ
    /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// ´ÙÀ½¾ÀÀÌµ¿
    /// ´ÙÀ½¾ÀÀÌ¾øÀ¸¸é ·Î±×Ãâ·Â
    /// </summary>
    public void NextScene()
    {
        if (Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else { Debug.LogWarning("´ÙÀ½¾ÀÀÌ ¾ø½À´Ï´Ù."); }
    }

    /// <summary>
    /// ÀúÀåµÈ ¾À ºÒ·¯¿À±â
    /// </summary>
    /// <param name="buildindex">ÀúÀåµÈ¾À ÀÎµ¦½º</param>
    public void LoadScene(int buildindex)
    {
        SceneManager.LoadScene(buildindex);
    }

    /// <summary>
    /// ¸Ç Ã³À½ ¾ÀÀ¸·Î ÀÌµ¿
    /// </summary>
    public void GoFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// ¼±ÅÃÇÑ ¾ÀÀ» ºÒ·¯¿À´Â ¿ªÇÒ
    /// </summary>
    /// <param name="index"></param>
    public void ChoiceScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
