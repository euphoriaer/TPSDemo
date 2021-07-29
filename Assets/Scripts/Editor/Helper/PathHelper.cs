using UnityEngine;

namespace Oro
{
    public static class PathHelper
    {
        static string _dataPath;
        public static string DataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_dataPath))
                {
                    _dataPath = GetStandardPath(Application.dataPath);
                }

                return _dataPath;
            }
        }

        public static string AbsoluteToAssetPath(string absolutePath)
        {
            if (absolutePath.StartsWith(DataPath))
            {
                return "Assets" + absolutePath.Substring(DataPath.Length);
            }

            return absolutePath;
        }

        public static string GetStandardPath(string path)
        {
            return path.Replace('/', '\\');
        }
    }
}