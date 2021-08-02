using UnityEngine;

// Token: 0x02000039 RID: 57
public class NotebookScript : MonoBehaviour
{
	// Token: 0x06000114 RID: 276 RVA: 0x0000B7E8 File Offset: 0x000099E8
	private void Start()
	{
		this.up = true;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x0000B7F4 File Offset: 0x000099F4
	private void Update()
	{
		if (this.gc.mode == "endless")
		{
			if (this.respawnTime > 0f)
			{
				if ((base.transform.position - this.player.position).magnitude > 60f)
				{
					this.respawnTime -= Time.deltaTime;
				}
			}
			else if (!this.up)
			{
				base.transform.position = new Vector3(base.transform.position.x, 4f, base.transform.position.z);
				this.up = true;
				this.audioDevice.Play();
			}
		}
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit) && (raycastHit.transform.tag == "Notebook" & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance))
		{
			base.transform.position = new Vector3(base.transform.position.x, -20f, base.transform.position.z);
			this.up = false;
			this.respawnTime = 120f;
			this.gc.CollectNotebook();
			GameObject gameObject = Object.Instantiate<GameObject>(this.learningGame);
			gameObject.GetComponent<MathGameScript>().gc = this.gc;
			gameObject.GetComponent<MathGameScript>().baldiScript = this.bsc;
			gameObject.GetComponent<MathGameScript>().playerPosition = this.player.position;
		}
	}

	// Token: 0x0400023E RID: 574
	public float openingDistance;

	// Token: 0x0400023F RID: 575
	public GameControllerScript gc;

	// Token: 0x04000240 RID: 576
	public BaldiScript bsc;

	// Token: 0x04000241 RID: 577
	public float respawnTime;

	// Token: 0x04000242 RID: 578
	public bool up;

	// Token: 0x04000243 RID: 579
	public Transform player;

	// Token: 0x04000244 RID: 580
	public GameObject learningGame;

	// Token: 0x04000245 RID: 581
	public AudioSource audioDevice;
}
