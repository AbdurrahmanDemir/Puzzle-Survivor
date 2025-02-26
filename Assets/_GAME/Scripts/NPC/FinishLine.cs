using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public static Action onRunnerGameWin;
    public static Action onRunnerGameLose;
    public static Action onPlayerBasket;
    public static Action onNPCBasket;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            onRunnerGameWin?.Invoke();
            Debug.Log("de�di");

            if (SceneManager.GetActiveScene().name == "Tutorial")
                PlayerPrefs.SetInt("Tutorial", 1);

        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            onRunnerGameLose?.Invoke();
            Debug.Log("de�di enemy");

        }
        else if (collision.gameObject.CompareTag("Basketball"))
        {
            onPlayerBasket?.Invoke();
        }
        else if (collision.gameObject.CompareTag("BasketballNPC"))
        {
            onNPCBasket?.Invoke();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
                PlayerPrefs.SetInt("Tutorial", 1);

            Debug.Log("de�di");

            onRunnerGameWin?.Invoke();
        }
        else if (other.gameObject.CompareTag("NPC"))
        {
            Debug.Log("de�di");
        onRunnerGameLose?.Invoke();

        }

        else if (other.gameObject.CompareTag("Basketball"))
        {
            onPlayerBasket?.Invoke();
        }
        else if (other.gameObject.CompareTag("BasketballNPC"))
        {
            onNPCBasket?.Invoke();
        }
    }
}
