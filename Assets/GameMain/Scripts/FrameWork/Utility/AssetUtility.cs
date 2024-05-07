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
        public static string GetMusicAsset(string assetName,string suffix="mp3")
        {
            return Utility.Text.Format("Assets/GameMain/Audios/Music/{0}.{1}", assetName,suffix);
        }

        public static string GetSoundAsset(string assetName,string suffix="wav")
        {
            return Utility.Text.Format("Assets/GameMain/Audios/SF/{0}.{1}", assetName,suffix);
        }
        public static string GetUISoundAsset(string assetName,string suffix="wav")
        {
            return Utility.Text.Format("Assets/GameMain/Audios/UI/{0}.{1}", assetName,suffix);
        }
        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
        }

        public static string GetPrefabAsset(string assetName)
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