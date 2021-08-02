using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001B RID: 27
public class DetentionTextScript : MonoBehaviour
{
	// Token: 0x06000070 RID: 112 RVA: 0x00004674 File Offset: 0x00002874
	private void Start()
	{
		this.text = base.GetComponent<Text>();
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00004684 File Offset: 0x00002884
	private void Update()
	{
		if (this.door.lockTime > 0f)
		{
			this.text.text = "So you really got detention\n didn't you Welp, ya gotta wait \n" + Mathf.CeilToInt(this.door.lockTime) + " seconds before you can leave!!!";
			return;
		}
		this.text.text = string.Empty;
	}

	// Token: 0x0400009D RID: 157
	public DoorScript door;

	// Token: 0x0400009E RID: 158
	private Text text;
}
