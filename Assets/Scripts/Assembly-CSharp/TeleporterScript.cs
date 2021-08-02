using UnityEngine;

// Token: 0x0200004B RID: 75
public class TeleporterScript : MonoBehaviour
{
	// Token: 0x0600017B RID: 379 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
	public void Start()
	{
		this.id = Mathf.RoundToInt(Random.Range(0f, 19f));
		base.transform.position = new Vector3(this.spawnPoints[this.id].position.x, 4f, this.spawnPoints[this.id].position.z);
		this.spawnCooldown = 15f;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000F068 File Offset: 0x0000D268
	private void Update()
	{
		if (this.gc.spoopMode)
		{
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
				return;
			}
			this.audioDevice.PlayOneShot(this.aud_respawn);
			this.id = Mathf.RoundToInt(Random.Range(0f, 19f));
			base.transform.position = new Vector3(this.spawnPoints[this.id].position.x, 4f, this.spawnPoints[this.id].position.z);
			this.spawnCooldown = Random.Range(30f, 45f);
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000F128 File Offset: 0x0000D328
	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") & this.gc.spoopMode && gc.notebooks >= 2)
		{
			GameControllerScript.i.EndAllGrapplingHooks();
			this.audioDevice.PlayOneShot(this.aud_teleport);
			this.id = Mathf.RoundToInt(Random.Range(0f, 19f));
			base.transform.position = new Vector3(this.spawnPoints[this.id].position.x, 4f, this.spawnPoints[this.id].position.z);
			this.id = Mathf.RoundToInt(Random.Range(0f, 19f));
			if (PlayerScript.instance.currentHeldNumberBallon != null) PlayerScript.instance.UnpickupNumberBallon();
			this.player.position = new Vector3(this.spawnPoints[this.id].position.x, 4f, this.spawnPoints[this.id].position.z);
			this.spawnCooldown = Random.Range(30f, 45f);
		}
	}

	// Token: 0x0400030C RID: 780
	public Transform[] spawnPoints = new Transform[20];

	// Token: 0x0400030D RID: 781
	private int id;

	// Token: 0x0400030E RID: 782
	public float spawnCooldown;

	// Token: 0x0400030F RID: 783
	public Transform player;

	// Token: 0x04000310 RID: 784
	public GameControllerScript gc;

	// Token: 0x04000311 RID: 785
	public AudioSource audioDevice;

	// Token: 0x04000312 RID: 786
	public AudioClip aud_teleport;

	// Token: 0x04000313 RID: 787
	public AudioClip aud_respawn;
}
