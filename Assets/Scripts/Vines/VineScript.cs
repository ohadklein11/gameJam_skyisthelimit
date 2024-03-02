using UnityEngine;

namespace Vines
{
    public class VineScript : MonoBehaviour, IClimbable
    {
        public float GetXPosition()
        {
            return transform.position.x;
        }
        
        public float GetHeadYPosition()
        {
            Transform firstStem = transform.parent.GetChild(0).transform;
            return firstStem.position.y + firstStem.GetComponent<Collider2D>().bounds.extents.y;
        }
        
        public Transform GetParentTransform()
        {
            return transform.parent.transform;
        }
    }
}
