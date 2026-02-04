using UnityEngine;

public class GhostDrawer : MonoBehaviour
{
    public GameObject[] childList;
    public int selectedChild;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject child in childList)
        {
            child.SetActive(false);
        }
        childList[selectedChild].SetActive(true);
    }
}
