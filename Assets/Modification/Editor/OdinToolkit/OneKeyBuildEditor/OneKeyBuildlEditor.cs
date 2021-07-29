using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

[TypeInfoBox("<size=20>一键打包</size>")]
public class OneKeyBuildlEditor : GlobalConfig<OneKeyBuildlEditor>
{
    private const int addressableBundlesDirOrder = 3;
    private const int AddressableButtonOrder = 2;
    private const int Addressables地址管理Order = 3;
    private const int PlayerSetting打包设置Order = 4;
    private const int PlayerSetting打包结果Order = 5;
    private const string HFS = "HFS";
    private string androidPath;
    private string winPath;
#pragma warning disable CS0414
    private bool isBuild = false;
#pragma warning restore CS0414
    //#region Addressables Button

    //[TitleGroup("Addressables")]
    //[ButtonGroup("Addressables/buttons", AddressableButtonOrder)]
    //public void Groups()
    //{
    //    Debug.Log("打开窗口：" + "Window/Asset Management/Addressables/Groups");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Groups");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void Settings()
    //{
    //    Debug.Log("打开窗口：" + "Window/Asset Management/Addressables/Settings");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Settings");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void Profiles()
    //{
    //    Debug.Log("打开窗口：" + "Window/Asset Management/Addressables/Profiles");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Profiles");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void EventViewer()
    //{
    //    Debug.Log("打开窗口：" + "Window/Asset Management/Addressables/Event Viewer");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Event Viewer");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void Anallyze()
    //{
    //    Debug.Log("打开窗口：" + "Window/Asset Management/Addressables/Analyze");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Analyze");
    //}

    //[ButtonGroup("Addressables/buttons")]
    //public void Hosting()
    //{
    //    Debug.Log("打开窗口：" + "Window/Asset Management/Addressables/Hosting");
    //    EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Hosting");
    //}

    //[FolderPath]
    //[TitleGroup("Addressables", order: addressableBundlesDirOrder)]
    //public List<string> addressableBundlesDir;

    //[TitleGroup("Addressables")]
    //public List<AddressableAssetGroupSchema> addressableAssetGroupSchema;

    //#endregion Addressables Button

    //#region AddressableAssetSettings

    //[TitleGroup("Addressables 地址管理 (激活的地址配置)")]
    //public AddressableAssetSettings addressableConfig;

    //[TitleGroup("Addressables 地址管理 (激活的地址配置)"), ShowInInspector]
    //[InfoBox("激活的配置")]
    //[ReadOnly]
    //public string activeProfile
    //{
    //    get
    //    {
    //        if (addressableConfig == null)
    //        {
    //            return "";
    //        }
    //        return addressableConfig.profileSettings.GetProfileName(addressableConfig.activeProfileId);
    //    }
    //}

    //[TitleGroup("Addressables 地址管理 (激活的地址配置)", null, TitleAlignments.Left, true, true, false, Addressables地址管理Order), ShowInInspector]
    //public string hFS
    //{
    //    get
    //    {
    //        if (addressableConfig == null)
    //        {
    //            return "";
    //        }
    //        // 当前激活的 配置名

    //        return addressableConfig.profileSettings.GetValueByName(addressableConfig.activeProfileId, HFS);
    //    }
    //    set
    //    {
    //        addressableConfig.profileSettings.SetValue(addressableConfig.activeProfileId, HFS, value);
    //    }
    //}

    //#endregion AddressableAssetSettings

    [TitleGroup("PlayerSetting 打包设置", null, TitleAlignments.Left, true, true, false, PlayerSetting打包设置Order)]
    public BuildConfig buildConfig;

    private List<string> m_activeScene = new List<string>();

    [ShowInInspector]
    [TitleGroup("PlayerSetting 打包设置", null, TitleAlignments.Left, true, true, false, PlayerSetting打包设置Order)]
    public List<string> activeScenes
    {
        get
        {
            var tempScenes = new List<string>();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            foreach (var item in scenes)
            {
                if (item.enabled)
                {
                    tempScenes.Add(item.path);
                }
            }
            return tempScenes;
        }
    }

    private bool isBuilding;

