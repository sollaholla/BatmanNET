using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace BatmanNET.Native
{
    public static class PedExtensions
    {
        public static void PlayAnimation(this Ped ped, string dictionary, string name, float blendIn, float blendOut, int duration, AnimationFlags flags, float playbackRate = 0.0f, float timeUntilForceAnim = 0.5f)
        {
            if (ped.IsPlayingAnimation(dictionary, name))
            {
                return;
            }

            ped.Task.PlayAnimation(dictionary, name, blendIn, blendOut, duration, flags, playbackRate);

            var waitTime = DateTime.Now.AddSeconds(timeUntilForceAnim);

            while (!ped.IsPlayingAnimation(dictionary, name))
            {
                if (DateTime.Now > waitTime)
                {
                    ped.Task.ClearAllImmediately();

                    var didSetVel = false;
                    var initialVel = Vector3.Zero;

                    if (ped.IsFalling)
                    {
                        initialVel = ped.Velocity;
                        didSetVel = true;
                        ped.Velocity = Vector3.Zero;
                    }

                    ped.Task.PlayAnimation(dictionary, name, blendIn, blendOut, duration, flags, playbackRate);

                    if (didSetVel)
                    {
                        ped.SetConfigFlag(60, true);
                        ped.Velocity = initialVel;
                    }

                    waitTime = DateTime.Now.AddSeconds(timeUntilForceAnim);
                }

                Script.Yield();
            }
        }
    }

    public static class EntityExtensions
    {
        public static bool IsPlayingAnimation(this Entity entity, string dictionary, string name)
        {
            return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, entity.Handle, dictionary, name, 3);
        }

        public static float GetSpeed(this Entity entity)
        {
            return Function.Call<float>(Hash.GET_ENTITY_SPEED, entity.Handle);
        }

        public static void SetAnimationSpeed(this Entity entity, string dictionary, string name, float speedMultiplier)
        {
            Function.Call(Hash.SET_ENTITY_ANIM_SPEED, entity.Handle, dictionary, name, speedMultiplier);
        }
    }
}
