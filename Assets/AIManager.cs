using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    private PlayerManager player;
    private VersusSceneManager vs;
    private GameObject target;
    private NavMeshAgent nma;
    private float timeSinceStartedMoving = 0f;
    private float checkLocationInterval = 0.5f;
    private float checkLocationTimer;
    private Vector3 prevLocation;
    private float targetDist;
    private float targetXZDist;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerManager>();
        nma = GetComponent<NavMeshAgent>();
        vs = VersusSceneManager.instance;
        prevLocation = transform.position;
        checkLocationTimer = checkLocationInterval;
    }

    private void FindClosestEnemy()
    {
        var t = vs.playerList.Where(c => c.tag != tag).OrderBy(c => Vector3.Distance(c.transform.position, transform.position)).FirstOrDefault<GameObject>(c => c);
        if (t != null)
        {
            target = t;
            Vector3 d = target.transform.position - transform.position;
            targetDist = d.magnitude;
            d.y = 0;
            targetXZDist = d.magnitude;

        }

    }

    private void MoveTowardsTarget()
    {
        Vector2 targetXZ = new Vector2(target.transform.position.x, target.transform.position.z);
        Vector2 meXZ = new Vector2(transform.position.x, transform.position.z);

        player.inputMovement = (targetXZ - meXZ).normalized;
        player.isMovePressed = true;

    }

    // Update is called once per frame
    void Update()
    {
        player.isMovePressed = false;
        if(player.currentState == player.Special1State) return;

        FindClosestEnemy();
        // Attempt to get NavMesh agent working with this
        // Debug.Log("BUST");
        // nma.SetDestination(new Vector3(0f, 0f, 0f));

        // THE OMEGA BASIC AI
        if(player.stats.mana == player.stats.baseStats.baseMana)
        {
            player.isSpecial1Triggered = true;
        }

        else if (targetDist < 1)
        {
            player.isLightAttackTriggered = true;
            timeSinceStartedMoving = 0f;
            checkLocationTimer = checkLocationInterval;
        }

        else if (targetXZDist > 1)
        {
            timeSinceStartedMoving += Time.deltaTime;
            checkLocationTimer -= Time.deltaTime;
            if (checkLocationTimer <= 0)
            {
                checkLocationTimer = checkLocationInterval;
                if ((transform.position - prevLocation).magnitude < 1f)
                {
                    // maybe we're stuck so jump
                    player.isJumpTriggered = true;

                }
                prevLocation = transform.position;
            }
            MoveTowardsTarget();
        }


    }
}
