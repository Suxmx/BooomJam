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
    }
}