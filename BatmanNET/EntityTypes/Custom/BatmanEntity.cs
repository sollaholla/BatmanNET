using System;
using BatmanNET.Native;
using GTA;
using GTA.Math;
using GTA.Native;
using BatmanNET.Utilities;

namespace BatmanNET.EntityTypes.Custom
{
    public class BatmanEntity : Character
    {
        public BatmanEntity(Ped ped) : base(ped)
        {
            MinGlideHeight = 1.5f;
            MinGlideStartHeight = 20f;
            MaxQuickAttackDistance = 5f;
            MaxAttackDistance = 15f;
            MaxLandHeight = 3.5f;
            BatGrappleModel = "w_pi_pistol";

            Streaming.RequestAnimDict("move_fall");
            Streaming.RequestAnimDict("move_fall@beastjump");
            Streaming.RequestAnimDict("skydive@parachute@");
            Streaming.RequestAnimDict("skydive@freefall");
            Streaming.RequestAnimDict("weapons@pistol_1h@");
            Streaming.RequestAnimDict("swimming@first_person@");
            Streaming.RequestAnimDict("misslamar1leadinout");
        }

        /// <summary>
        /// </summary>
        /// <param name="ped">The ped who will become batman.</param>
        /// <param name="minGlideHeight">The minimum height above the ground allowed for a glide.</param>
        /// <param name="minGlideStartHeight">The minimum height above the ground to start a glide.</param>
        public BatmanEntity(Ped ped, float minGlideHeight, float minGlideStartHeight) : base(ped)
        {
            MinGlideHeight = minGlideHeight;
            MinGlideStartHeight = minGlideStartHeight;
            MaxQuickAttackDistance = 5f;
            MaxAttackDistance = 15f;
            MaxLandHeight = 5f;
        }

        /// <summary>
        /// </summary>
        /// <param name="ped">The ped who will become batman.</param>
        /// <param name="minGlideHeight">The minimum height above the ground allowed for a glide.</param>
        /// <param name="minGlideStartHeight">The minimum height above the ground to start a glide.</param>
        /// <param name="maxQuickAttackDistance">The maximum distance that allowed for batman to perform a quick attack.</param>
        /// <param name="maxAttackDistance">The maximum distance allowed for batman to perform any attack.</param>
        public BatmanEntity(Ped ped, float minGlideHeight, float minGlideStartHeight, float maxQuickAttackDistance, float maxAttackDistance) : base(ped)
        {
            MinGlideHeight = minGlideHeight;
            MinGlideStartHeight = minGlideStartHeight;
            MaxQuickAttackDistance = maxQuickAttackDistance;
            MaxAttackDistance = maxAttackDistance;
            MaxLandHeight = 5f;
        }

        /// <summary>
        /// </summary>
        /// <param name="ped">The ped who will become batman.</param>
        /// <param name="minGlideHeight">The minimum height above the ground allowed for a glide.</param>
        /// <param name="minGlideStartHeight">The minimum height above the ground to start a glide.</param>
        /// <param name="maxQuickAttackDistance">The maximum distance that allowed for batman to perform a quick attack.</param>
        /// <param name="maxAttackDistance">The maximum distance allowed for batman to perform any attack.</param>
        /// <param name="maxLandHeight">The maximum height the player had to have fallen from in order to roll.</param>
        public BatmanEntity(Ped ped, float minGlideHeight, float minGlideStartHeight, float maxQuickAttackDistance, float maxAttackDistance, float maxLandHeight) : base(ped)
        {
            MinGlideHeight = minGlideHeight;
            MinGlideStartHeight = minGlideStartHeight;
            MaxQuickAttackDistance = maxQuickAttackDistance;
            MaxAttackDistance = maxAttackDistance;
            MaxLandHeight = maxLandHeight;
        }

        public string BatGrappleModel {
            get; set;
        }

        /// <summary>
        /// Returns true if the ped entity is falling.
        /// </summary>
        public bool PedFallFlag {
            get; private set;
        }

        /// <summary>
        /// The height we where when we started falling.
        /// </summary>
        public float StartFallHeight {
            get; private set;
        }

        /// <summary>
        /// Returns true if we just started falling.
        /// </summary>
        public bool StartedFalling {
            get; private set;
        }

        /// <summary>
        /// Returns true if the ped is falling or in parachute freefall.
        /// </summary>
        public bool IsFalling {
            get {
                return Ped.IsFalling || Ped.IsInAir || Ped.IsInParachuteFreeFall;
            }
        }

