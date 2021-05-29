//If you haven't done so yet, run the setup.bat file in your project/libs folder to acquire the needed references.

using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DifficultyMod.Modifiers;
using DifficultyMod.Properties;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using Resources = DifficultyMod.Properties.Resources;
using SkillCatalog = RoR2.Skills.SkillCatalog;

namespace DifficultyMod
{

	//This is an example plugin that can be put in BepInEx/plugins/DifficultyMod/DifficultyMod.dll to test out.
    //It's a small plugin that adds a relatively simple item to the game, and gives you that item whenever you press F2.

    //This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
    //You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency("com.bepis.r2api")]

    //This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(
	    //The GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config). Java package notation is commonly used, which is "com.[your name here].[your plugin name here]"
	    BasicConfig.GUID,
	    //The name is the name of the plugin that's displayed on load
	    BasicConfig.NAME,
	    //The version number just specifies what version the plugin is.
	    BasicConfig.VERSION)]
    //Like seriously, if we see this boilerplate on thunderstore, we will deprecate this mod. Change that name!
    //If you want to test package uploading in general, try using beta.thunderstore.io

    //We will be using 3 modules from R2API: ItemAPI to add our item, ItemDropAPI to have our item drop ingame, and LanguageAPI to add our language tokens.
    [R2APISubmoduleDependency(nameof(LoadoutAPI),nameof(DifficultyAPI),nameof(ItemAPI))]
    
    //This is the main declaration of our plugin class. BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class DifficultyPlugin : BaseUnityPlugin
    {
	    private static DifficultyPlugin difficultyPlugin;
	    private static ManualLogSource _logger;
		public static DifficultyPlugin instance => difficultyPlugin;
		public static ManualLogSource Logger => _logger;

		private IModifier[] runModifier =
		{
			new ArtificierSkillModifier(),
			new SacrificeModifier(),
			new DifficultyModifier(),
			new SpawnCardModifier(),
			new UrnItemModifier(),

		};
		public DifficultyPlugin()
		{


			On.RoR2.DifficultyDef.GetIconSprite += (orig, self) =>
			{
				if (self.nameToken.Equals("V3nuz Design"))
				{
					return self.iconSprite;
				}

				return orig(self);
			};
		}

		private ItemDef myItemDef;
		private DifficultyDef longDifficulty;
		//The Awake() method is run at the very start when the game is initialized.
		public void Awake()
		{
			_logger = base.Logger;
			base.Logger.Log(LogLevel.Info, $"Loaded {BasicConfig.NAME} version {BasicConfig.VERSION}");
			difficultyPlugin = this;
			Run.onRunStartGlobal += OnRunStart;

			byte[] bytes = Properties.Resources.icon;

			var tex = AssetHelper.LoadTexture2D(bytes);
			var spr = Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
			longDifficulty = new DifficultyDef(0.2f, "V3nuz Design",
				null,
				"V3nuz",
				Color.blue,
				"longDifficultyTag",
				false) {iconSprite = spr, foundIconSprite = true};

			DifficultyModifier.index = DifficultyAPI.AddDifficulty(longDifficulty);

		}
		private void OnRunStart(Run runStart)
		{
			foreach (var modifier in runModifier)
			{
				modifier.Init();
			}
		}
    }
}