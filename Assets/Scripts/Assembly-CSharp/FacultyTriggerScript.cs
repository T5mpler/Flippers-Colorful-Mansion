using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class FacultyTriggerScript : MonoBehaviour
{
	// Token: 0x0600008C RID: 140 RVA: 0x00004CB6 File Offset: 0x00002EB6
	private void Start()
	{
		this.hitBox = base.GetComponent<BoxCollider>();
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Update()
	{
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00004CC4 File Offset: 0x00002EC4
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.ps.ResetGuilt("faculty", 1f);
		}
	}

	// Token: 0x040000C9 RID: 201
	public PlayerScript ps;

	// Token: 0x040000CA RID: 202
	private BoxCollider hitBox;
}