        /// <summary>
        /// Returns true if batman is in glide mode.
        /// </summary>
        public bool IsGliding {
            get; private set;
        }

        /// <summary>
        /// Returns true if batman is doing a glide dive.
        /// </summary>
        public bool IsDiving {
            get; private set;
        }

        /// <summary>
        /// The minimum height above the ground allowed for a glide.
        /// </summary>
        public float MinGlideHeight {
            get; private set;
        }

        /// <summary>
        /// The minimum height above the ground to start a glide.
        /// </summary>
        public float MinGlideStartHeight {
            get; private set;
        }

        /// <summary>
        /// True if batman is doing a glide kick.
        /// </summary>
        public bool IsDoingGlideKick {
            get; private set;
        }

        /// <summary>
        /// True if batman is playing his gliding animation.
        /// </summary>
        public bool IsPlayingGlideAnim {
            get; private set;
        }

        /// <summary>
        /// True if batman is doing a dive animation.
        /// </summary>
        public bool IsPlayingDiveAnim {
            get; private set;
        }

        /// <summary>
        /// True if batman is playing his glide kick animation.
        /// </summary>
        public bool IsPlayingGlideKickAnim {
            get; private set;
        }

        /// <summary>
        /// True if batman is attacking something.
        /// </summary>
        public bool IsAttackingAnything {
            get; private set;
        }

        /// <summary>
        /// True if batman is performing a combat roll to the next entity
        /// he needs to punch.
        /// </summary>
        public bool IsDoingCombatRoll {
            get; private set;
        }

        /// <summary>
        /// The minimum distance from an attack entity for batman
        /// to perform a non-rolling combat move.
        /// </summary>
        public float MaxQuickAttackDistance {
            get; private set;
        }

        /// <summary>
        /// The maximum distance from an attack entity for batman
        /// to attack.
        /// </summary>
        public float MaxAttackDistance {
            get; private set;
        }

        /// <summary>
        /// The movement vector to use for gliding.
        /// </summary>
        public Vector3 GlidingMovement {
            get; set;
        }

        /// <summary>
        /// The entity batman is currently attacking.
        /// </summary>
        public Entity LastAttackEntity {
            get; private set;
        }

        /// <summary>
        /// True if batman is performing a batarang throw.
        /// </summary>
        public bool IsThrowingBatarang {
            get; private set;
        }

        /// <summary>
        /// The maximum height the player had to have fallen from in order to roll.
        /// </summary>
        public float MaxLandHeight {
            get; private set;
        }

        /// <summary>
        /// The velocity of the player before gliding.
        /// </summary>
        public Vector3 InitialGlideVelocity {
            get; private set;
        }

        /// <summary>
        /// The horizontal additive angle of the batman during his glide.
        /// </summary>
        public Vector3 GlideHorizontalAdditive {
            get; private set;
        }

        /// <summary>
        /// The vertical additive angle of the batman during his glide.
        /// </summary>
        public Vector3 GlideVerticalAdditive {
            get; private set;
        }

        /// <summary>
        /// The normal vector of the wall we grappled to.
        /// </summary>
        public Vector3 LastGrappleWallNormal {
            get; private set;
        }

        /// <summary>
        /// The amount of drag applied to the player after a dive or fall.
        /// </summary>
        public float GlideDrag {
            get; private set;
        }

        /// <summary>
        /// The initial downwards speed of batman before gliding.
        /// </summary>
        public float GlideDownSpeed {
            get; private set;
        }

        /// <summary>
        /// The current world point to grapple.
        /// </summary>
        public Vector3 LastGrapplePoint {
            get; private set;
        }

        /// <summary>
        /// Makes batman perform a special landing next time.
        /// </summary>
        public bool SpecialLanding {
            get; private set;
        }

        /// <summary>
        /// True if batman is grappling.
        /// </summary>
        public bool IsGrappling {
            get; private set;
        }

        /// <summary>
        /// True if batman is playing the grapple animation.
        /// </summary>
        public bool IsPlayingGrappleAnimation {
            get; private set;
        }

        /// <summary>
        /// True if batman is playing the grapple hook reeling animation.
        /// </summary>
        public bool IsPlayingGrappleReelAnimation {
            get; private set;
        }

        /// <summary>
        /// The current grapple hook. Spawned when batman does a grapple.
        /// </summary>
        public GrappleHook GrappleHook {
            get; private set;
        }

