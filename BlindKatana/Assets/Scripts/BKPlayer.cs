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
        [Header("枪")]
        [HideInInspector]public int bulletNum;
        public int maxBulletNum;
        protected override void Start()
        {
            base.Start();
            bulletNum = 0;
            GUIManager.Instance.UpdateAmmoDisplays(true, bulletNum, maxBulletNum, bulletNum, maxBulletNum, _character.PlayerID, true);
            vibrationInterval = stay_motorDuration;
            SetReweird();
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
                AddBullet();
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
            }
        }
    }

}

