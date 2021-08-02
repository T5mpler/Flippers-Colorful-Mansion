using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000029 RID: 41
public class FPSCounterScript : MonoBehaviour
{
	// Token: 0x060000A5 RID: 165 RVA: 0x00005954 File Offset: 0x00003B54
	public void Update()
	{
		if (Time.deltaTime != 0f)
		{
			this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
			float f = 1f / this.deltaTime;
			this.fpsText.text = "FPS: " + Mathf.Ceil(f).ToString() + " FPS";
		}
	}

	// Token: 0x040000FD RID: 253
	public Text fpsText;

	// Token: 0x040000FE RID: 254
	public float deltaTime;
}
