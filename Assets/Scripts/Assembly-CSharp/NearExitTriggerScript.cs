using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class NearExitTriggerScript : MonoBehaviour
{
	// Token: 0x06000112 RID: 274 RVA: 0x0000B778 File Offset: 0x00009978
	private void OnTriggerEnter(Collider other)
	{
		if (this.gc.exitsReached < 3 & this.gc.finaleMode & other.tag == "Player")
		{
			this.gc.ExitReached();
			this.es.Lower();
			this.gc.baldiScrpt.Hear(base.transform.position, 5f);
		}
	}

	// Token: 0x0400023C RID: 572
	public GameControllerScript gc;

	// Token: 0x0400023D RID: 573
	public EntranceScript es;
}
