using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField]
    private float life;
    [SerializeField]
    private float maxLife;
    [SerializeField]
    private int currentScene;
    [SerializeField]
    private Vector3 playerPos;
    [SerializeField]
    private int ranura;

    public float Life
    {
        get { return life; }
        set { life = value; }
    }
    public float MaxLife
    {
        get { return maxLife; }
        set { maxLife = value; }
    }
    public int CurrentScene
    {
        get { return currentScene; }
        set { currentScene = value; }
    }
    public Vector3 PlayerPos
    {
        get { return playerPos; }
        set { playerPos = value; }
    }
    public int Ranura
    {
        get { return ranura; }
        set { ranura = value; }
    }
}
