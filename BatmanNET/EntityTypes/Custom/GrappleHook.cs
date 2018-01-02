using BatmanNET.Interfaces;
using BatmanNET.Utilities;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Drawing;

namespace BatmanNET.EntityTypes.Custom
{
    public class GrappleHook
    {
        public GrappleHook(Vector3 targetPoint, Character owner, Entity grappleGun)
        {
            TargetPoint = targetPoint;
            Character = owner;
            GrappleGun = grappleGun;
        }

        /// <summary>
        /// The point we want to attach the hook to.
        /// </summary>
        public Vector3 TargetPoint {
            get;
        }

        /// <summary>
        /// The owner of the grapple hook.
        /// </summary>
        public Character Character {
            get;
        }

        /// <summary>
        /// The rope attaching the hook to the owner.
        /// </summary>
        public Rope Rope {
            get; private set;
        }

        /// <summary>
        /// The hook at the end of the grapple.
        /// </summary>
        public Entity Hook {
            get; private set;
        }

        /// <summary>
        /// True if this rope has spawned it's entities.
        /// </summary>
        public bool Initialized {
            get; private set;
        }

        public Entity GrappleGun {
            get; private set;
        }

        /// <summary>
        /// True if the hook has locked on to the target point.
        /// </summary>
        public bool IsHooked {
            get; private set;
        }

        public void Update()
        {
            Init();

            //DrawLine(GrappleGun.GetBoneCoord("Gun_Muzzle"), TargetPoint, Color.Black);

            //Rope.AttachEntities(GrappleGun, Vector3.Zero, Hook, Hook.Position, float.MaxValue, "Gun_Muzzle", string.Empty);

            if (IsHooked)
            {
                return;
            }

            var distance = Vector3.Distance(Hook.Position, TargetPoint);
            if (distance > 5f)
            {
                Hook.FreezePosition = true;
                var lerp = Mathf.MoveTowards(Hook.Position, TargetPoint, Game.LastFrameTime * 150f);
                Function.Call(Hash.SET_ENTITY_COORDS, Hook.Handle, lerp.X, lerp.Y, lerp.Z);
            }
            else
            {
                Hook.Position = TargetPoint;
                Hook.FreezePosition = true;
                IsHooked = true;
                //Rope.StartWinding();
            }
        }

        //private void DrawLine(Vector3 from, Vector3 to, Color col)
        //{
        //    Function.Call(Hash.DRAW_LINE, from.X, from.Y, from.Z, to.X, to.Y, to.Z, col.R, col.G, col.B, col.A);
        //}

        public void Abort()
        {
        }

        private void Init()
        {
            if (Initialized)
            {
                return;
            }

            if (!Entity.Exists(Hook))
            {
                Hook = World.CreateProp("prop_cs_dildo_01", GrappleGun.Position, true, false);

                Hook.IsVisible = false;
            }

            //if (!Rope.Exists(Rope))
            //{
            //    Function.Call(Hash.ROPE_LOAD_TEXTURES);

            //    float length = float.MaxValue;

            //    Rope = Rope.AddRope(Hook.Position, Hook.Rotation, 0f, GTARopeType.MetalWire, length, 0f, true, false);

            //    Rope.UseShadows = false;

            //    Rope.AttachEntities(GrappleGun, Vector3.Zero, Hook, Hook.Position, length, "Gun_Muzzle", string.Empty);
            //}

            Initialized = true;
        }

        /// <summary>
        /// Delete the grapple hook.
        /// </summary>
        public void Delete()
        {
            try
            {
                //if (Rope == null)
                //{
                //    return;
                //}

                //if (Character != null)
                //{
                //    Rope.DetachEntity(Character);
                //}

                if (Hook != null)
                {
                    //Rope.DetachEntity(Hook);

                    Hook.Delete();
                }

                if (GrappleGun != null)
                {
                    GrappleGun.Delete();
                }

                //Rope.Delete();
            }
            catch
            { 
                // ignored
            }
        }
    }
}
