using System;
using System.IO;
using SMLHelper.V2.Utility;
using UnityEngine;
using UWE;
using LitJson;

namespace techpistol
{
	// Token: 0x02000004 RID: 4
	[RequireComponent(typeof(EnergyMixin))]
	internal class Gun : PlayerTool, IProtoEventListener
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002131 File Offset: 0x00000331
		public override string animToolName
		{
			get
			{
				return "Flashlight";
			}
		}
		
		// Token: 0x06000005 RID: 5 RVA: 0x00002138 File Offset: 0x00000338
		private void Tagetlaser(float range, LineRenderer lineder)
		{
			GameObject gameObject;
			float num;
			bool target = Targeting.GetTarget(Player.main.gameObject, range, out gameObject, out num);
			if (target)
			{
				lineder.SetPosition(0, Radical.FindChild(base.gameObject, "Point").transform.position);
				lineder.SetPosition(1, Player.main.camRoot.mainCamera.transform.forward * num + Player.main.camRoot.mainCamera.transform.position);
				this.dis.transform.position = Player.main.camRoot.mainCamera.transform.forward * num + Player.main.camRoot.mainCamera.transform.position;
			}
			else
			{
				lineder.SetPosition(0, Radical.FindChild(base.gameObject, "Point").transform.position);
				lineder.SetPosition(1, Player.main.camRoot.mainCamera.transform.forward * range + Player.main.camRoot.mainCamera.transform.position);
				this.dis.transform.position = Player.main.camRoot.mainCamera.transform.forward * range + Player.main.camRoot.mainCamera.transform.position;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022D5 File Offset: 0x000004D5
		public override void OnHolster()
		{
			this.reset();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022E0 File Offset: 0x000004E0
		private void Start()
		{
			try
			{
				Console.WriteLine("初始化");
				this.cof = JsonUtility.FromJson<config>(File.ReadAllText(Environment.CurrentDirectory + "/QMods/techpistol/config.json"));
				Console.WriteLine("初始化");
				GameObject gameObject = techpistol.darktest.LoadAsset<GameObject>("Dis.prefab");
				Console.WriteLine("初始化");
				this.dis = UnityEngine.Object.Instantiate<GameObject>(gameObject, base.transform.position, base.transform.rotation);
				Console.WriteLine("初始化");
				this.par[0] = Radical.FindChild(base.gameObject, "modech").GetComponent<ParticleSystem>();
				Console.WriteLine("初始化");
				GameObject gameObject2 = Radical.FindChild(base.gameObject, "Cannonmode");
				Console.WriteLine("初始化");
				this.par[1] = Radical.FindChild(gameObject2, "Ball").GetComponent<ParticleSystem>();
				Console.WriteLine("初始化");
				this.par[2] = Radical.FindChild(gameObject2, "Charge").GetComponent<ParticleSystem>();
				Console.WriteLine("初始化");
				this.par[3] = Radical.FindChild(gameObject2, "shoot").GetComponent<ParticleSystem>();
				Console.WriteLine("初始化");
				GameObject gameObject3 = Radical.FindChild(base.gameObject, "Lasermode");
				Console.WriteLine("初始化");
				this.par[4] = Radical.FindChild(gameObject3, "Laser").GetComponent<ParticleSystem>();
				Console.WriteLine("初始化");
				this.Line[1] = Radical.FindChild(gameObject3, "line").GetComponent<LineRenderer>();
				Console.WriteLine("初始化");
				GameObject gameObject4 = Radical.FindChild(base.gameObject, "Scalemode");
				Console.WriteLine("初始化");
				this.par[5] = Radical.FindChild(gameObject4, "Laser").GetComponent<ParticleSystem>();
				Console.WriteLine("初始化");
				this.par[6] = Radical.FindChild(gameObject4, "Lasersamm").GetComponent<ParticleSystem>();
				Console.WriteLine("初始化");
				this.Line[2] = Radical.FindChild(gameObject4, "linebig").GetComponent<LineRenderer>();
				Console.WriteLine("初始化");
				this.Line[3] = Radical.FindChild(gameObject4, "linesamm").GetComponent<LineRenderer>();
				Console.WriteLine("初始化");
				this.textname = base.gameObject.transform.Find("miazhun/name").gameObject.GetComponent<TextMesh>();
				Console.WriteLine("初始化");
				this.textblood = base.gameObject.transform.Find("miazhun/blood").gameObject.GetComponent<TextMesh>();
				Console.WriteLine("初始化");
				this.textmode = base.gameObject.transform.Find("modech/modehud").gameObject.GetComponent<TextMesh>();
				Console.WriteLine("初始化");
			}
			catch
			{
				Console.WriteLine("初始化错误");
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000025E4 File Offset: 0x000007E4
		public override bool OnAltDown()
		{
			bool flag = this.energyMixin.charge > 0f;
			if (flag)
			{
				this.par[0].Play();
				this.reset();
				this.mode++;
				FMODUWE.PlayOneShot(this.modechang, base.transform.position, 1f);
				bool flag2 = this.mode == 1;
				if (flag2)
				{
					this.textmode.text = "Cannon";
					this.time = 10f;
					this.time2 = 10f;
				}
				bool flag3 = this.mode == 2;
				if (flag3)
				{
					this.textmode.text = "Laser";
				}
				bool flag4 = this.mode == 3;
				if (flag4)
				{
					this.textmode.text = "Scale";
				}
				bool flag5 = this.mode == 4;
				if (flag5)
				{
					this.textmode.text = "Standby";
					this.mode = 0;
				}
			}
			else
			{
				this.textmode.text = "No Power";
			}
			return true;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002708 File Offset: 0x00000908
		public void Update()
		{
			bool flag = this.energyMixin.charge > 0f && base.isDrawn;
			if (flag)
			{
				bool laserStart = this.LaserStart;
				if (laserStart)
				{
					this.Tagetlaser(this.cof.LaserRange, this.Line[1]);
				}
				bool flag2 = this.mode == 3;
				if (flag2)
				{
					bool scalebig = this.Scalebig;
					if (scalebig)
					{
						this.Tagetlaser(this.cof.ScaleRange, this.Line[2]);
					}
					bool scalesamm = this.Scalesamm;
					if (scalesamm)
					{
						this.Tagetlaser(this.cof.ScaleRange, this.Line[3]);
					}
					bool flag3 = Input.GetKeyDown(KeyCode.Q) && this.energyMixin.charge > 0f;
					if (flag3)
					{
						bool flag4 = !this.Scalebig;
						if (flag4)
						{
							Radical.FindChild(this.dis, "scale").GetComponent<ParticleSystem>().Play();
							this.par[6].Play();
							FMODUWE.PlayOneShot(this.shoot1, base.transform.position, 1f);
							this.Scalesamm = true;
						}
					}
					bool keyUp = Input.GetKeyUp(KeyCode.Q);
					if (keyUp)
					{
						this.par[6].Stop();
						Radical.FindChild(this.dis, "scale").GetComponent<ParticleSystem>().Stop();
						this.Line[3].SetPosition(0, new Vector3(0f, 0f, 0f));
						this.Line[3].SetPosition(1, new Vector3(0f, 0f, 0f));
						this.Scalesamm = false;
					}
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000028C8 File Offset: 0x00000AC8
		public void LateUpdate()
		{
			bool isDrawn = base.isDrawn;
			if (isDrawn)
			{
				bool flag = this.energyMixin.charge > 0f;
				if (flag)
				{
					bool flag2 = this.mode == 1 && this.CannonStart;
					if (flag2)
					{
						this.energyMixin.ConsumeEnergy(0.05f);
						bool flag3 = this.time > 0f;
						if (flag3)
						{
							this.time -= 5f * Time.deltaTime;
						}
						else
						{
							this.par[2].Stop();
							bool flag4 = this.time2 > 0f;
							if (flag4)
							{
								this.time2 -= 5f * Time.deltaTime;
							}
							else
							{
								this.energyMixin.ConsumeEnergy(30f);
								FMODUWE.PlayOneShot(this.shoot1, base.transform.position, 1f);
								FMODUWE.PlayOneShot(this.shoot2, base.transform.position, 1f);
								this.par[1].Stop();
								this.par[1].Clear();
								this.par[3].transform.rotation = Player.main.camRoot.mainCamera.transform.rotation;
								this.par[3].Play();
								this.time = 10f;
								this.time2 = 10f;
								this.CannonStart = false;
							}
						}
					}
					bool flag5 = this.mode == 2 && this.LaserStart;
					if (flag5)
					{
						this.energyMixin.ConsumeEnergy(0.2f);
						this.par[4].gameObject.transform.Rotate(Vector3.forward * 5f);
						GameObject gameObject;
						float num;
						bool flag6 = Targeting.GetTarget(Player.main.gameObject, this.cof.LaserRange, out gameObject, out num) && gameObject.GetComponentInChildren<LiveMixin>();
						if (flag6)
						{
							UWE.Utils.GetEntityRoot(gameObject).GetComponentInChildren<LiveMixin>().TakeDamage(this.cof.LaserDamage, gameObject.transform.position, DamageType.Explosive);
						}
						else
						{
							bool flag7 = gameObject;
							if (flag7)
							{
								DamageSystem.RadiusDamage(this.cof.LaserDamage, gameObject.transform.position, 1f, DamageType.Explosive, UWE.Utils.GetEntityRoot(gameObject));
							}
						}
					}
					bool flag8 = this.mode == 3;
					if (flag8)
					{
						bool scalebig = this.Scalebig;
						if (scalebig)
						{
							this.energyMixin.ConsumeEnergy(0.1f);
							this.par[5].gameObject.transform.Rotate(Vector3.forward * 5f);
							GameObject gameObject2;
							float num2;
							bool flag9 = Targeting.GetTarget(Player.main.gameObject, this.cof.ScaleRange, out gameObject2, out num2) && gameObject2.GetComponentInChildren<Creature>();
							if (flag9)
							{
								float x = UWE.Utils.GetEntityRoot(gameObject2).transform.localScale.x;
								UWE.Utils.GetEntityRoot(gameObject2).GetComponentInChildren<Creature>().SetScale(x + this.cof.ScaleUpspeed);
							}
						}
						bool scalesamm = this.Scalesamm;
						if (scalesamm)
						{
							this.energyMixin.ConsumeEnergy(0.1f);
							this.par[6].gameObject.transform.Rotate(-Vector3.forward * 5f);
							GameObject gameObject3;
							float num3;
							bool flag10 = Targeting.GetTarget(Player.main.gameObject, this.cof.ScaleRange, out gameObject3, out num3) && gameObject3.GetComponentInChildren<Creature>();
							if (flag10)
							{
								float x2 = UWE.Utils.GetEntityRoot(gameObject3).transform.localScale.x;
								UWE.Utils.GetEntityRoot(gameObject3).GetComponentInChildren<Creature>().SetScale(x2 - this.cof.ScaleDownspeed);
							}
						}
					}
				}
				else
				{
					this.mode = 0;
					this.reset();
				}
				GameObject gameObject4;
				float num4;
				bool flag11 = Targeting.GetTarget(Player.main.gameObject, this.cof.HealthDetectionRange, out gameObject4, out num4) && gameObject4.GetComponentInChildren<LiveMixin>();
				if (flag11)
				{
					string text = gameObject4.GetComponentInChildren<LiveMixin>().health.ToString();
					string text2 = gameObject4.GetComponentInChildren<LiveMixin>().name;
					text2 = text2.Replace("(Clone)", "");
					text2 = text2.Replace("Leviathan", "");
					bool flag12 = text == "0";
					if (flag12)
					{
						text = "0-death";
					}
					this.textname.text = text2;
					this.textblood.text = text;
				}
				else
				{
					this.textname.text = "No target";
					this.textblood.text = "";
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002DCC File Offset: 0x00000FCC
		public override bool OnRightHandDown()
		{
			bool flag = this.energyMixin.charge > 0f;
			if (flag)
			{
				bool flag2 = this.mode == 1;
				if (flag2)
				{
					this.CannonStart = true;
					this.par[1].Play();
					this.par[2].Play();
					this.time = 10f;
					this.time2 = 10f;
					this.xulikai.StartEvent();
				}
				bool flag3 = this.mode == 2;
				if (flag3)
				{
					FMODUWE.PlayOneShot(this.shoot1, base.transform.position, 1f);
					this.laseroopS.Play();
					this.LaserStart = true;
					this.par[4].Play();
					Radical.FindChild(this.dis, "Laserend").GetComponent<ParticleSystem>().Play();
				}
				bool flag4 = this.mode == 3 && !this.Scalesamm;
				if (flag4)
				{
					FMODUWE.PlayOneShot(this.shoot1, base.transform.position, 1f);
					this.par[5].Play();
					this.Scalebig = true;
					Radical.FindChild(this.dis, "scale").GetComponent<ParticleSystem>().Play();
				}
			}
			return true;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002F20 File Offset: 0x00001120
		public override bool OnRightHandUp()
		{
			bool flag = this.mode == 1;
			if (flag)
			{
				this.par[1].Stop();
				this.par[2].Stop();
				this.par[3].Stop();
				this.xulikai.Stop(false);
				this.CannonStart = false;
			}
			bool flag2 = this.mode == 2;
			if (flag2)
			{
				this.Line[1].SetPosition(0, new Vector3(0f, 0f, 0f));
				this.Line[1].SetPosition(1, new Vector3(0f, 0f, 0f));
				this.par[4].Stop();
				this.laseroopS.Stop();
				this.LaserStart = false;
				Radical.FindChild(this.dis, "Laserend").GetComponent<ParticleSystem>().Stop();
			}
			bool flag3 = this.mode == 3;
			if (flag3)
			{
				Radical.FindChild(this.dis, "scale").GetComponent<ParticleSystem>().Stop();
				this.Line[2].SetPosition(0, new Vector3(0f, 0f, 0f));
				this.Line[2].SetPosition(1, new Vector3(0f, 0f, 0f));
				this.par[5].Stop();
				this.Scalebig = false;
			}
			return true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000309C File Offset: 0x0000129C
		public void reset()
		{
			Radical.FindChild(this.dis, "scale").GetComponent<ParticleSystem>().Stop();
			Radical.FindChild(this.dis, "Laserend").GetComponent<ParticleSystem>().Stop();
			this.par[1].Stop();
			this.par[2].Stop();
			this.par[3].Stop();
			this.par[4].Stop();
			this.par[5].Stop();
			this.par[6].Stop();
			this.par[4].gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
			this.laseroopS.Stop();
			this.xulikai.Stop(true);
			this.LaserStart = false;
			this.CannonStart = false;
			this.Scalebig = false;
			this.Scalesamm = false;
			this.Line[1].SetPosition(0, new Vector3(0f, 0f, 0f));
			this.Line[1].SetPosition(1, new Vector3(0f, 0f, 0f));
			this.Line[2].SetPosition(0, new Vector3(0f, 0f, 0f));
			this.Line[2].SetPosition(1, new Vector3(0f, 0f, 0f));
			this.Line[3].SetPosition(0, new Vector3(0f, 0f, 0f));
			this.Line[3].SetPosition(1, new Vector3(0f, 0f, 0f));
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000326C File Offset: 0x0000146C
		public void OnProtoSerialize(ProtobufSerializer serializer)
		{
			Console.WriteLine("保存电池存档");
			string contents = null;
			GameObject battery = this.energyMixin.GetBattery();
			bool flag = battery;
			if (flag)
			{
				CraftData.GetTechType(battery);
				bool flag2 = CraftData.GetTechType(battery) == TechType.PrecursorIonBattery;
				if (flag2)
				{
					contents = "PrecursorIonBattery";
				}
				bool flag3 = CraftData.GetTechType(battery) == TechType.Battery;
				if (flag3)
				{
					contents = "Battery";
				}
				bool flag4 = CraftData.GetTechType(battery) == TechType.PowerCell;
				if (flag4)
				{
					contents = "PowerCell";
				}
				bool flag5 = CraftData.GetTechType(battery) == TechType.PrecursorIonPowerCell;
				if (flag5)
				{
					contents = "PrecursorIonPowerCell";
				}
				bool flag6 = CraftData.GetTechType(battery) != TechType.None && this.energyMixin.HasItem();
				if (flag6)
				{
					File.WriteAllText(SaveUtils.GetCurrentSaveDataDir() + "/" + base.GetComponent<PrefabIdentifier>().Id + ".type", contents);
					File.WriteAllText(SaveUtils.GetCurrentSaveDataDir() + "/" + base.GetComponent<PrefabIdentifier>().Id + ".charge", this.energyMixin.charge.ToString());
				}
			}
			else
			{
				contents = "None";
				File.WriteAllText(SaveUtils.GetCurrentSaveDataDir() + "/" + base.GetComponent<PrefabIdentifier>().Id + ".type", contents);
				File.WriteAllText(SaveUtils.GetCurrentSaveDataDir() + "/" + base.GetComponent<PrefabIdentifier>().Id + ".charge", "0");
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000033E8 File Offset: 0x000015E8
		public void OnProtoDeserialize(ProtobufSerializer serializer)
		{
			bool flag = this.energyMixin == null;
			if (flag)
			{
				this.energyMixin = base.GetComponent<EnergyMixin>();
			}
			bool flag2 = File.Exists(SaveUtils.GetCurrentSaveDataDir() + "/" + base.GetComponent<PrefabIdentifier>().Id + ".type");
			if (flag2)
			{
				string a = File.ReadAllText(SaveUtils.GetCurrentSaveDataDir() + "/" + base.GetComponent<PrefabIdentifier>().Id + ".type");
				float num = float.Parse(File.ReadAllText(SaveUtils.GetCurrentSaveDataDir() + "/" + base.GetComponent<PrefabIdentifier>().Id + ".charge"));
				bool flag3 = a != "None";
				if (flag3)
				{
					bool flag4 = a == "PrecursorIonBattery";
					if (flag4)
					{
						this.energyMixin.SetBattery(TechType.PrecursorIonBattery, num / 500f);
					}
					bool flag5 = a == "Battery";
					if (flag5)
					{
						this.energyMixin.SetBattery(TechType.Battery, num / 100f);
					}
					bool flag6 = a == "PowerCell";
					if (flag6)
					{
						this.energyMixin.SetBattery(TechType.PowerCell, num / 200f);
					}
					bool flag7 = a == "PrecursorIonPowerCell";
					if (flag7)
					{
						this.energyMixin.SetBattery(TechType.PrecursorIonPowerCell, num / 1000f);
					}
				}
			}
		}

		// Token: 0x0400000A RID: 10
		public FMODAsset shoot1;

		// Token: 0x0400000B RID: 11
		public FMODAsset shoot2;

		// Token: 0x0400000C RID: 12
		public FMOD_StudioEventEmitter xulikai;

		// Token: 0x0400000D RID: 13
		public FMOD_CustomLoopingEmitter laseroopS;

		// Token: 0x0400000E RID: 14
		public FMODAsset modechang;

		// Token: 0x0400000F RID: 15
		public ParticleSystem[] par = new ParticleSystem[10];

		// Token: 0x04000010 RID: 16
		public LineRenderer[] Line = new LineRenderer[10];

		// Token: 0x04000011 RID: 17
		public GameObject dis;

		// Token: 0x04000012 RID: 18
		public bool CannonStart = false;

		// Token: 0x04000013 RID: 19
		public bool LaserStart = false;

		// Token: 0x04000014 RID: 20
		public bool Scalebig = false;

		// Token: 0x04000015 RID: 21
		public bool Scalesamm = false;

		// Token: 0x04000016 RID: 22
		public float time;

		// Token: 0x04000017 RID: 23
		public float time2;

		// Token: 0x04000018 RID: 24
		public int mode;

		// Token: 0x04000019 RID: 25
		public TextMesh textname;

		// Token: 0x0400001A RID: 26
		public TextMesh textblood;

		// Token: 0x0400001B RID: 27
		public TextMesh textmode;

		// Token: 0x0400001C RID: 28
		public config cof;
	}
}
