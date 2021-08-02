using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class EndlessNotebookScript : MonoBehaviour
{
	// Token: 0x0600007F RID: 127 RVA: 0x00004AB3 File Offset: 0x00002CB3
	private void Start()
	{
		this.gc = GameObject.Find("Game Controller").GetComponent<GameControllerScript>();
		this.player = GameObject.Find("Player").GetComponent<Transform>();
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00004AE0 File Offset: 0x00002CE0
	private void Update()
	{
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit) && (raycastHit.transform.tag == "Notebook" & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance))
		{
			base.gameObject.SetActive(false);
			this.gc.CollectNotebook();
			this.learningGame.SetActive(true);
		}
	}

	// Token: 0x040000BE RID: 190
	public float openingDistance;

	// Token: 0x040000BF RID: 191
	public GameControllerScript gc;

	// Token: 0x040000C0 RID: 192
	public Transform player;

	// Token: 0x040000C1 RID: 193
	public GameObject learningGame;
}
