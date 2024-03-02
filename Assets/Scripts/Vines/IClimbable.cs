using UnityEngine;

namespace Vines
{
    public interface IClimbable
    {
        public float GetXPosition();
        public float GetHeadYPosition();

        public Transform GetParentTransform();
    }
}
