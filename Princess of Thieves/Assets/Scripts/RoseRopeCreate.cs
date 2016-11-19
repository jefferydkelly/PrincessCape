#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RoseRopeCreate : MonoBehaviour {

    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject ropeBit;

    public float scalela = 1.5f;
    public float mass = 0.1f;
    public float grav = 1;
    public string layer;

    //GameObject startPointGo;
    //GameObject endPointGo;

    List<GameObject> ropeBitsList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        GetEndPoint();
        GenerateRope();
       
	}

    void GetEndPoint()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 200.0f, (1 << LayerMask.NameToLayer("Platforms")));
        if (hit.collider)
        {
            endPoint = hit.collider.gameObject;
        }
    }
    void GenerateRope()
    {
        float distance = Vector2.Distance(new Vector2(endPoint.transform.position.x, endPoint.transform.position.y), new Vector2(startPoint.transform.position.x, startPoint.transform.position.y));

        if (distance == 0)
            Debug.LogError("Rope Creator: The distance between the rope's endpoints = 0!");

        //1 is prefab scale
        // The length of the ropeBit prefab (CircleCollider2D diameter * scale)
        float bitlength = scalela * 2 * ropeBit.GetComponent<CircleCollider2D>().radius;
        // The number of the sections we need for the rope
        int sectionsNumber = Mathf.CeilToInt(distance / bitlength);

        // Triangle data
        float triangleX = endPoint.transform.position.x - startPoint.transform.position.x;
        float triangleY = endPoint.transform.position.y - startPoint.transform.position.y;
        float triangleDiagonal = Mathf.Sqrt(Mathf.Pow(triangleX, 2) + Mathf.Pow(triangleY, 2));

        // The angle of the triangle
        float angle = Mathf.Atan2(triangleY, triangleX) * Mathf.Rad2Deg;

        // Data for the ropeBit prefab's HingeJoint2D component
        float localX = Mathf.Abs(ropeBit.GetComponent<CircleCollider2D>().radius * triangleX / triangleDiagonal);
        float localY = Mathf.Abs(ropeBit.GetComponent<CircleCollider2D>().radius * triangleY / triangleDiagonal);

        // Signs for the ropeBit prefab's HingeJoint2D's ConnectedAnchor / Anchor component
        int signX, signY;

        if (triangleX >= 0) { signX = 1; }
        else { signX = -1; }
        if (triangleY >= 0) { signY = 1; }
        else { signY = -1; }

        if (ropeBit == null)
            Debug.LogError("The ropeBit prefab is not loaded! Place the ropeBit prefab in the Resources/Prefabs/ folder");

        if (ropeBit != null)
        {

            // Create a folder for the ropeBit prefabs
            GameObject ropeFolder = new GameObject("Rope");
            // Set the folder's position to (0,0,0) - optional
            ropeFolder.transform.position = new Vector3(0, 0, 0);

            // Creating the rope
            for (int i = 0; i < sectionsNumber; i++)
            {

                Vector3[] pPoints = new Vector3[sectionsNumber];

                // The positions of the ropeBit prefabs
                pPoints[i] = new Vector3(((distance - bitlength * i) * startPoint.transform.position.x + bitlength * i * endPoint.transform.position.x) / distance,
                                         ((distance - bitlength * i) * startPoint.transform.position.y + bitlength * i * endPoint.transform.position.y) / distance,
                                           0);

                // Instantiate the ropeBit prefabs
                GameObject ropeBitPref = (GameObject)PrefabUtility.InstantiatePrefab(ropeBit);

                // Set every ropeBit prefab's position to the calculated position
                ropeBitPref.transform.position = pPoints[i];

                // Give an index to the ropeBit prefabs's name - optional
                string name = string.Format(ropeBitPref.name + "_{0}", i);
                ropeBitPref.name = name;

                // Scale the prefab
                ropeBitPref.transform.localScale = new Vector3(1, 1, 1);

				// Add Gizmos to see the CircleCollider2D component
				if (ropeBitPref.GetComponent<CircleCollider2D>() == null)
				{
					Debug.LogError("Rope Creator: No CircleCollider2D added to the ropeBit prefab!");
				}

                // Add the prefabs to a list
                ropeBitsList.Add(ropeBitPref);

                // Put the rope prefabs to the created folder
                ropeBitsList[i].transform.parent = ropeFolder.transform;

                // The ropeBit prefabs must contain a child gameobject with a Sprite Renderer component (rope graphics)! - the child gameobject (graphics) will be rotated, not the actual ropeBit prefab
                if (ropeBitsList[i].transform.childCount == 0)
                    Debug.LogError("No child object in the rope prefab. You need to add a child object with a Sprite Renderer component to the rope prefab!");
                if (ropeBitsList[i].transform.GetComponentInChildren<SpriteRenderer>() == null)
                    Debug.LogError("No Sprite Renderer component on the ropeBit prefab's child object. You need to add a Sprite Renderer component to the rope prefab's child gameobject!");

                // Get the child ("Graphics") form the ropeBit prefab, and rotate it to the correct angle
                if (ropeBitsList[i].transform.childCount != 0 && ropeBitsList[i].transform.GetComponentInChildren<SpriteRenderer>() != null)
                    ropeBitsList[i].transform.GetComponentInChildren<SpriteRenderer>().transform.GetComponent<Transform>().Rotate(0, 0, angle);

                // Add Joints to the ropeBit prefabs
                if (ropeBitsList[i].GetComponent<DistanceJoint2D>() == null)
                    ropeBitsList[i].AddComponent<DistanceJoint2D>();

                if (ropeBitsList[i].GetComponentInChildren<HingeJoint2D>() == null)
                    ropeBitsList[i].AddComponent<HingeJoint2D>();

                // Set the mass and gravity scale for the ropeBit prefab
                if (ropeBitsList[i].GetComponent<Rigidbody2D>() != null)
                {
                    ropeBitsList[i].GetComponent<Rigidbody2D>().mass = 1;
                    ropeBitsList[i].GetComponent<Rigidbody2D>().gravityScale = 0.1f;
                }
                else
                {
                    ropeBitsList[i].AddComponent<Rigidbody2D>();
                    ropeBitsList[i].GetComponent<Rigidbody2D>().mass = 1;
                    ropeBitsList[i].GetComponent<Rigidbody2D>().gravityScale = 0.1f;

                }

                // Set the layer for the rope prefabs - optional
                int layerInt = LayerMask.NameToLayer("Rope");

                if (layerInt == -1)
                    Debug.LogWarning("Rope Creator: There is no such layer! - use an existing Layer");
                else
                    ropeBitsList[i].layer = layerInt;

                // Configure the Joints
                if (i == 0)
                {
                    if (ropeBitsList[i].GetComponent<HingeJoint2D>() != null)
                        ropeBitsList[i].GetComponent<HingeJoint2D>().enabled = false;
                    if (ropeBitsList[i].GetComponent<DistanceJoint2D>() != null)
                        ropeBitsList[i].GetComponent<DistanceJoint2D>().enabled = false;
                }


                if (i >= 1)
                {
                    // DistanceJoint2D
                    ropeBitsList[i].GetComponent<DistanceJoint2D>().enabled = true;
                    ropeBitsList[i].GetComponent<DistanceJoint2D>().connectedBody = ropeBitsList[i - 1].GetComponent<Rigidbody2D>();
                    ropeBitsList[i].GetComponent<DistanceJoint2D>().connectedAnchor = new Vector2(0, 0);
                    ropeBitsList[i].GetComponent<DistanceJoint2D>().anchor = new Vector2(0, 0);
                    ropeBitsList[i].GetComponent<DistanceJoint2D>().distance = ropeBitsList[i - 1].GetComponent<CircleCollider2D>().radius * 2.04f * scalela;
                    ropeBitsList[i].GetComponent<DistanceJoint2D>().maxDistanceOnly = true;

                    //if (useBreakForce)
                    //    ropeBitsList[i].GetComponent<DistanceJoint2D>().breakForce = breakForce;


                    // HingeJoint2D
                    ropeBitsList[i].GetComponent<HingeJoint2D>().enabled = true;
                    ropeBitsList[i].GetComponent<HingeJoint2D>().connectedBody = ropeBitsList[i - 1].GetComponent<Rigidbody2D>();
                    ropeBitsList[i].GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(localX * signX, localY * signY);
                    ropeBitsList[i].GetComponent<HingeJoint2D>().anchor = new Vector2(localX * signX * -1, localY * signY * -1);


                    ropeBitsList[i].GetComponent<DistanceJoint2D>().enableCollision = false;
                    ropeBitsList[i].GetComponent<HingeJoint2D>().enableCollision = false;
                  
                }

                //  Unity - Edit    - Undo function
                //Undo.RegisterCreatedObjectUndo(ropeBitPref, "Created rope");
                if (i == 0)
                {
                    ropeBitsList[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}
#endif