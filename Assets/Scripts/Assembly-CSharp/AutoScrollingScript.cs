using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class AutoScrollingScript : MonoBehaviour
{
	// Token: 0x06000021 RID: 33 RVA: 0x00002D00 File Offset: 0x00000F00
	private void Update()
	{
		base.transform.position += new Vector3(0f, 1f) * (Time.deltaTime * 30f);
	}
}
