using R2API;
using RoR2.Skills;

namespace DifficultyMod.Modifiers
{
    public class ArtificierSkillModifier : IModifier
    {
	    public void Init()
	    {
		    ModifySkill(
				SkillCatalog.GetSkillDef(
					SkillCatalog.FindSkillIndexByName(
							"FireLightningBolt"
						)
					)
				);
	    }
	    public static void ModifySkill(SkillDef skillDefinition)
	    {

		    if (!skillDefinition.skillName.Equals("FireLightningBolt"))
		    {
			    return;
		    }
		    skillDefinition.baseMaxStock = 1;
		    skillDefinition.baseRechargeInterval = 0;

		    LoadoutAPI.AddSkillDef(skillDefinition);


	    }
	}
}
