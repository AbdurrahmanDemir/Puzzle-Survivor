using CrazyGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyGamesController : MonoBehaviour
{
    private void Awake()
    {
        if (CrazySDK.IsAvailable)
        {
            CrazySDK.Init(() =>
            {
                Debug.Log("CrazySDK initialized");
                CrazySDK.Game.GameplayStart();
            });
        }

        DontDestroyOnLoad(gameObject);

    }

}
