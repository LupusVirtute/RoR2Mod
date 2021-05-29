using RoR2;

namespace DifficultyMod.Modifiers
{
    public class SpawnCardModifier : IModifier
    {
	    public void Init()
	    {
		    SceneDirector.onGenerateInteractableCardSelection += (self, cardSelector) =>
			{
				for (var i = 0; i < cardSelector.categories.Length; i++)
				{
					var cat = cardSelector.categories[i];
					foreach (var interactableCategoriesCategory in ClassicStageInfo.instance.interactableCategories.categories)
					{
						int j = 0;
						foreach (var directorCard in interactableCategoriesCategory.cards)
						{
							DifficultyPlugin.Logger.LogInfo("		Card Name : " + directorCard.spawnCard.name);

						}
					}
					//DifficultyPlugin.Logger.LogInfo("Category Name : "+cat.name);
					if (!ProcessCategory(self, cat))
					{
						cardSelector.RemoveCardsThatFailFilter(o => o.spawnCard.name.Equals(BasicConfig.BOSS_SHRINE_NAME));
					}
					foreach (var directorCard in cat.cards)
					{
						//DifficultyPlugin.Logger.LogInfo("		Card Name : " + directorCard.spawnCard.name);
					}
				}

			};
		}

	    private bool ProcessCategory(SceneDirector dir,DirectorCardCategorySelection.Category cardCat)
	    {
		    if (cardCat.name.Equals(BasicConfig.BOSS_SHRINE_CATEGORY))
		    {
			    return LoopThroughEveryDirectorCard(dir,cardCat);
		    }
		    else
			    cardCat.selectionWeight = 0f;
		    return false;
	    }

	    private bool LoopThroughEveryDirectorCard(SceneDirector dir, DirectorCardCategorySelection.Category cardCat)
	    {
		    foreach (var directorCard in cardCat.cards)
			    return ProcessDirectorCard(dir,directorCard);
		    return false;
	    }

	    private bool ProcessDirectorCard(SceneDirector dir,DirectorCard directorCard)
	    {
		    if (directorCard.spawnCard.name.Equals(BasicConfig.BOSS_SHRINE_NAME))
		    {
			    directorCard.spawnCard.directorCreditCost = 10;
			    directorCard.selectionWeight = 10000;

			    return true;
		    }
		    else
		    {
			    directorCard.selectionWeight = -1;
		    }
		    return false;
	    }
    }

}
