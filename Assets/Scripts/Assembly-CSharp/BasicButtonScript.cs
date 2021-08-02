using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200000C RID: 12
public class BasicButtonScript : MonoBehaviour
{
	// Token: 0x06000034 RID: 52 RVA: 0x000033C2 File Offset: 0x000015C2
	private void Start()
	{
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this.OpenScreen));
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000033EC File Offset: 0x000015EC
	private void OpenScreen()
	{
		this.screen.SetActive(true);
	}

	// Token: 0x04000050 RID: 80
	private Button button;

	// Token: 0x04000051 RID: 81
	public GameObject screen;
}
