using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class EntranceScript : MonoBehaviour
{
	// Token: 0x06000084 RID: 132 RVA: 0x00004B98 File Offset: 0x00002D98
	public void Lower()
	{
		base.transform.position = base.transform.position - new Vector3(0f, 10f, 0f);
		if (this.gc.finaleMode)
		{
			this.wall.material = this.map;
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00004BF2 File Offset: 0x00002DF2
	public void Raise()
	{
		base.transform.position = base.transform.position + new Vector3(0f, 10f, 0f);
	}

	// Token: 0x040000C3 RID: 195
	public GameControllerScript gc;

	// Token: 0x040000C4 RID: 196
	public Material map;

	// Token: 0x040000C5 RID: 197
	public MeshRenderer wall;
}
