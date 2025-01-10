using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRune : MonoBehaviour
{
    [SerializeField]
    private string runaName;
    // Start is called before the first frame update
    void Start()
    {
        switch (runaName)
        {
            case "FireRune":
                if (GameManager.instance.gameData.FireRune == true)
                {
                    Destroy(gameObject);
                }
                break;

            case "AirRune":
                if (GameManager.instance.gameData.AirRune > 1)
                {
                    Destroy(gameObject);
                }
                break;

            case "DashRune":
                if (GameManager.instance.gameData.DashRune == true)
                {
                    Destroy(gameObject);
                }

                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            switch (runaName)
            {
                case "FireRune":
                    GameManager.instance.gameData.FireRune = true;
                    Destroy(gameObject);
                    break;

                case "AirRune":
                    GameManager.instance.gameData.AirRune = 2;
                    Destroy(gameObject);
                    break;

                case "DashRune":
                    GameManager.instance.gameData.DashRune = true;
                    Destroy(gameObject);

                    break;
            }
        }
    }
}
