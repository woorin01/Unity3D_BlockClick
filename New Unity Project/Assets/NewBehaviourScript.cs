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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
        {
            Debug.Log(hit.point);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                int x = 0, y = 0, z = 0;
                Vector3 htp = hit.transform.position;

                if (htp.x + 0.4f <= hit.point.x) { x = 1; y = 0; z = 0; }
                else if (htp.x - 0.4f >= hit.point.x) { x = -1; y = 0; z = 0; }

                if (htp.y + 0.4f <= hit.point.y) { x = 0; y = 1; z = 0; }
                else if (htp.y - 0.4f >= hit.point.y) { x = 0; y = -1; z = 0; }

                if (htp.z + 0.4f <= hit.point.z) { x = 0; y = 0; z = 1; }
                else if (htp.z - 0.4f >= hit.point.z) { x = 0; y = 0; z = -1; }

                mCube.transform.position = new Vector3(htp.x + x, htp.y + y, htp.z + z);

                if (Input.GetMouseButtonUp(0))
                    Instantiate<GameObject>(Resources.Load<GameObject>("Cube")).transform.position = new Vector3(htp.x + x, htp.y + y, htp.z + z);

                mCube.SetActive(true);
            }
        }
        else
            mCube.SetActive(false);
        
    }
}
