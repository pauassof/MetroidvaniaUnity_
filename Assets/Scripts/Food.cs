using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    private float regenHP;
    [SerializeField]
    private float regenMana;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.gameData.Life += regenHP;
            GameManager.instance.gameData.Mana += regenMana;
            levelManager.UpdateLife();
            levelManager.UpdateMana();
            Destroy(gameObject);
        }
    }
}
