using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

public class Prefabs : MonoBehaviour
{
    /// <summary>
    /// 生成的预制体路径
    /// </summary>
    private static string buildPath="Assets/BatchPrefabs/";

    [MenuItem("Tools/BatchPrefab All Children")]
    public static void BatchPrefab()
    {
        Transform tParent = ((GameObject) Selection.activeObject).transform;//选中要批量制作的预制体的父级对象
        Debug.Log("生成的prefab："+buildPath);
        Directory.CreateDirectory(Prefabs.buildPath);
        Object tempPrefab;
        int i = 0;
        foreach (Transform t in tParent)//遍历父级对象的子物体，生成预制体，此处没有深度遍历，保留每个子物体的层次结构
        {
            if (File.Exists(Prefabs.buildPath + t.gameObject.name + ".prefab"))
            {
                Debug.Log("同名文件已存在：" + t.gameObject.name);
                continue;
            }
            tempPrefab = PrefabUtility.CreateEmptyPrefab(Prefabs.buildPath + t.gameObject.name + ".prefab");
            tempPrefab = PrefabUtility.ReplacePrefab(t.gameObject, tempPrefab);
            i++;
            Debug.Log("创建了prefab" + t.gameObject.name);
        }
    }
}