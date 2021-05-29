using On.RoR2.Artifacts;
using RoR2;
using UnityEngine;

namespace DifficultyMod.Modifiers
{
    public class SacrificeModifier : IModifier
    {
	    private static Xoroshiro128Plus rngSeed = new Xoroshiro128Plus(0UL);
	    private PickupDropTable dropTable = Resources.Load<PickupDropTable>("DropTables/dtSacrificeArtifact");
	    public void Init()
	    {
		    On.RoR2.Artifacts.SacrificeArtifactManager.OnServerCharacterDeath += OnServerCharDeath;
		    On.RoR2.Artifacts.SacrificeArtifactManager.OnServerStageBegin += OnServerStageBegin;
		    On.RoR2.Artifacts.SacrificeArtifactManager.OnPrePopulateSceneServer += OnPopulate;

	    }

	    private void OnPopulate(SacrificeArtifactManager.orig_OnPrePopulateSceneServer orig, SceneDirector scenedirector)
	    {
		    
	    }

	    private void OnServerStageBegin(SacrificeArtifactManager.orig_OnServerStageBegin orig, Stage stage)
	    {
			rngSeed.ResetSeed(Run.instance.treasureRng.nextUlong);
		    orig(stage);
	    }

	    private void OnServerCharDeath(SacrificeArtifactManager.orig_OnServerCharacterDeath orig, DamageReport damageReport)
	    {
		    if (!damageReport.victimMaster)
		    {
			    return;
		    }
		    if (damageReport.attackerTeamIndex == damageReport.victimTeamIndex && damageReport.victimMaster.minionOwnership.ownerMaster)
		    {
			    return;
		    }
		    float expAdjustedDropChancePercent = Util.GetExpAdjustedDropChancePercent(3f, damageReport.victim.gameObject);
		    Debug.LogFormat("Drop chance from {0}: {1}", new object[]
		    {
			    damageReport.victimBody,
			    expAdjustedDropChancePercent
		    });
		    if (Util.CheckRoll(expAdjustedDropChancePercent, 0f, null))
			{
			    PickupIndex pickupIndex = dropTable.GenerateDrop(rngSeed);
			    if (pickupIndex != PickupIndex.none)
			    {
				    PickupDropletController.CreatePickupDroplet(pickupIndex, damageReport.victimBody.corePosition, Vector3.up * 20f);
			    }
		    }
		}

    }
}
