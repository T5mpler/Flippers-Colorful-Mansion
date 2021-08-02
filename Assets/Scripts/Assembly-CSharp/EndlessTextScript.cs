using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001F RID: 31
public class EndlessTextScript : MonoBehaviour
{
	// Token: 0x06000082 RID: 130 RVA: 0x00004B6C File Offset: 0x00002D6C
	private void Start()
	{
		this.text.text = "Endless Mode: You seem like then\ndesktop Applications,\nhow many can you get?\n" + PlayerPrefs.GetInt("HighBooks") + " Desktop Applications";
	}

	// Token: 0x040000C2 RID: 194
	public Text text;
}
