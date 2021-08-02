using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000025 RID: 37
public class FarmModeTextScript : MonoBehaviour
{
	// Token: 0x06000094 RID: 148 RVA: 0x000050CC File Offset: 0x000032CC
	private void Start()
	{
		this.farmText.color = Color.green;
		Text text = this.farmText;
		text.text = string.Concat(new object[]
		{
			text.text,
			"\nBest Time:\n",
			PlayerPrefs.GetFloat("FinishedTime"),
			" seconds"
		});
	}

	// Token: 0x040000D9 RID: 217
	public Text farmText;
}
