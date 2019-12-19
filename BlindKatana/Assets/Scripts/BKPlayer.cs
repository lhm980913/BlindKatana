using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using Rewired;
namespace MoreMountains.CorgiEngine
{
    public class BKPlayer : CharacterAbility
    {
        public GameObject ring;
        public List<Color> colors;
        public LayerMask vibrationMask;
        private Player player;
        protected override void Start()
        {
            base.Start();
            char[] c = _character.PlayerID.ToCharArray() ;
            // Get the Player for a particular playerId
            player = ReInput.players.GetPlayer((int)(c[c.Length-1]-1));

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
                BlinkSelf();

        }
        void BlinkSelf()
        {
            GameObject currentRing= Instantiate(ring,transform.position,Quaternion.identity);
            SpriteRenderer sprite = currentRing.GetComponent<SpriteRenderer>();
            if (_character.PlayerID == "Player1")
                sprite.color = colors[0];
            if (_character.PlayerID == "Player2")
                sprite.color = colors[1];
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(MMLayers.LayerInLayerMask(collision.gameObject.layer, vibrationMask)){
                player.SetVibration(1, 1f);
            }
        }
    }
}
