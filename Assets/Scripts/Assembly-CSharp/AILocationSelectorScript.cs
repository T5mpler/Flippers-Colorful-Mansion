using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class AILocationSelectorScript : MonoBehaviour
{
	// Token: 0x0600000F RID: 15 RVA: 0x000029F4 File Offset: 0x00000BF4
	public void GetNewTarget()
	{
		this.id = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 32f));
		base.transform.position = this.newLocation[this.id].position;
		this.ambience.PlayAudio();
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002A44 File Offset: 0x00000C44
	public void GetNewTargetHallway()
	{
		this.id = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 16f));
		base.transform.position = this.newLocation[this.id].position;
		this.ambience.PlayAudio();
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002A93 File Offset: 0x00000C93
	public void QuarterExclusive()
	{
		this.id = Mathf.RoundToInt(UnityEngine.Random.Range(1f, 15f));
		base.transform.position = this.newLocation[this.id].position;
	}

	// Token: 0x04000013 RID: 19
	public Transform[] newLocation = new Transform[33];

	// Token: 0x04000014 RID: 20
	public AmbienceScript ambience;

	// Token: 0x04000015 RID: 21
	private int id;
}
