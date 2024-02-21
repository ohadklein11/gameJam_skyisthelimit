using UnityEngine;

namespace Vines
{
    public class VineScript : MonoBehaviour, IClimbable
    {
        public float GetXPosition()
        {
            return transform.position.x;
        }
        
        public Transform GetParentTransform()
        {
            return transform.parent.transform;
        }
    }
}
