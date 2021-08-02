using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004C RID: 76
public class TipScreenScript : MonoBehaviour
{
	// Token: 0x0600017F RID: 383 RVA: 0x0000F278 File Offset: 0x0000D478
	private void Awake()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, 3f));
		this.tipText.fontSize = 30;
		this.tipText.text = this.tipTexts[num];
	}

	// Token: 0x04000314 RID: 788
	public Text tipText;

	// Token: 0x04000315 RID: 789
	private string[] tipTexts = new string[]
	{
		"If there's a slope-intercept question then you need to use this (y2 - y1) / (x2 - x1)",
		"Square root questions mean 'What number can I multiply twice to get that number'",
		"If you have a trivia Question then make sure your letters are capitalized to be valid",
		"Some trivia questions have more than one answer! Make sure you read it carefully"
	};
}
