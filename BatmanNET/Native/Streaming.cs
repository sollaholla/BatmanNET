using GTA.Native;

namespace BatmanNET.Native
{
    public static class Streaming
    {
        /// <summary>
        /// Requests an animation dictionary.
        /// </summary>
        /// <param name="name"></param>
        public static void RequestAnimDict(string name)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, name);
        }
    }
}