        /// <summary>
        /// The current grapple hook task sequence.
        /// </summary>
        public TaskSequence GrappleTaskSequence {
            get; private set;
        }

        /// <summary>
        /// The bat grapple prop instance. Only not null when grappling.
        /// </summary>
        public Prop BatGrappleInstance {
            get; private set;
        }

        /// <summary>
        /// The speed of the grapple reeling.
        /// </summary>
        public float GrappleSpeed {
            get; private set;
        }

        public override void Update()
        {
            HandleGlide();
            HandleAttack();
            HandleGrapple();
            HandleRagdoll();
            HandleFalling();
            HandleMoveRate();
        }

        private void HandleMoveRate()
        {
            Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, Ped.Handle, 1.15f);
        }

        private void HandleRagdoll()
        {
            if (Ped.IsRagdoll)
            {
                return;
            }

            Ped.CanRagdoll = false;
        }

        private void HandleFalling()
        {
            TestFall();

            if (Ped.IsClimbing)
            {
                StartFallHeight = 0f;

                StartedFalling = false;
            }

            if (!StartedFalling || Ped.HeightAboveGround > 1.5f)
            {
                return;
            }

            if (StartFallHeight < MaxLandHeight)
            {
                return;
            }

            IsGrappling = false;

            IsGliding = false;

            IsDoingGlideKick = false;

            Function.Call(Hash.SET_ENTITY_COORDS, Ped.Handle, Ped.Position.X, Ped.Position.Y, Ped.Position.Z - Ped.HeightAboveGround);

            Ped.Task.ClearAll();

            var direction = Quaternion.Euler(0, 0, Ped.Rotation.Z) * Vector3.RelativeFront;

            Ped.Heading = direction.ToHeading();

            if (!SpecialLanding)
            {
                Ped.PlayAnimation("move_fall", "land_roll", 8.0f, -4.0f, 750, AnimationFlags.AllowRotation, 0.0f);
            }
            else
            {
                Ped.PlayAnimation("move_fall@beastjump", "low_land_stand", 8.0f, -4.0f, 750, AnimationFlags.AllowRotation, 0.05f);

                SpecialLanding = false;
            }

            StartedFalling = false;

            StartFallHeight = 0.0f;
        }

        /// <summary>
        /// Throw a batarang at an entity.
        /// </summary>
        /// <param name="entity"></param>
        public void ThrowBatarangAtEntity(Entity entity)
        {
            if (IsFalling || IsGliding || IsAttackingAnything || IsDoingCombatRoll || IsThrowingBatarang)
            {
                return;
            }

            IsThrowingBatarang = true;
        }

        private void ClearBatarangAnimation()
        {

        }

        /// <summary>
        /// Starts a glide if we're in the air.
        /// </summary>
        public void StartGlide()
        {
            if (!IsFalling || IsGliding || Ped.HeightAboveGround < MinGlideStartHeight || IsGrappling)
            {
                return;
            }

            IsGliding = true;
        }

