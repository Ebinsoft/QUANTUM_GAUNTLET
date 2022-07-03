using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIManager : MonoBehaviour
{
    private PlayerManager player;
    private VersusSceneManager vs;
    private GameObject target;
    private float targetDist;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerManager>();
        vs = VersusSceneManager.instance;
    }

    private void FindClosestEnemy()
    {
        target = vs.playerList.Where(c => c.tag != tag).OrderBy(c => Vector3.Distance(c.transform.position, transform.position)).First(c => c);
        targetDist = (target.transform.position - transform.position).magnitude;
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
