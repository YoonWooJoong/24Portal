using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    private Coroutine nowCo;

    public Elevator elevator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            elevator.IsIn = true;
            nowCo = StartCoroutine(DelayNextScene());

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (nowCo != null)
            {
                StopCoroutine(nowCo);
            }
        }
    }

    /// <summary>
    /// 2초 후 다음씬으로 넘어가면서 레벨을 저장함
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayNextScene()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.stageChanger.NextScene();
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
    }
}
