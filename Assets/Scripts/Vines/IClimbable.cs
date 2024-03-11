using UnityEngine;

namespace Vines
{
    public interface IClimbable
    {
        public bool IsGrowing();
        public float GetXPosition();
        public float GetHeadYPosition();
        public float GetBottomYPosition();

        public Transform GetParentTransform();
    }
}
