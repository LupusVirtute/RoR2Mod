using System;
using System.IO;
using System.Linq;
using System.Reflection;
using R2API;
using UnityEngine;


namespace DifficultyMod.Properties
{
    /// <summary>
    /// A helper class for loading embedded resources into the game.
    /// </summary>
    public static class AssetHelper
    {
        /// <summary>
        /// Call this to automatically register all the language tokens in your string resources through assetplus
        /// </summary>
        public static void RegisterLanguageTokens()
        {
            Type type = typeof(Properties.Resources);
            if (type == null) throw new NullReferenceException("Could not find the resources type");

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            for (Int32 i = 0; i < properties.Length; ++i)
            {
                var prop = properties[i];
                if (prop.PropertyType != typeof(String)) continue;
                var propName = prop.Name;
                if (String.IsNullOrEmpty(propName) || !propName.StartsWith("lang__")) continue;
                var langKey = propName.Substring(6);
                if (String.IsNullOrEmpty(langKey)) continue;
                var langValue = (String)prop.GetValue(null);
                langValue = langValue.Replace(@"\n", Environment.NewLine);
                R2API.AssetPlus.Languages.AddToken(langKey, langValue);
            }
        }


        /// <summary>
        /// A simple helper to generate a unique mod prefix for you.
        /// </summary>
        /// <param name="plugin">A reference to your plugin. (this.GetModPrefix)</param>
        /// <param name="bundleName">A unique name for the bundle (Unique within your mod)</param>
        /// <returns>The generated prefix</returns>
        public static String GetModPrefix(this BepInEx.BaseUnityPlugin plugin, String bundleName)
        {
            return String.Format("@{0}+{1}", plugin.Info.Metadata.Name, bundleName);
        }

        /// <summary>
        /// Loads an embedded .png or .jpg image as a Texture2D
        /// </summary>
        /// <param name="resourceBytes">The bytes returned by Properties.Resources.ASSETNAME</param>
        /// <returns>The loaded texture</returns>
        public static Texture2D LoadTexture2D(Byte[] resourceBytes)
        {
            if (resourceBytes == null) throw new ArgumentNullException(nameof(resourceBytes));

            var tempTex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            tempTex.LoadImage(resourceBytes, false);

            return tempTex;
        }
        /// <summary>
        /// Loads an embedded assetbundle and automatically registers an AssetBundleResourcesProvider for it.
        /// </summary>
        /// <param name="prefix">Your mod prefix (Try this.GetModPrefix("ASSETNAME"))</param>
        /// <param name="resourceBytes">The bytes returned by Properties.Resources.ASSETNAME</param>
        /// <returns>The loaded bundle</returns>
        public static AssetBundle LoadAssetBundleResourcesProvider(String prefix, byte[] resourceBytes)
        {
	        if (resourceBytes == null) throw new ArgumentNullException(nameof(resourceBytes));
	        if (String.IsNullOrEmpty(prefix) || !prefix.StartsWith("@")) throw new ArgumentException("Invalid prefix format", nameof(prefix));

	        var bundle = AssetBundle.LoadFromMemory(resourceBytes);
	        if (bundle == null) throw new NullReferenceException(String.Format("{0} did not resolve to an assetbundle.", nameof(resourceBytes)));

	        var provider = new AssetBundleResourcesProvider(prefix, bundle);
	        ResourcesAPI.AddProvider(provider);

	        return bundle;
        }
    }
}