    [TitleGroup("PlayerSetting 打包设置")]
    public AndroidSdkVersions androidSDK_Version
    {
        get
        {
            return buildConfig.androidSDK_Version;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.androidSDK_Version = value;
            }
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    [FolderPath(AbsolutePath = true)]
    public string buildPath
    {
        get
        {
            if (buildConfig == null)
            {
                return "";
            }
            return buildConfig.buildPath;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.buildPath = value;
            }
            EditorPrefs.SetString("OdinBuild.BuildPath", value);
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    [ReadOnly]
    public BuildTargetGroup buildTarget
    {
        get
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                return BuildTargetGroup.Android;
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                return BuildTargetGroup.iOS;
            }
            else
            {
                return BuildTargetGroup.Standalone;
            }
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    public string companyName
    {
        get
        {
            return PlayerSettings.companyName;
        }
        set
        {
            PlayerSettings.companyName = value;
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    public string productName
    {
        get
        {
            return PlayerSettings.productName;
        }
        set
        {
            PlayerSettings.productName = value;
        }
    }

    [ReadOnly]
    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    [InfoBox("将要打包的版本号（不可修改，默认自增）")]
    public string version
    {
        get
        {
            return PlayerSettings.bundleVersion;
        }
        set
        {
            PlayerSettings.bundleVersion = value;
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    public int versionIncrease
    {
        get
        {
            if (buildConfig == null)
            {
                return 0;
            }
            return buildConfig.versionIncrease;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.versionIncrease = value;
            }
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    public ScriptingImplementation scriptingBacked
    {
        get
        {
            ScriptingImplementation ScriptingBackend = PlayerSettings.GetScriptingBackend(buildTarget);
            return ScriptingBackend;
        }
        set
        {
            PlayerSettings.SetScriptingBackend(buildTarget, value);
        }
    }

    /// <summary>
    /// 打包
    /// </summary>
    [TitleGroup("PlayerSetting 打包设置")]
    [Button(90), GUIColor(0.4f, 0.8f, 1f)]
    public void BuildPackage()
    {
        //this.isBuild = !this.isBuild;
        PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = version;

        PlayerSettings.Android.targetSdkVersion = androidSDK_Version;

        #region 开始打包

        isBuilding = BuildPipeline.isBuildingPlayer;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = activeScenes.ToArray();
        //打包目标路径
        androidPath = buildPath + @"\" + buildTarget.ToString() + @"\" + productName + "_" + version + ".apk";
        winPath = buildPath + @"\" + buildTarget.ToString() + @"\" + productName + "_" + version + ".exe";
        if (buildTarget == BuildTargetGroup.Android)
        {
            buildPlayerOptions.locationPathName = androidPath;
        }
        else
        {
            buildPlayerOptions.locationPathName = winPath;
        }
        //打包目标平台
        if (buildTarget.ToString() == "Standalone")
        {
            buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        }
        else
        {
            buildPlayerOptions.target = (BuildTarget)Enum.Parse(typeof(BuildTarget), buildTarget.ToString());
        }

        buildPlayerOptions.options = BuildOptions.None;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.platform);
            Debug.Log("Build succeeded: " + summary.totalSize + "bytes");
            Debug.Log("Build succeeded: " + summary.totalTime + "s");

            SucessBuild(report);
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
            FailBuild(report);
        }
        isBuilding = BuildPipeline.isBuildingPlayer;

        #endregion 开始打包
    }

    //[HideIf("isBuild")]
    //[TitleGroup("PlayerSetting 打包设置")]
    //[DisableIf("isBuilding")]
    //[Button(90), GUIColor(0f, 1f, 0f)]
    //public void AddressablesBundle()
    //{
    //    #region 添加Group文件

    //    bool m_creatGroup = true;

    //    foreach (var dirPath in addressableBundlesDir)
    //    {
    //        DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);//获取目录下文件夹信息

    //        //检测Group中有没有文件夹命名的组，有就将文件夹添加到Groups，没有就创建

    //        var groups = addressableConfig.groups;
    //        AddressableAssetGroup accessionGroup = null;
    //        foreach (var group in groups)
    //        {
    //            Debug.Log("Group名：  " + group.name);
    //            if (group.name == directoryInfo.Name)
    //            {
    //                Debug.Log("有与文件夹同名的Group");
    //                accessionGroup = group;
    //                m_creatGroup = false;
    //                group.Settings.DefaultGroup = group;
    //                break;
    //            }
    //        }
    //        if (m_creatGroup)
    //        {
    //            accessionGroup = addressableConfig.CreateGroup(directoryInfo.Name, true, false, true, addressableAssetGroupSchema /*typeof(BundledAssetGroupSchema),*//*typeof(ContentUpdateGroupSchema)*/);//后两个参数，拷贝现有的（Assets中的），创造全新的（参数默认）
    //        }

    //        foreach (var objGuid in directoryInfo.GetFiles()) //将文件夹下的所有文件添加到Groups中，
    //        {
    //            if (objGuid.Name.Contains("meta"))//meta文件不是资产
    //            {
    //                continue;
    //            }
    //            string[] m_path = objGuid.FullName.Split(new string[] { "Assets" }, StringSplitOptions.RemoveEmptyEntries);
    //            string m_guidPath = "Assets" + m_path[1];

    //            string guid = AssetDatabase.GUIDFromAssetPath(m_guidPath).ToString();
    //            var asset = accessionGroup.Settings.CreateAssetReference(guid);//加入到组

    //            #endregion 添加Group文件

    //            #region 添加标签
    //            addressableConfig.AddLabel(directoryInfo.Name);
    //            AddressableAssetEntry resourcesEntry = addressableConfig.FindAssetEntry(asset.AssetGUID);//修改/添加，标签

    //            resourcesEntry.SetLabel(directoryInfo.Name, true);
    //        }
    //    }

    //    #endregion 添加标签

    //    //加入Group
    //    this.isBuild = !this.isBuild;
    //}

    /// <summary>
    /// 打包失败
    /// </summary>
    /// <param name="report"></param>
    private void FailBuild(BuildReport report)
    {
        buildResult = "打包失败，请看Console信息";
    }

    /// <summary>
    /// 打包成功
    /// </summary>
    /// <param name="report"></param>
    private void SucessBuild(BuildReport report)
    {
        BuildSummary summary = report.summary;
        string size = "检测打包大小失败";
        if (File.Exists(androidPath))
        {
            FileInfo apk = new FileInfo(androidPath);
            size = " " + (apk.Length / (1024.00 * 1024.00)).ToString("f2") + "MB";
        }
        else
        {
            long m_size = 0;
            GetDirSizeByPath((buildPath + @"\" + buildTarget.ToString()).Replace(@"/", @"\"), ref m_size);
            size = " " + (m_size / (1024.00 * 1024.00)).ToString("f2") + "MB";
        }

        string time = " " + summary.totalTime + "s";
        buildResult = "打包成功: " + summary.outputPath + "\n" +
            "安装后大小: " + size + "\n" +
            "打包时长: " + time + "\n";

        string[] versionsNum = PlayerSettings.bundleVersion.Split('.');
        int tempInt = int.Parse(versionsNum[2]) + versionIncrease;
        versionsNum[2] = tempInt.ToString();
        var tempVersionsNum = String.Join(".", versionsNum);

        PlayerSettings.bundleVersion = tempVersionsNum;
        EditorUtility.OpenWithDefaultApp(buildPath.Replace(@"/", @"\"));
    }

    [TitleGroup("PlayerSetting 打包结果", null, TitleAlignments.Left, true, true, false, PlayerSetting打包结果Order)]
    [ReadOnly]
    [MultiLineProperty(3), ShowInInspector]
    public string buildResult
    {
        get
        {
            return EditorPrefs.GetString("OdinBuild.buildResult");
        }
        set
        {
            EditorPrefs.SetString("OdinBuild.buildResult", value);
        }
    }

    /// <summary>
    /// 获取文件夹的大小
    /// </summary>
    /// <param name="dir">文件夹目录</param>
    /// <param name="dirSize">返回文件夹大小，传递引用</param>
    private static void GetDirSizeByPath(string dir, ref long dirSize)
    {
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);

            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            FileInfo[] files = dirInfo.GetFiles();

            foreach (var item in dirs)
            {
                GetDirSizeByPath(item.FullName, ref dirSize);
            }

            foreach (var item in files)
            {
                dirSize += item.Length;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("获取文件大小失败" + ex.Message);
        }
    }
}