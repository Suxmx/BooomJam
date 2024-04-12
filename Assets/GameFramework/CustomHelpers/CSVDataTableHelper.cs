using System;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameFramework.CustomHelpers
{
    public class CSVDataTableHelper : DataTableHelperBase
    {
        private ResourceComponent m_ResourceComponent = null;

        private static readonly string CSVAssetExtension = ".csv";

        public override bool ReadData(DataTableBase dataTable, string dataTableAssetName, object dataTableAsset,
            object userData)
        {
            TextAsset configTextAsset = dataTableAsset as TextAsset;
            if (configTextAsset != null)
            {
                if (dataTableAssetName.EndsWith(CSVAssetExtension, StringComparison.Ordinal))
                {
                    return dataTable.ParseData(configTextAsset.text, userData);
                }
                else
                {
                    Log.Warning("Config asset '{0}' doesn't end with .csv.", dataTableAssetName);
                    return false;
                }
            }
            Log.Warning("Config asset '{0}' is invalid.", dataTableAssetName);
            return false;
        }

        public override bool ReadData(DataTableBase dataTable, string dataTableAssetName, byte[] dataTableBytes,
            int startIndex, int length,
            object userData)
        {
            throw new System.NotImplementedException();
        }

        public override bool ParseData(DataTableBase dataTable, string dataTableString, object userData)
        {
            try
            {
                int position = 0;
                string dataRowString = null;
                while ((dataRowString = dataTableString.ReadLine(ref position)) != null)
                {
                    if (dataRowString[0] == '#')
                    {
                        continue;
                    }
                    if (!dataTable.AddDataRow(dataRowString, userData))
                    {
                        Log.Warning("Can not parse data row string '{0}'.", dataRowString);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse data table string with exception '{0}'.", exception.ToString());
                return false;
            }
        }

        public override bool ParseData(DataTableBase dataTable, byte[] dataTableBytes, int startIndex, int length,
            object userData)
        {
            throw new System.NotImplementedException();
        }

        public override void ReleaseDataAsset(DataTableBase dataTable, object dataTableAsset)
        {
            m_ResourceComponent.UnloadAsset(dataTableAsset);
        }
        private void Start()
        {
            m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}