using BatmanNET.Interfaces;
using BatmanNET.Utilities;
using GTA;
using GTA.Math;
using GTA.Native;

namespace BatmanNET.EntityTypes.Custom
{
    public class GrappleHook
    {
        public GrappleHook(Vector3 targetPoint, Character owner, Entity grappleGun)
        {
            TargetPoint = targetPoint;
            Owner = owner;
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
        public Character Owner {
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
            }
        }

        private void Trim()
        {
            Rope.StartWinding();
            Rope.Length = GrappleGun.Position.DistanceTo(Hook.Position);
        }

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

            if (!Rope.Exists(Rope))
            {
                Function.Call(Hash.ROPE_LOAD_TEXTURES);
                var d = Hook.Position.DistanceTo(TargetPoint);
                Rope = Rope.AddRope(Hook.Position, Hook.Rotation, 1f, GTARopeType.MetalWire, float.MaxValue, 0f, true, false);
                Rope.AttachEntities(Hook, Vector3.Zero, GrappleGun, Vector3.Zero, float.MaxValue, string.Empty, string.Empty);
                Rope.ActivatePhysics();
                Rope.ConvertToSimple();
            }

            Initialized = true;
        }

        public void Delete()
        {
            if (Rope == null)
            {
                return;
            }

            if (Owner != null)
            {
                Rope.DetachEntity(Owner);
            }

            if (Hook != null)
            {
                Rope.DetachEntity(Hook);
            }

            Rope.Delete();
            Hook.Delete();
        }
    }
}
