using BatmanNET.Handlers;
using GTA;
using ScriptCommunicator;
using System;

namespace BatmanNET
{
    public class Main : Script
    {
        public Main()
        {
            Tick += OnTick;
            Aborted += OnAborted;

            ProfileMenuHandler = new MenuHandler();
        }

        /// <summary>
        /// The player's batman handler used to update the batman entity that's attached to the player ped.
        /// </summary>
        public PlayerBatmanHandler PlayerBatman {
            get; private set;
        }

        /// <summary>
        /// This handles activating/deactivating the profiles.
        /// </summary>
        public MenuHandler ProfileMenuHandler {
            get; private set;
        }

        /// <summary>
        /// True if batman powers are enabled.
        /// </summary>
        public bool Enabled {
            get => ProfileMenuHandler.BatmanProfile != null;
        }

        private void OnTick(object sender, EventArgs e)
        {
            TogglePlayerBatman();
            ProfileMenuHandler.Update();
        }

        private void TogglePlayerBatman()
        {
            if (!Enabled)
            {
                if (PlayerBatman == null)
                {
                    return;
                }

                PlayerBatman.Abort();

                PlayerBatman = null;
            }
            else
            {
                if (PlayerBatman == null)
                {
                    PlayerBatman = new PlayerBatmanHandler();
                }

                PlayerBatman.Update();
            }
        }

        private void OnAborted(object sender, EventArgs e)
        {
            if (PlayerBatman != null)
            {
                PlayerBatman.Abort();
            }
        }
    }
}
