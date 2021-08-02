using UnityEngine;

public class WaterFountainScript : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 10f) && raycastHit.collider.name == name)
			{
				PlayerScript.instance.stamina = PlayerScript.instance.maxStamina;
			}
		}
	}
}