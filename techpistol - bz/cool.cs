using System;
using System.IO;
using UnityEngine;
using UWE;
using Oculus.Newtonsoft.Json.Utilities;

// Token: 0x02000002 RID: 2
public class cool : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void OnParticleCollision(GameObject taget)
	{
		try
		{
			int num = UWE.Utils.OverlapSphereIntoSharedBuffer(taget.transform.position, this.cof.CannonExplosionDamageRange, -1, 0);
			for (int i = 0; i < num; i++)
			{
				GameObject entityRoot = UWE.Utils.GetEntityRoot(UWE.Utils.sharedColliderBuffer[i].gameObject);
				bool flag = entityRoot != null && entityRoot.GetComponent<LiveMixin>() != null;
				if (flag)
				{
					entityRoot.GetComponent<LiveMixin>().TakeDamage(this.cof.CannonDamage, entityRoot.transform.position, DamageType.Explosive, null);
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x04000001 RID: 1
	private config cof = JsonUtility.FromJson<config>(File.ReadAllText(Environment.CurrentDirectory + "/QMods/techpistol/config.json"));
}
