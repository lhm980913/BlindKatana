using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using DG.Tweening;
namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Spawns the players and handles end game
	/// </summary>
	[AddComponentMenu("Corgi Engine/Managers/Multiplayer Level Manager")]
	public class MultiplayerLevelManager : LevelManager
	{
     
        public float respawnTime;
        public GameObject rip;
		/// <summary>
		/// Checks the multiplayer end game conditions
		/// </summary>
        ///
		public virtual void CheckMultiplayerEndGame()
		{
			int stillAlive = 0;
			string winnerID = "";
			foreach (Character player in Players)
			{
				if (player.ConditionState.CurrentState != CharacterStates.CharacterConditions.Dead)
				{
					stillAlive++;
					winnerID = player.PlayerID;
				}
			}
			if (stillAlive == 1)
			{
				StartCoroutine(MultiplayerEndGame (winnerID));
			}
		}
        public override void CheckEnd(string playerID)
        {
            StartCoroutine(MultiplayerEndGame(playerID));
        }
        /// <summary>
        /// Handles the endgame
        /// </summary>
        /// <returns>The end game.</returns>
        /// <param name="winnerID">Winner I.</param>
        protected virtual IEnumerator MultiplayerEndGame(string winnerID)
		{
			// we wait for 1 second
			yield return new WaitForSeconds (1f);
			// we freeze all characters
			FreezeCharacters ();
			// wait for another second
			yield return new WaitForSeconds (1f);

			// if we find a MPGUIManager, we display the end game screen with the name of the winner
			if (GUIManager.Instance.GetComponent<MultiplayerGUIManager>() != null)
			{
				GUIManager.Instance.GetComponent<MultiplayerGUIManager> ().ShowMultiplayerEndgame ();
				GUIManager.Instance.GetComponent<MultiplayerGUIManager> ().SetMultiplayerEndgameText (winnerID+" WINS");
			}
			// we wait for 2 seconds
			yield return new WaitForSeconds (3f);
            if (winnerID == "Player1")
                dontDes.instance.Player1Score();
            else
                dontDes.instance.Player2Score();
            dontDes.instance.NextLevel();
			// we reload the current scene to start a new game
			//LoadingSceneManager.LoadScene(SceneManager.GetActiveScene ().name);
		}

		/// <summary>
		/// Kills the specified player 
		/// </summary>
		public override void KillPlayer(Character player)
		{
			Health characterHealth = player.GetComponent<Health>();
			if (characterHealth == null)
			{
				return;
			} 
			else
			{
				// we kill the character
				characterHealth.Kill ();

				StartCoroutine (RespawnPlayer(player));
			}

			//CheckMultiplayerEndGame ();
		}

		/// <summary>
		/// Removes the specified player from the game.
		/// </summary>
		/// <returns>The player.</returns>
		/// <param name="player">Player.</param>
		protected virtual IEnumerator RespawnPlayer(Character player)
		{
            GameObject newRip = Instantiate(rip, player.transform.position+new Vector3(0,0.5f,0), Quaternion.identity);
            BKPlayer bk = player.GetComponent<BKPlayer>();
            bk.DisplayCircle();
            bk.WithCoinDead();
			yield return new WaitForSeconds (respawnTime);
            newRip.GetComponent<SpriteRenderer>().DOFade(0, 1);
            player.RespawnAt(Checkpoints[UnityEngine.Random.Range(0, Checkpoints.Count)].transform, Character.FacingDirections.Right);
		}
	}
}