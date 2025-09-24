using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Spawn timing  y = m*x + b")]
    public int m = 3;
    public int b = 2;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public float enemySpeed = 6f;

    [Header("Player")]
    public float moveSpeed = 10f;

    [Header("Fake gravity (demo)")]
    public float fakeGravity = -10f;
}
