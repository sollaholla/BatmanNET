using BatmanNET.Interfaces;
using GTA;

namespace BatmanNET.EntityTypes
{
    public abstract class Character : Entity, IUpdatable
    {
        protected Character(Ped ped) : base(ped.Handle)
        {
            Ped = ped;
        }

        /// <summary>
        /// The main ped.
        /// </summary>
        public Ped Ped { get; }
        public abstract void Update();
        public abstract void Abort();
    }
}
