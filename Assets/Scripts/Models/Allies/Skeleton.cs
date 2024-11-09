using UnityEngine;

namespace Models.Allies
{
    public class Skeleton : AI.DefencePointAI
    {
        public override void Reset()
        {
            base.Reset();
            DefencePoint = null;
        }
    }
}