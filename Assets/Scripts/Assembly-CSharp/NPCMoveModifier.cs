using UnityEngine;
using UnityEngine.AI;

public class NPCMoveModifier : MonoBehaviour
{
    NavMeshAgent agent;
    ActivityModifierScript activityModifier;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        activityModifier = GetComponent<ActivityModifierScript>();
    }
    private void Update()
    {
        Vector3 totalModifier = Vector3.zero;
        totalModifier += activityModifier.TotalAdder;
        totalModifier *= activityModifier.TotalMultipler;
        if (totalModifier.magnitude > 0f)
        {
            agent.speed *= totalModifier.magnitude;
            agent.velocity += totalModifier;
        }
    }
}