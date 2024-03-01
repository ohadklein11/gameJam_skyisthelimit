using System;
using UnityEngine;

namespace Utils
{
    public class SlopeChecker: MonoBehaviour
    {
        [SerializeField] private float slopeCheckDistance;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private PhysicsMaterial2D fullFriction;
        [SerializeField] private PhysicsMaterial2D noFriction;
        
        private Rigidbody2D _rb;
        private bool _isOnSlope;
        private Vector2 _slopeNormalPerp;
        private float _slopeDownAngle;
        private float _slopeSideAngle;
        private float _lastSlopeAngle;
        private bool _canWalkOnSlope;
        private const float Tolerance = .001f;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void SlopeCheck(Vector2 checkPos, int direction, float xInput)
        {
            SlopeCheckHorizontal(checkPos, direction);
            SlopeCheckVertical(checkPos, xInput);
        }
        
        public bool CheckUnwalkableSlopeInFront(Vector2 checkPos, int direction)
        {
            var right = transform.right * direction;
            RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, right, slopeCheckDistance * 1.5f, whatIsGround);
            if (slopeHitFront)
            {
                return Vector2.Angle(slopeHitFront.normal, Vector2.up) >= maxSlopeAngle;
            }

            return false;
        }

        private void SlopeCheckHorizontal(Vector2 checkPos, int direction)
        {
            var right = transform.right * direction;
            RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, right, slopeCheckDistance, whatIsGround);
            RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -right, slopeCheckDistance, whatIsGround);
            Debug.DrawRay(checkPos, right * slopeCheckDistance, Color.red);
            Debug.DrawRay(checkPos, -right * slopeCheckDistance, Color.red);
            
            if (slopeHitFront)
            {
                _isOnSlope = true;
                _slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
            }
            else if (slopeHitBack)
            {
                _isOnSlope = true;
                _slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
            }
            else
            {
                _slopeSideAngle = 0.0f;
                _isOnSlope = false;
            }
        }

        private void SlopeCheckVertical(Vector2 checkPos, float xInput)
        {
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

            if (hit)
            {
                _slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

                _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (Math.Abs(_slopeDownAngle - _lastSlopeAngle) > Tolerance)
                {
                    _isOnSlope = true;
                }

                _lastSlopeAngle = _slopeDownAngle;

                Debug.DrawRay(hit.point, _slopeNormalPerp, Color.blue);
                Debug.DrawRay(hit.point, hit.normal, Color.green);
            }

            if (_slopeDownAngle > maxSlopeAngle || _slopeSideAngle > maxSlopeAngle)
            {
                _canWalkOnSlope = false;
            }
            else
            {
                _canWalkOnSlope = true;
            }

            if (_isOnSlope && _canWalkOnSlope && xInput == 0.0f)
            {
                _rb.sharedMaterial = fullFriction;
            }
            else
            {
                _rb.sharedMaterial = noFriction;
            }
        }
        
        public bool IsOnSlope()
        {
            return _isOnSlope;
        }
        
        public bool CanWalkOnSlope()
        {
            return _canWalkOnSlope;
        }
        
        public float GetSlopeDownAngle()
        {
            return _slopeDownAngle;
        }
        
        public float GetMaxSlopeAngle()
        {
            return maxSlopeAngle;
        }

        public Vector2 GetSlopeNormalPerp()
        {
            return _slopeNormalPerp;
        }
    }
}