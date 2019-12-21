using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using Rewired;
using DG.Tweening;
using MoreMountains.Feedbacks;
namespace MoreMountains.CorgiEngine
{
    public class BKPlayer : CharacterAbility
    {
        public MMFeedbacks reloadSound;
        [HideInInspector]public int playerId;
        [Header("闪烁")]
        public GameObject blinkRing;
        public GameObject blinkDiamond;
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
        public Player player;
        private float vibrationInterval;
        private bool canStayVibrate;
        public float LadderTime =2;
        public float ladderBlinkInterval;
        public float AddBulletCD = 5;
        
        [Header("枪")]
        [HideInInspector]public int bulletNum;
        public int maxBulletNum;
        public GameObject addBullet;
        [Header("CD")]
        [HideInInspector]public float playerOnLadderCount = 0;
        [HideInInspector]public float AddBulletCount=0 ;
        [Header("金币")]
        [HideInInspector]public bool playerWithCoin = false;
        public float scoreAddInterval;
        public float blinkInterval;
        [HideInInspector] public int score = 0;
        public GameObject Coin;
        private bool isLadderBlink;
        public LayerMask shakeLayer;
        protected override void Start()
        {
            base.Start();
            bulletNum = 0;
            AddBulletCount = AddBulletCD;
            GUIManager.Instance.UpdateAmmoCD(AddBulletCount, AddBulletCD, _character.PlayerID);
            GUIManager.Instance.UpdateAmmoDisplays(true, bulletNum, maxBulletNum, bulletNum, maxBulletNum, _character.PlayerID, true);
            vibrationInterval = stay_motorDuration;
            SetReweird();
            StartCoroutine(Fplayerwithcoin());
            StartCoroutine(FplayerwithcoinBlink());
        }
        void SetReweird()
        {
            char[] c = _character.PlayerID.ToCharArray();
            playerId = (int)c[c.Length - 1] - '0' - 1;
            // Get the Player for a particular playerId
            if (playerId == 0)
                player = ReInput.players.GetPlayer(0);
            else if (playerId == 1)
            {
                player = ReInput.players.GetPlayer(1);

            }
            if (is8BitDo)
            {
                enter_motorLevel = 1;
                enter_motorDuration *= 2;
                stay_motorLevel = 1;
                stay_motorDuration *= 2;
                move_motorLevel = 1;
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
                if (AddBulletCount >= AddBulletCD && bulletNum < 3)
                {
                    AddBullet();
                    AddBulletCount = 0;
                }
            }
            if (bulletNum <= 3 && AddBulletCount<AddBulletCD)
                CDcount();
            if (score >= 100)
                MultiplayerLevelManager.Instance.CheckEnd(_character.PlayerID);
        }
        //玩家携带金币加分
        IEnumerator Fplayerwithcoin()
        {
            while(true)
            {
                if (playerWithCoin && score<=100)
                {
                    score = Mathf.Clamp(score + 1,0,100);
                    GUIManager.Instance.UpdateScoreText(score + "%", _character.PlayerID);
                }
                yield return new WaitForSeconds(scoreAddInterval);
            }
        }  
        //玩家携带闪烁
        IEnumerator FplayerwithcoinBlink()
        {
            while (true)
            {
                if (playerWithCoin)
                {
                    BlinkSelf();
                    Instantiate(blinkDiamond, transform.position+Vector3.up, Quaternion.identity);
                }
                yield return new WaitForSeconds(blinkInterval);
            }
        }
        //公共cd计时
        void CDcount()
        {
            AddBulletCount = Mathf.Clamp( Time.deltaTime+ AddBulletCount,0,AddBulletCD);
            GUIManager.Instance.UpdateAmmoCD(AddBulletCount,AddBulletCD,_character.PlayerID);
        }
        //死亡时生成一个coin
        public void WithCoinDead()
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
                reloadSound.PlayFeedbacks();
                Instantiate(addBullet, transform.position, Quaternion.identity);
                GUIManager.Instance.UpdateAmmoDisplays(true, bulletNum, maxBulletNum, bulletNum, maxBulletNum, _character.PlayerID, true);
            }
        }
        public void DecreaseNum()
        {
            bulletNum -= 1;
            GUIManager.Instance.UpdateAmmoDisplays(true, bulletNum, maxBulletNum, bulletNum, maxBulletNum, _character.PlayerID, true);
        }
        public void BlinkSelf()
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
            if (MMLayers.LayerInLayerMask(collision.gameObject.layer, shakeLayer))
            {
                collision.gameObject.transform.DOShakePosition(0.3f, 0.2f, 10, 90, false, false);
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
                    if(!isLadderBlink)
                    StartCoroutine(LadderBlink(collision));
                }

            }
            IEnumerator LadderBlink(Collider2D c)
            {
                isLadderBlink = true;
                LadderShake shake = c.GetComponent<LadderShake>();
                while (_movement.CurrentState == CharacterStates.MovementStates.LadderClimbing && !_controller.State.IsGrounded)
                {
                    BlinkSelf();
                    shake.Shake();
                    yield return new WaitForSeconds(ladderBlinkInterval);
                }
                isLadderBlink = false;
            }
            //吃金币
            if (collision.tag == "coin"&&_health.CurrentHealth>0)
            {
                playerWithCoin = true;
                BlinkSelf();
                Instantiate(blinkDiamond, transform.position + Vector3.up, Quaternion.identity);
                Destroy(collision.gameObject);
            }

        }
    }

}

//
//1、把换子弹cd打出到ui
//1、把玩家得分打出到ui
//4、拿到钻石3秒无敌和100分胜利没写  近战攻击功能没写   场景自动生成钻石没写