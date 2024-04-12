using System;
using GameFramework.Config;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameFramework.CustomHelpers
{
    public class CSVConfigHelper: ConfigHelperBase
    {
        private ResourceComponent m_ResourceComponent = null;
        
        private static readonly string CSVAssetExtension = ".csv";
        public override bool ReadData(IConfigManager configManager, string configAssetName, object configAsset, object userData)
        {
            TextAsset configTextAsset = configAsset as TextAsset;
            if (configTextAsset != null)
            {
                if (configAssetName.EndsWith(CSVAssetExtension, StringComparison.Ordinal))
                {
                    return configManager.ParseData(configTextAsset.text, userData);
                }
                else
                {
                    Log.Warning("Config asset '{0}' doesn't end with .csv.", configAssetName);
                    return false;
                }
            }
            Log.Warning("Config asset '{0}' is invalid.", configAssetName);
            return false;
        }

        public override bool ReadData(IConfigManager configManager, string configAssetName, byte[] configBytes, int startIndex, int length,
            object userData)
        {
            throw new System.NotImplementedException();
        }

        public override bool ParseData(IConfigManager configManager, string configString, object userData)
        {
            try
            {
                int position = 0;
                string strLine = "";
                //逐行读取CSV中的数据
                while ((strLine = configString.ReadLine(ref position)) != null)
                {
                    var column = strLine.Split(',');
                    if (column.Length < 2)
                    {
                        Log.Warning("Can not parse config line string '{0}' which column count is less than 2.", strLine);
                        return false;
                    }
                    string configName = column[0];
                    string configValue = column[1];
                    if (!configManager.AddConfig(configName, configValue))
                    {
                        Log.Warning("Can not add config with config name '{0}' which may be invalid or duplicate.", configName);
                        return false;
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Log.Warning("Can not parse config string with exception '{0}'.", e.ToString());
                return false;
            }
        }

        public override bool ParseData(IConfigManager configManager, byte[] configBytes, int startIndex, int length, object userData)
        {
            throw new System.NotImplementedException();
        }

        public override void ReleaseDataAsset(IConfigManager configManager, object configAsset)
        {
            m_ResourceComponent.UnloadAsset(configAsset);
        }

        private void Start()
        {
            m_ResourceComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}