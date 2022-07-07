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
    private float targetDist;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerManager>();
        nma = GetComponent<NavMeshAgent>();
        vs = VersusSceneManager.instance;
    }

    private void FindClosestEnemy()
    {
        var t = vs.playerList.Where(c => c.tag != tag).OrderBy(c => Vector3.Distance(c.transform.position, transform.position)).FirstOrDefault<GameObject>(c => c);
        Debug.Log(t);
        if (t != null)
        {
            target = t;
            targetDist = (target.transform.position - transform.position).magnitude;
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
        FindClosestEnemy();
        // Attempt to get NavMesh agent working with this
        // Debug.Log("BUST");
        // nma.SetDestination(new Vector3(0f, 0f, 0f));

        // THE OMEGA BASIC AI
        player.isSpecial1Triggered = true;
        if (targetDist < 1)
        {
            player.isLightAttackTriggered = true;
        }

        else
        {
            MoveTowardsTarget();
        }


    }
}
