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

                break;

            case "EarthRune":

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

                    break;

                case "EarthRune":

                    break;
            }
        }
    }
}
