using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000015 RID: 21
public class ClockTextScript : MonoBehaviour
{
	// Token: 0x06000058 RID: 88 RVA: 0x00003F9C File Offset: 0x0000219C
	private void Update()
	{
		this.clockText.text = string.Concat(new object[]
		{
			DateTime.Now.Hour,
			":",
			DateTime.Now.Minute,
			":",
			DateTime.Now.Second
		});
		this.dateText.text = string.Concat(new object[]
		{
			DateTime.Now.Month,
			"/",
			DateTime.Now.Day,
			"/",
			DateTime.Now.Year
		});
	}

	// Token: 0x0400007F RID: 127
	public Text clockText;

	// Token: 0x04000080 RID: 128
	public Text dateText;
}
