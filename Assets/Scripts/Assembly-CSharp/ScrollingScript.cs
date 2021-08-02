using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class ScrollingScript : MonoBehaviour
{
	// Token: 0x06000159 RID: 345 RVA: 0x0000E45E File Offset: 0x0000C65E
	public void Start()
	{
		this.scrollingSpeed = 500f;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000E46C File Offset: 0x0000C66C
	private void Update()
	{
		float keyValue = 0f;
		if (Input.GetKey(KeyCode.UpArrow))
		{
			keyValue = 1f;
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			keyValue = -1f;
		}
		Vector3 increaseVelocity = Vector3.zero;
		increaseVelocity += Vector3.up * -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollingSpeed * multipler;
		increaseVelocity += Vector3.up * keyValue;
		transform.position += increaseVelocity;
                Vector3 position = transform.position;
                position.y = Mathf.Clamp(position.y, limits.y, limits.x);
                transform.position = position;
	}
	// Token: 0x040002D9 RID: 729
	public float scrollingSpeed;
	float multipler = 50f;
	public Vector2 limits = new Vector2(1000f, -500f);
}
