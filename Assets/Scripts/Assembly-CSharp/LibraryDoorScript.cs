using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class LibraryDoorScript : MonoBehaviour
{
	// Token: 0x060000F8 RID: 248 RVA: 0x00009580 File Offset: 0x00007780
	private void Update()
	{
		if (DateTime.Now.Day != 1 & DateTime.Now.Day != 7 & DateTime.Now.Day != 4)
		{
			if (DateTime.Now.Hour > 9 & DateTime.Now.Hour <= 20)
			{
				this.isLibraryClosed = false;
			}
			else
			{
				this.isLibraryClosed = true;
				this.inside.sharedMaterial = this.libraryClosed;
			}
		}
		else if (DateTime.Now.Day == 4)
		{
			if (DateTime.Now.Hour > 8 || DateTime.Now.Hour <= 20)
			{
				this.isLibraryClosed = false;
			}
			else
			{
				this.isLibraryClosed = true;
				this.inside.sharedMaterial = this.libraryClosed;
			}
		}
		else if (DateTime.Now.Day == 1 || DateTime.Now.Day == 7)
		{
			if (DateTime.Now.Hour > 11 & DateTime.Now.Hour <= 20)
			{
				this.isLibraryClosed = false;
			}
			else
			{
				this.isLibraryClosed = true;
				this.inside.sharedMaterial = this.libraryClosed;
			}
		}
		if (this.isLibraryClosed)
		{
			this.invisibleBarrier.enabled = true;
			this.barrier.enabled = true;
			this.outside.GetComponent<DoorScript>().enabled = false;
			return;
		}
		this.outside.GetComponent<DoorScript>().enabled = true;
	}

	// Token: 0x040001F4 RID: 500
	public bool isLibraryClosed;

	// Token: 0x040001F5 RID: 501
	public Material libraryClosed;

	// Token: 0x040001F6 RID: 502
	public Material libraryNormal;

	// Token: 0x040001F7 RID: 503
	public MeshRenderer inside;

	// Token: 0x040001F8 RID: 504
	public MeshCollider invisibleBarrier;

	// Token: 0x040001F9 RID: 505
	public MeshCollider barrier;

	// Token: 0x040001FA RID: 506
	public GameObject outside;

	// Token: 0x040001FB RID: 507
	public GameObject insideDoor;
}
