using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000043 RID: 67
public class ScoreScript : MonoBehaviour
{
	// Token: 0x06000152 RID: 338 RVA: 0x0000E3B8 File Offset: 0x0000C5B8
	private void Start()
	{
		if (PlayerPrefs.GetString("CurrentMode") == "endless")
		{
			this.scoreText.SetActive(true);
			this.text.text = "Score:\n" + PlayerPrefs.GetInt("CurrentBooks") + " Notebooks";
		}
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Update()
	{
	}

	// Token: 0x040002D5 RID: 725
	public GameObject scoreText;

	// Token: 0x040002D6 RID: 726
	public Text text;
}
