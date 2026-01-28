using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BodyPlacing : MonoBehaviour
{
    public Camera gameCamera;
    public GameObject selectedBodyPrefab;
    public GameObject placePointPrefab;
    public GameObject bodyParent;
    public GameObject placeParent;
    public GameObject ghostMassPrefab;
    public GameObject ghostBlock;
    GameObject[] placePoints;
    public int gridSize;

    public float massCount = 1;
    public float legCount = 1;

    public GameObject[] placeablebodyParts;
    int selectedBodyPart = 0;
    public Boolean placingBody = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePlacePoints();
        gameCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!placingBody)
        {
            ghostBlock.SetActive(false);
            return;
        }    
        Vector3 mousePos = gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        GameObject closestPoint = null;
        float distanceToClosestPoint = 100000f;
        foreach (GameObject obj in placePoints)
        {
            if ((obj != null) && (Vector3.Distance(obj.transform.position, mousePos) < distanceToClosestPoint))
            {
                closestPoint = obj;
                distanceToClosestPoint = Vector3.Distance(obj.transform.position, mousePos);
            }
        }

        if (closestPoint != null)
        {
            float distanceToMass = 100000f;
            float placeAngle = 0;
            for (int i = 0; i < 4; i++)
            {
                float currentRadian = Mathf.PI / 2 * i;
                float massX = closestPoint.transform.position.x - Mathf.Cos(currentRadian);
                float massZ = closestPoint.transform.position.z + Mathf.Sin(currentRadian);
                if (Physics.Raycast(new Vector3(massX, 1.5f, massZ), -transform.up, 100, LayerMask.GetMask("Mass")))
                {
                    float dist = Vector3.Distance(mousePos, new Vector3(massX, 0, massZ));
                    if (dist < distanceToMass)
                    {
                        placeAngle = currentRadian;
                        distanceToMass = dist;
                    }
                }
            }
            ghostBlock.SetActive(true);
            ghostBlock.transform.position = closestPoint.transform.position;
            ghostBlock.transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * placeAngle, 0);
            ghostBlock.GetComponent<GhostDrawer>().selectedChild = selectedBodyPart;

            if (Input.GetKeyDown(KeyCode.Mouse0) && (closestPoint != null))
            {
                Instantiate(selectedBodyPrefab, new Vector3(closestPoint.transform.position.x, closestPoint.transform.position.y, closestPoint.transform.position.z), Quaternion.Euler(0, Mathf.Rad2Deg * placeAngle, 0), bodyParent.transform);
                if (selectedBodyPart == 0) massCount++;
                else if (selectedBodyPart == 1) legCount++;
                UpdatePlacePoints();
                placingBody = false;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                selectedBodyPart++;
                if (selectedBodyPart > placeablebodyParts.Length - 1) selectedBodyPart = 0;
                selectedBodyPrefab = placeablebodyParts[selectedBodyPart];
            }
        }
        
    }

    public void UpdatePlacePoints()
    {
        if (placePoints != null)
        {
            foreach (GameObject obj in placePoints)
            {
                Destroy(obj);
            }
        }
        for (int i = 0; i < gridSize*gridSize; i++)
        {
            float placeX = transform.position.x + (i % gridSize) - 3;
            float placeZ = transform.position.z + Mathf.Floor(i / gridSize) - 3;

            //Instantiate(placePointPrefab, new Vector3(placeX, 0.5f, placeZ), Quaternion.identity, transform);

            if (Physics.Raycast(new Vector3(placeX+1, 1.5f, placeZ), -transform.up, 100, LayerMask.GetMask("Mass")) ||
                Physics.Raycast(new Vector3(placeX-1, 1.5f, placeZ), -transform.up, 100, LayerMask.GetMask("Mass")) ||
                Physics.Raycast(new Vector3(placeX, 1.5f, placeZ+1), -transform.up, 100, LayerMask.GetMask("Mass")) ||
                Physics.Raycast(new Vector3(placeX, 1.5f, placeZ-1), -transform.up, 100, LayerMask.GetMask("Mass")))
            {
                if (!Physics.Raycast(new Vector3(placeX, 1.5f, placeZ), -transform.up, 100, LayerMask.GetMask("Mass","Weapon")))
                {
                    Debug.Log("SPAWN");
                    Instantiate(placePointPrefab, new Vector3(placeX, 0.5f, placeZ), Quaternion.identity, placeParent.transform);
                }
            }
        }
        placePoints = GameObject.FindGameObjectsWithTag("PlacePoint");

        Debug.Log("HELP!");
    }
}
