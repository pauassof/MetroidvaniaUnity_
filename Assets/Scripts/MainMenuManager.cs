using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelPartidas;
    [SerializeField]
    private AudioClip menuMusic;
    [SerializeField]
    private AudioClip clickSFX;

    private void Start()
    {
        AudioManager.instance.PlayMusic(menuMusic, 1);
    }
    public void PlayButton()
    {
        AudioManager.instance.PlaySFX(clickSFX, 1);
        panelPartidas.SetActive(true);
        panelPartidas.transform.GetChild(1).GetComponent<Button>().Select();
        RevisarPartidas();
    }

    public void ExitButton() 
    {
        AudioManager.instance.PlaySFX(clickSFX, 1);
        Application.Quit();
    }

    public void BackButton()
    {
        AudioManager.instance.PlaySFX(clickSFX, 1);
        panelPartidas.SetActive(false);
        GameObject.Find("ButtonPlay").GetComponent<Button>().Select();
    }

    void RevisarPartidas()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (PlayerPrefs.HasKey("gameData" + i.ToString()))
            {
                GameManager.instance.LoadData("gameData"+i.ToString());

                panelPartidas.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    "Partida " + i.ToString() + "\nVida: " + GameManager.instance.gameData.Life + "/" + GameManager.instance.gameData.MaxLife;
            }
            else
            {
                panelPartidas.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text ="Vacio";
            }
        }
    }
    
    public void StartGame(int ranura)
    {
        AudioManager.instance.PlaySFX(clickSFX, 1);
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
}
