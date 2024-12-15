using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{

    public enum semiBossStates { Idle, Attack, Spell, Walk, Death }

    [SerializeField]
    private bossStates state;
    private Animator animator;
    private Rigidbody2D rb;
    public bool waiting2;
    private bool isHit;
    private Transform player;

    [Header("Stats")]
    [SerializeField]
    private float knockBackForce;
    [SerializeField]
    private float damage;
    public float life2;
    public float maxLife2;

    [Header("Movimiento")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float stopingDistance;

    [Header("Spell")]
    [SerializeField]
    private GameObject spellPrefab;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        waiting2 = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        ChangeStates(semiBossStates.Idle);
    }

    void CheckDirection()
    {
        Vector2 distanciaVector = player.position - transform.position;
        Vector2 direction = distanciaVector.normalized;

        if (direction.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction.x < 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
    }

    public void ChangeStates(semiBossStates _state)
    {
        state = _state;
        CheckDirection();
        switch (state)
        {
            case semiBossStates.Idle:
                StartCoroutine(Idle());
                break;

            case semiBossStates.Rugido:
                StartCoroutine(Attack());
                break;

            case semiBossStates.Spines:
                StartCoroutine(Spell());
                break;

            case semiBossStates.Walk:
                StartCoroutine(Walk());
                break;

            case semiBossStates.Death:

                break;

            default:

                break;

        }
    }

    IEnumerator Idle()
    {
        while (waiting2 == true)
        {
            yield return null;
        }

        ChangeStates(semiBossStates.Walk);

    }

    IEnumerator Walk()
    {
        animator.SetBool("Walk", true);
        Vector2 distanciaVector = player.position - transform.position;
        Vector2 direction = distanciaVector.normalized;
        float distancia = distanciaVector.magnitude;
        while (distancia > stopingDistance)
        {
            transform.Translate(new Vector2(speed * distanciaVector.x, 0) * Time.deltaTime, Space.World);
            //rb.velocity = new Vector2(speed * distanciaVector.x, 0);
            CheckDirection();
            distanciaVector = player.position - transform.position;
            direction = distanciaVector.normalized;
            distancia = distanciaVector.magnitude;
            yield return null;
        }
        rb.velocity = Vector2.zero;
        animator.SetBool("Walk", false);
        int azar = Random.Range(1, 5);
        ChangeStates((semiBossStates)azar);
    }
    IEnumerator Attack()
    {
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1f);

        int azar = Random.Range(1, 4);
        ChangeStates((semiBossStates)azar);

    }

    IEnumerator Spell()
    {
        animator.SetTrigger("Spell");
        yield return new WaitForSeconds(1);
        int azar = Random.Range(1, 4);
        ChangeStates((semiBossStates)azar);
    }



    public void TakeDamage(float _damage)
    {
        life2 -= _damage;
        if (life2 <= 0)
        {
            animator.SetTrigger("Death");
            StopAllCoroutines();
            GetComponent<CapsuleCollider2D>().enabled = false;
            rb.isKinematic = true;
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }
}
