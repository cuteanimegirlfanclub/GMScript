using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GMEngine
{
    public class ViewConeDetector : MonoBehaviour
    {
        public float distance = 10f;
        public float angel = 30f;
        public float height = 1.0f;
        public Color meshColor = Color.cyan;

        public int scanFrequency = 30;
        public LayerMask layers;
        public LayerMask occulusionLayers;

        private Collider[] colliderBuffer = new Collider[10];
        private int count;

        private float scanInterval;
        private float scanTimer;

        private Mesh viewMesh;

        private PlayerKnowledge knowledge;

        private Mesh CreateDetectMesh()
        {
            Mesh mesh = new Mesh();

            int segments = 10;
            int numTriangle = (segments * 4) + 2 + 2;
            int numVertices = 3 * numTriangle;

            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -angel, 0) * Vector3.forward * distance;
            Vector3 bottomRight = Quaternion.Euler(0, angel, 0) * Vector3.forward * distance;

            Vector3 topCenter = bottomCenter + Vector3.up * height;
            Vector3 topLeft = bottomLeft + Vector3.up * height;
            Vector3 topRight = bottomRight + Vector3.up * height;

            int vert = 0;

            //left side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            //right side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;


            float currentAngel = -angel;
            float deltaAngel = (angel * 2) / segments;
            for (int i = 0; i < segments; ++i)
            {
                currentAngel += deltaAngel;
                bottomLeft = Quaternion.Euler(0, currentAngel, 0) * Vector3.forward * distance;
                bottomRight = Quaternion.Euler(0, currentAngel + deltaAngel, 0) * Vector3.forward * distance;

                topLeft = bottomLeft + Vector3.up * height;
                topRight = bottomRight + Vector3.up * height;

                //far side
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;

                //top 
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;

                //bottom side
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
            }

            for(int i = 0; i < numVertices; ++i)
            {
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }

        private void Awake()
        {
        }

        private void Start()
        {
            scanInterval = 1.0f / scanFrequency;
            knowledge = GetComponentInParent<PlayerKnowledge>();
        }

        private void Update()
        {
            scanTimer -= Time.deltaTime;
            if(scanTimer < 0 )
            {
                scanTimer += scanInterval;
                Scan();
            }
        }

        private void Scan()
        {
            count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliderBuffer, layers, QueryTriggerInteraction.Collide);

            knowledge.ClearKonwledge();

            for(int i = 0; i < count; ++i)
            {
                Transform transform = colliderBuffer[i].transform;
                if (isInSight(transform.gameObject))
                {
                    knowledge.AddToKnowledge(transform);
                }
            }
        }

        public bool isInSight(GameObject obj)
        {
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 direction = dest - origin;
            if(direction.y < 0 || direction.y > height) return false;

            direction.y = 0;
            float deltaAngel = Vector3.Angle(direction, transform.forward);
            if (deltaAngel > angel) return false;

            origin.y += height / 2;
            dest.y = origin.y;
            if(Physics.Linecast(origin, dest, occulusionLayers))
            {
                return false;
            }

            return true;
        }

        private void OnValidate()
        {
            viewMesh = CreateDetectMesh();
            scanInterval = 1.0f / scanFrequency;
        }

        private void OnDrawGizmos()
        {
            if (viewMesh)
            {
                Gizmos.color = meshColor;
                Gizmos.DrawMesh(viewMesh, transform.position, transform.rotation);
            }

            Gizmos.DrawWireSphere(transform.position, distance);
            for (int i = 0; i < count; ++i)
            {
                Gizmos.DrawSphere(colliderBuffer[i].transform.position, 0.2f);
            }
        }
    }


}
