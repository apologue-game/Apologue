using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public Transform startBounds;
        public Transform endBounds;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public float verticalPositionLimit = -1;

        private float m_OffsetZ;
        public float cameraYPositionOffset = 0f;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            if (target == null)
            {
                StartCoroutine("SearchForPlayer");
                return;
            }
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            newPos = new Vector3(Mathf.Clamp(newPos.x, startBounds.position.x, endBounds.position.x), Mathf.Clamp(newPos.y, target.position.y + cameraYPositionOffset, Mathf.Infinity), newPos.z);
            if (newPos.y < verticalPositionLimit)
            {
               
                newPos = new Vector3(Mathf.Clamp(newPos.x, startBounds.position.x, endBounds.position.x), Mathf.Clamp(newPos.y, verticalPositionLimit, Mathf.Infinity), newPos.z);
            }
            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }

        IEnumerator SearchForPlayer()
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
                yield break;
            }
            else if (playerObject == null)
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine("SearchForPlayer");
            }
        }
    }
}
