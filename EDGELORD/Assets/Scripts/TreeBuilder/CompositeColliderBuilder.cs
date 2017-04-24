using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    public class CompositeColliderBuilder : MonoBehaviour
    {
        public TreeRoot treeRoot;
        public Transform treeStart;
        public KeyCode testKey;
        public PolygonCollider2D myCollider;
        public float currentArea;
        [Space]
        public Color TeritoryColor;
        public MeshFilter myMeshFilter;
        public MeshRenderer myMeshRenderer;
        public int orderInLayer = -1;

        private void Start()
        {
            treeRoot.OnUpdateTreeCollider += BuildCompositeCollider;
        }

        private void Update()
        {
            if (Input.GetKeyDown(testKey))
            {
                BuildCompositeCollider();
            }
        }

        private void BuildCompositeCollider()
        {
            var colliders = treeStart.GetComponentsInChildren<PolygonCollider2D>();
            Debug.Log("Polygons: " + colliders.Length);
            List<Vector2> AllPoints = new List<Vector2>();
            foreach (PolygonCollider2D c in colliders)
            {
                foreach (Vector2 point in c.points)
                {
                    //AllPoints.Add(c.transform.TransformPoint(point));
                    AllPoints.Add(myCollider.transform.InverseTransformPoint(c.transform.TransformPoint(point)));
                }
            }
            Vector2[] pointArray = ArrangeVertices(ref AllPoints);

            myCollider.points = pointArray;
            MakeConvex(ref myCollider);
            pointArray = myCollider.points;
            if (!SpriteSlicer2D.IsConvex(ref pointArray))
            {
                Debug.Log("Trying again.");
                MakeConvex(ref myCollider);
            }
            var area = GetArea(ref pointArray);
            currentArea = area;
            treeRoot.SetTotalArea(area);
            //Debug.Log(area);

            CreateMesh();
            //InitSprite(gameObject, GetComponent<Renderer>(), ref myCollider,);
        }

        private void CreateMesh()
        {
            Vector2 minCoords = myCollider.bounds.min;
            Vector2 maxCoords = myCollider.bounds.max;
            Bounds spriteBounds = myCollider.bounds;
            Vector2 parentCentroid = Vector2.zero;
            Vector2 uvOffset = Vector2.zero;

            Mesh spriteMesh = new Mesh();
            spriteMesh.name = "SlicedSpriteMesh";
            myMeshFilter.mesh = spriteMesh;
            myMeshRenderer.sortingOrder = orderInLayer;
            myMeshRenderer.material.color = TeritoryColor;

            Vector2[] polygonPoints = myCollider.points;
            int numVertices = polygonPoints.Length;
            Vector3[] vertices = new Vector3[numVertices];
            Color[] colors = new Color[numVertices];
            Vector2[] uvs = new Vector2[numVertices];
            int[] triangles;

            // Convert vector2 -> vector3
            for (int loop = 0; loop < vertices.Length; loop++)
            {
                vertices[loop] = polygonPoints[loop];
                colors[loop] = Color.white;
            }

            Vector2 uvWidth = maxCoords - minCoords;
            Vector3 boundsSize = spriteBounds.size;
            Vector2 invBoundsSize = new Vector2(1.0f / boundsSize.x, 1.0f / boundsSize.y);

            for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
            {
                Vector2 vertex = polygonPoints[vertexIndex] + parentCentroid;
                float widthFraction = 0.5f + ((vertex.x * invBoundsSize.x) + (uvOffset.x));
                float heightFraction = 0.5f + ((vertex.y * invBoundsSize.y) + (uvOffset.y));

                

                Vector2 texCoords = new Vector2();

                //if (rotated)
                //{
                //    texCoords.y = maxCoords.y - (uvWidth.y * (1.0f - widthFraction));
                //    texCoords.x = minCoords.x + (uvWidth.x * heightFraction);
                //}
                //else
                //{
                    texCoords.x = minCoords.x + (uvWidth.x * widthFraction);
                    texCoords.y = minCoords.y + (uvWidth.y * heightFraction);
                //}

                uvs[vertexIndex] = texCoords;
            }

            List<Vector2> polyPointList = new List<Vector2>(polygonPoints);
            triangles = SpriteSlicer2D.Triangulate(ref polyPointList);


            spriteMesh.Clear();
            spriteMesh.vertices = vertices;
            spriteMesh.uv = uvs;
            spriteMesh.triangles = triangles;
            spriteMesh.colors = colors;
            spriteMesh.RecalculateBounds();
            spriteMesh.RecalculateNormals();
        }


        //============================================================================================================================================

        // Vector sorting function
        static SpriteSlicer2D.VectorComparer s_VectorComparer = new SpriteSlicer2D.VectorComparer();

        // Get the area of the given polygon
        public static float Area(ref List<Vector2> points)
        {
            int n = points.Count;
            float A = 0.0f;

            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Vector2 pval = points[p];
                Vector2 qval = points[q];
                A += pval.x * qval.y - qval.x * pval.y;
            }

            return (A * 0.5f);
        }
        /// <summary>
        /// Sort the vertices into a counter clockwise order
        /// </summary>
        static Vector2[] ArrangeVertices(ref List<Vector2> vertices)
        {
            float determinant;
            int numVertices = vertices.Count;
            int counterClockWiseIndex = 1;
            int clockWiseIndex = numVertices - 1;
            vertices.Sort(s_VectorComparer);
            List<Vector2> sortedVertices = new List<Vector2>(numVertices);

            for (int vertex = 0; vertex < numVertices; vertex++)
            {
                sortedVertices.Add(Vector2.zero);
            }

            Vector2 startPoint = vertices[0];
            Vector2 endPoint = vertices[numVertices - 1];
            sortedVertices[0] = startPoint;

            for (int vertex = 1; vertex < numVertices - 1; vertex++)
            {
                determinant = CalculateDeterminant2x3(startPoint, endPoint, vertices[vertex]);

                if (determinant < 0)
                {
                    sortedVertices[counterClockWiseIndex++] = vertices[vertex];
                }
                else
                {
                    sortedVertices[clockWiseIndex--] = vertices[vertex];
                }
            }

            sortedVertices[counterClockWiseIndex] = endPoint;
            return sortedVertices.ToArray();
        }
        static float CalculateDeterminant2x3(Vector2 start, Vector2 end, Vector2 point)
        {
            return start.x * end.y + end.x * point.y + point.x * start.y - start.y * end.x - end.y * point.x - point.y * start.x;
        }

        /// <summary>
        /// Work out the area defined by the vertices
        /// </summary>
        static float GetArea(ref Vector2[] vertices)
        {
            // Check that the total area isn't stupidly small
            int numVertices = vertices.Length;
            float area = vertices[0].y * (vertices[numVertices - 1].x - vertices[1].x);

            for (int i = 1; i < numVertices; i++)
            {
                area += vertices[i].y * (vertices[i - 1].x - vertices[(i + 1) % numVertices].x);
            }

            return Mathf.Abs(area * 0.5f);
        }

        static void MakeConvex(ref PolygonCollider2D polyCollider)
        {
            //for (int loop = 0; loop < Selection.gameObjects.Length; loop++)
            //{
            //    PolygonCollider2D polyCollider = Selection.gameObjects[loop].GetComponent<PolygonCollider2D>();

                if (polyCollider)
                {
                    List<Vector2> vertices = new List<Vector2>(polyCollider.points);
                    int originalNumVertices = vertices.Count;
                    int iterations = 0;

                    if (SpriteSlicer2D.IsConvex(ref vertices))
                    {
                        //Debug.Log(Selection.gameObjects[loop].name + " is already convex - no work to do");
                    }
                    else
                    {
                        do
                        {
                            float determinant;
                            Vector3 v1 = vertices[0] - vertices[vertices.Count - 1];
                            Vector3 v2 = vertices[1] - vertices[0];
                            float referenceDeterminant = SpriteSlicer2D.CalculateDeterminant2x2(v1, v2);

                            for (int i = 1; i < vertices.Count - 1;)
                            {
                                v1 = v2;
                                v2 = vertices[i + 1] - vertices[i];
                                determinant = SpriteSlicer2D.CalculateDeterminant2x2(v1, v2);

                                if (referenceDeterminant * determinant < 0.0f)
                                {
                                    vertices.RemoveAt(i);
                                }
                                else
                                {
                                    i++;
                                }
                            }

                            v1 = v2;
                            v2 = vertices[0] - vertices[vertices.Count - 1];
                            determinant = SpriteSlicer2D.CalculateDeterminant2x2(v1, v2);

                            if (referenceDeterminant * determinant < 0.0f)
                            {
                                vertices.RemoveAt(vertices.Count - 1);
                            }

                            iterations++;
                        } while (!SpriteSlicer2D.IsConvex(ref vertices) && iterations < 25);
                    }

                if (SpriteSlicer2D.IsConvex(ref vertices))
                {
                    polyCollider.points = vertices.ToArray();
                    //Debug.Log(Selection.gameObjects[loop].name + " points reduced to " + vertices.Count.ToString() + " from " + originalNumVertices.ToString());
                }
                //else
                //{
                //    Debug.Log(Selection.gameObjects[loop].name + " could not be made convex, please adjust shape manually");
                //}
            }
            

        }
        /// <summary>
        /// Initialise this sprite using the given polygon definition
        /// </summary>
        void InitSprite(GameObject parentObject, Renderer parentRenderer, ref PolygonCollider2D polygon, ref Vector2[] polygonPoints, Vector3 minCoords, Vector3 maxCoords, Bounds spriteBounds, Material material, bool rotated, bool hFlipped, bool vFlipped, Vector2 parentCentroid, Vector2 uvOffset, bool isConcave)
        {
            var m_MinCoords = minCoords;
            var m_MaxCoords = maxCoords;
            var m_SpriteBounds = spriteBounds;
            var m_VFlipped = vFlipped;
            var m_HFlipped = hFlipped;
            var m_Rotated = rotated;
            m_SpriteBounds = spriteBounds;
            var m_UVOffset = uvOffset;

            //var m_Centroid 

            gameObject.tag = parentObject.tag;
            gameObject.layer = parentObject.layer;

            Mesh spriteMesh = new Mesh();
            spriteMesh.name = "SlicedSpriteMesh";
            myMeshFilter.mesh = spriteMesh;

            int numVertices = polygonPoints.Length;
            Vector3[] vertices = new Vector3[numVertices];
            Color[] colors = new Color[numVertices];
            Vector2[] uvs = new Vector2[numVertices];
            int[] triangles;

            // Convert vector2 -> vector3
            for (int loop = 0; loop < vertices.Length; loop++)
            {
                vertices[loop] = polygonPoints[loop];
                colors[loop] = Color.white;
            }

            Vector2 uvWidth = maxCoords - minCoords;
            Vector3 boundsSize = spriteBounds.size;
            Vector2 invBoundsSize = new Vector2(1.0f / boundsSize.x, 1.0f / boundsSize.y);

            for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
            {
                Vector2 vertex = polygonPoints[vertexIndex] + parentCentroid;
                float widthFraction = 0.5f + ((vertex.x * invBoundsSize.x) + (uvOffset.x));
                float heightFraction = 0.5f + ((vertex.y * invBoundsSize.y) + (uvOffset.y));

                if (hFlipped)
                {
                    widthFraction = 1.0f - widthFraction;
                }

                if (vFlipped)
                {
                    heightFraction = 1.0f - heightFraction;
                }

                Vector2 texCoords = new Vector2();

                if (rotated)
                {
                    texCoords.y = maxCoords.y - (uvWidth.y * (1.0f - widthFraction));
                    texCoords.x = minCoords.x + (uvWidth.x * heightFraction);
                }
                else
                {
                    texCoords.x = minCoords.x + (uvWidth.x * widthFraction);
                    texCoords.y = minCoords.y + (uvWidth.y * heightFraction);
                }

                uvs[vertexIndex] = texCoords;
            }

            if (isConcave)
            {
                List<Vector2> polyPointList = new List<Vector2>(polygonPoints);
                triangles = SpriteSlicer2D.Triangulate(ref polyPointList);
            }
            else
            {
                int triangleIndex = 0;
                triangles = new int[numVertices * 3];

                for (int vertexIndex = 1; vertexIndex < numVertices - 1; vertexIndex++)
                {
                    triangles[triangleIndex++] = 0;
                    triangles[triangleIndex++] = vertexIndex + 1;
                    triangles[triangleIndex++] = vertexIndex;
                }
            }

            spriteMesh.Clear();
            spriteMesh.vertices = vertices;
            spriteMesh.uv = uvs;
            spriteMesh.triangles = triangles;
            spriteMesh.colors = colors;
            spriteMesh.RecalculateBounds();
            spriteMesh.RecalculateNormals();
            ;

            Vector2 localCentroid = Vector3.zero;

            if (SpriteSlicer2D.s_CentreChildSprites)
            {
                localCentroid = spriteMesh.bounds.center;

                // Finally, fix up our mesh, collider, and object position to at the same position as the pivot point
                for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
                {
                    vertices[vertexIndex] -= (Vector3)localCentroid;
                }

                for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
                {
                    polygonPoints[vertexIndex] -= localCentroid;
                }

                //m_Centroid = localCentroid + parentCentroid;
                polygon.points = polygonPoints;
                spriteMesh.vertices = vertices;
                spriteMesh.RecalculateBounds();
            }

            //Transform parentTransform = parentObject.transform;
            //m_Transform.parent = parentTransform.parent;
            //m_Transform.position = parentTransform.position + (parentTransform.rotation * (Vector3)localCentroid);
            //m_Transform.rotation = parentTransform.rotation;
            //m_Transform.localScale = parentTransform.localScale;
            //m_MeshRenderer.material = material;

            //m_MeshRenderer.sortingLayerID = parentRenderer.sortingLayerID;
            //m_MeshRenderer.sortingOrder = parentRenderer.sortingOrder;
        }
    }
}