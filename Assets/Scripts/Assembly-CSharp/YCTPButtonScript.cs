using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000050 RID: 80
public class YCTPButtonScript : MonoBehaviour
{
	// Token: 0x06000196 RID: 406 RVA: 0x0000FC45 File Offset: 0x0000DE45
	private void Start()
	{
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this.AddString));
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000FC6F File Offset: 0x0000DE6F
	public void AddString()
	{
		InputField playerAnswer = this.mathGame.playerAnswer;
		playerAnswer.text += this.text;
	}

	// Token: 0x04000346 RID: 838
	public Button button;

	// Token: 0x04000347 RID: 839
	public MathGameScript mathGame;

	// Token: 0x04000348 RID: 840
	public string text;
}
