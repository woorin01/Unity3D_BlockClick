using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject mCube;
    public GameObject[,,] mBlocks = new GameObject[200, 200, 200];

    private void Awake()
    {
        mCube.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;//마우스 커서 고정
        Cursor.visible = false;
    }

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                mBlocks[99 + i, 100, 99 + j] = Instantiate<GameObject>(Resources.Load<GameObject>("Cube"));
                mBlocks[99 + i, 100, 99 + j].transform.position = new Vector3(99 + i, 100, 99 + j);
            }
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                int x = 0, y = 0, z = 0;
                Vector3 htp = hit.transform.position;

                if (htp.x + 0.4f <= hit.point.x && IsBlockNone((int)htp.x + 1, (int)htp.y, (int)htp.z)) { x = 1; y = 0; z = 0; }
                else if (htp.x - 0.4f >= hit.point.x && IsBlockNone((int)htp.x - 1, (int)htp.y, (int)htp.z)) { x = -1; y = 0; z = 0; }

                if (htp.y + 0.4f <= hit.point.y && IsBlockNone((int)htp.x, (int)htp.y + 1, (int)htp.z)) { x = 0; y = 1; z = 0; }
                else if (htp.y - 0.4f >= hit.point.y && IsBlockNone((int)htp.x, (int)htp.y - 1, (int)htp.z)) { x = 0; y = -1; z = 0; }

                if (htp.z + 0.4f <= hit.point.z && IsBlockNone((int)htp.x, (int)htp.y, (int)htp.z + 1)) { x = 0; y = 0; z = 1; }
                else if (htp.z - 0.4f >= hit.point.z && IsBlockNone((int)htp.x, (int)htp.y, (int)htp.z - 1)) { x = 0; y = 0; z = -1; }

                mCube.transform.position = new Vector3(htp.x + x, htp.y + y, htp.z + z);

                if (Input.GetMouseButtonUp(1))
                {
                    GameObject temp = Instantiate<GameObject>(Resources.Load<GameObject>("Cube"));
                    temp.transform.position = new Vector3(htp.x + x, htp.y + y, htp.z + z);
                    mBlocks[(int)htp.x + x, (int)htp.y + y, (int)htp.z + z] = temp;
                }

                mCube.SetActive(true);
            }
        }
        else
            mCube.SetActive(false);

    }

    private bool IsBlockNone(int x, int y, int z)
    {
        if (mBlocks[x, y, z] != null)
            return false;

        return true;
    }
}
