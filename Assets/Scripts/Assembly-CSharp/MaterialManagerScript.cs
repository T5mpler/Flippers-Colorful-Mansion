using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MaterialManagerScript : MonoBehaviour
{
	public List<MaterialClass> materialClasses;
	public Material fogEffectionMaterial;
	public static MaterialManagerScript instance;
	private void Start()
	{
		instance = this;
	}
	private void Update()
	{
		for (int i = 0; i < materialClasses.Count; i++)
		{
			if (materialClasses[i].spriteRenderer.material == materialClasses[i].outlineMaterial && materialClasses[i].spriteRenderer.gameObject.activeSelf)
			{
				materialClasses[i].outlineMaterial.SetTexture("_MainTex", materialClasses[i].spriteRenderer.sprite.texture);
			}
		}
	}
	public void SetMateiralToOutline(MaterialClass materialClass)
	{
		materialClass.spriteRenderer.material = materialClass.outlineMaterial;
	}
	public void SetAllToOutline()
	{
		for (int i = 0; i < materialClasses.Count; i++)
		{
			materialClasses[i].spriteRenderer.material = materialClasses[i].outlineMaterial;
		}
	}
	public void SetMaterialToFog(MaterialClass materialClass)
	{
		materialClass.spriteRenderer.material = fogEffectionMaterial;
	}
	public void SetAllToFog()
	{
		for (int i = 0; i < materialClasses.Count; i++)
		{
			materialClasses[i].spriteRenderer.material = fogEffectionMaterial;
		}
	}
	[Serializable]
	public class MaterialClass
	{
		public string name;
		public SpriteRenderer spriteRenderer;
		public Material outlineMaterial;
	}
}
