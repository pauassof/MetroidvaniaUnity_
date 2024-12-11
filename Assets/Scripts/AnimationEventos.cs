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
    public void Step()
    {
        player.PlayStepSound();
    }
    public void ChangeLayer()
    {
        player.ChangeLayer();
    }
}
