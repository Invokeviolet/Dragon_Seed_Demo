using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class Data
{
    public List<Vector3> position = new List<Vector3>();
    public List<Vector3> scale = new List<Vector3>();
    public List<string> name = new List<string>();
    //public List<Vector3> rotation = new List<Vector3>();
}

public class SaveLoadManager : MonoSingleTon<SaveLoadManager>
{
    public GameObject PoolingZone;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Load();
        }
    }

    public void Save()
    {
        Debug.Log("저장시작" + Time.time);
        int objectCount = PoolingZone.transform.childCount;
        Data data = new Data();
        for (int i = 0; i < objectCount; i++)
        {
            data.name.Add(PoolingZone.transform.GetChild(i).name);
            data.position.Add(PoolingZone.transform.GetChild(i).position);
            data.scale.Add(PoolingZone.transform.GetChild(i).localScale);
        }
        Debug.Log("저장완료" + Time.time);

        File.WriteAllText(Application.dataPath + "/Test.json", JsonUtility.ToJson(data));
    }

    public void Load()
    {
        Debug.Log("로드시작");
        Data data2 = JsonUtility.FromJson<Data>(File.ReadAllText(Application.dataPath + "/Test.json"));  
    }
}


