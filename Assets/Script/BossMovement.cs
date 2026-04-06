using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossMovement : MonoBehaviour
{
    public float baseSpeed = 2f;
    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

      
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void MoveTowards(Vector2 target, float speedMultiplier)
    {
        agent.speed = baseSpeed * speedMultiplier;
        agent.SetDestination(target);
    }

    public void Stop()
    {
        agent.ResetPath();
    }

    public void Dash(Vector2 direction, float dashSpeed)
    {
        Vector2 dashTarget = (Vector2)transform.position + direction.normalized * 3f;
        agent.speed = dashSpeed;
        agent.SetDestination(dashTarget);
    }
}