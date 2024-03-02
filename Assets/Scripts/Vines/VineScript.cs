using UnityEngine;

namespace Vines
{
    public class VineScript : MonoBehaviour, IClimbable
    {
        private GrowVineScript _growVineScript;
        
        void Start()
        {
            _growVineScript = GetComponentInParent<GrowVineScript>();
        }
        
        public float GetXPosition()
        {
            return transform.position.x;
        }
        
        public float GetHeadYPosition()
        {
            Transform firstStem = transform.parent.GetChild(0).transform;
            return firstStem.position.y + firstStem.GetComponent<Collider2D>().bounds.extents.y;
        }
        
        public bool IsGrowing()
        {
            return _growVineScript.growing;;
        }
        public Transform GetParentTransform()
        {
            return transform.parent.transform;
        }
    }
}
