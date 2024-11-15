using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Image lifeBar;
    [SerializeField] 
    private Image manaBar;
    [SerializeField]
    private Transform[] SpawnPoints;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = SpawnPoints[GameManager.instance.nextSpawnPoint].position;//GameManager.instance.gameData.PlayerPos;
        GameObject.FindGameObjectWithTag("Player").transform.rotation = SpawnPoints[GameManager.instance.nextSpawnPoint].rotation;
        UpdateLife();
        UpdateMana();
    }
    public void UpdateLife()
    {
        lifeBar.fillAmount = GameManager.instance.gameData.Life/GameManager.instance.gameData.MaxLife;
    }

    public void UpdateMana()
    {
        manaBar.fillAmount = GameManager.instance.gameData.Mana / GameManager.instance.gameData.MaxMana;
    }
}
