using System;
using Unity.Cinemachine;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    
    [Header("Spawn Points Objects")]
    [Tooltip("Spawn point for Player (KittyPrefab)")]
    public Transform spawnPointPlayer;
    [Tooltip("Spawn point for CharChase")]
    public Transform spawnPointChaseChar;
    
    [Header("Prefabs")]
    [Tooltip("Prefab for Player")]
    public GameObject playerPrefab;
    [Tooltip("Prefab for CharChase")]
    public GameObject charChasePrefab;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Instantiate(playerPrefab, spawnPointChaseChar.position, spawnPointChaseChar.rotation);
        cinemachineCamera.Follow = playerPrefab.transform;
        Instantiate(charChasePrefab, spawnPointPlayer.position, spawnPointPlayer.rotation);
    }
}
