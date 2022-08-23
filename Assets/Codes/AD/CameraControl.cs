// Character Control V1.1 By VF
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AD
{
    public class CameraControl : MonoBehaviour
    {
        [HideInInspector] public Transform follow;
        //private PlayerControl playerControl;
        [Range(0, 360)] public float degreeX = 10;
        [Range(0, 30)] public float speed = 10;
        [Range(15, 190)] public float degreeY = 55;
        [Range(1, 15)] public float distance = 4;
        [Range(1.2f, 2)] public float hight = 1.7f;

        [Range(1, 20)] public float rotationSpeed = 2f;

        private float toDegreeX, toDegreeY, toDistance;
        private Vector3 center;
        private Vector3 toPosition, toLookAt;
        public float distanceMin = 2f, distanceMax = 15f;
        private float hightMin = 1.2f, hightMax = 2.0f;
        

        void Update()
        {
            if (follow == null) return;
            CameraInput();
            CameraSystem();
        }
        public void Setup(Transform follow)
        {
            this.follow = follow;
            toDegreeX = degreeX;
            toDegreeY = degreeY;
            toDistance = distance;
            toPosition = transform.position;
            toLookAt = follow.position;
            CameraSystem();
        }

        private void CameraSystem()
        {
            toDegreeX = Mathf.Lerp(toDegreeX, degreeX, speed * Time.deltaTime);
            toDegreeY = Mathf.Lerp(toDegreeY, degreeY, speed * Time.deltaTime);
            toDistance = Mathf.Lerp(toDistance, distance, speed * Time.deltaTime);
            float x = toDegreeX * Mathf.Deg2Rad;
            float y = toDegreeY * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(x) * Mathf.Sin(y), Mathf.Cos(y), Mathf.Cos(x) * Mathf.Sin(y)) * toDistance;
            center = follow.position + (Vector3.up * hight);
            toPosition = center + position;
            if (toPosition.y < follow.position.y + 0.15f) toPosition.y = follow.position.y + 0.15f;
            transform.position = toPosition;
            transform.LookAt(center);
        }

        private void CameraInput()
        {
            distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y, distanceMin, distanceMax);
            float distanceRaw = distance - distanceMin;
            float distanceMaxRaw = distanceMax - distanceMin;
            float distancePercent = distanceRaw / distanceMaxRaw;
            hight = Mathf.Lerp(hight, hightMin + ((hightMax - hightMin) * (1 - distancePercent)), speed * Time.deltaTime);
            if (Input.GetButton("Fire2"))
            {
                degreeX += Input.GetAxis("Mouse X") * rotationSpeed;
                degreeY = Mathf.Clamp(degreeY + Input.GetAxis("Mouse Y") * rotationSpeed, 15, 120);
            }
        }
    }
}
