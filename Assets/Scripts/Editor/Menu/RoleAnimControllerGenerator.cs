using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Oro.Menu
{
    public class RoleAnimControllerGenerator
    {
        private const string MENU_ITEM_NAME = "Assets/生成角色动画控制器";
        private const string CONTROLLER_FOLDER_NAME = "Controller";
        private const string CLIP_FOLDER_NAME = "Anim";

        [MenuItem(MENU_ITEM_NAME)]
        protected static void Generate()
        {
            var selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var selectDirInfo = new DirectoryInfo(selectPath);
            var roleName = selectDirInfo.Name;

            #region 生成clip到Anim中

            Debug.Log("检查所选目录下的所有子目录，将Anmation文件夹下的clip放到Anim中,同时根据Anim生成控制器到Controller中");
            DirectoryInfo[] directoryInfos = selectDirInfo.GetDirectories();
            foreach (var item in directoryInfos)
            {
                if (item.Name != "Animation")
                {
                    Debug.Log("非Animatiton 文件夹:" + item.Name);
                    continue;
                }
                var anims = item.GetFiles();//得到二级目录下的所有文件
                if (anims != null && anims.Length >= 1)
                {
                    foreach (var anim in anims)
                    {
                        if (anim.FullName.Contains(".meta"))
                        {
                            // Debug.Log("忽略meta文件");
                            continue;
                        }
                        string[] strArray = anim.FullName.Split(new string[] { "Assets" }, StringSplitOptions.RemoveEmptyEntries);//分割字符串
                        string fbxPath = "Assets" + strArray[1];
                        // Debug.Log("fbx路径" + fbxPath);
                        if (strArray.Length > 2)
                        {
                            Debug.Log("项目路径不要具有两个Assets");
                            return;
                        }
                        OnPostprocessAnimation(fbxPath);
                    }
                }
            }

            #endregion 生成clip到Anim中

            var controllerFolderPath = Path.Combine(selectPath, CONTROLLER_FOLDER_NAME);
            if (!AssetDatabase.IsValidFolder(controllerFolderPath))
            {
                AssetDatabase.CreateFolder(selectPath, CONTROLLER_FOLDER_NAME);
                AssetDatabase.Refresh();
            }

            var controllerDirInfo = new DirectoryInfo(controllerFolderPath);

            string controllerPath;
            AnimatorController animatorController;

            // ------ Load AnimatorController ------

            var controllerFileInfos = controllerDirInfo.GetFiles("*.controller");
            if (controllerFileInfos.Length == 0)
            {
                controllerPath = Path.Combine(controllerFolderPath, roleName + ".controller");
                animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            }
            else
            {
                controllerPath = PathHelper.AbsoluteToAssetPath(controllerFileInfos[0].FullName);
                animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            }

            // ------ ------ ------ ------ ------ ------

            var rootStateMachine = animatorController.layers[0].stateMachine;

            // ------ Remove Old State ------

            var animatorStates = new List<AnimatorState>();
            foreach (var childState in rootStateMachine.states)
            {
                animatorStates.Add(childState.state);
            }
            foreach (var animationState in animatorStates)
            {
                rootStateMachine.RemoveState(animationState);
            }

            // ------ ------ ------ ------ ------ ------

            // ------ ------ Load clip ------ ------

            string clipFolderPath = Path.Combine(selectPath, CLIP_FOLDER_NAME);
            var clipFolderDirInfo = new DirectoryInfo(clipFolderPath);

            Vector3 statePosition = new Vector3(400, 0, 0);
            var clipFileInfos = clipFolderDirInfo.GetFiles("*.anim");
            foreach (var clipFileInfo in clipFileInfos)
            {
                string assetPath = PathHelper.AbsoluteToAssetPath(clipFileInfo.FullName);
                var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);

                var animatorState = rootStateMachine.AddState(clip.name, statePosition);
                animatorState.motion = clip;

                statePosition += 50 * Vector3.up;
            }

            // ------ ------ ------ ------ ------ ------

            EditorUtility.SetDirty(animatorController);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private const string ANIM_PREFIX = "Anim_";
        private const string NEW_FOLDER_NAME = "Anim";
        private static readonly string[] LOOP_KEYS = new string[]{
            "idle", "walk", "run", "loop"
        };

        /// <summary>
        /// 根据FBX路径生成其clip文件
        /// </summary>
        /// <param name="fbxpath">FBX路径 以Assets为根</param>
        private static void OnPostprocessAnimation(string fbxpath)
        {
            //从fbx 文件目录 获取其clip
            AnimationClip clip = AssetDatabase.LoadAssetAtPath(fbxpath, typeof(AnimationClip)) as AnimationClip;

            var selectPath = AssetDatabase.GetAssetPath(clip);

            DirectoryInfo dirInfo = new DirectoryInfo(selectPath);

            string newAnimFolderPath = Path.Combine(dirInfo.Parent.Parent.FullName, NEW_FOLDER_NAME);
            if (!Directory.Exists(newAnimFolderPath))
            {
                Directory.CreateDirectory(newAnimFolderPath);
                AssetDatabase.Refresh();
            }

            newAnimFolderPath = PathHelper.AbsoluteToAssetPath(newAnimFolderPath);

            string clipName = clip.name;

            if (clipName.StartsWith(ANIM_PREFIX))
                clipName = clipName.Substring(ANIM_PREFIX.Length);

            string lowClipName = clipName.ToLower();
            bool isLoop = false;
            foreach (var loopKey in LOOP_KEYS)
            {
                if (lowClipName.Contains(loopKey))
                {
                    isLoop = true;
                    break;
                }
            }

            string clipPath = Path.Combine(newAnimFolderPath, clipName + ".anim");
            var newClip = UnityEngine.Object.Instantiate(clip);
            AssetDatabase.CreateAsset(newClip, clipPath);

            var chip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            var clipSetting = AnimationUtility.GetAnimationClipSettings(chip);

            clipSetting.loopTime = isLoop;
            AnimationUtility.SetAnimationClipSettings(chip, clipSetting);

            EditorUtility.SetDirty(chip);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}