        private void HandleGrapple()
        {
            if (IsGrappling)
            {
                if (!Exists(BatGrappleInstance))
                {
                    var prop = World.CreateProp(BatGrappleModel, Ped.Position + Vector3.RelativeTop * 100, false, false);

                    prop.FreezePosition = true;

                    prop.AttachTo(Ped, Ped.GetBoneIndex(Bone.SKEL_R_Hand), new Vector3(0.1f, 0f, 0), new Vector3(-90, 0, 0));

                    BatGrappleInstance = prop;
                }
            }
            else
            {
                if (Exists(BatGrappleInstance))
                {
                    BatGrappleInstance.Delete();
                }
            }

            if (!IsGrappling || Ped.IsRagdoll || IsGliding)
            {
                if (IsPlayingGrappleAnimation)
                {
                    ClearGrappleAnimation();
                }

                if (IsPlayingGrappleReelAnimation)
                {
                    ClearGrappleReelAnimation();
                }

                if (GrappleHook != null)
                {
                    GrappleHook.Delete();
                }

                if (IsGrappling)
                {
                    IsGrappling = false;
                }

                GrappleSpeed = 0f;

                return;
            }

            if (Exists(BatGrappleInstance) && BatGrappleInstance.IsAttachedTo(Ped))
            {
                Function.Call(Hash.PROCESS_ENTITY_ATTACHMENTS, Ped.Handle);
            }

            if (!IsPlayingGrappleAnimation)
            {
                Ped.Heading = (LastGrapplePoint - Ped.Position).ToHeading();

                StartGrappleTaskSequence();

                Ped.SetConfigFlag(60, true);

                IsPlayingGrappleAnimation = true;

                GrappleHook = new GrappleHook(LastGrapplePoint, this, BatGrappleInstance);
            }
            else
            {
                if (!GrappleHook.IsHooked)
                {
                    Ped.SetConfigFlag(60, true);

                    GrappleHook.Update();
                }
                else
                {
                    if (!IsPlayingGrappleReelAnimation)
                    {
                        var vel = Ped.Velocity;

                        Ped.Task.ClearAll();

                        Ped.Velocity = Vector3.Zero;

                        Ped.PlayAnimation("skydive@parachute@", "chute_idle", 8.0f, -8.0f, -1, AnimationFlags.Loop, 0.0f);

                        Ped.PlayAnimation("weapons@pistol_1h@", "aim_high_loop", 8.0f, -8.0f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation | AnimationFlags.StayInEndFrame, 0f);

                        Ped.SetAnimationSpeed("skydive@parachute@", "chute_idle", 0f);

                        Ped.SetAnimationSpeed("weapons@pistol_1h@", "aim_high_loop", 0f);

                        Ped.Velocity = vel;

                        Ped.SetConfigFlag(60, true);

                        IsPlayingGrappleReelAnimation = true;
                    }
                    else
                    {
                        var dirNormalized = (LastGrapplePoint - Ped.Position).Normalized;

                        Ped.SetConfigFlag(60, true);
                        
                        if (Ped.HasCollidedWithAnything && Ped.Velocity.Length() > 5f)
                        {
                            var ray = World.Raycast(Ped.Position + (-LastGrappleWallNormal * 1.15f) + (Vector3.RelativeTop * 2f), Vector3.RelativeBottom, 5f, IntersectOptions.Map, Ped);

                            if (!ray.DitHitAnything)
                            {
                                var dir = Vector3.Reflect(dirNormalized, LastGrappleWallNormal);

                                Ped.Heading = dir.ToHeading();

                                Ped.Velocity = Ped.ForwardVector * 50;

                                IsGliding = true;

                                GlideHorizontalAdditive = new Vector3(0, -90, 0f);
                            }
                            else
                            {
                                Ped.Heading = (-LastGrappleWallNormal).ToHeading();

                                Ped.Task.ClearAll();

                                Ped.Velocity = Vector3.Zero;

                                Ped.Task.Climb();
                            }

                            IsGrappling = false;
                        }
                        else
                        {
                            Ped.Quaternion = Quaternion.FromToRotation(Ped.UpVector, dirNormalized) * Ped.Quaternion;

                            GrappleHook.Update();

                            GrappleSpeed = Mathf.Lerp(GrappleSpeed, 90f, Game.LastFrameTime * 2.5f);

                            Ped.Velocity = dirNormalized * GrappleSpeed;
                        }
                    }
                }
            }
        }

