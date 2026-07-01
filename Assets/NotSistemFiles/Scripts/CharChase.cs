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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelController.Instance.GameOver();
        }
    }

    public void EnterChaseState()
    {
        _currentState = CharState.Chase;
        _animator.SetTrigger("Chase");
        _agent.isStopped = false;
        _agent.SetDestination(player.position);
    }

    public void EnterIdleState()
    {
        _currentState = CharState.Idle;
        _agent.isStopped = true;
        _agent.ResetPath();
        _animator.SetTrigger("Idle");
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
