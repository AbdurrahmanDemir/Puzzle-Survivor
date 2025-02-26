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
            Debug.Log("deðdi");

            if (SceneManager.GetActiveScene().name == "Tutorial")
                PlayerPrefs.SetInt("Tutorial", 1);

        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            onRunnerGameLose?.Invoke();
            Debug.Log("deðdi enemy");

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

            Debug.Log("deðdi");

            onRunnerGameWin?.Invoke();
        }
        else if (other.gameObject.CompareTag("NPC"))
        {
            Debug.Log("deðdi");
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
