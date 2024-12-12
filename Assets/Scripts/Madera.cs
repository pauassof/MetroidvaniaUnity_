using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Madera : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.instance.gameData.FireRune == true)
                {
                    Destroy(gameObject);
                }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fireball")
        {
            GameManager.instance.gameData.Madera = true;
            Destroy(gameObject);
        }
    }
}
