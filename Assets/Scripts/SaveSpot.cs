using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSpot : MonoBehaviour
{
    private Animator animator;
    bool estaDentro;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            animator.SetBool("Save", true);
            estaDentro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            animator.SetBool("Save", false);
            estaDentro = false;
        }
    }

    private void Update()
    {
        if (estaDentro)
        {
            if (Input.GetAxis("Vertical") >= 0.5f)
            {
                GameManager.instance.SaveData(GameManager.instance.gameData.Ranura);
                animator.SetBool("Save", false);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

}
