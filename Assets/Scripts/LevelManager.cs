using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Image lifeBar;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = GameManager.instance.gameData.PlayerPos;
        UpdateLife();
    }
    public void UpdateLife()
    {
        lifeBar.fillAmount = GameManager.instance.gameData.Life/GameManager.instance.gameData.MaxLife;
    }
}
