using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200000A RID: 10
public class BackButtonScript : MonoBehaviour
{
	// Token: 0x06000023 RID: 35 RVA: 0x00002D37 File Offset: 0x00000F37
	private void Start()
	{
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this.CloseScreen));
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002D61 File Offset: 0x00000F61
	private void CloseScreen()
	{
		this.screen.SetActive(false);
	}

	// Token: 0x04000022 RID: 34
	private Button button;

	// Token: 0x04000023 RID: 35
	public GameObject screen;
}
