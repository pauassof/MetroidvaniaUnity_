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
        UpdateLife();
    }
    public void UpdateLife()
    {
        lifeBar.fillAmount = GameManager.instance.life/GameManager.instance.maxLife;
    }
}
