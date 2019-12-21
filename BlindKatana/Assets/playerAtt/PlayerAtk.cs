using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
public class PlayerAtk : CharacterAbility
{
    public MMFeedbacks feedbacks;
    [Header("近战")]
    public GameObject daoguang;
    public float radius = 2;
    public float AtkCD = 5;
    [HideInInspector] public float AtkCount;
    [HideInInspector] public GameObject effect;
    private bool canAttack=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //输入判定
        //_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonDown
        if (_inputManager.DashButton.State.CurrentState == MMInput.ButtonStates.ButtonDown && AtkCount==AtkCD && _health.CurrentHealth>0)
        {
            playerAtk();
            AtkCount = 0;
        }

        //攻击特效在场时检测攻击到玩家
        if (effect != null && canAttack)
        {
            RaycastHit2D[] castAll = Physics2D.CircleCastAll(effect.transform.position, radius, Vector2.zero);
            foreach(RaycastHit2D hit in castAll)
            {
                if (hit.collider.gameObject.tag == "Player")
                    if (hit.collider.gameObject!=gameObject)
                    {
                        Health h = hit.collider.GetComponent<Health>();
                        if (h.CurrentHealth > 0)
                        {
                            h.CurrentHealth = 0;

                            //玩家死亡
                            MultiplayerLevelManager.Instance.KillPlayer(hit.collider.gameObject.GetComponent<Character>());
                        }
                           
                       
                      
                    }
            }
        }
        //攻击读秒
        AtkCount = Mathf.Clamp(AtkCount + Time.deltaTime, 0, AtkCD);

    }

    void playerAtk()
    {
        effect = Instantiate(daoguang, transform.position, Quaternion.identity);
        effect.transform.localScale = Vector3.one * 2 * radius;
        StartCoroutine(AttDisable());
        feedbacks.PlayFeedbacks();
        GetComponent<BKPlayer>().BlinkSelf();
        Destroy(effect, 2);
    }
    IEnumerator AttDisable()
    {
        canAttack = true;
        yield return new WaitForSeconds(0.5f);
        canAttack = false;
    }
}
