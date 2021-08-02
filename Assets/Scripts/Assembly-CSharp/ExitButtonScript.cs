using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000021 RID: 33
public class ExitButtonScript : MonoBehaviour
{
	// Token: 0x06000087 RID: 135 RVA: 0x00004C23 File Offset: 0x00002E23
	private void Start()
	{
		this.cursorController.UnlockCursor();
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this.ExitGame));
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00004C58 File Offset: 0x00002E58
	private void ExitGame()
	{
		Application.Quit();
	}

	// Token: 0x040000C6 RID: 198
	public CursorControllerScript cursorController;

	// Token: 0x040000C7 RID: 199
	private Button button;
}
