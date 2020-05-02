using System;
using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using UnityEngine;

namespace techpistol
{
	// Token: 0x02000006 RID: 6
	public static class techpistol
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000039A0 File Offset: 0x00001BA0
		public static void Patch()
		{
			techpistol.darktest = AssetBundle.LoadFromFile(Environment.CurrentDirectory + "/QMods/techpistol/Assets");
			Sprite Sprite = (techpistol.darktest.LoadAsset<Sprite>("Icon"));
			TechType techType = TechTypeHandler.AddTechType("techpistol", "tech pistol", "tech pistol", true);
			SpriteHandler.RegisterSprite(techType, Sprite);
			GunPrefab gunPrefab = new GunPrefab("techpistol", "WorldEntities/Tools/techpistol", techType);
			PrefabHandler.RegisterPrefab(gunPrefab);
			CraftDataHandler.SetEquipmentType(techType, EquipmentType.Hand);
			
			RecipeData recipeData = new RecipeData
			{
				craftAmount = 1,
				Ingredients = new List<Ingredient>
				{
					new Ingredient(TechType.Silver, 1),
					new Ingredient(TechType.TitaniumIngot, 2),
					new Ingredient(TechType.Lubricant, 1),
					new Ingredient(TechType.EnameledGlass, 3)
				}
			};
			CraftDataHandler.SetTechData(techType, recipeData);
			CraftDataHandler.SetCraftingTime(techType, 5f);
			CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, techType, new string[]
			{
				"Personal",
				"Tools",
				"techpistol"
			});
			CraftDataHandler.SetItemSize(techType, 2, 2);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003AB8 File Offset: 0x00001CB8
		public static T AgComponent<T>(this GameObject objec) where T : Component
		{
			bool flag = objec.GetComponent<T>() != null;
			T result;
			if (flag)
			{
				result = objec.GetComponent<T>();
			}
			else
			{
				result = objec.AddComponent<T>();
			}
			return result;
		}

		// Token: 0x0400001D RID: 29
		public static AssetBundle darktest;
	}
}
