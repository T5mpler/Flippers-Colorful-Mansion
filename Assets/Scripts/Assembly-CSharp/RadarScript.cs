using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000041 RID: 65
public class RadarScript : MonoBehaviour
{
	// Token: 0x0600014A RID: 330 RVA: 0x0000E1C9 File Offset: 0x0000C3C9
	public void Start()
	{
		this.cooldown = 15f;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
	public void Update()
	{
		if (this.cooldown > 0f)
		{
			this.cooldown -= Time.deltaTime;
		}
		if (this.cooldown > 0f)
		{
			this.radarText.text = string.Concat(new object[]
			{
				"Toggle on and off with R",
				"\nour position: " + this.characterTransforms[0].position,
				"\nBaldi position: " + this.characterTransforms[1].position,
				"\nPlaytime Position: " + this.characterTransforms[2].position,
				"\nPrincipal Position: " + this.characterTransforms[3].position,
				"\nBully Position: " + this.characterTransforms[4].position,
				"\nSweep Position: " + this.characterTransforms[5].position,
				"\nJoe Position: " + this.characterTransforms[6].position,
				"\nWizard Position: " + this.characterTransforms[7].position,
				"\n1stPrize Position: " + this.characterTransforms[8].position
			});
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x040002CB RID: 715
	public Text radarText;

	// Token: 0x040002CC RID: 716
	public Transform[] characterTransforms;

	// Token: 0x040002CD RID: 717
	public float cooldown;
}
