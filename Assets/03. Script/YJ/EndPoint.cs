using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.stageChanger.NextScene();
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        }
    }
}
