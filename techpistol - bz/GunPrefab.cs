using System;
using System.Collections.Generic;
using SMLHelper.V2.Assets;
using SMLHelper.V2.MonoBehaviours;
using UnityEngine;

namespace techpistol
{
	// Token: 0x02000005 RID: 5
	internal class GunPrefab : ModPrefab
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000358E File Offset: 0x0000178E
		public GunPrefab(string classId, string prefabFileName, TechType techType = TechType.None) : base(classId, prefabFileName, techType)
		{
			base.ClassID = classId;
			base.PrefabFileName = prefabFileName;
			base.TechType = techType;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000035B4 File Offset: 0x000017B4
		public override GameObject GetGameObject()
		{
			GameObject result;
			try
			{
				GameObject gameObject = techpistol.darktest.LoadAsset<GameObject>("techpistol.prefab");
				MeshRenderer[] componentsInChildren = Radical.FindChild(gameObject, "HandGun").GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer meshRenderer in componentsInChildren)
				{
					meshRenderer.material.shader = Shader.Find("MarmosetUBER");
					meshRenderer.material.SetColor("_Emission", new Color(1f, 1f, 1f));
				}
				gameObject.transform.Find("Cannonmode/shoot/shoo").gameObject.AgComponent<cool>();
				gameObject.AgComponent<PrefabIdentifier>().ClassId = base.ClassID;
				gameObject.AgComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;
				gameObject.AgComponent<Pickupable>().isPickupable = true;
				gameObject.AgComponent<TechTag>().type = base.TechType;
				gameObject.AgComponent<Fixer>().techType = base.TechType;
				gameObject.AgComponent<Fixer>().ClassId = base.ClassID;
				WorldForces worldForces = gameObject.AgComponent<WorldForces>();
				Rigidbody useRigidbody = gameObject.AgComponent<Rigidbody>();
				worldForces.underwaterGravity = 0f;
				worldForces.useRigidbody = useRigidbody;
				EnergyMixin energyMixin = gameObject.AgComponent<EnergyMixin>();
				energyMixin.storageRoot = Radical.FindChild(gameObject, "BatteryRoot").AgComponent<ChildObjectIdentifier>();
				energyMixin.allowBatteryReplacement = true;
				energyMixin.compatibleBatteries = new List<TechType>
				{
					TechType.PrecursorIonBattery,
					TechType.Battery,
					TechType.PrecursorIonPowerCell,
					TechType.PowerCell
				};
				energyMixin.batteryModels = new EnergyMixin.BatteryModels[]
				{
					new EnergyMixin.BatteryModels
					{
						techType = TechType.PrecursorIonPowerCell,
						model = gameObject.transform.Find("BatteryRoot/PrecursorIonPowerCell").gameObject
					},
					new EnergyMixin.BatteryModels
					{
						techType = TechType.Battery,
						model = gameObject.transform.Find("BatteryRoot/Battery").gameObject
					},
					new EnergyMixin.BatteryModels
					{
						techType = TechType.PrecursorIonBattery,
						model = gameObject.transform.Find("BatteryRoot/PrecursorIonBattery").gameObject
					},
					new EnergyMixin.BatteryModels
					{
						techType = TechType.PowerCell,
						model = gameObject.transform.Find("BatteryRoot/PowerCell").gameObject
					}
				};
				Gun gun = gameObject.AgComponent<Gun>();
				RepulsionCannon component = CraftData.InstantiateFromPrefab(TechType.RepulsionCannon, false).GetComponent<RepulsionCannon>();
				StasisRifle component2 = CraftData.InstantiateFromPrefab(TechType.StasisRifle, false).GetComponent<StasisRifle>();
				PropulsionCannon component3 = CraftData.InstantiateFromPrefab(TechType.PropulsionCannon, false).GetComponent<PropulsionCannon>();
				Welder component4 = CraftData.InstantiateFromPrefab(TechType.Welder, false).GetComponent<Welder>();
				VFXFabricating vfxfabricating = Radical.FindChild(gameObject, "HandGun").AgComponent<VFXFabricating>();
				vfxfabricating.localMinY = -3f;
				vfxfabricating.localMaxY = 3f;
				vfxfabricating.posOffset = new Vector3(0f, 0f, 0f);
				vfxfabricating.eulerOffset = new Vector3(0f, 90f, -90f);
				vfxfabricating.scaleFactor = 1f;
				gun.shoot1 = component.shootSound;
				gun.shoot2 = component2.fireSound;
				gun.xulikai = component2.chargeBegin;
				gun.modechang = component3.shootSound;
				gun.laseroopS = component4.weldSound;
				gun.mainCollider = gameObject.GetComponent<BoxCollider>();
				gun.ikAimRightArm = true;
				gun.useLeftAimTargetOnPlayer = true;
				UnityEngine.Object.Destroy(component2);
				UnityEngine.Object.Destroy(component3);
				UnityEngine.Object.Destroy(component);
				UnityEngine.Object.Destroy(component4);
				result = gameObject;
			}
			catch
			{
				Console.WriteLine("初始化错误");
				result = new GameObject();
			}
			return result;
		}
	}
}
