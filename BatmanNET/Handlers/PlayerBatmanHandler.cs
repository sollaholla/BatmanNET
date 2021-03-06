﻿using BatmanNET.EntityTypes.Custom;
using BatmanNET.Interfaces;
using GTA;
using GTA.Math;
using GTA.Native;

namespace BatmanNET.Handlers
{
    public class PlayerBatmanHandler : IUpdatable
    {
        public PlayerBatmanHandler()
        {

        }

        private BatmanEntity PlayerBatman { get; set; }

        public void Update()
        {
            if (PlayerBatman == null || !PlayerBatman.Exists())
            {
                PlayerBatman = new BatmanEntity(Game.Player.Character);
            }

            Function.Call(Hash.SET_PLAYER_HAS_RESERVE_PARACHUTE, Game.Player);

            UI.ShowHudComponentThisFrame(HudComponent.Reticle);

            PlayerBatman.Update();

            PlayerBatman.GlidingMovement = new Vector3(Game.GetControlNormal(2, Control.MoveLeftRight), -Game.GetControlNormal(2, Control.MoveUpDown), 0f);

            if (Game.IsControlJustPressed(2, Control.Jump))
            {
                PlayerBatman.StartGlide();
            }

            var ray = World.Raycast(GameplayCamera.Position, GameplayCamera.Direction, 200, IntersectOptions.Map, PlayerBatman);

            if (ray.DitHitAnything)
            {
                Game.DisableControlThisFrame(2, Control.Cover);

                if (Game.IsDisabledControlJustPressed(2, Control.Cover))
                {
                    PlayerBatman.GrappleToPoint(ray.HitCoords, ray.SurfaceNormal);
                }
            }

            if (PlayerBatman.IsGliding && !PlayerBatman.IsDiving)
            {
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, PlayerBatman.Handle, 8, 1, 0, 0);
            }
            else
            {
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, PlayerBatman.Handle, 8, 0, 0, 0);
            }

            PlayerBatman.SetDiveToggle(Game.CurrentInputMode == InputMode.MouseAndKeyboard && Game.IsControlPressed(2, Control.Sprint) || Game.CurrentInputMode == InputMode.GamePad && Game.IsControlPressed(2, Control.Attack));

            //PropertyViewer.Analyse(PlayerBatman.Ped, new Point(Game.ScreenResolution.Width / 2, 0), 0.2f, 10f, true);
            //PropertyViewer.Analyse(PlayerBatman, Point.Empty, 0.2f, 10f, false);
        }

        public void Abort()
        {
            if (PlayerBatman != null)
            {
                PlayerBatman.Abort();
            }
        }
    }
}
