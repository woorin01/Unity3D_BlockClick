using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject mCube;
    public GameObject[,,] mBlocks;
    public ChangeColor changeColor;
    public int startingPoint;
    private int mapEnd = 300;
    private int mapStart = 0;

    private void Awake()
    {
        mCube.SetActive(false);
        mBlocks = new GameObject[mapEnd, mapEnd, mapEnd];
        changeColor = GetComponent<ChangeColor>();

        Cursor.lockState = CursorLockMode.Locked;//마우스 커서 고정
        Cursor.visible = false;

    }

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                mBlocks[startingPoint + i, mapEnd / 2, startingPoint + j] = Instantiate<GameObject>(Resources.Load<GameObject>("Cube"));
                mBlocks[startingPoint + i, mapEnd / 2, startingPoint + j].transform.position = new Vector3(startingPoint + i, mapEnd / 2, startingPoint + j);
            }
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));//Ray를 쏠 지점을 화면의 정가운데로 한다

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                int x = 0, y = 0, z = 0;
                Vector3 htp = hit.transform.position;

                if (htp.x + 0.5f <= hit.point.x && IsBlockNone((int)htp.x + 1, (int)htp.y, (int)htp.z, htp)) { x = 1; y = 0; z = 0; }
                else if (htp.x - 0.5f >= hit.point.x && IsBlockNone((int)htp.x - 1, (int)htp.y, (int)htp.z, htp)) { x = -1; y = 0; z = 0; }

                if (htp.y + 0.5f <= hit.point.y && IsBlockNone((int)htp.x, (int)htp.y + 1, (int)htp.z, htp)) { x = 0; y = 1; z = 0; }
                else if (htp.y - 0.5f >= hit.point.y && IsBlockNone((int)htp.x, (int)htp.y - 1, (int)htp.z, htp)) { x = 0; y = -1; z = 0; }

                if (htp.z + 0.5f <= hit.point.z && IsBlockNone((int)htp.x, (int)htp.y, (int)htp.z + 1, htp)) { x = 0; y = 0; z = 1; }
                else if (htp.z - 0.5f >= hit.point.z && IsBlockNone((int)htp.x, (int)htp.y, (int)htp.z - 1, htp)) { x = 0; y = 0; z = -1; }

                mCube.transform.position = new Vector3(htp.x + x, htp.y + y, htp.z + z);

                if (Input.GetMouseButtonUp(1) && IsBlockNone((int)htp.x + x, (int)htp.y + y, (int)htp.z + z, htp))
                {
                    GameObject temp = Instantiate<GameObject>(Resources.Load<GameObject>("Cube"));
                    temp.transform.position = new Vector3(htp.x + x, htp.y + y, htp.z + z);
                    temp.GetComponent<MeshRenderer>().material.color = changeColor.color;
                    mBlocks[(int)htp.x + x, (int)htp.y + y, (int)htp.z + z] = temp;
                    mCube.SetActive(false);
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    Destroy(mBlocks[(int)htp.x, (int)htp.y, (int)htp.z]);
                    mBlocks[(int)htp.x, (int)htp.y, (int)htp.z] = null;
                }

            }
        }
        else
            mCube.SetActive(false);

    }

    private bool IsBlockNone(int x, int y, int z, Vector3 htp)
    {
        if(x - htp.x == 1  || x - htp.x == -1 ||
           y - htp.y == 1 || y - htp.y == -1 ||
           z - htp.z ==1  || z - htp.z == -1)
        {
            if (x >= mapEnd || x <= -1 ||
                y >= mapEnd || y <= -1 ||
                z >= mapEnd || z <= -1 )
            {
                mCube.SetActive(false);
                return false;
            }
        }
        
        if (mBlocks[x, y, z] != null)
        {
            mCube.SetActive(false);
            return false;
        }

        mCube.SetActive(true);
        return true;
    }
}
