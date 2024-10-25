using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyController
{
    [SerializeField]
    private GameObject proyectilPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float force;

    public void ThrowProyectil()
    {
        GameObject clone = Instantiate(proyectilPrefab, spawnPoint.position, spawnPoint.rotation);
        clone.GetComponent<Rigidbody2D>().AddRelativeForce(clone.transform.right*force);
        Destroy(clone, 5);
    }
}
