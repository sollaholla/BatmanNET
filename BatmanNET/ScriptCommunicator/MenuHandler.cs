using BatmanNET.Interfaces;
using ProfileSystem;
using ScriptCommunicator;
using SimpleUI;
using System.IO;
using System;
using GTA;

namespace ScriptCommunicator
{
    public class MenuHandler : IUpdatable
    {
        public const string ProfileDirectory = ".\\scripts\\Batman Files\\Profiles\\";

        public MenuHandler()
        {
            ScriptCommunicatorHelper = new ScriptCommunicatorHelper("Batman Script");
            ScriptCommunicatorHelper.Init("Batman Script", "Pre-Alpha");

            MainMenu = new UIMenu("Batman Script");
            MainMenu.CalculateMenuPositioning();

            MenuPool = new MenuPool();
            MenuPool.AddMenu(MainMenu);

            FindProfiles();
            BatmanProfile.ProfileActivated += OnProfileActivated;
        }

        /// <summary>
        /// The main menu.
        /// </summary>
        public UIMenu MainMenu { get; }

        /// <summary>
        /// The main menu pool.
        /// </summary>
        public MenuPool MenuPool { get; }

        /// <summary>
        /// The helper class that let's us communicate with the ScriptCommunicator menu system.
        /// </summary>
        public ScriptCommunicatorHelper ScriptCommunicatorHelper { get; }

        /// <summary>
        /// The main profile.
        /// </summary>
        public BatmanProfile BatmanProfile { get; private set; }

        /// <summary>
        /// True if the profile will be set to null next frame.
        /// </summary>
        public bool SetProfileNull { get; private set; }

        /// <summary>
        /// True if the script communicator event was triggered.
        /// </summary>
        public bool WasEventTriggered { get; private set; }

        private void OnProfileActivated(object sender, EventArgs e, Profile newProfile)
        {
            BatmanProfile = (BatmanProfile)newProfile;
            UI.Notify("This is a PRE-ALPHA release. ~p~All~s~ features are subject to change, and this does not reflect the final version.", true);
        }

        private void FindProfiles()
        {
            if (!Directory.Exists(ProfileDirectory))
            {
                return;
            }

            var files = Directory.GetFiles(ProfileDirectory, "*.ini");

            foreach (var fileName in files)
            {
                var profile = new BatmanProfile(fileName);

                profile.SetDefault();

                profile.Read();

                profile.AddToMenu(MainMenu, MenuPool);
            }

            AddDeactivationButton();
        }

        private void AddDeactivationButton()
        {
            var menuItem = new UIMenuItem("Deactivate Powers");

            MainMenu.OnItemSelect += (sender, e, index) =>
            {
                if (menuItem == e)
                {
                    SetProfileNull = true;
                }
            };

            MainMenu.AddMenuItem(menuItem);
        }

        public void Update()
        {
            MenuPool.ProcessMenus();

            UpdateCommunityEvent();

            if (SetProfileNull && BatmanProfile != null)
            {
                BatmanProfile = null;
                SetProfileNull = false;
            }
        }

        private void UpdateCommunityEvent()
        {
            if (ScriptCommunicatorHelper.IsEventTriggered())
            {
                ScriptCommunicatorHelper.BlockScriptCommunicatorModMenu();
                ScriptCommunicatorHelper.ResetEvent();
                WasEventTriggered = true;
                Script.Wait(100);
                MainMenu.IsVisible = true;
            }

            if (!MainMenu.IsVisible && WasEventTriggered)
            {
                ScriptCommunicatorHelper.UnblockScriptCommunicatorModMenu();
                WasEventTriggered = false;
            }
        }

        public void Abort()
        {
        }
    }
}
