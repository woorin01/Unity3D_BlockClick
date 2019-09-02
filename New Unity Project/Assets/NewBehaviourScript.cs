using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject mCube;

    private void Awake()
    {
        mCube.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Debug.Log(hit.point);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                if(hit.transform.position.x + 0.4f <= hit.point.x)
                    mCube.transform.position = new Vector3(hit.transform.position.x + 1, hit.transform.position.y, hit.transform.position.z);
                else if(hit.transform.position.x - 0.4f >= hit.point.x)
                    mCube.transform.position = new Vector3(hit.transform.position.x - 1, hit.transform.position.y, hit.transform.position.z);

                if (hit.transform.position.y + 0.4f <= hit.point.y)
                    mCube.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
                else if (hit.transform.position.y - 0.4f >= hit.point.y)
                    mCube.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y - 1, hit.transform.position.z);

                if (hit.transform.position.z + 0.4f <= hit.point.z)
                    mCube.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + 1);
                else if (hit.transform.position.z - 0.4f >= hit.point.z)
                    mCube.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - 1);

                mCube.SetActive(true);
            }
        }
        else
            mCube.SetActive(false);
        
    }
}
