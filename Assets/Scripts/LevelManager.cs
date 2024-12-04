using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Image lifeBar;
    [SerializeField] 
    private Image manaBar;
    [SerializeField]
    private Transform[] SpawnPoints;
    [SerializeField]
    private AudioClip levelMusic;
    [SerializeField]
    private GameObject panelTactil;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject deathPanel;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = SpawnPoints[GameManager.instance.nextSpawnPoint].position;//GameManager.instance.gameData.PlayerPos;
        GameObject.FindGameObjectWithTag("Player").transform.rotation = SpawnPoints[GameManager.instance.nextSpawnPoint].rotation;
        UpdateLife();
        UpdateMana();
        if (levelMusic != null)
        {
            AudioManager.instance.PlayMusic(levelMusic, 1);
        }
#if UNITY_ANDROID
        panelTactil.SetActive(true);
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public void DeathPanel()
    {
        deathPanel.SetActive(true);
    }

    public void PauseMenu()
    {
        if (pausePanel.activeInHierarchy == true)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Restart(int ranura)
    {
        ranura = GameManager.instance.gameData.Ranura;
        if (PlayerPrefs.HasKey("gameData" + ranura.ToString()))
        {
            GameManager.instance.LoadData("gameData" + ranura.ToString());
            SceneManager.LoadScene(GameManager.instance.gameData.CurrentScene);
        }
        else
        {
            GameManager.instance.LoadData("Pepe");
            GameManager.instance.gameData.Ranura = ranura;
            SceneManager.LoadScene(1);
        }
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