        private void HandleGlide()
        {
            if (!IsGliding)
            {
                if (IsPlayingGlideAnim)
                {
                    ClearGlideAnimation();

                    StartFallHeight = float.MaxValue;

                    StartedFalling = true;
                }

                if (IsPlayingGlideKickAnim)
                {
                    ClearGlideKickAnimation();
                }

                if (IsPlayingDiveAnim)
                {
                    ClearDiveAnim();

                    StartFallHeight = float.MaxValue;

                    StartedFalling = true;

                    SpecialLanding = true;
                }
                return;
            }

            if (IsDoingGlideKick)
            {
                SubGlide_HandleGlideKick();

                return;
            }
            else
            {
                if (IsPlayingGlideKickAnim)
                {
                    ClearGlideKickAnimation();
                }
            }


            if (IsDiving)
            {
                if (!IsPlayingDiveAnim)
                {
                    if (IsPlayingGlideAnim)
                    {
                        ClearGlideAnimation();
                    }

                    var initialVelocity = Ped.Velocity;

                    if (Ped.IsVaulting)
                    {
                        Ped.Task.ClearAllImmediately();
                    }
                    else
                    {
                        Ped.Task.ClearAll();
                    }

                    Ped.Velocity = Vector3.Zero;

                    Ped.PlayAnimation("skydive@freefall", "free_forward", 4.0f, -4.0f, -1, AnimationFlags.Loop, 0f);

                    Ped.SetConfigFlag(60, true);

                    Ped.Velocity = initialVelocity;

                    IsPlayingDiveAnim = true;
                }
                else
                {
                    GlideVerticalAdditive = Vector3.Lerp(GlideVerticalAdditive, new Vector3(22.5f * GlidingMovement.X, 0f, 0f), Game.LastFrameTime * 2f);

                    var pedUpQuaternion = Quaternion.Euler(0, 0, Ped.Rotation.Z) * Quaternion.Euler(new Vector3(0, -60f, 0)) * Quaternion.Euler(GlideVerticalAdditive);

                    Ped.SetConfigFlag(60, true);

                    Ped.ApplyForce((Vector3.RelativeBottom * (float)Math.Pow(9.81f, 2)) * 0.5f * Game.LastFrameTime);

                    Ped.Quaternion = pedUpQuaternion;
                }
            }
            else
            {
                if (!IsPlayingGlideAnim)
                {
                    InitialGlideVelocity = Vector3.RelativeFront * Math.Min(new Vector3(Ped.Velocity.X, Ped.Velocity.Y, 0f).Length(), 30f) + new Vector3(0, 0, Math.Min(Ped.Velocity.Z, 30f));

                    GlideDrag = 3f;

                    GlideDownSpeed = -0.6f;

                    var wasDiving = false;

                    if (IsPlayingDiveAnim)
                    {
                        ClearDiveAnim();

                        GlideDrag = 4f;

                        wasDiving = true;
                    }

                    var initialVelocity = Ped.Velocity;

                    Ped.Task.ClearAll();

                    Ped.Velocity = Vector3.Zero;

                    Ped.PlayAnimation("swimming@first_person@", "walk_fwd_0_up_loop", 4.0f, -4.0f, -1, AnimationFlags.Loop, 0.5f);

                    Ped.PlayAnimation("misslamar1leadinout", "magenta_idle", 4.0f, -4.0f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation, 0f);

                    Ped.SetAnimationSpeed("swimming@first_person@", "walk_fwd_0_up_loop", 0f);

                    Ped.SetAnimationSpeed("misslamar1leadinout", "magenta_idle", 0f);

                    if (wasDiving)
                    {
                        GlideHorizontalAdditive = new Vector3(0f, -10f, 0f);
                    }

                    Ped.SetConfigFlag(60, true);

                    Ped.Velocity = initialVelocity;

                    IsPlayingGlideAnim = true;
                }
                else
                {
                    Ped.SetConfigFlag(60, true);

                    GlideHorizontalAdditive = Vector3.Lerp(GlideHorizontalAdditive, new Vector3(0, (32f * GlidingMovement.Y), 0), Game.LastFrameTime * 5f);

                    GlideVerticalAdditive = Vector3.Lerp(GlideVerticalAdditive, new Vector3(6.5f * GlidingMovement.X, 0f, 0f), Game.LastFrameTime * 5f);

                    var pedUpQuaternion = Quaternion.Euler(0, 0, Ped.Rotation.Z) * Quaternion.Euler(new Vector3(0, -85f, 0) - GlideHorizontalAdditive) * Quaternion.Euler(GlideVerticalAdditive);

                    Ped.Quaternion = pedUpQuaternion;

                    GlideDrag = Mathf.Lerp(GlideDrag, 0.1f, Game.LastFrameTime * 0.15f);

                    GlideDownSpeed = Mathf.Lerp(GlideDownSpeed, 0.6f, Game.LastFrameTime * 0.8f);

                    Ped.Velocity = (Ped.ForwardVector + ((Ped.UpVector + Vector3.WorldUp * GlideDownSpeed) * GlideDrag)) * 0.25f * Math.Min(InitialGlideVelocity.Length(), 50) + new Vector3(0, 0, -9.81f);
                }
            }

            if (Ped.HeightAboveGround < MinGlideHeight)
            {
                IsGliding = false;
            }
        }

        private void StartGrappleTaskSequence()
        {
            var vel = Ped.Velocity;

            Ped.Task.ClearAll();

            Ped.Velocity = Vector3.Zero;

            if (!Ped.GetConfigFlag(60))
            {
                Ped.PlayAnimation("move_fall", "fall_med", 8.0f, -8.0f, -1, AnimationFlags.Loop);
            }

            Ped.PlayAnimation("weapons@pistol_1h@", "fire_med", 8.0f, -8.0f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.StayInEndFrame | AnimationFlags.AllowRotation, 0.0f);

            Ped.Velocity = vel;

            Ped.SetConfigFlag(60, true);
        }

