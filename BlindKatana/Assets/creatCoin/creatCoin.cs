using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
public class creatCoin : MonoBehaviour
{
    [SerializeField]
    public Transform[] positions;
    public GameObject Coin;

    public float DelayTime;

    bool alreadyCreat = false;
    float delaycount;
    public int daojishi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delaycount = Mathf.Clamp(delaycount + Time.deltaTime, 0, DelayTime);

        daojishi = (int)DelayTime - (int)delaycount;
        GUIManager.Instance.UpdateTimeText(daojishi);
        if (delaycount>=DelayTime&&!alreadyCreat)
        {
            alreadyCreat = true;
            Instantiate(Coin, positions[Random.Range(0, positions.Length - 1)].position, Quaternion.identity);
        }
    }
}



//发射子弹提示圈 颜色