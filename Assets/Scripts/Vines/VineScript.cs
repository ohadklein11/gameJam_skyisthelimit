using UnityEngine;

namespace Vines
{
    public class VineScript : MonoBehaviour, IClimbable
    {
        public float GetXPosition()
        {
            return transform.position.x;
        }
        // need to implement growth over time
    }
}
