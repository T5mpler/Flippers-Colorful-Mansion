using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class ClickableTest : MonoBehaviour
{
	// Token: 0x06000055 RID: 85 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Start()
	{
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00003F48 File Offset: 0x00002148
	private void Update()
	{
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit) && raycastHit.transform.name == "MathNotebook")
		{
			base.gameObject.SetActive(false);
		}
	}
}
