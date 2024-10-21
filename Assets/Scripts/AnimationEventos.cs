using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventos : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    public void FinishAttack()
    {
        player.isAttacking = false;
    }
}
