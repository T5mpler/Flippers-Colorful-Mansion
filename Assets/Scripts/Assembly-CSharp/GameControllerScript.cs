using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x0200002B RID: 43
public class GameControllerScript : MonoBehaviour
{
	// Token: 0x060000A9 RID: 169 RVA: 0x00005A30 File Offset: 0x00003C30
	public GameControllerScript()
	{
		int[] array = new int[3];
		array[0] = 245;
		array[1] = 285;
		array[2] = 315;
		this.itemSelectOffset = array;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00005BB8 File Offset: 0x00003DB8
	private void Start()
	{
		i = this;
		ChallengeManagerScript challengeManager = GetComponent<ChallengeManagerScript>();
		if (PlayerPrefs.GetString("ChallengeMode") != string.Empty)
		{
			challengeManager.OnPlayerInChallenge += ChallengeManager_OnPlayerInChallenge;
			challengeManager.TriggerEvent();
		}
		FloodEventScript.instance = flood.GetComponent<FloodEventScript>();
		this.UpdateNotebookCount();
		this.UpdateMoneyCount();
		this.money = 0f;
		this.playerLight = this.playerTransform.gameObject.GetComponent<Light>();
		this.playerLight.range = 500f;
		this.playerLight.color = new Color(0.3f, 0.3f, 0.3f);
		this.playerLight.type = LightType.Directional;
		if (SceneManager.GetActiveScene().name != "Shop" && !grapplingChallenge)
		{
			for (int i = 0; i < item.Length; i++)
			{
				item[i] = PlayerPrefs.GetInt("StartingItem" + (i + 1));
				PlayerPrefs.SetInt("StartingItem" + (i + 1), 0);
				itemSlot[i].texture = itemTextures[item[i]];
			}
		}
		if (PlayerPrefs.GetInt("fpsDisplay") == 1)
		{
			this.fpsCounter.SetActive(true);
		}
		else
		{
			this.fpsCounter.SetActive(false);
		}
		if (PlayerPrefs.GetFloat("Time") == 1f)
		{
			this.clockText.SetActive(true);
		}
		else
		{
			this.clockText.SetActive(false);
			this.dateText.SetActive(false);
		}
		if (PlayerPrefs.GetFloat("Date") == 1f)
		{
			this.dateText.SetActive(true);
		}
		else
		{
			this.dateText.SetActive(false);
		}
		this.stunnedCharacter = this.testEnemy;
		this.christmasEvent.SetActive(false);
		this.eventStarts = false;
		this.eventCooldown = (float)Mathf.RoundToInt(UnityEngine.Random.Range(3f, 8f));
		this.originalAmbientLight = RenderSettings.ambientLight;
		this.noSpeed = false;
		this.stunned = false;
		this.bg.SetActive(false);
		this.increaseFactor += (float)Mathf.RoundToInt(UnityEngine.Random.Range(0.5f, 1.3f));
		this.teleporting = false;
		this.audioDevice = base.GetComponent<AudioSource>();
		this.mode = PlayerPrefs.GetString("CurrentMode");
		if (this.mode == "endless")
		{
			this.baldiScrpt.endless = true;
		}
		this.schoolMusic.volume = 0.3f;
		this.schoolMusic.Play();
		this.LockMouse();
		this.UpdateItemName();
		this.itemSelected = 0;
		this.gameOverDelay = 1.5f;
		for (int i = 0; i < 3; i++)
		{
			UpdateItemNameArray(itemPickupNames[0], i);
		}
		grapplingHooks = new List<GrapplingHookScript>();
	}

	private void ChallengeManager_OnPlayerInChallenge(object sender, ChallengeManagerScript.OnPlayerStartChallenge e)
	{
		if (SceneManager.GetActiveScene().name == "Race")
		{
			currentChallenge = ChallengeManagerScript.ChallengeType.Race;
			return;
		}
		switch (e.challengeType)
		{
			default:
				skyboxMaterial.SetColor("_TintColor", new Color(127f, 127f, 127f));
				stealthyChallenge = false;
				grapplingChallenge = false;
				hydrationChallenge = false;
				break;
			case ChallengeManagerScript.ChallengeType.Grapple:
				haveEvents = false;
				grapplingChallenge = true;
				break;
			case ChallengeManagerScript.ChallengeType.Hydration:
				PickupScript[] pickups = FindObjectsOfType<PickupScript>();
				foreach (PickupScript pickup in pickups)
				{
					if (pickup.gameObject != quarter)
					{
						pickup.name = "Pickup_EnergyFlavoredZestyBar";
					}
					pickup.GetComponentInChildren<SpriteRenderer>().sprite = itemSprites[1];
				}
				hydrationChallenge = true;
				break;
			case ChallengeManagerScript.ChallengeType.NoItems:
				PickupScript[] itemPickups = FindObjectsOfType<PickupScript>();
				foreach (PickupScript pickup in itemPickups)
				{
					pickup.gameObject.SetActive(false);
				}
				FindObjectOfType<BreakRoomScript>().enabled = false;
				for (int i = 0; i < item.Length; i++)
				{
					LoseItem(i);
				}
				break;
			case ChallengeManagerScript.ChallengeType.Speedy:
				speedyChallenege = true;
				haveEvents = false;
				player.walkSpeed = 35f;
				player.runSpeed = 50f;
				baldiScrpt.GetAngry(125f);
				break;
			case ChallengeManagerScript.ChallengeType.Stealthy:
				skyboxMaterial.SetColor("Tint Color", Color.black);
				stealthyChallenge = true;
				break;
		}
		currentChallenge = e.challengeType;
		if (PlayerPrefs.GetString("ChallengeMode") == "Stealthy")
		{
			skyboxMaterial.SetColor("_TintColor", Color.black);
			currentChallenge = ChallengeManagerScript.ChallengeType.Stealthy;
			haveEvents = false;
			stealthyChallenge = true;
		}
		PlayerPrefs.SetString("ChallengeMode", string.Empty);
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00005DB4 File Offset: 0x00003FB4
	private void Update()
	{
		if (speedyChallenege && spoopMode)
		{
			GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
			foreach (GameObject npc in npcs)
			{
				if (npc.name != "Baldi")
				{
					npc.SetActive(false);
				}
			}
		}
		if (stealthyChallenge && spoopMode)
		{
			GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
			foreach (GameObject npc in npcs)
			{
				if (npc != principal && npc != baldi)
				{
					npc.SetActive(false);
				}
			}
		}
		if (grapplingChallenge && spoopMode)
		{
			PlayerScript.instance.walkSpeed = 0f;
			PlayerScript.instance.runSpeed = 0f;
			if (!HasItemInItemSlot(15, 0))
			{
				AddItem(15, 0);
			}
		}
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
		{
			DropItem(itemSelected);
		}
		this.UpdateMoneyCount();
		for (int i = 0; i < this.ach.images.Length; i++)
		{
			int num = 0;
			if (this.ach.images[i].color == Color.white)
			{
				num++;
				if (num == 17 & PlayerPrefs.GetFloat("AllUnlocked") == 0f)
				{
					this.aPopUp.QueueAchievement(this.aPopUp.achievements[17]);
					PlayerPrefs.SetFloat("AllUnlocked", 1f);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.R) & this.radarMode)
		{
			if (this.radar.activeSelf)
			{
				this.radar.SetActive(false);
			}
			else if (!this.radar.activeSelf)
			{
				this.radar.SetActive(true);
			}
		}
		if (this.saleEvent)
		{
			this.cost[0] = 0.5f;
			this.cost[1] = 0.25f;
			this.cost[2] = 0.1f;
			for (int j = 0; j < this.bsodaMachines.Length; j++)
			{
				this.bsodaMachines[j].material = this.machineSales[0];
			}
			for (int k = 0; k < this.zestyMachines.Length; k++)
			{
				this.zestyMachines[k].material = this.machineSales[1];
			}
			for (int l = 0; l < this.crazyItemMachines.Length; l++)
			{
				this.crazyItemMachines[l].material = this.machineSales[2];
			}
			if (!this.payPhonedPaided)
			{
				this.payPhone.sprite = this.payPhone_sale;
			}
		}
		else
		{
			this.cost[0] = 1f;
			this.cost[1] = 0.5f;
			this.cost[2] = 0.25f;
			for (int m = 0; m < this.bsodaMachines.Length; m++)
			{
				this.bsodaMachines[m].material = this.normal[0];
			}
			for (int n = 0; n < this.zestyMachines.Length; n++)
			{
				this.zestyMachines[n].material = this.normal[1];
			}
			for (int num2 = 0; num2 < this.crazyItemMachines.Length; num2++)
			{
				this.crazyItemMachines[num2].material = this.normal[2];
			}
			if (!this.payPhonedPaided)
			{
				this.payPhone.sprite = this.payPhone_normal;
			}
		}
		if (this.spoopMode)
		{
			if (haveEvents)
			{
				if (this.eventStarts)
				{
					if (this.eventTimer > 0f)
					{
						this.eventTimer -= Time.deltaTime;
					}
					else if (this.eventStarted)
					{
						this.StopEvent();
					}
				}
				if (this.eventCooldown > 0f)
				{
					this.eventCooldown -= Time.deltaTime;
				}
				else if (!this.eventStarts)
				{
					this.eventStarts = true;
					base.StartCoroutine(this.StartEvent());
				}
			}
		}
		if (this.noKnifeTime > 0f)
		{
			this.baldiScrpt.baldiSprite.sprite = this.baldiScrpt.noKnifeSprite;
			this.noKnifeTime -= Time.deltaTime;
		}
		else if (this.noKnife)
		{
			this.noKnife = false;
			this.baldi.SetActive(true);
			Destroy(this.baldiScrpt.clonedNoKnifeBaldi);
		}
		if (this.magicCooldown > 0f)
		{
			this.magicCooldown -= Time.deltaTime;
		}
		else if (this.higherThanRun)
		{
			this.player.runSpeed += this.player.walkSpeed;
			this.higherThanRun = false;
		}
		else if (this.lowerThanRun)
		{
			this.player.walkSpeed -= this.player.runSpeed;
			this.lowerThanRun = false;
		}
		if (this.stunTime > 0f)
		{
			this.stunTime -= Time.deltaTime;
		}
		else if (this.stunned)
		{
			this.noSpeed = false;
			this.stunned = false;
			this.stunnedCharacter = this.testEnemy;
		}
		if (this.imageCooldown >= 0f)
		{
			this.imageCooldown -= Time.deltaTime;
			this.bg.SetActive(true);
		}
		else
		{
			this.bg.SetActive(false);
			this.imageText.text = string.Empty;
		}
		int num3 = 0;
		if (this.teleporting)
		{
			if (this.cooldown > 0f)
			{
				this.cooldown -= Time.deltaTime;
			}
			else if (this.teleportCount < this.teleports)
			{
				num3++;
				this.teleportCount++;
				this.increaseFactor *= (float)num3;
				this.audioDevice.PlayOneShot(this.aud_MagicWand);
				int num4 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 19f));
				this.playerTransform.position = new Vector3(this.spawnPoints[num4].position.x, this.playerTransform.position.y, this.spawnPoints[num4].position.z);
				this.cooldown += this.increaseFactor;
			}
			else
			{
				this.teleporting = false;
				this.cooldown = 0f;
			}
		}
		if (!this.learningActive)
		{
			if (!this.inJoeGame && Input.GetButtonDown("Pause"))
			{
				if (!this.gamePaused)
				{
					this.PauseGame();
				}
				else
				{
					this.UnpauseGame();
				}
			}
			if (Input.GetKeyDown(KeyCode.Y) & this.gamePaused)
			{
				SceneManager.LoadScene("MainMenu");
			}
			else if (Input.GetKeyDown(KeyCode.N) & this.gamePaused)
			{
				this.UnpauseGame();
			}
			if (!this.gamePaused & Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
			}
			if (Input.GetMouseButtonDown(1))
			{
				this.UseItem();
				this.MoneyUse();
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				this.DecreaseItemSelection();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				this.IncreaseItemSelection();
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				this.itemSelected = 0;
				this.UpdateItemSelection();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				this.itemSelected = 1;
				this.UpdateItemSelection();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				this.itemSelected = 2;
				this.UpdateItemSelection();
			}
		}
		else if (Time.timeScale != 0f)
		{
			Time.timeScale = 0f;
		}
		if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) & !this.miniMap.activeSelf)
		{
			this.miniMap.SetActive(true);
		}
		else if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) & this.miniMap.activeSelf)
		{
			this.miniMap.SetActive(false);
		}
		if (Input.GetKey(KeyCode.Tab) && this.spoopMode)
		{
			this.warning.GetComponent<Text>().fontSize = 15;
			this.warning.SetActive(true);
			this.warning.GetComponent<Text>().text = "Distance from Flipper: " + Mathf.RoundToInt(Vector3.Distance(this.player.transform.position, this.baldi.transform.position)).ToString();
		}
		else if (this.player.stamina < 0f)
		{
			this.warning.SetActive(true);
			this.warning.GetComponent<Text>().text = "Chill out dude...";
			this.warning.GetComponent<Text>().fontSize = 20;
		}
		else if (this.player.stamina >= 0f)
		{
			int num5 = Mathf.RoundToInt(this.player.stamina);
			if (this.player.stamina < (float)num5)
			{
				num5--;
			}
			this.warning.SetActive(true);
			if (num5 >= 1)
			{
				this.warning.GetComponent<Text>().text = num5.ToString() + "% Stamina Left";
				this.warning.GetComponent<Text>().fontSize = 20;
			}
		}
		for (int i = 0; i < staminaOutlineColors.Length; i++)
		{
			string property = i == 0 ? "_OutlineColor" : "_OutlineColor2";
			if (warning.GetComponent<Text>().text.Contains("Distance from Flipper"))
			{
				float maxDistance = 500f;
				float value = Mathf.InverseLerp(0f, maxDistance, (baldi.transform.position - playerTransform.position).magnitude);
				warning.GetComponent<Text>().material.SetColor(property, staminaOutlineColors[0].Evaluate(value));
			}
			else
			{
				warning.GetComponent<Text>().material.SetColor(property, staminaOutlineColors[i].Evaluate(player.stamina / player.maxStamina));
			}
		}
		if (this.player.gameOver)
		{
			if (!takeAwayShopPointsCoroutineStarted)
			{
				takeAwayShopPointsCoroutineStarted = true;
				StartCoroutine(TakeAwayShopPointsAdded());
			}
			Time.timeScale = 0f;
			this.gameOverDelay -= Time.unscaledDeltaTime;
			this.audioDevice.PlayOneShot(this.aud_buzz);
			if (this.exitsReached == 3 & PlayerPrefs.GetFloat("So Close!") == 0f)
			{
				this.aPopUp.QueueAchievement(this.aPopUp.achievements[5]);
				PlayerPrefs.SetFloat("So Close!", 1f);
			}
			if (this.gameOverDelay <= 0f)
			{
				if (this.mode == "endless")
				{
					if (this.notebooks > PlayerPrefs.GetInt("HighBooks"))
					{
						PlayerPrefs.SetInt("HighBooks", this.notebooks);
						PlayerPrefs.SetInt("HighTime", Mathf.FloorToInt(this.time));
						this.highScoreText.SetActive(true);
					}
					else if (this.notebooks == PlayerPrefs.GetInt("HighBooks") & Mathf.FloorToInt(this.time) > PlayerPrefs.GetInt("HighTime"))
					{
						PlayerPrefs.SetInt("HighTime", Mathf.FloorToInt(this.time));
						this.highScoreText.SetActive(true);
					}
					PlayerPrefs.SetInt("CurrentBooks", this.notebooks);
					PlayerPrefs.SetInt("CurrentTime", Mathf.FloorToInt(this.time));
				}
				Time.timeScale = 1f;
				SceneManager.LoadScene("GameOver");
			}
		}
		this.time += Time.deltaTime;
	}
	public void AddItem(int itemID, int itemSlot)
	{
		item[itemSlot] = itemID;
		this.itemSlot[0].texture = this.itemTextures[itemID];
		itemNameArray[0] = itemPickupNames[itemID];
	}
	public GameObject DropItem(int itemID)
	{
		if (item[itemID] != 0 && SceneManager.GetActiveScene().name != "Shop")
		{
			GameObject droppedItem = new GameObject(itemNameArray[itemID]);
			droppedItem.transform.position = playerTransform.position;
			Rigidbody rb = droppedItem.AddComponent<Rigidbody>();
			rb.useGravity = false;
			rb.isKinematic = true;
			rb.constraints = RigidbodyConstraints.FreezeRotation;
			CapsuleCollider capsuleCollider = droppedItem.AddComponent<CapsuleCollider>();
			capsuleCollider.isTrigger = true;
			capsuleCollider.center = Vector3.up;
			capsuleCollider.radius = 1.5f;
			capsuleCollider.height = 2f;
			capsuleCollider.direction = 1;
			PickupScript pickupScript = droppedItem.AddComponent<PickupScript>();
			pickupScript.gc = this;
			pickupScript.player = playerTransform;
			GameObject childGameObject = new GameObject("Sprite");
			childGameObject.transform.parent = droppedItem.transform;
			childGameObject.transform.position = Vector3.up;
			childGameObject.transform.localScale = Vector3.one * 2f;
			SpriteRenderer spriteRenderer = childGameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = itemSprites[item[itemID]];
			childGameObject.AddComponent<Billboard>();
			childGameObject.AddComponent<PickupAnimationScript>();
			LoseItem(itemID);
			if (pickupScript.breakRoomItem)
			{
				Destroy(droppedItem);
			}
			return droppedItem;
		}
		return null;
	}
	public GameObject MakeNewItem(Vector3 position, Transform parent, Sprite itemSprite)
	{
		if (SceneManager.GetActiveScene().name != "Shop")
		{
			int intSprite = ItemSpriteToInt(itemSprite);
			GameObject newItem = new GameObject(itemPickupNames[intSprite]);
			newItem.transform.position = position;
			Rigidbody rb = newItem.AddComponent<Rigidbody>();
			rb.useGravity = false;
			rb.isKinematic = true;
			rb.constraints = RigidbodyConstraints.FreezeRotation;
			CapsuleCollider capsuleCollider = newItem.AddComponent<CapsuleCollider>();
			capsuleCollider.isTrigger = true;
			capsuleCollider.center = Vector3.up;
			capsuleCollider.radius = 1.5f;
			capsuleCollider.height = 2f;
			capsuleCollider.direction = 1;
			PickupScript pickupScript = newItem.AddComponent<PickupScript>();
			pickupScript.gc = this;
			pickupScript.player = playerTransform;
			pickupScript.canBePickedUp = true;
			GameObject child = new GameObject("Sprite");
			child.transform.parent = newItem.transform;
			child.transform.position = Vector3.up;
			child.transform.localScale = Vector3.one * 2f;
			SpriteRenderer spriteRenderer = child.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = itemSprite;
			child.AddComponent<Billboard>();
			child.AddComponent<PickupAnimationScript>();
			newItem.transform.SetParent(parent);
			return newItem;
		}
		return null;
	}
	int ItemSpriteToInt(Sprite itemSprite)
	{
		for (int i = 1; i < itemSprites.Length; i++)
		{
			if (itemSprites[i].name == itemSprite.name)
			{
				return i;
			}
		}
		return 0;
	}
	// Token: 0x060000AC RID: 172 RVA: 0x00006784 File Offset: 0x00004984
	private void UpdateNotebookCount()
	{
		switch (mode)
		{
			case "story": this.notebookCount.text = this.notebooks + "/12 Desktop\n Applications"; break;
			case "freeRun": this.notebookCount.text = this.notebooks + "/12 Desktop\n Applications"; break;
			case "endless":
				switch (notebooks)
				{
					default: this.notebookCount.text = notebooks + " Desktop\n Applications"; break;
					case 1: this.notebookCount.text = "1 Desktop\n Application"; break;
				}
				break;
		}
		if (this.notebooks == 12 & (this.mode == "story" || this.mode == "freeRun") & !this.finaleMode)
		{
			this.ActivateFinaleMode();
			return;
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00006843 File Offset: 0x00004A43
	public void CollectNotebook()
	{
		this.notebooks++;
		this.UpdateNotebookCount();
		this.time = 0f;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00006864 File Offset: 0x00004A64
	public void LockMouse()
	{
		if (!this.learningActive)
		{
			this.cursorController.LockCursor();
			this.mouseLocked = true;
			this.reticle.SetActive(true);
		}
	}

	// Token: 0x060000AF RID: 175 RVA: 0x0000688C File Offset: 0x00004A8C
	public void UnlockMouse()
	{
		this.cursorController.UnlockCursor();
		this.mouseLocked = false;
		this.reticle.SetActive(false);
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x000068AC File Offset: 0x00004AAC
	private void PauseGame()
	{
		UnlockMouse();
		Time.timeScale = 0f;
		this.gamePaused = true;
		this.pauseText.SetActive(true);
		this.baldiNod.SetActive(true);
		this.baldiShake.SetActive(true);
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000068E3 File Offset: 0x00004AE3
	private void UnpauseGame()
	{
		LockMouse();
		Time.timeScale = 1f;
		this.gamePaused = false;
		this.pauseText.SetActive(false);
		this.baldiNod.SetActive(false);
		this.baldiShake.SetActive(false);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000691C File Offset: 0x00004B1C
	public void ActivateSpoopMode()
	{
		this.spoopMode = true;
		if (currentChallenge == ChallengeManagerScript.ChallengeType.Grapple)
		{
			LoseItem(0);
		}
		this.entrance_0.Lower();
		this.entrance_1.Lower();
		this.entrance_2.Lower();
		this.entrance_3.Lower();
		this.baldiTutor.SetActive(false);
		if (PlayerPrefs.GetString("CurrentMode") != "freeRun")
		{
			this.baldi.SetActive(true);
		}
		else
		{
			this.baldi.SetActive(false);
		}
		this.principal.SetActive(true);
		this.crafters.SetActive(true);
		this.playtime.SetActive(true);
		this.gottaSweep.SetActive(true);
		this.bully.SetActive(true);
		this.firstPrize.SetActive(true);
		this.joeWonderer.SetActive(true);
		this.wizard.SetActive(true);
		larry.SetActive(true);
		inverja.SetActive(true);
		this.audioDevice.PlayOneShot(this.aud_Hang);
		this.learnMusic.Stop();
		this.schoolMusic.Stop();
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00006A1F File Offset: 0x00004C1F
	private void ActivateFinaleMode()
	{
		this.finaleMode = true;
		this.entrance_0.Raise();
		this.entrance_1.Raise();
		this.entrance_2.Raise();
		this.entrance_3.Raise();
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00006A54 File Offset: 0x00004C54
	public void GetAngry(float value)
	{
		if (!this.spoopMode)
		{
			this.ActivateSpoopMode();
		}
		this.baldiScrpt.GetAngry(value);
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00006A70 File Offset: 0x00004C70
	public void ActivateLearningGame()
	{
		this.learningActive = true;
		this.UnlockMouse();
		this.tutorBaldi.Stop();
		if (!this.spoopMode)
		{
			this.schoolMusic.Stop();
			this.learnMusic.volume = 3f;
			this.learnMusic.Play();
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00006AC4 File Offset: 0x00004CC4
	public void DeactivateLearningGame(GameObject subject)
	{
		this.learningActive = false;
		Destroy(subject);
		this.LockMouse();
		if (this.player.stamina < 100f)
		{
			this.player.stamina = 100f;
		}
		if (!this.spoopMode)
		{
			this.schoolMusic.volume = 0.3f;
			this.schoolMusic.Play();
			this.learnMusic.Stop();
		}
		if (this.notebooks == 1 & !this.spoopMode)
		{
			this.quarter.SetActive(true);
			this.tutorBaldi.PlayOneShot(this.aud_Prize);
			return;
		}
		if (this.notebooks == 12 & (this.mode == "story" || this.mode == "freeRun"))
		{
			this.audioDevice.PlayOneShot(this.aud_AllNotebooks, 0.8f);
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00006BB0 File Offset: 0x00004DB0
	private void IncreaseItemSelection()
	{
		this.itemSelected++;
		if (this.itemSelected > 2)
		{
			this.itemSelected = 0;
		}
		UpdateItemSelection();
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00006C10 File Offset: 0x00004E10
	private void DecreaseItemSelection()
	{
		this.itemSelected--;
		if (this.itemSelected < 0)
		{
			this.itemSelected = 2;
		}
		UpdateItemSelection();
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00006C6E File Offset: 0x00004E6E
	private void UpdateItemSelection()
	{
		this.itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], itemSelect.anchoredPosition.y, 0f);
		this.UpdateItemName();
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00006CA4 File Offset: 0x00004EA4
	public void CollectItem(int item_ID)
	{
		if (this.item[0] == 0)
		{
			this.item[0] = item_ID;
			this.itemSlot[0].texture = this.itemTextures[item_ID];
			itemNameArray[0] = itemPickupNames[item_ID];
		}
		else if (this.item[1] == 0)
		{
			this.item[1] = item_ID;
			this.itemSlot[1].texture = this.itemTextures[item_ID];
			itemNameArray[1] = itemPickupNames[item_ID];
		}
		else if (this.item[2] == 0)
		{
			this.item[2] = item_ID;
			this.itemSlot[2].texture = this.itemTextures[item_ID];
			itemNameArray[2] = itemPickupNames[item_ID];
		}
		else
		{
			this.item[this.itemSelected] = item_ID;
			this.itemSlot[this.itemSelected].texture = this.itemTextures[item_ID];
			itemNameArray[itemSelected] = itemPickupNames[item_ID];
		}
		this.UpdateItemName();
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00006D60 File Offset: 0x00004F60
	private void UseItem()
	{
		if (this.item[this.itemSelected] != 0 && SceneManager.GetActiveScene().name != "Shop")
		{
			if (this.item[this.itemSelected] == 1)
			{
				this.audioDevice.PlayOneShot(this.eating);
				this.player.stamina += this.player.maxStamina * 2f;
				this.ResetItem();
				return;
			}
			if (this.item[this.itemSelected] == 2)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
				{
					raycastHit.collider.GetComponent<SwingingDoorScript>().LockDoor(15f);
					this.ResetItem();
					return;
				}
			}
			else if (this.item[this.itemSelected] == 3)
			{
				RaycastHit raycastHit2;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit2) && (raycastHit2.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit2.transform.position) <= 10f))
				{
					raycastHit2.collider.gameObject.GetComponent<DoorScript>().UnlockDoor();
					raycastHit2.collider.gameObject.GetComponent<DoorScript>().OpenDoor();
					this.ResetItem();
					return;
				}
			}
			else
			{
				if (this.item[this.itemSelected] == 4)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.bsodaSpray, this.playerTransform.position, this.cameraTransform.rotation);
					this.ResetItem();
					this.player.ResetGuilt("drink", 1f);
					this.audioDevice.PlayOneShot(this.aud_Soda);
					this.audioDevice.clip = this.drinking;
					this.audioDevice.PlayScheduled(1.5);
					this.audioDevice.clip = null;
					return;
				}
				if (this.item[this.itemSelected] == 5)
				{
					if (this.darkEvent)
					{
						this.lightEnabled = true;
						this.playerLight.enabled = true;
						this.playerLight.range = 100f;
						this.playerLight.type = LightType.Spot;
						this.playerLight.color = Color.white;
						this.ResetItem();
					}
					return;
				}
				if (this.item[this.itemSelected] == 6)
				{
					RaycastHit raycastHit3;
					if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit3) && (raycastHit3.collider.name == "TapePlayer" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f))
					{
						raycastHit3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
						this.ResetItem();
						return;
					}
				}
				else
				{
					if (this.item[this.itemSelected] == 7)
					{
						UnityEngine.Object.Instantiate<GameObject>(this.alarmClock, this.playerTransform.position, this.cameraTransform.rotation).GetComponent<AlarmClockScript>().baldi = this.baldiScrpt;
						this.ResetItem();
						return;
					}
					if (this.item[this.itemSelected] == 8)
					{
						RaycastHit raycastHit4;
						if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit4) && (raycastHit4.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit4.transform.position) <= 10f))
						{
							raycastHit4.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
							this.ResetItem();
							this.audioDevice.PlayOneShot(this.aud_Spray);
							return;
						}
					}
					else if (this.item[this.itemSelected] == 9)
					{
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						if (this.player.jumpRope)
						{
							if (PlayerPrefs.GetFloat("Erased") == 0f)
							{
								this.aPopUp.QueueAchievement(this.aPopUp.achievements[4]);
								PlayerPrefs.SetFloat("Erased", 1f);
							}
							this.player.DeactivateJumpRope();
							this.playtimeScript.Disappoint();
							this.ResetItem();
							this.player.ResetGuilt("drawing", 1f);
							return;
						}
						RaycastHit raycastHit5;
						if (Physics.Raycast(ray, out raycastHit5) && raycastHit5.collider.name == "1st Prize")
						{
							this.firstPrizeScript.GoCrazy();
							this.ResetItem();
							return;
						}
					}
					else
					{
						if (this.item[this.itemSelected] == 10)
						{
							int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 3f));
							if (num == 0 && currentChallenge != ChallengeManagerScript.ChallengeType.NoItems)
							{
								int num2 = new System.Random().Next(1, itemSprites.Length);
								this.CollectItem(num2);
								this.imageCooldown = 3f;
								this.imageText.fontSize = 20;
								this.imageText.text = "You just collected:" + this.itemNames[num2];
								this.imageText.color = Color.blue;
							}
							else if (num == 1 && spoopMode && notebooks >= 2)
							{
								this.teleportCount = 0;
								this.teleports = 0;
								this.teleports = Mathf.RoundToInt(UnityEngine.Random.Range(5f, 12f));
								EndAllGrapplingHooks();
								if (PlayerScript.instance.currentHeldNumberBallon != null) PlayerScript.instance.UnpickupNumberBallon();
								this.TeleportationalWand();
							}
							else if (num == 2)
							{
								this.magicCooldown = 25f;
								this.higherThanRun = true;
								this.imageCooldown = 2f;
								this.imageText.fontSize = 15;
								this.imageText.text = "Your run speed is lower than your walk speed";
								this.higherThanRun = true;
								this.player.runSpeed -= this.player.walkSpeed;
							}
							else if (num == 3)
							{
								this.magicCooldown = 20f;
								this.imageCooldown = 2f;
								this.imageText.fontSize = 15;
								this.imageText.text = "Your walk speed is higher than your run speed";
								this.lowerThanRun = true;
								this.player.walkSpeed += this.player.runSpeed;
							}
							this.ResetItem();
							return;
						}
						if (this.item[this.itemSelected] == 11)
						{
							if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.raycastHit) && (this.raycastHit.collider.tag == "NPC" & this.raycastHit.collider.name != "Baldi" & this.stunnedCharacter == this.testEnemy))
							{
								this.Stun(25f, this.raycastHit.collider.gameObject);
								this.raycastHit.collider.gameObject.GetComponent<NavMeshAgent>().speed = 0f;
								base.StartCoroutine(this.LightningBoltAlpha());
								if (PlayerPrefs.GetFloat("Struck") == 0f)
								{
									this.aPopUp.QueueAchievement(this.aPopUp.achievements[16]);
									PlayerPrefs.SetFloat("Struck", 1f);
								}
								this.ResetItem();
								return;
							}
						}
						else if (this.item[this.itemSelected] == 12)
						{
							if (Vector3.Distance(this.player.transform.position, this.baldi.transform.position) <= 20f)
							{
								if (PlayerPrefs.GetFloat("KnifeSteamed") == 0f)
								{
									this.aPopUp.QueueAchievement(this.aPopUp.achievements[3]);
									PlayerPrefs.SetFloat("KnifeSteamed", 1f);
								}
								this.audioDevice.PlayOneShot(this.baldiScrpt.aud_NoKnife);
								this.baldiScrpt.RemoveKnife(15f);
								this.ResetItem();
								return;
							}
						}
						else if (this.item[this.itemSelected] == 13)
						{
							if (this.eventStarts & this.eventStarted)
							{
								this.audioDevice.PlayOneShot(this.falseAlarm);
								this.StopEvent();
								this.ResetItem();
								return;
							}
						}
						else if (this.item[this.itemSelected] == 14)
						{
							if (this.inJoeGame)
							{
								this.joe.GetSad();
								this.ResetItem();
							}
							if (this.player.jumpRope)
							{
								if (PlayerPrefs.GetFloat("Erased") == 0f)
								{
									this.aPopUp.QueueAchievement(this.aPopUp.achievements[4]);
									PlayerPrefs.SetFloat("Erased", 1f);
								}
								this.player.DeactivateJumpRope();
								this.playtimeScript.Disappoint();
								this.ResetItem();
								this.player.ResetGuilt("drawing", 1f);
								return;
							}
						}
						else
						{
							if (this.item[this.itemSelected] == 15)
							{
								GrapplingHookScript grapplingHook = Instantiate(grapplingHookObject, playerTransform.position, cameraTransform.rotation).GetComponent<GrapplingHookScript>();
								grapplingHook.player = playerTransform;
								ResetItem();
								return;
							}
							if (this.item[this.itemSelected] == 16)
							{
								RaycastHit raycastHit6;
								if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit6) && (raycastHit6.collider.name == "MemeComputer" & Vector3.Distance(this.playerTransform.position, raycastHit6.transform.position) <= 10f))
								{
									int num3 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
									this.memePlayer.SetActive(true);
									this.memePlayer.GetComponent<VideoPlayer>().clip = this.videoClips[num3];
									PlayerPrefs.SetFloat(num3.ToString(), 1f);
									for (int i = 0; i < 5; i++)
									{
										int num4 = 0;
										if (PlayerPrefs.GetFloat(i.ToString()) == 1f)
										{
											num4++;
											if (num4 == 5 & PlayerPrefs.GetFloat("AllMemes") == 0f)
											{
												this.aPopUp.QueueAchievement(this.aPopUp.achievements[8]);
												PlayerPrefs.SetFloat("AllMemes", 1f);
											}
										}
									}
									if (PlayerPrefs.GetFloat("Memes") != 1f)
									{
										this.aPopUp.QueueAchievement(this.aPopUp.achievements[6]);
										PlayerPrefs.SetFloat("Memes", 1f);
									}
									this.ResetItem();
									return;
								}
							}
							else if (this.item[this.itemSelected] == 17)
							{
								RaycastHit raycastHit7;
								if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit7) && (raycastHit7.collider.name == "TuneComputer" & Vector3.Distance(this.playerTransform.position, raycastHit7.transform.position) <= 10f))
								{
									int num5 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f));
									this.tunePlayer.SetActive(true);
									this.tunePlayer.GetComponent<VideoPlayer>().clip = this.tuneClips[num5];
									PlayerPrefs.SetFloat(num5.ToString() + " Tunes", 1f);
									for (int j = 0; j < 3; j++)
									{
										int num6 = 0;
										if (PlayerPrefs.GetFloat(j.ToString() + " Tunes") == 1f)
										{
											num6++;
											if (num6 == 3 & PlayerPrefs.GetFloat("AllTunes") == 0f)
											{
												this.aPopUp.QueueAchievement(this.aPopUp.achievements[9]);
												PlayerPrefs.SetFloat("AllTunes", 1f);
											}
										}
									}
									if (PlayerPrefs.GetFloat("Tunes") != 1f)
									{
										this.aPopUp.QueueAchievement(this.aPopUp.achievements[7]);
										PlayerPrefs.SetFloat("Tunes", 1f);
									}
									this.ResetItem();
									return;
								}
							}
							else if (this.item[this.itemSelected] == 18)
							{
								this.radarScript.cooldown = 15f;
								this.radar.SetActive(true);
								this.radarMode = true;
								this.ResetItem();
							}
							else if (item[itemSelected] == 19)
							{
								Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
								RaycastHit raycastHit;
								if (Physics.Raycast(ray, out raycastHit))
								{
									MeshRenderer meshRenderer = raycastHit.collider.GetComponent<MeshRenderer>();
									if (meshRenderer != null)
									{
										if (meshRenderer.sharedMaterial == crackedWall || meshRenderer.sharedMaterial == crackedWallDark)
										{
											StartCoroutine(BreakWall(meshRenderer));
											chainSawUses--;
											if (chainSawUses == 0)
											{
												ResetItem();
											}
										}
									}
								}
							}
							else if (item[itemSelected] == 20)
							{
								Instantiate(boomBoxPrefab, playerTransform.position - cameraTransform.forward * 10f, Quaternion.identity);
								ResetItem();
							}
							else if (item[itemSelected] == 21)
							{
								StartCoroutine(SpellOnWizard());
								woodWantUses--;
								if (woodWantUses == 0)
								{
									ResetItem();
								}
								WizardScript wizardScript2 = wizard.GetComponent<WizardScript>();
								if (wizardScript2.inSpell)
								{
									player.StopSpell();
								}
							}
							else if (item[itemSelected] == 22)
							{
								Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
								RaycastHit raycastHit;
								if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider.CompareTag("BoomBox"))
								{
									BoomBoxScript boomBox = raycastHit.collider.GetComponent<BoomBoxScript>();
									boomBox.AddVolume(UnityEngine.Random.Range(0.1f, 0.3f));
								}
							}
							else if (item[itemSelected] == 23)
							{
								RaycastHit raycastHit;
								Physics.Raycast(playerTransform.position, cameraTransform.forward, out raycastHit, float.PositiveInfinity);
								print(raycastHit.collider.name);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060000BC RID: 188 RVA: 0x000079ED File Offset: 0x00005BED
	public void ResetItem()
	{
		this.item[this.itemSelected] = 0;
		this.itemSlot[this.itemSelected].texture = this.itemTextures[0];
		this.UpdateItemName();
	}
	IEnumerator BreakWall(MeshRenderer wallRenderer)
	{
		audioDevice.PlayOneShot(chainSaw);
		yield return new WaitForSeconds(1f);
		audioDevice.PlayOneShot(snap);
		wallRenderer.material = blankMaterial;
		wallRenderer.GetComponent<MeshCollider>().enabled = false;
		float timer = 10f;
		yield return new WaitForSeconds(timer);
		wallRenderer.material = crackedWall;
		wallRenderer.GetComponent<MeshCollider>().enabled = true;
		yield break;
	}
	IEnumerator SpellOnWizard()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit))
		{
			WizardScript wizardScript = raycastHit.collider.GetComponent<WizardScript>();
			if (wizardScript != null)
			{
				if (wizardScript.targetingPlayer && !wizardScript.brokeWand)
				{
					Rigidbody rb = wizardScript.GetComponent<Rigidbody>();
					wizardScript.agent.isStopped = true;
					rb.isKinematic = false;
					rb.AddForce(cameraTransform.forward * 600f * Time.deltaTime, ForceMode.Impulse);
					audioDevice.PlayOneShot(aud_lightningBolt);
					wizardScript.ClearAudioQueue();
					wizardScript.RemoveAllinQueue();
					wizardScript.audioDevice.Stop();
					wizardScript.QueueAudio(wizardScript.aud_Gah);
					wizardScript.QueueAudio(wizardScript.aud_DoToWand);
					wizardScript.QueueAudio(wizardScript.aud_Pay);
					wizardScript.BreakWand(false);
					yield return StartCoroutine(wizardScript.WaitForAllAudio());
					yield return new WaitUntil(() => !wizardScript.agent.isStopped);
					rb.isKinematic = true;
				}
			}
		}
		yield break;
	}
	// Token: 0x060000BD RID: 189 RVA: 0x00007A1D File Offset: 0x00005C1D
	public void LoseItem(int id)
	{
		this.item[id] = 0;
		this.itemSlot[id].texture = this.itemTextures[0];
		this.UpdateItemName();
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00007A44 File Offset: 0x00005C44
	private void UpdateItemName()
	{
		int num = Mathf.RoundToInt(UnityEngine.Random.Range(0, 9));
		Color color = Color.white;
		switch (num)
		{
			case 0: color = Color.red; break;
			case 1: color = Color.black; break;
			case 2: color = Color.blue; break;
			case 3: color = Color.cyan; break;
			case 4: color = Color.magenta; break;
			case 5: color = Color.black; break;
			case 6: color = Color.white; break;
			case 7: color = Color.white; break;
			case 8: color = Color.grey; break;
			case 9: color = Color.black; break;
		}
		this.itemText.material.SetColor("_OutlineColor", color);
		this.itemText.text = "Item: " + this.itemNames[this.item[this.itemSelected]];
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00007BA4 File Offset: 0x00005DA4
	public void ExitReached()
	{
		this.exitsReached++;
		this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
		this.audioDevice.clip = this.aud_MachineQuiet;
		this.audioDevice.loop = true;
		this.audioDevice.Play();
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00007BFD File Offset: 0x00005DFD
	public void DespawnCrafters()
	{
		this.crafters.SetActive(false);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00007C0C File Offset: 0x00005E0C
	private void TeleportationalWand()
	{
		this.teleporting = true;
		int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 19f));
		this.playerTransform.position = new Vector3(this.spawnPoints[num].position.x, this.playerTransform.position.y, this.spawnPoints[num].position.z);
		this.cooldown = 0.5f;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00007C84 File Offset: 0x00005E84
	private void Stun(float stunTime, GameObject characterStunned)
	{
		this.stunnedCharacter = characterStunned;
		this.noSpeed = true;
		this.stunned = true;
		this.stunTime = stunTime;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00007CA2 File Offset: 0x00005EA2
	private IEnumerator StartEvent()
	{
		this.eventStopped = false;
		this.audioDevice.PlayOneShot(this.eventStartedBell);
		this.timer = 3f;
		yield return new WaitForSeconds(this.timer);
		if (!this.eventStarted)
		{
			this.eventStarted = true;
		}
		this.chance = Mathf.RoundToInt(UnityEngine.Random.Range(1f, 1f));
		this.imageCooldown = 3f;
		this.imageText.text = this.eventTexts[this.chance];
		this.imageText.color = Color.white;
		switch (chance)
		{
			case 0:
				this.audioDevice.PlayOneShot(this.blackout);
				this.darkEvent = true;
				RenderSettings.ambientLight = new Color(0.23f, 0.23f, 0.23f, 1f);
				this.player.GetComponent<Light>().enabled = false;
				this.NormalText();
				break;
			case 1: base.StartCoroutine(this.Fog("fog")); break;
			case 2: this.christmasEvent.SetActive(true); break;
			case 3: this.hardSlapEvent = true; audioDevice.PlayOneShot(snap); break;
			case 4: this.saleEvent = true; break;
			case 5:
				this.officeParty = true;
				this.partyAtOffice.SetActive(true);
				this.audioDevice.clip = this.aud_party;
				this.audioDevice.loop = true;
				this.audioDevice.Play();
				for (int i = 0; i < this.presents.Length; i++)
				{
					this.presents[i].SetActive(true);
				}
				break;
			case 6: floodEvent = true; flood.SetActive(true); break;
			case 7:
				mysteryRoom.gameObject.SetActive(true);
				break;
		}
		this.eventTimer = Mathf.RoundToInt(UnityEngine.Random.Range(25f, 45f));
		yield break;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00007CB4 File Offset: 0x00005EB4
	public void StopEvent()
	{
		this.eventStopped = true;
		switch (chance)
		{
			case 0:
				this.audioDevice.PlayOneShot(this.restored);
				this.darkEvent = false;
				RenderSettings.ambientLight = this.originalAmbientLight;
				if (!this.player.isUnderground)
				{
					this.player.GetComponent<Light>().enabled = true;
					this.playerLight.range = 500f;
					this.playerLight.color = new Color(0.3f, 0.3f, 0.3f);
					this.playerLight.type = LightType.Directional;
				}
				break;
			case 1: StartCoroutine(this.Fog("fogOut")); break;
			case 2: this.christmasEvent.SetActive(false); break;
			case 3: this.hardSlapEvent = false; break;
			case 4: this.saleEvent = false; break;
			case 5:
				this.audioDevice.loop = false;
				this.audioDevice.clip = null;
				this.audioDevice.Stop();
				this.officeParty = false;
				gottaSweep.GetComponent<SweepScript>().GoHome();
				this.partyAtOffice.SetActive(false);
				break;
			case 6:
				StartCoroutine(flood.GetComponent<FloodEventScript>().StopEvent());
				floodEvent = false;
				break;
			case 7:
				mysteryRoom.StopEvent();
				break;
		}
		if ((this.eventTimer <= 0f & this.eventStarts & this.eventStarted) || this.eventStopped)
		{
			this.eventCooldown = Mathf.RoundToInt(UnityEngine.Random.Range(45f, 70f));
		}
		this.eventStarted = false;
		this.eventStarts = false;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00007E3B File Offset: 0x0000603B
	public IEnumerator Fog(string type)
	{
		if (type == "fog")
		{
			MaterialManagerScript.instance.SetAllToFog();
			this.audioDevice.clip = this.mus_fog;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
			RenderSettings.fogColor = Color.white;
			RenderSettings.fogDensity = 0f;
			RenderSettings.fog = true;
			while (RenderSettings.fogDensity < 0.1f)
			{
				RenderSettings.fogDensity += 0.025f * Time.deltaTime;
				yield return null;
			}
			RenderSettings.fogDensity = 0.1f;
		}
		if (type == "fogOut")
		{
			MaterialManagerScript.instance.SetAllToOutline();
			this.audioDevice.loop = false;
			this.audioDevice.clip = null;
			this.audioDevice.Stop();
			RenderSettings.fogDensity = 0.1f;
			while (RenderSettings.fogDensity > 0f)
			{
				RenderSettings.fogDensity -= 0.025f * Time.deltaTime;
				yield return null;
			}
			RenderSettings.fogDensity = 0f;
			RenderSettings.fog = false;
		}
		yield break;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00007E54 File Offset: 0x00006054
	public void ActivateJoeGame()
	{
		this.inJoeGame = true;
		this.UnlockMouse();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.joeGame, this.joeGame.transform.position, Quaternion.identity);
		gameObject.GetComponent<JoeGameScript>().gc = base.gameObject.GetComponent<GameControllerScript>();
		gameObject.GetComponent<JoeGameScript>().joe = this.joe;
		this.player.frozenPosition = this.playerTransform.position;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00007ECA File Offset: 0x000060CA
	public void DeactivateJoeGame(GameObject theGame)
	{
		this.player.jw.playCooldown = 15f;
		this.LockMouse();
		this.inJoeGame = false;
		Destroy(theGame);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00007EF4 File Offset: 0x000060F4
	public void NormalText()
	{
		this.fpsCounter.GetComponent<Text>().color = Color.white;
		this.notebookCount.color = Color.white;
		this.warning.GetComponent<Text>().color = Color.white;
		this.cs.clockText.color = Color.black;
		this.cs.dateText.color = Color.black;
		this.moneyText.color = Color.white;
		itemText.color = Color.white;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00007F78 File Offset: 0x00006178
	public void BlackText()
	{
		this.fpsCounter.GetComponent<Text>().color = Color.black;
		this.notebookCount.color = Color.black;
		this.warning.GetComponent<Text>().color = Color.black;
		this.cs.clockText.color = Color.black;
		this.cs.dateText.color = Color.black;
		this.moneyText.color = Color.black;
		this.itemText.material.SetColor("_OutlineColor", Color.white);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00007FFC File Offset: 0x000061FC
	public void UpdateMoneyCount()
	{
		if (this.money < 1f)
		{
			this.moneyText.text = "Money: ¢" + this.money;
			return;
		}
		this.moneyText.text = "Money: $" + this.money;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x0000806C File Offset: 0x0000626C
	public void MoneyUse()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
		{
			if (raycastHit.collider.name == "BSODAMachine" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f & this.money >= this.cost[0])
			{
				this.money -= this.cost[0];
				this.UpdateMoneyCount();
				this.audioDevice.PlayOneShot(this.boughtSound);
				this.CollectItem(4);
				return;
			}
			if (raycastHit.collider.name == "ZestyMachine" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f & this.money >= this.cost[1])
			{
				this.money -= this.cost[1];
				this.UpdateMoneyCount();
				this.audioDevice.PlayOneShot(this.boughtSound);
				this.CollectItem(1);
				return;
			}
			if (raycastHit.collider.name == "PayPhone" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f & this.money >= this.cost[2] & !this.payPhonedPaided)
			{
				this.money -= this.cost[2];
				this.UpdateMoneyCount();
				this.payPhonedPaided = true;
				raycastHit.collider.gameObject.GetComponent<TapePlayerScript>().Play();
				this.player.ResetGuilt("phone", 1f);
				return;
			}
			if (raycastHit.collider.name == "CrazyItemMachine" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f & this.money >= this.cost[2] & !this.payPhonedPaided)
			{
				int item_ID = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 18f));
				this.CollectItem(item_ID);
				this.money -= this.cost[1];
				this.audioDevice.PlayOneShot(this.boughtSound);
				this.UpdateMoneyCount();
				return;
			}
			if ((raycastHit.collider.name == "CrazyItemMachine" || raycastHit.collider.name == "PayPhone" || raycastHit.collider.name == "BSODAMachine" || raycastHit.collider.name == "ZestyMachine") & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f)
			{
				base.StartCoroutine(this.NotEnoughMoney());
			}
		}
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0000838A File Offset: 0x0000658A
	public IEnumerator NotEnoughMoney()
	{
		this.notEnoughMoneyText.color = new Color(255f, 255f, 255f, 1f);
		this.rawImage.color = new Color(this.rawImage.color.r, this.rawImage.color.g, this.rawImage.color.b, 1f);
		this.notEnoughMoney.SetActive(true);
		while (this.notEnoughMoneyText.color.a > 0f & this.rawImage.color.a > 0f)
		{
			this.notEnoughMoneyText.color -= new Color(0f, 0f, 0f, Time.deltaTime);
			this.rawImage.color -= new Color(0f, 0f, 0f, Time.deltaTime);
			yield return null;
		}
		this.notEnoughMoney.SetActive(false);
		yield break;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00008399 File Offset: 0x00006599
	public IEnumerator LightningBoltAlpha()
	{
		this.audioDevice.PlayOneShot(this.aud_lightningBolt);
		this.lightningBolt.transform.position = this.stunnedCharacter.transform.position + Vector3.up * 3f;
		this.lightningBoltSprite.color = new Color(this.lightningBoltSprite.color.r, this.lightningBoltSprite.color.g, this.lightningBoltSprite.color.b, 1f);
		while (this.lightningBoltSprite.color.a > 0f)
		{
			this.lightningBoltSprite.color -= new Color(0f, 0f, 0f, Time.deltaTime);
			yield return null;
		}
		this.lightningBoltSprite.color = new Color(this.lightningBoltSprite.color.r, this.lightningBoltSprite.color.g, this.lightningBoltSprite.color.b, 0f);
		yield break;
	}
	void UpdateItemNameArray(string name, int itemID)
	{
		itemNameArray[itemID] = name;
	}
	public bool HasItemInItemSlot(int itemID, int itemSlot)
	{
		return item[itemSlot] == itemID;
	}
	public bool IsFloatBetweenValue(float target, float min, float max)
	{
		max = max < min || max == min ? min + Mathf.Epsilon : max;
		if (target > min && target < max || target == min || target == max)
		{
			return true;
		}
		return false;
	}
	public void EndAllGrapplingHooks()
	{
		if (grapplingHooks.Count > 0)
		{
			foreach (GrapplingHookScript grapplingHook in grapplingHooks)
			{
				grapplingHook.EndGrapplingHook();
			}
		}
	}
	public IEnumerator TakeAwayShopPointsAdded()
	{
		int currentPoints = PlayerPrefs.GetInt("ShopPoints");
		PlayerPrefs.SetInt("ShopPoints", currentPoints - pointsCurrentlyAdded);
		shopPointsAdder.rectTransform.anchoredPosition = Vector2.zero;
		shopPoints.gameObject.SetActive(true);
		shopPointsAdder.gameObject.SetActive(true);
		shopPoints.text = "Shop Points:\n" + currentPoints;
		shopPointsAdder.text = (-pointsCurrentlyAdded).ToString();
		RectTransform rectTransform = shopPointsAdder.rectTransform;
		Vector2 direction = (rectTransform.anchoredPosition - shopPoints.rectTransform.anchoredPosition);
		while (direction.magnitude > 12f)
		{
			direction = (rectTransform.anchoredPosition - shopPoints.rectTransform.anchoredPosition);
			shopPointsAdder.rectTransform.anchoredPosition -= direction.normalized * Time.unscaledDeltaTime * 65f;
			yield return null;
		}
		int newPoints = currentPoints - pointsCurrentlyAdded;
		shopPoints.text = "Shop Points:\n" + newPoints;
		shopPointsAdder.text = string.Empty;
		shopPointsAdder.rectTransform.anchoredPosition = Vector2.zero;
		yield break;
	}

	public CursorControllerScript cursorController;

	// Token: 0x04000105 RID: 261
	public PlayerScript player;

	// Token: 0x04000106 RID: 262
	public Transform playerTransform;

	// Token: 0x04000107 RID: 263
	public Transform cameraTransform;

	// Token: 0x04000108 RID: 264
	public EntranceScript entrance_0;

	// Token: 0x04000109 RID: 265
	public EntranceScript entrance_1;

	// Token: 0x0400010A RID: 266
	public EntranceScript entrance_2;

	// Token: 0x0400010B RID: 267
	public EntranceScript entrance_3;

	// Token: 0x0400010C RID: 268
	public GameObject baldiTutor;

	// Token: 0x0400010D RID: 269
	public GameObject baldi;

	// Token: 0x0400010E RID: 270
	public BaldiScript baldiScrpt;

	// Token: 0x0400010F RID: 271
	public AudioClip aud_Prize;

	// Token: 0x04000110 RID: 272
	public AudioClip aud_AllNotebooks;

	// Token: 0x04000111 RID: 273
	public GameObject principal;

	// Token: 0x04000112 RID: 274
	public GameObject crafters;

	// Token: 0x04000113 RID: 275
	public GameObject playtime;

	// Token: 0x04000114 RID: 276
	public PlaytimeScript playtimeScript;

	// Token: 0x04000115 RID: 277
	public GameObject gottaSweep;

	// Token: 0x04000116 RID: 278
	public GameObject bully;

	// Token: 0x04000117 RID: 279
	public GameObject firstPrize;

	// Token: 0x04000118 RID: 280
	public FirstPrizeScript firstPrizeScript;

	// Token: 0x04000119 RID: 281
	public GameObject quarter;

	// Token: 0x0400011A RID: 282
	public AudioSource tutorBaldi;

	// Token: 0x0400011B RID: 283
	public string mode;

	// Token: 0x0400011C RID: 284
	public int notebooks;

	// Token: 0x0400011D RID: 285
	public GameObject[] notebookPickups;

	// Token: 0x0400011E RID: 286
	public int failedNotebooks;

	// Token: 0x0400011F RID: 287
	public float time;

	// Token: 0x04000120 RID: 288
	public bool spoopMode;

	// Token: 0x04000121 RID: 289
	public bool finaleMode;

	// Token: 0x04000122 RID: 290
	public bool debugMode;

	// Token: 0x04000123 RID: 291
	public bool mouseLocked;

	// Token: 0x04000124 RID: 292
	public int exitsReached;

	// Token: 0x04000125 RID: 293
	public int itemSelected;

	// Token: 0x04000126 RID: 294
	public int[] item = new int[3];

	// Token: 0x04000127 RID: 295
	public RawImage[] itemSlot = new RawImage[3];

	// Token: 0x04000128 RID: 296
	private string[] itemNames = new string[]
	{
		"Nothing here",
		"Rainbow Bread Sandwich",
		"Ceil Barrier Lock",
		"Principal's Keys",
		"Rainbow Throwup-er",
		"Flashlight",
		"Bubbling Sound Tape",
		"Alarm Clock",
		"Attractive NoSquee",
		"Eraser",
		"Magic Wand",
		"Zip Zapper",
		"Knife Steamer",
		"False Alarm",
		"Safety Flamethrower",
		"Grappling Hook",
		"Meme CD",
		"Song CD",
		"Radar",
		"Colorful Saw",
		"Boom Box",
		"Wood Wand",
		"Amp",
	};
	public static GameControllerScript i;
	// Token: 0x04000129 RID: 297
	public Text itemText;

	// Token: 0x0400012A RID: 298
	public UnityEngine.Object[] items = new UnityEngine.Object[10];

	// Token: 0x0400012B RID: 299
	public Texture[] itemTextures = new Texture[16];
	public Sprite[] itemSprites;
	public string[] itemPickupNames;
	public string[] itemNameArray = new string[3];

	// Token: 0x0400012C RID: 300
	public GameObject bsodaSpray;

	// Token: 0x0400012D RID: 301
	public GameObject alarmClock;

	// Token: 0x0400012E RID: 302
	public Text notebookCount;

	// Token: 0x0400012F RID: 303
	public GameObject pauseText;

	// Token: 0x04000130 RID: 304
	public GameObject highScoreText;

	// Token: 0x04000131 RID: 305
	public GameObject baldiNod;

	// Token: 0x04000132 RID: 306
	public GameObject baldiShake;

	// Token: 0x04000133 RID: 307
	public GameObject warning;

	// Token: 0x04000134 RID: 308
	public GameObject reticle;

	// Token: 0x04000135 RID: 309
	public RectTransform itemSelect;

	// Token: 0x04000136 RID: 310
	private int[] itemSelectOffset;

	// Token: 0x04000137 RID: 311
	private bool gamePaused;

	// Token: 0x04000138 RID: 312
	private bool learningActive;

	// Token: 0x04000139 RID: 313
	private float gameOverDelay;

	// Token: 0x0400013A RID: 314
	private AudioSource audioDevice;

	// Token: 0x0400013B RID: 315
	public AudioClip aud_Soda;

	// Token: 0x0400013C RID: 316
	public AudioClip aud_Spray;

	// Token: 0x0400013D RID: 317
	public AudioClip aud_buzz;

	// Token: 0x0400013E RID: 318
	public AudioClip aud_Hang;

	// Token: 0x0400013F RID: 319
	public AudioClip aud_MachineQuiet;

	// Token: 0x04000140 RID: 320
	public AudioClip aud_MachineStart;

	// Token: 0x04000141 RID: 321
	public AudioClip aud_MachineRev;

	// Token: 0x04000142 RID: 322
	public AudioClip aud_MachineLoop;

	// Token: 0x04000143 RID: 323
	public AudioClip aud_Switch;

	// Token: 0x04000144 RID: 324
	public AudioSource schoolMusic;

	// Token: 0x04000145 RID: 325
	public AudioSource learnMusic;

	// Token: 0x04000146 RID: 326
	public AudioClip boughtSound;

	// Token: 0x04000147 RID: 327
	public Transform[] spawnPoints = new Transform[20];

	// Token: 0x04000148 RID: 328
	public bool teleporting;

	// Token: 0x04000149 RID: 329
	public float cooldown;

	// Token: 0x0400014A RID: 330
	private float increaseFactor;

	// Token: 0x0400014B RID: 331
	private int teleportCount;

	// Token: 0x0400014C RID: 332
	private int teleports;

	// Token: 0x0400014D RID: 333
	public GameObject bg;

	// Token: 0x0400014E RID: 334
	public Text imageText;

	// Token: 0x0400014F RID: 335
	public float imageCooldown;

	// Token: 0x04000150 RID: 336
	private RaycastHit raycastHit;

	// Token: 0x04000151 RID: 337
	private float stunTime;

	// Token: 0x04000152 RID: 338
	public bool stunned;

	// Token: 0x04000153 RID: 339
	public bool noSpeed;

	// Token: 0x04000154 RID: 340
	public bool noKnife;

	// Token: 0x04000155 RID: 341
	public float noKnifeTime;

	// Token: 0x04000156 RID: 342
	public Transform baldiTransform;

	// Token: 0x04000157 RID: 343
	public float magicCooldown;

	// Token: 0x04000158 RID: 344
	public bool higherThanRun;

	// Token: 0x04000159 RID: 345
	public bool lowerThanRun;

	// Token: 0x0400015A RID: 346
	public Color originalAmbientLight;

	// Token: 0x0400015B RID: 347
	private string[] eventTexts = new string[]
	{
		"Oh noes, it seems like Flipper was to\n lazy to pay the electricity bill. Gosh Dangit",
		"Uh oh, It seems like someone turned on the fog machine. Achikey Hackiey Shakiey",
		"Oooo It's snowing, seems like we're preparing for Christmas",
		"Oh no, Flipper slapped so much his knife snapped. Seems like he needs a new knife",
		"ANNOUNCEMENT: All vending machines are on sale! But hurry up they won't be on sale for long!",
		"Ooo um ok, the Official's birthday has come earlier than usual and he's hosting a party for it. Come to the party yourself!",
		"Uh oh, something happened and now there's flood. Oh ye, make sure you don't get into and watch for the whirlpools!!",
		"Uh huh, seems like there this mystery room here in the mansion for a limited time. Maybe get there before it disappears"
	};

	// Token: 0x0400015C RID: 348
	public AudioClip eventStartedBell;

	// Token: 0x0400015D RID: 349
	public float eventCooldown;

	// Token: 0x0400015E RID: 350
	public float timer;

	// Token: 0x0400015F RID: 351
	public bool eventStarts;

	// Token: 0x04000160 RID: 352
	public int chance;

	// Token: 0x04000161 RID: 353
	public float eventTimer;

	// Token: 0x04000162 RID: 354
	public bool eventStarted;

	// Token: 0x04000163 RID: 355
	public AudioClip aud_MagicWand;

	// Token: 0x04000164 RID: 356
	public GameObject christmasEvent;

	// Token: 0x04000165 RID: 357
	private bool eventStopped;

	// Token: 0x04000166 RID: 358
	public GameObject joeGame;

	// Token: 0x04000167 RID: 359
	public bool inJoeGame;

	// Token: 0x04000168 RID: 360
	public bool started;

	// Token: 0x04000169 RID: 361
	public JoeWondererScript joe;

	// Token: 0x0400016A RID: 362
	public GameObject joeWonderer;

	// Token: 0x0400016B RID: 363
	public GameObject stunnedCharacter;

	// Token: 0x0400016C RID: 364
	public GameObject testEnemy;

	// Token: 0x0400016D RID: 365
	public Light playerLight;

	// Token: 0x0400016E RID: 366
	public bool hardSlapEvent;

	// Token: 0x0400016F RID: 367
	public GameObject fpsCounter;

	// Token: 0x04000170 RID: 368
	public ClockTextScript cs;

	// Token: 0x04000171 RID: 369
	public GameObject clockText;

	// Token: 0x04000172 RID: 370
	public GameObject dateText;

	// Token: 0x04000173 RID: 371
	public GameObject wizard;

	// Token: 0x04000174 RID: 372
	public GameObject miniMap;

	// Token: 0x04000175 RID: 373
	public float money;

	// Token: 0x04000176 RID: 374
	public Text moneyText;

	// Token: 0x04000177 RID: 375
	public GameObject notEnoughMoney;

	// Token: 0x04000178 RID: 376
	public Text notEnoughMoneyText;

	// Token: 0x04000179 RID: 377
	public RawImage rawImage;

	// Token: 0x0400017A RID: 378
	public AchievementPopUpScript aPopUp;

	// Token: 0x0400017B RID: 379
	public AchievementsScript ach;

	// Token: 0x0400017C RID: 380
	public float[] cost = new float[3];

	// Token: 0x0400017D RID: 381
	public bool payPhonedPaided;

	// Token: 0x0400017E RID: 382
	public bool officeParty;

	// Token: 0x0400017F RID: 383
	public bool darkEvent;

	// Token: 0x04000180 RID: 384
	public GameObject partyAtOffice;

	// Token: 0x04000181 RID: 385
	public GameObject principalsOffice;

	// Token: 0x04000182 RID: 386
	public GameObject lightningBolt;

	// Token: 0x04000183 RID: 387
	public SpriteRenderer lightningBoltSprite;

	// Token: 0x04000184 RID: 388
	public GameObject memePlayer;

	// Token: 0x04000185 RID: 389
	public GameObject tunePlayer;

	// Token: 0x04000186 RID: 390
	public VideoClip[] videoClips = new VideoClip[5];

	// Token: 0x04000187 RID: 391
	public VideoClip[] tuneClips = new VideoClip[3];

	// Token: 0x04000188 RID: 392
	private bool saleEvent;

	// Token: 0x04000189 RID: 393
	public AudioClip eating;

	// Token: 0x0400018A RID: 394
	public AudioClip drinking;

	// Token: 0x0400018B RID: 395
	public AudioClip aud_lightningBolt;

	// Token: 0x0400018C RID: 396
	public GameObject radar;

	// Token: 0x0400018D RID: 397
	public bool radarMode;

	// Token: 0x0400018E RID: 398
	public RadarScript radarScript;

	// Token: 0x0400018F RID: 399
	public bool lightEnabled;

	// Token: 0x04000190 RID: 400
	public MeshRenderer[] bsodaMachines;

	// Token: 0x04000191 RID: 401
	public MeshRenderer[] zestyMachines;

	// Token: 0x04000192 RID: 402
	public MeshRenderer[] crazyItemMachines;

	// Token: 0x04000193 RID: 403
	public Material[] machineSales;

	// Token: 0x04000194 RID: 404
	public Material[] normal;

	// Token: 0x04000195 RID: 405
	public SpriteRenderer payPhone;

	// Token: 0x04000196 RID: 406
	public Sprite payPhone_normal;

	// Token: 0x04000197 RID: 407
	public GameObject[] presents = new GameObject[3];

	// Token: 0x04000198 RID: 408
	public Sprite payPhone_sale;

	// Token: 0x04000199 RID: 409
	public Transform[] partyPositions;

	// Token: 0x0400019A RID: 410
	public AudioClip falseAlarm;

	// Token: 0x0400019C RID: 412
	public AudioClip blackout;

	// Token: 0x0400019D RID: 413
	public AudioClip restored;

	// Token: 0x0400019E RID: 414
	public AudioClip aud_party;

	// Token: 0x0400019F RID: 415
	public AudioClip mus_fog;
	public AudioClip chainSaw;
	public AudioClip snap;
	public int chainSawUses;
	public bool floodEvent;
	public GameObject flood;
	public GameObject boomBoxPrefab;
	public int woodWantUses;
	public GameObject grapplingHookObject;
	public Material crackedWall;
	public Material crackedWallDark;
	public Material blankMaterial;
	public Material skyboxMaterial;
	public bool haveEvents = true;
	public Text shopPoints;
	public Text shopPointsAdder;
	public List<GrapplingHookScript> grapplingHooks = new List<GrapplingHookScript>();
	public int pointsCurrentlyAdded;
	bool takeAwayShopPointsCoroutineStarted;
	bool grapplingChallenge;
	public bool hydrationChallenge;
	public bool stealthyChallenge;
	public bool speedyChallenege;

	public GameObject inverja;
	public GameObject larry;
	public Image blackScreen;
	public ChallengeManagerScript.ChallengeType currentChallenge;
	public Gradient[] staminaOutlineColors;
	public MysteryRoomScript mysteryRoom;
}