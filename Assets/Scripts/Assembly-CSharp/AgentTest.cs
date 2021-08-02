using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000004 RID: 4
public class AgentTest : MonoBehaviour
{
	// Token: 0x06000009 RID: 9 RVA: 0x000028B2 File Offset: 0x00000AB2
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.Wander();
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000028C6 File Offset: 0x00000AC6
	private void Update()
	{
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000028F0 File Offset: 0x00000AF0
	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity, 3, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player")
		{
			this.db = true;
			this.TargetPlayer();
			return;
		}
		this.db = false;
		if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
		{
			this.Wander();
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002997 File Offset: 0x00000B97
	private void Wander()
	{
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		this.coolDown = 1f;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000029C6 File Offset: 0x00000BC6
	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

	// Token: 0x0400000D RID: 13
	public bool db;

	// Token: 0x0400000E RID: 14
	public Transform player;

	// Token: 0x0400000F RID: 15
	public Transform wanderTarget;

	// Token: 0x04000010 RID: 16
	public AILocationSelectorScript wanderer;

	// Token: 0x04000011 RID: 17
	public float coolDown;

	// Token: 0x04000012 RID: 18
	private NavMeshAgent agent;
}
