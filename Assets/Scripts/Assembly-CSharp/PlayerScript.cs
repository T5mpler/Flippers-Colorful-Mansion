using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

// Token: 0x0200003C RID: 60
public class PlayerScript : MonoBehaviour
{
	public static PlayerScript instance;
	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.stamina = this.maxStamina;
		this.playerRotation = base.transform.rotation;
		this.mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000C454 File Offset: 0x0000A654
	private void Update()
	{
		if (currentHeldNumberBallon != null)
		{
			Vector3 numberBallonHoldOffset = new Vector3(0, -0.75f, 3f);
			Vector3 numberBallonHoldScaleOffset = Vector3.right * 3f;
			currentHeldNumberBallon.transform.localPosition =numberBallonHoldOffset;
			currentHeldNumberBallon.transform.localScale = Vector3.one + numberBallonHoldScaleOffset;
			currentHeldNumberBallon.transform.localRotation = Quaternion.identity;
		}
		if (Time.timeScale != 0f)
		{
			if (!this.gc.inJoeGame)
			{
				this.PlayerMove();
			}
			if (this.readingTime > 0f)
			{
				if (this.reading)
				{
					this.readingTime -= Time.deltaTime;
					base.transform.position = this.frozenPosition;
					this.stamina += this.staminaRate * 2f * Time.deltaTime;
					stamina = Mathf.Clamp(stamina, 0f, maxStamina);
				}
			}
			else if (this.reading)
			{
				this.readingGO.SetActive(false);
				this.reading = false;
				this.readingCooldown = Random.Range(35f, 50f);
			}
			if (this.readingCooldown > 0f)
			{
				this.readingCooldown -= Time.deltaTime;
			}
			if (this.inLibrary && (Input.GetKeyDown(KeyCode.R) & this.readingCooldown <= 0f & !this.reading))
			{
				this.frozenPosition = base.transform.position;
				this.readingGO.SetActive(true);
				this.reading = true;
				this.readingTime = 10f;
			}
			if (this.baldi.upSideDown)
			{
				this.playerRotation.eulerAngles = new Vector3(this.playerRotation.x, this.playerRotation.y, -180f);
			}
			if (this.undergroundCooldown > 0f)
			{
				this.undergroundCooldown -= Time.deltaTime;
			}
			if (this.surfaceCooldown > 0f)
			{
				this.surfaceCooldown -= Time.deltaTime;
			}
			if (this.wizard.inSpell & this.wizard.num == 0)
			{
				base.GetComponent<Light>().enabled = false;
			}
			this.MouseMove();
			this.StaminaCheck();
			this.GuiltCheck();
			if (this.totalMoveDirection.magnitude > 0f)
			{
				this.gc.LockMouse();
			}
			if (this.jumpRope & (base.transform.position - this.frozenPosition).magnitude >= 1f)
			{
				this.DeactivateJumpRope();
			}
			if (this.sweepingFailsave > 0f)
			{
				this.sweepingFailsave -= Time.deltaTime;
				return;
			}
			this.sweeping = false;
			this.hugging = false;
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(transform.position, Camera.main.transform.forward, out raycastHit, 10f) && raycastHit.collider.CompareTag("NumberBallon"))
				{
					NumberBallonScript numberBallon = raycastHit.collider.GetComponent<NumberBallonScript>();
					PickupNumberBallon(numberBallon);
				}
				else if (Physics.Raycast(transform.position, Camera.main.transform.forward, out raycastHit, 10f) && raycastHit.collider.CompareTag("Machimatics") && currentHeldNumberBallon != null)
				{
					MachimaticsScript machimatics = raycastHit.collider.GetComponent<MachimaticsScript>();
					machimatics.CheckAnswer(currentHeldNumberBallon.value);
				}
			}
			this.MouseMove();
		}
	}
	public void PickupNumberBallon(NumberBallonScript numberBallon)
	{
		if (currentHeldNumberBallon != null)
		{
			UnpickupNumberBallon();
		}
		currentHeldNumberBallon = numberBallon;
		currentHeldNumberBallon.enabled = false;
		currentHeldNumberBallon.GetComponent<BoxCollider>().enabled = false;
		currentHeldNumberBallon.GetComponent<CapsuleCollider>().enabled = false;
		currentHeldNumberBallon.transform.SetParent(transform);
	}
	public void UnpickupNumberBallon()
	{
		currentHeldNumberBallon.enabled = true;
		currentHeldNumberBallon.GetComponent<BoxCollider>().enabled = true;
		currentHeldNumberBallon.GetComponent<CapsuleCollider>().enabled = true;
		currentHeldNumberBallon.transform.SetParent(currentHeldNumberBallon.ballonHolder);
		currentHeldNumberBallon = null;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000C880 File Offset: 0x0000AA80
	private void MouseMove()
	{
		if (!mouseLocked)
		{
			this.playerRotation.eulerAngles += new Vector3(-Input.GetAxisRaw("Mouse Y") * this.mouseSensitivity, Input.GetAxisRaw("Mouse X") * this.mouseSensitivity, 0f);
		}
	}
	public void LockMouseMove()
	{
		mouseLocked = true;
	}
	public void UnlockMouseMove()
	{
		mouseLocked = false;
	}
	public void FreezePlayer()
	{
		freezePlayer = true;
		totalMoveDirection = Vector3.zero;
		rb.velocity = totalMoveDirection;
	}
	public void UnFreezePlayer()
	{
		freezePlayer = false;
	}
	// Token: 0x06000120 RID: 288 RVA: 0x0000C964 File Offset: 0x0000AB64
	private void PlayerMove()
	{
		if (GameControllerScript.i.hydrationChallenge && stamina <= 0f)
		{
			gameOver = true;
		}
		base.transform.rotation = this.playerRotation;
		if (!freezePlayer)
		{
			this.db = Input.GetAxisRaw("Forward");
			if (this.stamina > 0f)
			{
				if (Input.GetAxisRaw("Run") > 0f)
				{
					this.playerSpeed = this.runSpeed;
					if (this.totalMoveDirection.magnitude > 0.1f & !this.hugging & !this.sweeping & !this.isOutside)
					{
						this.ResetGuilt("running", 0.1f);
					}
				}
				else
				{
					this.playerSpeed = this.walkSpeed;
					if (GameControllerScript.i.hydrationChallenge && Input.GetAxisRaw("Run") <= 0f)
					{
						this.stamina -= (this.staminaRate / 2) * Time.deltaTime;
					}
				}
			}
			else
			{
				this.playerSpeed = this.walkSpeed;
			}
			float x, z;
			x = Input.GetAxisRaw("Strafe");
			z = Input.GetAxisRaw("Forward");
			Vector3 moveVector = (transform.right * x + transform.forward * z).normalized;
			if (!this.jumpRope & !this.sweeping & !this.hugging & !this.gc.inJoeGame)
			{
				totalMoveDirection = moveVector * this.playerSpeed;
			}
			else if (this.sweeping)
			{
				totalMoveDirection = this.gottaSweep.velocity + moveVector * this.playerSpeed * 0.3f;
			}
			else if (this.hugging)
			{
				totalMoveDirection = this.firstPrize.velocity * 1.2f + (this.firstPrizeTransform.position + new Vector3((float)Mathf.RoundToInt(this.firstPrizeTransform.forward.x), 0f, (float)Mathf.RoundToInt(this.firstPrizeTransform.forward.z)) * 3f - base.transform.position);
			}
			else
			{
				totalMoveDirection = Vector3.zero;
			}
			totalMoveDirection += ActivityModifier.TotalAdder;
			totalMoveDirection *= ActivityModifier.TotalMultipler;
		}
	}
	private void FixedUpdate()
	{
		rb.velocity = totalMoveDirection;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0000CC58 File Offset: 0x0000AE58
	private void StaminaCheck()
	{
		if (this.totalMoveDirection.magnitude > 0.1f)
		{
			if (Input.GetAxisRaw("Run") > 0f & this.stamina > 0f)
			{
				this.stamina -= GameControllerScript.i.hydrationChallenge ? this.staminaRate * 2f * Time.deltaTime : this.staminaRate * Time.deltaTime ;
			}
			if (this.stamina < 0f & this.stamina > -5f)
			{
				this.stamina = -1f;
			}
		}
		else if (this.stamina < this.maxStamina)
		{
			if (!this.cs.inPool && !GameControllerScript.i.hydrationChallenge)
			{
				this.stamina += this.staminaRate * Time.deltaTime;
			}
			else if (this.cs.poolRides != 4 && !GameControllerScript.i.hydrationChallenge)
			{
				this.stamina += this.staminaRate * 2f * Time.deltaTime;
			}
			else if (this.cs.poolDirty && !GameControllerScript.i.hydrationChallenge)
			{
				this.stamina += this.staminaRate * -2f * Time.deltaTime;
			}
		}
		this.staminaBar.value = this.stamina / this.maxStamina * 100f;
	}

	int HasKnifeSteamer()
	{
		for (int i = 0; i < GameControllerScript.i.item.Length; i++)
		{
			if (GameControllerScript.i.HasItemInItemSlot(12, i))
			{
				return i;
			}
		}
		return -1;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "Baldi" & !this.gc.debugMode & !this.gc.teleporting && SceneManager.GetActiveScene().name != "Race")
		{
			int itemId = HasKnifeSteamer();
			if (itemId != -1)
			{
				if (PlayerPrefs.GetFloat("KnifeSteamed") == 0f)
				{
					GameControllerScript.i.aPopUp.QueueAchievement(GameControllerScript.i.aPopUp.achievements[3]);
					PlayerPrefs.SetFloat("KnifeSteamed", 1f);
				}
				this.audioDevice.PlayOneShot(this.baldi.aud_NoKnife);
				this.baldi.RemoveKnife(15f);
				GameControllerScript.i.LoseItem(itemId);
			}
			else
			{
				this.gameOver = true;
			}
		}
		else if (other.name == "UndergrondEntrance")
		{
			if (!this.isUnderground & this.undergroundCooldown <= 0f & !this.goingToSurface & !this.principal.angry)
			{
				this.isUnderground = true;
				base.StartCoroutine(this.GoUnderGround());
				this.gc.playerLight.enabled = false;
				base.GetComponent<Light>().enabled = false;
				gc.EndAllGrapplingHooks();
			}
			if (this.isUnderground & this.surfaceCooldown <= 0f & !this.goingUnderground)
			{
				this.isUnderground = false;
				base.StartCoroutine(this.GoBackToSurface());
				if (!this.gc.darkEvent)
				{
					this.gc.playerLight.enabled = true;
					base.GetComponent<Light>().enabled = true;
				}
				gc.EndAllGrapplingHooks();
			}
		}
		else if (other.transform.name == "Playtime" & !this.jumpRope & this.playtime.playCool <= 0f & !this.gc.teleporting)
		{
			this.ActivateJumpRope();
		}
		else if (other.transform.name == "Joe Wonderer" & this.jw.playCooldown <= 0f & !this.gc.teleporting & !this.gc.inJoeGame)
		{
			this.gc.ActivateJoeGame();
		}
		else if (other.transform.name == "Joe Wonderer" & this.jw.beatingUp & this.jw.beatUps <= 6)
		{
			this.jw.BeatUp();
		}
		if (other.transform.name == "Library" & this.lb.isLibraryClosed)
		{
			base.transform.position = this.lb.insideDoor.transform.position + new Vector3(0f, 0f, 5f);
		}
		if (other.transform.name == "Le Wizard De Heckizos" & !this.wizard.spellCast & this.wizard.spellCooldown <= 0f & this.wizard.spells != 4 & !this.wizard.brokeWand)
		{
			StartCoroutine(this.wizard.DoSpell());
			return;
		}
		if (other.transform.name == "Le Wizard De Heckizos" & !this.wizard.brokeWand & this.wizard.spells == 4)
		{
			this.wizard.BreakWand(true);
			return;
		}
		if (other.transform.name == "Outside Collider")
		{
			this.isOutside = true;
			return;
		}
		if (other.transform.name == "Pole")
		{
			this.fcs.finished = true;
			return;
		}
		if (other.transform.name == "Library")
		{
			this.inLibrary = true;
		}
		if (other.transform.name == "Principal Of The Thing")
		{
			gameOver = true;
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000D044 File Offset: 0x0000B244
	private void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "Gotta Sweep")
		{
			this.sweeping = true;
			this.sweepingFailsave = 1f;
			return;
		}
		if (other.transform.name == "1st Prize" & this.firstPrize.velocity.magnitude > 5f)
		{
			this.hugging = true;
			this.sweepingFailsave = 1f;
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
	private void OnTriggerExit(Collider other)
	{
		if (other.transform.name == "Office Trigger")
		{
			this.ResetGuilt("escape", this.door.lockTime);
			return;
		}
		if (other.transform.name == "Gotta Sweep")
		{
			this.sweeping = false;
			return;
		}
		if (other.transform.name == "1st Prize")
		{
			this.hugging = false;
			return;
		}
		if (other.transform.name == "Outside Collider")
		{
			this.isOutside = false;
			return;
		}
		if (other.transform.name == "Library")
		{
			this.inLibrary = false;
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000D176 File Offset: 0x0000B376
	public void ResetGuilt(string type, float amount)
	{
		if (amount >= this.guilt)
		{
			this.guilt = amount;
			this.guiltType = type;
		}
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0000D18F File Offset: 0x0000B38F
	private void GuiltCheck()
	{
		if (this.guilt > 0f)
		{
			this.guilt -= Time.deltaTime;
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
	public void ActivateJumpRope()
	{
		this.jumpRopeScreen.SetActive(true);
		this.jumpRope = true;
		this.frozenPosition = base.transform.position;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x0000D1D6 File Offset: 0x0000B3D6
	public void DeactivateJumpRope()
	{
		this.jumpRopeScreen.SetActive(false);
		this.jumpRope = false;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x0000D1EC File Offset: 0x0000B3EC
	public void CastSpell()
	{
		if ((float)this.wizard.num == 0f)
		{
			RenderSettings.ambientLight = new Color(0.23f, 0.23f, 0.23f, 1f);
			base.GetComponent<Light>().enabled = false;
			this.gc.NormalText();
		}
		else if ((float)this.wizard.num == 1f)
		{
			this.isDeaf = true;
			AudioListener.volume = 0f;
		}
		if ((float)this.wizard.num == 2f)
		{
			transform.localScale = Vector3.one;
			transform.position -= Vector3.up * 2f;
			this.lowered = true;
			this.walkSpeed -= 5f;
			this.runSpeed -= 6f;
		}
		if (wizard.num == 3f)
		{
			StartCoroutine(Blur("in"));
		}
		if (wizard.num == 4f)
		{
			List<GameObject> gameObjects = new List<GameObject>();
			for (int i = 0; i < UnityEngine.Random.Range(1, GameControllerScript.i.item.Length); i++)
			{
				GameObject go = GameControllerScript.i.DropItem(i);
				if (go != null) gameObjects.Add(go);
			}
			StartCoroutine(SendItemsToWizard(gameObjects));
		}
		this.wizard.spellTime = 15f;
		this.wizard.inSpell = true;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000D328 File Offset: 0x0000B528
	public void StopSpell()
	{
		if ((float)this.wizard.num == 0f)
		{
			this.gc.playerLight.enabled = true;
			this.gc.playerLight.range = 500f;
			this.gc.playerLight.color = new Color(0.3f, 0.3f, 0.3f);
			this.gc.playerLight.type = LightType.Directional;
			RenderSettings.ambientLight = this.gc.originalAmbientLight;
			this.gc.NormalText();
		}
		if ((float)this.wizard.num == 1f)
		{
			this.isDeaf = false;
			AudioListener.volume = 1f;
		}
		if ((float)this.wizard.num == 2f)
		{
			transform.localScale = new Vector3(1f, 4f, 1f);
			transform.position += Vector3.up * 2f;
			if (this.lowered)
			{
				this.lowered = false;
				this.walkSpeed = 10f;
				this.runSpeed = 16f;
			}
		}
		if (wizard.num == 3f)
		{
			StartCoroutine(Blur("out"));
		}
		if (wizard.num == 4f)
		{
			return;
		}
	}


	// Token: 0x0600012F RID: 303 RVA: 0x0000D669 File Offset: 0x0000B869
	public IEnumerator GoUnderGround()
	{
		GetComponent<Collider>().enabled = false;
		FreezePlayer();
		this.goingUnderground = true;
		while (base.transform.position.y >= -6f)
		{
			base.transform.position -= Vector3.up * Time.deltaTime * 5f;
			yield return null;
		}
		base.transform.position = new Vector3(baldi.undergroundEntrance.transform.position.x, -6f, baldi.undergroundEntrance.transform.position.z);
		this.surfaceCooldown = 3f;
		this.goingUnderground = false;
		GetComponent<Collider>().enabled = true;
		UnFreezePlayer();
		yield break;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000D678 File Offset: 0x0000B878
	public IEnumerator GoBackToSurface()
	{
		GetComponent<Collider>().enabled = false;
		FreezePlayer();
		this.goingToSurface = true;
		while (base.transform.position.y <= 4f)
		{
			base.transform.position += new Vector3(0f, Time.deltaTime * 5f, 0f);
			yield return null;
		}
		base.transform.position = new Vector3(baldi.undergroundEntrance.transform.position.x, 4f, baldi.undergroundEntrance.transform.position.z);
		this.undergroundCooldown = 3f;
		this.goingToSurface = false;
		UnFreezePlayer();
		GetComponent<Collider>().enabled = true;
		yield break;
	}
	public ActivityModifierScript ActivityModifier
	{
		get
		{
			return GetComponent<ActivityModifierScript>();
		}
	}
	IEnumerator SendItemsToWizard(List<GameObject> itemList)
	{
		wizard.agent.isStopped = true;
		foreach (GameObject item in itemList)
		{
			PickupScript pickup = item.GetComponent<PickupScript>();
			pickup.canBePickedUp = false;
			if (item != null)
			{
				while ((item.transform.position - wizard.transform.position).magnitude >= 5f)
				{
					Vector3 direction = (wizard.transform.position - item.transform.position).normalized;
					item.transform.position += direction * 30f * Time.deltaTime;
					yield return null;
				}
			}
		}
		yield return new WaitForSeconds(2f);
		foreach (GameObject item in itemList)
		{
			Destroy(item);
		}
		wizard.agent.isStopped = false;
		yield break;
	}
	IEnumerator Blur(string type)
	{
		DepthOfField depthOfField = wizard.blurEffect.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>();
		if (type == "in")
		{
			wizard.blurEffect.SetActive(true);
			while (depthOfField.focalLength.value > 300)
			{
				depthOfField.focalLength.value += Mathf.RoundToInt(Time.deltaTime * 10f);
				yield return null;
			}
			depthOfField.focalLength.value = 300;
		}
		else if (type == "out")
		{
			while (depthOfField.focalLength.value < 1)
			{
				depthOfField.focalLength.value -= Mathf.RoundToInt(Time.deltaTime * 200f);
			}
			depthOfField.focalLength.value = 1;
			wizard.blurEffect.SetActive(false);
		}
		yield break;
	}
	// Token: 0x0400024B RID: 587
	public GameControllerScript gc;

	// Token: 0x0400024C RID: 588
	public BaldiScript baldi;

	// Token: 0x0400024D RID: 589
	public DoorScript door;

	// Token: 0x0400024E RID: 590
	public PlaytimeScript playtime;

	// Token: 0x0400024F RID: 591
	public bool gameOver;

	// Token: 0x04000250 RID: 592
	public bool jumpRope;

	// Token: 0x04000251 RID: 593
	public bool sweeping;

	// Token: 0x04000252 RID: 594
	public bool hugging;

	// Token: 0x04000253 RID: 595
	public float sweepingFailsave;

	// Token: 0x04000254 RID: 596
	private Quaternion playerRotation;

	// Token: 0x04000255 RID: 597
	public Vector3 frozenPosition;

	// Token: 0x04000256 RID: 598
	public float mouseSensitivity;

	// Token: 0x04000257 RID: 599
	public float walkSpeed;

	// Token: 0x04000258 RID: 600
	public float runSpeed;

	// Token: 0x04000259 RID: 601
	public float slowSpeed;

	// Token: 0x0400025A RID: 602
	public float maxStamina;

	// Token: 0x0400025B RID: 603
	public float staminaRate;

	// Token: 0x0400025C RID: 604
	public float guilt;

	// Token: 0x0400025D RID: 605
	public float initGuilt;

	// Token: 0x0400025E RID: 606
	private float playerSpeed;

	// Token: 0x0400025F RID: 607
	public float stamina;

	// Token: 0x04000260 RID: 608
	public Rigidbody rb;

	// Token: 0x04000261 RID: 609
	public NavMeshAgent gottaSweep;

	// Token: 0x04000262 RID: 610
	public NavMeshAgent firstPrize;

	// Token: 0x04000263 RID: 611
	public Transform firstPrizeTransform;

	// Token: 0x04000264 RID: 612
	public Slider staminaBar;

	// Token: 0x04000265 RID: 613
	public float db;

	// Token: 0x04000266 RID: 614
	public string guiltType;

	// Token: 0x04000267 RID: 615
	public GameObject jumpRopeScreen;

	// Token: 0x04000268 RID: 616
	public WizardScript wizard;

	// Token: 0x04000269 RID: 617
	public Camera mainCamera;

	// Token: 0x0400026B RID: 619
	public bool lowered;

	// Token: 0x0400026C RID: 620
	public GameObject joeGame;

	// Token: 0x0400026D RID: 621
	public JoeWondererScript jw;

	// Token: 0x04000270 RID: 624
	public LibraryDoorScript lb;

	// Token: 0x04000276 RID: 630
	public CameraFOVScript cameraFov;

	// Token: 0x04000277 RID: 631
	public CameraScript cs;

	// Token: 0x04000278 RID: 632
	public bool isUnderground;

	// Token: 0x04000279 RID: 633
	public float undergroundCooldown;

	// Token: 0x0400027A RID: 634
	public float surfaceCooldown;

	// Token: 0x0400027B RID: 635
	public bool goingUnderground;

	// Token: 0x0400027C RID: 636
	public bool goingToSurface;

	// Token: 0x0400027D RID: 637
	public PrincipalScript principal;

	// Token: 0x0400027E RID: 638
	public bool isOutside;

	// Token: 0x0400027F RID: 639
	public MathGameScript mathGame;

	// Token: 0x04000280 RID: 640
	public AudioSource audioDevice;

	public FarmControllerScript fcs;

	// Token: 0x04000287 RID: 647
	public bool inLibrary;

	// Token: 0x04000288 RID: 648
	public bool reading;

	// Token: 0x04000289 RID: 649
	public float readingCooldown;

	// Token: 0x0400028A RID: 650
	public float readingTime;

	// Token: 0x0400028B RID: 651
	public GameObject readingGO;

	// Token: 0x0400028C RID: 652
	public bool isDeaf;

	public NumberBallonScript currentHeldNumberBallon;

	public Vector3 totalMoveDirection;

	bool mouseLocked;

	bool freezePlayer;

}
