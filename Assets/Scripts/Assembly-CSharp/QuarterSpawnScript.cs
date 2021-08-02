using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class QuarterSpawnScript : MonoBehaviour
{
	// Token: 0x06000147 RID: 327 RVA: 0x0000E192 File Offset: 0x0000C392
	private void Start()
	{
		this.wanderer.QuarterExclusive();
		base.transform.position = this.location.position + Vector3.up * 4f;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Update()
	{
	}

	// Token: 0x040002C9 RID: 713
	public AILocationSelectorScript wanderer;

	// Token: 0x040002CA RID: 714
	public Transform location;
}
