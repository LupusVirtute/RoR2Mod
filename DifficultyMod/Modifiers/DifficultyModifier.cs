using System;
using System.Collections.Generic;
using BepInEx.Logging;
using RoR2;

namespace DifficultyMod.Modifiers
{
    public class DifficultyModifier : IModifier
    {
	    private Dictionary<string, bool> scenePool = new Dictionary<string, bool>();
	    public static DifficultyIndex index;
	    private bool init = false;
		public void Init()
		{
			if (init)
			{
				return;
			}
			init = true;

			Stage.onServerStageBegin += stage =>
		    {
				DifficultyPlugin.Logger.LogInfo(Run.instance.selectedDifficulty);
				DifficultyPlugin.Logger.LogInfo(index);
			    if (Run.instance.selectedDifficulty != index)
			    {
				    return;
			    }
				scenePool[stage.sceneDef.baseSceneName] = true;
			    if (stage.sceneDef.destinations.Length <= 0 || stage.sceneDef.stageOrder > 6)
			    {
				    return;
			    }
			    stage.sceneDef.destinations = ChangeDestinations(stage.sceneDef).destinations;

		    };
		    foreach (var allSceneDef in SceneCatalog.allStageSceneDefs)
		    {
			    scenePool[allSceneDef.baseSceneName] = false;
		    }
		}
		public SceneDef ChangeDestinations(SceneDef scene)
		{
			SceneDef[] orderDefs = GetAvialableScenesWithOrder(scene.stageOrder);
			if (orderDefs == null || scene.destinations.Length <= 0)
			{
				return scene;
			}
			for (var index = 0; index < scene.destinations.Length; index++)
			{
				scene.destinations[index] = orderDefs[new System.Random().Next(orderDefs.Length)];
			}
			return scene;
		}

		public SceneDef[] GetAvialableScenesWithOrder(int order, int iteration = 0)
		{
			iteration++;
			if (iteration > 2)
			{
				return Stage.instance.sceneDef.destinations;
			}
			var stages = new List<SceneDef>();
			if (stages.Count > 0)
			{
				throw new NullReferenceException();
			}
			foreach (var keyValuePair in scenePool)
			{
				if (keyValuePair.Value)
				{
					continue;
				}
				var scene = SceneCatalog.GetSceneDefFromSceneName(keyValuePair.Key);

				if (!keyValuePair.Key.Equals(scene?.baseSceneName) || scene?.stageOrder != order)
				{
					continue;
				}

				stages.Add(scene);
			}

			if (stages.Count <= 0)
			{
				return GetAvialableScenesWithOrder(order + 1);
			}

			return stages.ToArray();
		}
	}
}
