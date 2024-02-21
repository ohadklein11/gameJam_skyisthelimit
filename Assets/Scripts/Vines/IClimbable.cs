using UnityEngine;

namespace Vines
{
    public interface IClimbable
    {
        public float GetXPosition();
        public Transform GetParentTransform();
    }
}
