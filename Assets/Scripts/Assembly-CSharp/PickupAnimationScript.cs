using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class PickupAnimationScript : MonoBehaviour
{
	// Token: 0x06000117 RID: 279 RVA: 0x0000B9AE File Offset: 0x00009BAE
	private void Start()
	{
		this.itemPosition = base.GetComponent<Transform>();
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000B9BC File Offset: 0x00009BBC
	private void Update()
	{
		this.itemPosition.localPosition = new Vector3(0f, Mathf.Sin((float)Time.frameCount * 0.0174532924f) / 2f + 1f, 0f);
	}

	// Token: 0x04000246 RID: 582
	private Transform itemPosition;
}
