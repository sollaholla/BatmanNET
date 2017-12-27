using BatmanNET.Debug;
using BatmanNET.EntityTypes.Custom;
using GTA;
using GTA.Math;
using System;
using System.Drawing;
using System.Windows.Forms;
using Control = GTA.Control;

namespace BatmanNET
{
    public class Main : Script
    {
        public Main()
        {
            Tick += OnTick;
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.K)
            {
                PlayerBatman.Position = new Vector3(-127.11f, -645.21f, 176.67f);
                PlayerBatman.Heading = 178.76f;
            }
        }

        private BatmanEntity PlayerBatman { get; set; }

        private void OnTick(object sender, EventArgs e)
        {
            if (PlayerBatman == null || !PlayerBatman.Exists())
            {
                PlayerBatman = new BatmanEntity(Game.Player.Character);
            }

            PlayerBatman.Update();
            PlayerBatman.GlidingMovement = new Vector3(Game.GetControlNormal(2, Control.MoveLeftRight), -Game.GetControlNormal(2, Control.MoveUpDown), 0f);

            if (Game.IsControlJustPressed(2, Control.Jump))
                PlayerBatman.StartGlide();

            PlayerBatman.SetDiveToggle(Game.CurrentInputMode == InputMode.MouseAndKeyboard && Game.IsControlPressed(2, Control.Sprint) || Game.CurrentInputMode == InputMode.GamePad && Game.IsControlPressed(2, Control.Attack));
            return;

            PropertyViewer.Analyse(PlayerBatman, Point.Empty, 0.2f, 10f, false);

            PropertyViewer.Analyse(PlayerBatman.Ped, new Point(Game.ScreenResolution.Width / 2, 0), 0.2f, 10f, true);
        }
    }
}
