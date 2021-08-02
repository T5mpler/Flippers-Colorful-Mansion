using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200000F RID: 15
public class BsodaEffectScript : MonoBehaviour
{
	// Token: 0x0600003D RID: 61 RVA: 0x00003565 File Offset: 0x00001765
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003574 File Offset: 0x00001774
	private void Update()
	{
		if (this.inBsoda)
		{
			this.agent.velocity = this.otherVelocity;
		}
		if (this.failSave > 0f)
		{
			this.failSave -= Time.deltaTime;
			return;
		}
		this.inBsoda = false;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void FixedUpdate()
	{
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000035C4 File Offset: 0x000017C4
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "BSODA")
		{
			this.inBsoda = true;
			this.otherVelocity = other.GetComponent<Rigidbody>().velocity;
			this.failSave = 1f;
			return;
		}
		if (other.transform.name == "Gotta Sweep")
		{
			this.inBsoda = true;
			this.otherVelocity = base.transform.forward * this.agent.speed * 0.1f + other.GetComponent<NavMeshAgent>().velocity;
			this.failSave = 1f;
		}
	}

	// Token: 0x06000041 RID: 65 RVA: 0x0000366B File Offset: 0x0000186B
	private void OnTriggerExit()
	{
		this.inBsoda = false;
	}

	// Token: 0x04000058 RID: 88
	private NavMeshAgent agent;

	// Token: 0x04000059 RID: 89
	private Vector3 otherVelocity;

	// Token: 0x0400005A RID: 90
	private bool inBsoda;

	// Token: 0x0400005B RID: 91
	private float failSave;
}
