using GameFramework;

namespace GameMain
{
    public static class AssetUtility
    {
        public static string GetConfigAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Configs/{0}.{1}", assetName ,"csv");
        }
        public static string GetDataTableAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName,  "csv");
        }
        public static string GetMusicAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Music/{0}.mp3", assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Sounds/{0}.wav", assetName);
        }
        public static string GetUISoundAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UISounds/{0}.wav", assetName);
        }
        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
        }

        public static string GetEntityAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Prefabs/{0}.prefab", assetName);
        }
        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UIForms/{0}.prefab", assetName);
        }
        public static string GetGameSceneAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Prefabs/GameScenes/{0}.prefab", assetName);
        }
    }
}