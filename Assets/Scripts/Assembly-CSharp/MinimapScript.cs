using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class MinimapScript : MonoBehaviour
{
	// Token: 0x0600010D RID: 269 RVA: 0x0000B6E8 File Offset: 0x000098E8
	private void LateUpdate()
	{
		Vector3 position = this.player.position;
		position.y = base.transform.position.y;
		base.transform.position = position;
	}

	// Token: 0x0400023A RID: 570
	public Transform player;
}
