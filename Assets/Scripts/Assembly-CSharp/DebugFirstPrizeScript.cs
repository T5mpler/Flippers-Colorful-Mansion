using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class DebugFirstPrizeScript : MonoBehaviour
{
	// Token: 0x0600006D RID: 109 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Start()
	{
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000460C File Offset: 0x0000280C
	private void Update()
	{
		base.transform.position = this.first.position + new Vector3((float)Mathf.RoundToInt(this.first.forward.x), 0f, (float)Mathf.RoundToInt(this.first.forward.z)) * 3f;
	}

	// Token: 0x0400009B RID: 155
	public Transform player;

	// Token: 0x0400009C RID: 156
	public Transform first;
}
