using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using Rewired;
using DG.Tweening;
namespace MoreMountains.CorgiEngine
{
    public class BKPlayer : CharacterAbility
    {
        [HideInInspector]public int playerId;
        [Header("闪烁")]
        public GameObject blinkRing;
        public GameObject circle;
        public List<Color> colors;

        [Header("震动")]
        public bool is8BitDo;
        public LayerMask vibrationEnterMask;
        public LayerMask vibrationStayMask;
        public int enter_motorIndex;
        public float enter_motorLevel;
        public float enter_motorDuration;
        public int stay_motorIndex;
        public float stay_motorLevel;
        public float stay_motorDuration;
        public float stay_motorInterval;
        public int move_motorIndex;
        public float move_motorLevel;
        public float move_motorDuration;
        public float move_motorInterval;
        private Player player;
        private float vibrationInterval;
        private bool canStayVibrate;
        public float LadderTime =2;
        public float AddBulletCD = 5;
        
        [Header("枪")]
        [HideInInspector]public int bulletNum;
        public int maxBulletNum;
        [Header("CD")]
        [HideInInspector]public float playerOnLadderCount = 0;
        [HideInInspector]public float AddBulletCount = 5;
        [Header("金币")]
        [HideInInspector]public bool playerWithCoin = false;
        public int score = 0;
        public GameObject Coin;
        protected override void Start()
        {
            base.Start();
            bulletNum = 0;
            GUIManager.Instance.UpdateAmmoDisplays(true, bulletNum, maxBulletNum, bulletNum, maxBulletNum, _character.PlayerID, true);
            vibrationInterval = stay_motorDuration;
            SetReweird();
            StartCoroutine(Fplayerwithcoin());
        }
        void SetReweird()
        {
            char[] c = _character.PlayerID.ToCharArray();
            playerId = (int)c[c.Length - 1] - '0' - 1;
            // Get the Player for a particular playerId
            if (playerId == 0)
                player = ReInput.players.GetPlayer(1);
            else if (playerId == 1)
            {
                player = ReInput.players.GetPlayer(0);
                if (is8BitDo)
                {
                    enter_motorLevel = 1;
                    enter_motorDuration *= 2;
                    stay_motorLevel = 1;
                    stay_motorDuration *= 2;
                    move_motorLevel = 1;
                }
            }
            // Some more examples:

            // Get the System Player
            Player systemPlayer = ReInput.players.GetSystemPlayer();

            // Iterating through Players (excluding the System Player)
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                Player p = ReInput.players.Players[i];
            }

            // Iterating through Players (including the System Player)
            for (int i = 0; i < ReInput.players.allPlayerCount; i++)
            {
                Player p = ReInput.players.AllPlayers[i];
            }
        }
        void Update()
        {
            if (_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                BlinkSelf();
                if(AddBulletCount>AddBulletCD)
                {
                    AddBullet();
                    AddBulletCount = 0;
                }
                
            }
            CDcount();

            
        }
        //玩家携带金币
        IEnumerator Fplayerwithcoin()
        {
            while(true)
            {
                if (playerWithCoin)
                {
                    BlinkSelf();
                    score += 4;
                }
                print(score);
                yield return new WaitForSeconds(2.5f);
            }
        }
        //公共cd计时
        void CDcount()
        {
            AddBulletCount += Time.deltaTime;
        }
        //死亡时生成一个coin
        void WithCoinDead()
        {
            if (playerWithCoin)
            {
                playerWithCoin = false;
                Instantiate(Coin, transform.position, Quaternion.identity);
            }
        }
        void AddBullet()
        {
            if (bulletNum < maxBulletNum)
            {
                bulletNum += 1;
                GUIManager.Instance.UpdateAmmoDisplays(true, bulletNum, maxBulletNum, bulletNum, maxBulletNum, _character.PlayerID, true);
            }
        }
        public void DecreaseNum()
        {
            bulletNum -= 1;
            GUIManager.Instance.UpdateAmmoDisplays(true, bulletNum, maxBulletNum, bulletNum, maxBulletNum, _character.PlayerID, true);
        }
         void BlinkSelf()
        {
            GameObject currentRing = Instantiate(blinkRing, transform.position, Quaternion.identity);
            SpriteRenderer sprite = currentRing.GetComponent<SpriteRenderer>();
                sprite.color = colors[playerId];
        }
        public void DisplayCircle()
        {
            GameObject currentCircle = Instantiate(circle, transform.position, Quaternion.identity);
            SpriteRenderer sprite = currentCircle.GetComponent<SpriteRenderer>();
            sprite.color = colors[playerId];
            sprite.DOFade(1, 0);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (MMLayers.LayerInLayerMask(collision.gameObject.layer, vibrationEnterMask))
            {
                playerOnLadderCount = 0;
                player.SetVibration(enter_motorIndex, enter_motorLevel, enter_motorDuration);
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (MMLayers.LayerInLayerMask(collision.gameObject.layer, vibrationStayMask))
            {
                bool isMoveInLadder = false;
                vibrationInterval -= Time.deltaTime;
                if (vibrationInterval > 0)
                    canStayVibrate = false;
                if (vibrationInterval < 0)
                {
                    vibrationInterval = stay_motorInterval;
                    if (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing && !_controller.State.IsGrounded && _controller.Speed.y != 0)
                    {
                        vibrationInterval = move_motorInterval;
                        isMoveInLadder = true;
                    }
                    canStayVibrate = true;
                }
                if (canStayVibrate)
                {
                    if (isMoveInLadder)
                        player.SetVibration(move_motorIndex, move_motorLevel, move_motorDuration);
                    else
                        player.SetVibration(stay_motorIndex, stay_motorLevel, stay_motorDuration);
                }
                //停太久发出声音
                if (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing && !_controller.State.IsGrounded)
                    playerOnLadderCount += Time.deltaTime;
                if(playerOnLadderCount> LadderTime)
                {
                    BlinkSelf();
                }

            }
            //吃金币
            if (collision.tag == "coin")
            {
                playerWithCoin = true;
                Destroy(collision.gameObject);
            }

        }
    }

}

//
//1、把换子弹cd打出到ui
//1、把玩家得分打出到ui
//3、函数WithCoinDead()放到玩家死亡的时候 执行一次
//4、拿到钻石3秒无敌和100分胜利没写  近战攻击功能没写   场景自动生成钻石没写