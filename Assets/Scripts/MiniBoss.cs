using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{

    public enum semiBossStates { Idle, Attack, Spell, Walk, Death }

    [SerializeField]
    private semiBossStates state;
    [SerializeField]
    private GameObject fireRune;
    private Animator animator;
    private Rigidbody2D rb;
    public bool waiting2;
    private Transform player;

    [Header("Stats")]
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
    private GameObject clone;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
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

            case semiBossStates.Attack:
                StartCoroutine(Attack());
                break;

            case semiBossStates.Spell:
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
        int azar = Random.Range(1, 4);
        //ChangeStates((semiBossStates)azar);
        ChangeStates(semiBossStates.Spell);
    }
    IEnumerator Attack()
    {
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(3);

        int azar = Random.Range(1, 4);
        ChangeStates((semiBossStates)azar);

    }

    IEnumerator Spell()
    {
        animator.SetTrigger("Spell");
        yield return new WaitForSeconds(2.5f);
        clone = Instantiate(spellPrefab, player.position + new Vector3(0,(11.56f-8.15f),0), transform.rotation);
        clone.GetComponent<AnimationEventos>().miniBoss = this;
        yield return new WaitForSeconds(3);
        int azar = Random.Range(1, 4);
        ChangeStates((semiBossStates)azar);
    }

    public void EndSpell()
    {
        Destroy(clone);
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
            if (GameManager.instance.gameData.FireRune == false)
            {
                GameObject runeClone = Instantiate(fireRune, transform.position, transform.rotation);
                runeClone.SetActive(true);
            }
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }
    public void AnimationDeath()
    {
        Destroy(this.gameObject);
    }

}
