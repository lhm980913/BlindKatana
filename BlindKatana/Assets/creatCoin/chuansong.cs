using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
public class chuansong : MonoBehaviour
{
    public LayerMask portalLayer;
    public float chuansongRadius=0.5f;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] castAll = Physics2D.CircleCastAll(transform.position, chuansongRadius, Vector2.zero);
        foreach (RaycastHit2D hit in castAll)
        {
            if (MMLayers.LayerInLayerMask(hit.collider .gameObject.layer, portalLayer))
            {
                hit.collider.transform.position = target.position;
            }
        }


    }

}
