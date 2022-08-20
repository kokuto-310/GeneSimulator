using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Manager : MonoBehaviour
{
    //[SerializeField, Range(10, 1000)] int frameCount = 600;
    [SerializeField, Range(10, 1000)] int firstGene = 10;

    private void Start()
    {
        GameObject obj = (GameObject)Resources.Load("Gene");

        for (int i = 0; i < firstGene; i++)
        {
            // プレハブの位置と向きをランダムで設定
            float x = Random.Range(-10.0f, 10.0f);
            float z = Random.Range(-10.0f, 10.0f);
            Vector3 pos = new Vector3(x, 2.0f, z);
            var randomQ = Random.rotation;

            // プレハブを生成し、初期値を設定
            var temp = Instantiate(obj, pos, randomQ);
            temp.transform.GetComponent<SphereMove>().InitStatus();
        }
    }

    //// 毎シーンごと
    //void Update()
    //{
    //    // 600フレーム毎にシーンにプレハブを生成
    //    if (Time.frameCount % frameCount == 0)
    //    {
    //        GameObject obj = (GameObject)Resources.Load("Gene");

    //        // プレハブの位置と向きをランダムで設定
    //        float x = Random.Range(-5.0f, 5.0f);
    //        float z = Random.Range(-5.0f, 5.0f);
    //        Vector3 pos = new Vector3(x, 2.0f, z);
    //        var randomQ = Random.rotation;

    //        // プレハブを生成
    //        Instantiate(obj, pos, randomQ);
    //    }
    //}
}