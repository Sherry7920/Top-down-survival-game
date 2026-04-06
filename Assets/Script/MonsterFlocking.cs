using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterFlocking : MonoBehaviour
{
    public static List<MonsterFlocking> allMonsters =
        new List<MonsterFlocking>();

    [Header("Flocking")]
    public float separationRadius = 1.2f;
    public float separationStrength = 1.5f;
    public float encircleStrength = 1.2f;
    public LayerMask monsterLayer;

    [Header("Tactical")]
    public int maxAttackers = 4;
    public float orbitRadius = 2.5f;

    Rigidbody2D rb;
    Transform target;
    MonsterFSM fsm;

    // Determines if this specific monster prefers to circle clockwise or counter-clockwise
    float sideBias;

    public Vector2 CurrentDirection { get; private set; }
    public bool IsOrbiting { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fsm = GetComponent<MonsterFSM>();
        sideBias = Random.value > 0.5f ? 1f : -1f;

        allMonsters.Add(this);
    }

    void OnDestroy()
    {
        allMonsters.Remove(this);
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player)
            target = player.transform;
    }

    public void CalculateChaseDirection()
    {
        if (!target)
        {
            CurrentDirection = Vector2.zero;
            return;
        }

        //If there's an available attack slot, move in. Otherwise, stay in orbit.
        if (CanEnterAttack())
        {
            IsOrbiting = false;
            CalculateCloseCombat();
        }
        else
        {
            IsOrbiting = true;
            CalculateOrbit();
        }
    }

    bool CanEnterAttack()
    {
        int attackers = 0;

        foreach (var m in allMonsters)
        {
            if (m == null) continue;

            if (m.fsm.CurrentState ==
                MonsterFSM.State.Attack)
            {
                attackers++;
            }
        }

        return attackers < maxAttackers;
    }

    void CalculateCloseCombat()
    {
        Vector2 toPlayer =
            ((Vector2)target.position - rb.position).normalized;

        Vector2 separation = CalculateSeparation();

        Vector2 perpendicular =
            new Vector2(-toPlayer.y, toPlayer.x) * sideBias;

        Vector2 finalDir =
            toPlayer
          + separation * separationStrength
          + perpendicular * encircleStrength;

        CurrentDirection = finalDir.normalized;
    }

    void CalculateOrbit()
    {
        int index = allMonsters.IndexOf(this);

        float angle =
            index * Mathf.PI * 2f / allMonsters.Count;

        Vector2 orbitPos =
            (Vector2)target.position +
            new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            ) * orbitRadius;

        CurrentDirection =
            (orbitPos - rb.position).normalized;
    }

    Vector2 CalculateSeparation()
    {
        Collider2D[] neighbors =
            Physics2D.OverlapCircleAll(
                transform.position,
                separationRadius,
                monsterLayer
            );

        Vector2 force = Vector2.zero;
        int count = 0;

        foreach (Collider2D col in neighbors)
        {
            if (col.gameObject == gameObject) continue;

            Vector2 away =
                rb.position -
                (Vector2)col.transform.position;

            float dist = away.magnitude;

            if (dist > 0.01f)
            {
                force += away.normalized / dist;
                count++;
            }
        }

        if (count > 0)
            force /= count;

        return force.normalized;
    }
}