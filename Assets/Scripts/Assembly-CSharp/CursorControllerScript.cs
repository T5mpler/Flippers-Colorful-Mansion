using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class CursorControllerScript : MonoBehaviour
{
	// Token: 0x06000068 RID: 104 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Start()
	{
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Update()
	{
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000045EF File Offset: 0x000027EF
	public void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000045FD File Offset: 0x000027FD
	public void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
