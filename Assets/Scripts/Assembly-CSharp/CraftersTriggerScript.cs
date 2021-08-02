using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class CraftersTriggerScript : MonoBehaviour
{
	// Token: 0x06000065 RID: 101 RVA: 0x00004599 File Offset: 0x00002799
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.crafters.GiveLocation(this.goTarget.position, false);
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x000045C4 File Offset: 0x000027C4
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			this.crafters.GiveLocation(this.fleeTarget.position, true);
		}
	}

	// Token: 0x04000098 RID: 152
	public Transform goTarget;

	// Token: 0x04000099 RID: 153
	public Transform fleeTarget;

	// Token: 0x0400009A RID: 154
	public CraftersScript crafters;
}
