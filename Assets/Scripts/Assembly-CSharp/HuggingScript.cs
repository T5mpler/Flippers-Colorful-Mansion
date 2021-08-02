using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200002D RID: 45
public class HuggingScript : MonoBehaviour
{
	// Token: 0x060000D1 RID: 209 RVA: 0x0000854C File Offset: 0x0000674C
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0000855A File Offset: 0x0000675A
	private void Update()
	{
		if (this.failSave > 0f)
		{
			this.failSave -= Time.deltaTime;
			return;
		}
		this.inBsoda = false;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00008583 File Offset: 0x00006783
	private void FixedUpdate()
	{
		if (this.inBsoda)
		{
			this.rb.velocity = this.otherVelocity;
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000085A0 File Offset: 0x000067A0
	private void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "1st Prize")
		{
			this.inBsoda = true;
			this.otherVelocity = this.rb.velocity * 0.1f + other.GetComponent<NavMeshAgent>().velocity;
			this.failSave = 1f;
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00008601 File Offset: 0x00006801
	private void OnTriggerExit()
	{
		this.inBsoda = false;
	}

	// Token: 0x040001A8 RID: 424
	private Rigidbody rb;

	// Token: 0x040001A9 RID: 425
	private Vector3 otherVelocity;

	// Token: 0x040001AA RID: 426
	public bool inBsoda;

	// Token: 0x040001AB RID: 427
	private float failSave;
}
