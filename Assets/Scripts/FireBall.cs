using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
        if (collision.gameObject.tag != "Player")
        {
            animator.SetTrigger("Hit");
            Invoke("DestroyFireBall", 0.34f);
        }
    }

    void DestroyFireBall()
    {
        Destroy(gameObject);
    }
}
