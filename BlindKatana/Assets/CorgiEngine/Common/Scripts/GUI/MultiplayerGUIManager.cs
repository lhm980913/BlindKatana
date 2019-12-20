using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Handles all GUI effects and changes for Multiplayer scenes
	/// </summary>
	public class MultiplayerGUIManager : GUIManager 
	{
		[Header("Multiplayer Endgame")]
		/// the game over splash screen object
		public GameObject MPEndGameSplash;
		/// the game over text object
		public Text MPEndGameText;
        public Image[] crowns;
  
        protected override void Start()
        {
            base.Start();
            if (dontDes.instance)
            {
                if (dontDes.instance.player1Win)
                    crowns[0].enabled = true;
                else if (dontDes.instance.player2Win)
                    crowns[1].enabled = true;
            }
        }
        /// <summary>
        /// Shows the multiplayer endgame screen
        /// </summary>
        public virtual void ShowMultiplayerEndgame()
		{
			MPEndGameSplash.SetActive (true);
		}
   
		/// <summary>
		/// Sets the multiplayer endgame text.
		/// </summary>
		/// <param name="text">Text.</param>
		public virtual void SetMultiplayerEndgameText (string text)
		{
			MPEndGameText.text = text;
		}

	}
}