        /// <summary>
        /// Toggle dive on/off.
        /// </summary>
        /// <param name="toggle"></param>
        public void SetDiveToggle(bool toggle)
        {
            if (!IsGliding)
            {
                IsDiving = false;

                return;
            }

            IsDiving = toggle;
        }

        private void ClearDiveAnim()
        {
            Ped.Task.ClearAnimation("skydive@freefall", "free_forward");

            IsPlayingDiveAnim = false;
        }

        /// <summary>
        /// Stops the player glide.
        /// </summary>
        public void StopGlide()
        {
            if (!IsGliding)
            {
                return;
            }

            if (IsDoingGlideKick)
            {
                IsDoingGlideKick = false;

                return;
            }

            IsGliding = false;
        }

        /// <summary>
        /// Start's a glide kick while we're in the air.
        /// </summary>
        /// <param name="target">The entity to kick.</param>
        public void StartGlideKick(Entity target)
        {
            if (!IsGliding || !IsFalling)
            {
                return;
            }

            if (IsFalling && Ped.HeightAboveGround > MinGlideStartHeight)
            {
                IsGliding = true;
            }

            IsDoingGlideKick = true;
        }

        /// <summary>
        /// Make batman grapple towards the specified point.
        /// </summary>
        /// <param name="point"></param>
        public void GrappleToPoint(Vector3 point, Vector3 wallNormal)
        {
            if (IsAttackingAnything || IsDoingCombatRoll || IsDiving || Ped.IsRagdoll || Ped.IsGettingUp || IsGrappling)
            {
                return;
            }

            if (IsGliding)
            {
                IsGliding = false;
            }

            LastGrapplePoint = point;
            LastGrappleWallNormal = wallNormal;
            IsGrappling = true;
        }

        private void ClearGrappleAnimation()
        {
            IsPlayingGrappleAnimation = false;
        }

        private void ClearGrappleReelAnimation()
        {
            IsPlayingGrappleReelAnimation = false;
        }

        private void ClearGlideAnimation()
        {
            Ped.Task.ClearAnimation("swimming@first_person@", "walk_fwd_0_up_loop");

            Ped.Task.ClearAnimation("misslamar1leadinout", "magenta_idle");

            IsPlayingGlideAnim = false;
        }

        private void ClearGlideKickAnimation()
        {

        }

        private void SubGlide_HandleGlideKick()
        {

        }

        private void PerformAttack(Entity entity)
        {
            if (IsAttackingAnything || IsFalling || IsDoingCombatRoll || IsGliding || IsThrowingBatarang)
            {
                return;
            }

            LastAttackEntity = entity;
        }

        private void HandleAttack()
        {
            if (!Exists(LastAttackEntity) || IsFalling)
            {
                if (Exists(LastAttackEntity))
                {
                    LastAttackEntity = null;
                }

                return;
            }
        }

        private void ClearAttackAnimation()
        {

        }

        private void TestFall()
        {
            PedFallFlag = IsFalling;

            if (!PedFallFlag || StartedFalling)
            {
                return;
            }

            StartFallHeight = Ped.HeightAboveGround;

            StartedFalling = true;
        }

        public override void Abort()
        {
            //Ped.FreezePosition = false;

            if (GrappleHook != null)
            {
                GrappleHook.Delete();
            }

            if (GrappleTaskSequence != null)
            {
                GrappleTaskSequence.Dispose();
                Ped.Task.ClearAll();
            }

            if (IsPlayingGlideAnim)
            {
                ClearGlideAnimation();
            }

            if (IsDoingGlideKick)
            {
                ClearGlideKickAnimation();
            }

            if (IsDoingCombatRoll || IsAttackingAnything)
            {
                ClearAttackAnimation();
            }

            if (IsThrowingBatarang)
            {
                ClearBatarangAnimation();
            }

            if (IsPlayingGrappleAnimation)
            {
                ClearGrappleAnimation();
            }

            if (IsPlayingGrappleReelAnimation)
            {
                ClearGrappleReelAnimation();
            }

            if (BatGrappleInstance != null)
            {
                BatGrappleInstance.Delete();
            }
        }
    }
}
