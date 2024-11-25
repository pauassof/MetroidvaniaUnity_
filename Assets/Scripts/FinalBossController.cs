using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : MonoBehaviour
{

    public enum bossStates { Idle, Rugido, Roll, Spines, Jump, Walk, Death}

    [SerializeField]
    private bossStates state;
    private Animator animator;
    private Rigidbody2D rb;
    public bool waiting;
    private bool isHit;
    private Transform player;
    
    [Header("Stats")]
    [SerializeField]
    private float knockBackForce;
    [SerializeField]
    private float damage;
    public float life;
    public float maxLife;

    [Header("Movimiento")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float stopingDistance;

    [Header("Roar Attack")]
    [SerializeField]
    private Transform roarSpawnPoint;
    [SerializeField]
    private GameObject roarProyectlPrefab;
    [SerializeField]
    private float roarProyectilSpeed;
    [SerializeField]
    private float roarAttackTime;

    [Header("RollAttack")]
    [SerializeField]
    private float rollSpeed;
    private bool haChocado;
    private bool isRolling;

    [Header("SpikeAttack")]
    [SerializeField]
    private GameObject spikePrefab;
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private float spineSpeed;

    [Header("Jump")]
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpSpeed = 1;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        waiting = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        ChangeState(bossStates.Idle);
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

    public void ChangeState(bossStates _state)
    {
        state = _state;
        CheckDirection();
        switch (state)
        {
            case bossStates.Idle:
                StartCoroutine(Idle());
                break;

            case bossStates.Rugido:
                StartCoroutine(Roar());
                break;

            case bossStates.Roll:
                StartCoroutine(Roll());
                break;

            case bossStates.Spines:
                StartCoroutine(Spines());
                break;

            case bossStates.Jump:
                StartCoroutine(Jump());
                break;

            case bossStates.Walk:
                StartCoroutine(Walk());
                break;

            case bossStates.Death: 

                break;

            default:

                break;

        }
    }

    IEnumerator Idle()
    {
        while (waiting == true)
        {
            yield return null;
        }

        ChangeState(bossStates.Walk);

    }

    IEnumerator Walk()
    {
        animator.SetBool("Walk", true);
        Vector2 distanciaVector = player.position - transform.position;
        Vector2 direction = distanciaVector.normalized;
        float distancia = distanciaVector.magnitude;
        while (distancia > stopingDistance)
        {
            transform.Translate(new Vector2 (speed * distanciaVector.x, 0) * Time.deltaTime, Space.World);
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
        //ChangeState((bossStates)azar);
        ChangeState(bossStates.Spines);
    }
    IEnumerator Roar()
    {
        animator.SetTrigger("Roar");

        yield return new WaitForSeconds(roarAttackTime);

        int azar = Random.Range(1, 6);
        ChangeState((bossStates)azar);

    }

    IEnumerator Roll()
    {
        animator.SetTrigger("Roll");
        haChocado = false;
        yield return new WaitForSeconds(1.2f);
        isRolling = true;
        while (haChocado == false)
        {
            transform.Translate(transform.right * -1 * rollSpeed * Time.deltaTime, Space.World);
            //rb.velocity = transform.right * -1 * rollSpeed;
            yield return null;
        }
        isRolling = false;
        yield return new WaitForSeconds(1);
        int azar = Random.Range(1, 6);
        ChangeState((bossStates)azar);

    }

    IEnumerator Spines()
    {
        animator.SetTrigger("Spike");
        gameObject.layer = 7;
        float timePass = 0;
        while (timePass < 4 && isHit == false)
        {
            yield return null;
            timePass += Time.deltaTime;
        }
        animator.SetTrigger("NotTired");
        yield return new WaitForSeconds(1);
        int azar = Random.Range(1, 6);
        ChangeState((bossStates)azar);
    }

    IEnumerator Jump()
    {
        animator.SetBool("Jump",true);
        animator.SetTrigger("TakeOff");
        yield return new WaitForSeconds(0.5f);
        isJumping = true;
        Vector3 playerpos = player.position;
        Vector3 bossPoss = transform.position;
        rb.AddForce(Vector2.up * jumpForce);
        float t = 0;
        while (t<1 || isJumping == true)
        {
            t += Time.deltaTime * jumpSpeed;
            float x = Mathf.Lerp(bossPoss.x, playerpos.x, t);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            yield return null;
        }
        isJumping = false;
        yield return new WaitForSeconds(1);
        int azar = Random.Range(1, 6);
        ChangeState((bossStates)azar);
    }

    public void ShootRoarProyectil()
    {
        GameObject clone = Instantiate(roarProyectlPrefab, roarSpawnPoint.position, roarSpawnPoint.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(clone.transform.right * -1 * roarProyectilSpeed);
    }

    public void StartRoll()
    {
        gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(0.88f, 0.88f);
    }

    public void LaunchSpines()
    {
        for (int i=0; i<spawnPoints.Length; i++)
        {
            GameObject cloneSpike = Instantiate(spikePrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            cloneSpike.GetComponent<Rigidbody2D>().AddForce(cloneSpike.transform.right * -1 * spineSpeed);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRolling == true)
        {
            if (collision.gameObject.tag == "LimitRoll")
            {
                rb.velocity = Vector2.zero;
                gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.23f, 0.88f);
                animator.SetTrigger("Colisionado");
                haChocado = true;
                gameObject.layer = 7;
            }
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                gameObject.layer = 9;
            }
        }
        else if (isJumping == true)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                gameObject.layer = 9;
                AddKnockBackForceToPlayer();
            }
            if (collision.gameObject.tag == "Ground")
            {
                animator.SetBool("Jump", false);
                isJumping = false;
                gameObject.layer = 7;
            }
        }
    }

    public void TakeDamage(float _damage)
    {
        life -= _damage;
        if (life <= 0)
        {
            animator.SetTrigger("Death");
            StopAllCoroutines();
            GetComponent<CapsuleCollider2D>().enabled = false;
            rb.isKinematic = true;
        }
        else
        {
            StartCoroutine(HitAnim());
        }
    }

    void AddKnockBackForceToPlayer()
    {
        player.GetComponent<Rigidbody2D>().AddForce(transform.right * -1 * knockBackForce);
    }

    IEnumerator HitAnim()
    {
        //Color colorInicial = Color.white;
        //Color colorFinal = Color.red;
        float t = 0;
        SpriteRenderer bossSprite = GetComponent<SpriteRenderer>();
        while (t < 1)
        {
            bossSprite.color = Color.Lerp(Color.white, Color.red, t);
            t += Time.deltaTime * 3f;
            yield return null;
        }
        while (t > 0)
        {
            bossSprite.color = Color.Lerp(Color.white, Color.red, t);
            t -= Time.deltaTime * 3f;
            yield return null;
        }

    }
}
