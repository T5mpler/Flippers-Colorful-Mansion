using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class CameraFOVScript : MonoBehaviour
{
	// Token: 0x0600004D RID: 77 RVA: 0x00003B32 File Offset: 0x00001D32
	private void Start()
	{
		this.targetFOV = this.playerCamera.fieldOfView;
		this.fov = this.targetFOV;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003B54 File Offset: 0x00001D54
	private void Update()
	{
		float num = 4f;
		this.fov = Mathf.Lerp(this.fov, this.targetFOV, Time.deltaTime * num);
		this.playerCamera.fieldOfView = this.fov;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003B96 File Offset: 0x00001D96
	public void SetCameraFOV(float targetfov)
	{
		this.targetFOV = targetfov;
	}

	// Token: 0x0400006D RID: 109
	public Camera playerCamera;

	// Token: 0x0400006E RID: 110
	public float targetFOV;

	// Token: 0x0400006F RID: 111
	public float fov;
}
