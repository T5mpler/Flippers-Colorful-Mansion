using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class Billboard : MonoBehaviour
{
	// Token: 0x06000037 RID: 55 RVA: 0x000033FA File Offset: 0x000015FA
	private void Start()
	{
		this.m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003414 File Offset: 0x00001614
	private void LateUpdate()
	{
		base.transform.LookAt(base.transform.position + this.m_Camera.transform.rotation * Vector3.forward, this.m_Camera.transform.rotation * Vector3.up);
	}

	// Token: 0x04000052 RID: 82
	private Camera m_Camera;
}
