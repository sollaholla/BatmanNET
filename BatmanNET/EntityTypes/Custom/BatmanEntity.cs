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
            MinGlideHeight = 5f;
            MinGlideStartHeight = 20f;
            MaxQuickAttackDistance = 5f;
            MaxAttackDistance = 15f;
            MaxLandHeight = 5f;
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
        public Entity CurrentAttackEntity {
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
        /// The amount of drag applied to the player after a dive or fall.
        /// </summary>
        public float GlideDrag {
            get; private set;
        }

        public override void Update()
        {
            HandleGlide();
            HandleAttack();
            HandleFalling();
        }

        private void HandleFalling()
        {
            TestFall();

            if (!StartedFalling || Ped.HeightAboveGround > 1.5f)
            {
                return;
            }

            if (StartFallHeight > MaxLandHeight)
            {
                // TODO: Catch landing.
            }

            StartedFalling = false;
            StartFallHeight = 0.0f;
        }

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
            if (!IsFalling || IsGliding || Ped.HeightAboveGround < MinGlideStartHeight)
            {
                return;
            }

            IsGliding = true;
        }

        private void HandleGlide()
        {
            if (!IsGliding)
            {
                if (IsPlayingGlideAnim)
                {
                    ClearGlideAnimation();
                }

                if (IsPlayingGlideKickAnim)
                {
                    ClearGlideKickAnimation();
                }

                if (IsPlayingDiveAnim)
                {
                    ClearDiveAnim();
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

                    Ped.Task.ClearAllImmediately();

                    Ped.Velocity = Vector3.Zero;

                    Ped.Task.PlayAnimation("skydive@freefall", "free_forward", 4.0f, -8.0f, -1, AnimationFlags.Loop, 0f);

                    Ped.SetConfigFlag(60, true);

                    Ped.Velocity = initialVelocity;

                    IsPlayingDiveAnim = true;
                }
                else
                {
                    GlideVerticalAdditive = Vector3.Lerp(GlideVerticalAdditive, new Vector3(22.5f * GlidingMovement.X, 0f, 0f), Game.LastFrameTime * 2f);

                    var pedUpQuaternion = Quaternion.Euler(0, 0, Ped.Rotation.Z) * Quaternion.Euler(new Vector3(0, -60f, 0)) * Quaternion.Euler(GlideVerticalAdditive);

                    Ped.SetConfigFlag(60, true);

                    Ped.ApplyForce((Vector3.RelativeBottom * (float)Math.Pow(9.81f, 2)) * Game.LastFrameTime);

                    Ped.Quaternion = pedUpQuaternion;
                }
            }
            else
            {
                if (!IsPlayingGlideAnim)
                {
                    InitialGlideVelocity = Vector3.RelativeFront * Math.Min(new Vector3(Ped.Velocity.X, Ped.Velocity.Y, 0f).Length(), 30f) + new Vector3(0, 0, Math.Min(Ped.Velocity.Z, 30f));

                    GlideDrag = 3f;

                    if (IsPlayingDiveAnim)
                    {
                        ClearDiveAnim();

                        GlideHorizontalAdditive = new Vector3(0f, 80f, 0f);

                        GlideDrag = 4f;
                    }

                    var initialVelocity = Ped.Velocity;

                    Ped.Task.ClearAllImmediately();

                    Ped.Velocity = Vector3.Zero;

                    Ped.PlayAnimation("swimming@first_person@", "walk_fwd_0_up_loop", 8.0f, -8.0f, -1, AnimationFlags.Loop, 0.5f);

                    Ped.PlayAnimation("misslamar1leadinout", "magenta_idle", 8.0f, -8.0f, -1, AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation, 0f);

                    Ped.SetAnimationSpeed("swimming@first_person@", "walk_fwd_0_up_loop", 0f);

                    Ped.SetAnimationSpeed("misslamar1leadinout", "magenta_idle", 0f);

                    Ped.SetConfigFlag(60, true);

                    Ped.Velocity = initialVelocity;

                    IsPlayingGlideAnim = true;
                }
                else
                {
                    Ped.SetConfigFlag(60, true);

                    GlideHorizontalAdditive = Vector3.Lerp(GlideHorizontalAdditive, new Vector3(0, (22.5f * GlidingMovement.Y), 0), Game.LastFrameTime * 5f);

                    GlideVerticalAdditive = Vector3.Lerp(GlideVerticalAdditive, new Vector3(5.5f * GlidingMovement.X, 0f, 0f), Game.LastFrameTime * 5f);

                    var pedUpQuaternion = Quaternion.Euler(0, 0, Ped.Rotation.Z) * Quaternion.Euler(new Vector3(0, -85f, 0) - GlideHorizontalAdditive) * Quaternion.Euler(GlideVerticalAdditive);

                    Ped.Quaternion = pedUpQuaternion;

                    GlideDrag = Mathf.Lerp(GlideDrag, 0.1f, Game.LastFrameTime * 0.25f);

                    Ped.Velocity = (Ped.ForwardVector + ((Ped.UpVector + Vector3.WorldUp * 0.6f) * GlideDrag)) * 0.5f * Math.Min(InitialGlideVelocity.Length(), 50) + new Vector3(0, 0, -9.81f);
                }
            }

            if (Ped.HeightAboveGround < MinGlideHeight)
            {
                IsGliding = false;
            }
        }

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

            CurrentAttackEntity = entity;
        }

        private void HandleAttack()
        {
            if (!Exists(CurrentAttackEntity) || IsFalling)
            {
                if (Exists(CurrentAttackEntity))
                {
                    CurrentAttackEntity = null;
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
        }
    }
}
