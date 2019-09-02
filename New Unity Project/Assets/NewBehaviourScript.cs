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
                int x = 0, y = 0, z = 0;

                if (hit.transform.position.x + 0.4f <= hit.point.x) { x = 1; y = 0; z = 0; }
                else if (hit.transform.position.x - 0.4f >= hit.point.x) { x = -1; y = 0; z = 0; }

                if (hit.transform.position.y + 0.4f <= hit.point.y) { x = 0; y = 1; z = 0; }
                else if (hit.transform.position.y - 0.4f >= hit.point.y) { x = 0; y = -1; z = 0; }

                if (hit.transform.position.z + 0.4f <= hit.point.z) { x = 0; y = 0; z = 1; }
                else if (hit.transform.position.z - 0.4f >= hit.point.z) { x = 0; y = 0; z = -1; }

                mCube.transform.position = new Vector3(hit.transform.position.x + x, hit.transform.position.y + y, hit.transform.position.z + z);

                if (Input.GetMouseButtonUp(0))
                    Instantiate<GameObject>(Resources.Load<GameObject>("Cube")).transform.position = new Vector3(hit.transform.position.x + x, hit.transform.position.y + y, hit.transform.position.z + z);

                mCube.SetActive(true);
            }
        }
        else
            mCube.SetActive(false);
        
    }
}
