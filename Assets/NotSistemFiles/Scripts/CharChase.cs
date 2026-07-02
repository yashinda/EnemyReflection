using System;
using UnityEngine;
using UnityEngine.AI;

public enum CharState
{
    Idle,
    Chase
}

public class CharChase : MonoBehaviour
{
    private CharState _currentState = CharState.Idle;
    public Transform spawnPoint;
    public Transform player;
    private Animator _animator;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        EnterIdleState();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelController.Instance.GameOver();
        }
    }

    private void Update()
    {
        if (_currentState == CharState.Chase)
        {
            _agent.SetDestination(player.position);
        }
    }

    public void EnterChaseState()
    {
        _currentState = CharState.Chase;
        _animator.SetBool("Idle", false);
        _animator.SetTrigger("Run");
        _agent.isStopped = false;
        _agent.SetDestination(player.position);
    }

    public void EnterIdleState()
    {
        _currentState = CharState.Idle;
        _agent.isStopped = true;
        _agent.ResetPath();

        _agent.Warp(spawnPoint.position);
        transform.rotation = spawnPoint.rotation;
        _animator.SetBool("Idle", true);
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
