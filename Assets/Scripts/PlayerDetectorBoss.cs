using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectorBoss : MonoBehaviour
{
    [SerializeField]
    private FinalBossController finalBoss;
    [SerializeField]
    private GameObject muros;

    private void Update()
    {
        if (finalBoss.life <= 0)
        {
            muros.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            finalBoss.waiting = false;
            muros.gameObject.SetActive(true);
        }
    }
}
