using UnityEngine;
using UnityEditor;
using System.IO;
using System;
public class CSVImpoter
{
    [MenuItem("Tools/아이템/CSV로 생성")]
    public static void ImportFromCSV()
    {
        // 경로 설정을 위한 패널 켜기
        string path = EditorUtility.OpenFilePanel("아이템 csv", Application.dataPath, "csv");

        // 경로가 없으면 끝내기
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        // 해당 csv에서 모든 라인을 읽어와서 각 라인을 배열에 저장
        string[] lines = File.ReadAllLines(path);

        // 추후에 만들 스크립터블 오브젝트를 저장할 경로
        string outputFolder = "Assets/Data/ItemDatas";

        // 해당 경로에 폴더가 없으면 폴더 생성
        if (!AssetDatabase.IsValidFolder(outputFolder))
        {
            AssetDatabase.CreateFolder("Assets/Data", "ItemDatas");
        }
        // 에셋 제작 카운트
        int madeCount = 0;

        // 첫줄 이슈
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');

            ItemData data = ScriptableObject.CreateInstance<ItemData>();
            data.ItemId = int.Parse(fields[0]);
            data.ItemName = fields[1];
            data.itemType = (ItemType)Enum.Parse(typeof(ItemType), fields[2]);
            data.ItemValue = int.Parse(fields[3]);
            data.ItemDesc = fields[4];

            string assetPath = $"{outputFolder}/{data.ItemName}.asset";
            AssetDatabase.CreateAsset(data, assetPath);
            madeCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"{madeCount}만큼 생성되었습니다.");
    }
}