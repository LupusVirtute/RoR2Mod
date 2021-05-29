using R2API;
using RoR2;
using UnityEngine.Networking;

namespace DifficultyMod.Modifiers
{
    public class UrnItemModifier : IModifier
    {
	    public void Init()
	    {
		    //SiphonOnLowHealth
		    DifficultyPlugin.Logger.LogInfo("ITEMS : ");
		    On.RoR2.SiphonNearbyController.OnDeserialize += (orig,self,reader, initialState) =>
		    {
			    orig(self,reader,initialState);
			    self.radius = 1000f;
		    };
		    foreach (var itemDefinition in ItemAPI.ItemDefinitions)
		    {
			    DifficultyPlugin.Logger.LogInfo(itemDefinition.ItemDef.nameToken);

				if (itemDefinition.ItemDef.tier == ItemTier.Boss)
			    {
					DifficultyPlugin.Logger.LogInfo(itemDefinition.ItemDef.nameToken);

			    }
		    }
		    DifficultyPlugin.Logger.LogInfo("TOKENS : ");
		    foreach (var equDef in ItemAPI.EquipmentDefinitions)
		    { 
			    DifficultyPlugin.Logger.LogInfo(equDef.EquipmentDef.nameToken);   
		    }
	    }
    }
}
