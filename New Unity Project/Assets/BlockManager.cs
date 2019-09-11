using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GhostBlock mCube;
    public GameObject[,,] mBlocks;
    public ChangeImage changeImage;
    public int startingPoint;

    private int mapEnd = 400;
    private int mapStart = 0;

    private void Awake()
    {
        mCube.MeshRenderer.enabled = false;
        mCube.InPlayer = false;
        mBlocks = new GameObject[mapEnd, mapEnd, mapEnd];
        changeImage = GetComponent<ChangeImage>();

        Cursor.lockState = CursorLockMode.Locked;//마우스 커서 고정
        Cursor.visible = false;

    }

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                mBlocks[startingPoint + i, 20, startingPoint + j] = Instantiate<GameObject>(Resources.Load<GameObject>("Cube"));
                mBlocks[startingPoint + i, 20, startingPoint + j].transform.position = new Vector3(startingPoint + i, 20, startingPoint + j);
                mBlocks[startingPoint + i, 20, startingPoint + j].transform.parent = GameObject.Find("Map").transform;
            }
        }
        Debug.Log("Start1");

        Debug.Log("Start2");
    }

    IEnumerator a()
    {
        //Debug.Log("yield1");
        yield return null;
        //Debug.Log("yield2");
    }
    void Update()
    {
        //StartCoroutine("a");
        //Debug.Log("Update");
        int layerMask = 1 << LayerMask.NameToLayer("TranslucentBlock");    // TranslucentBlock레이어만 무시하고 충돌
        layerMask = ~layerMask;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));//Ray를 쏠 지점을 화면의 정가운데로 한다

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 5f, layerMask))
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

                if (Input.GetMouseButtonDown(1) && IsBlockNone((int)htp.x + x, (int)htp.y + y, (int)htp.z + z, htp) && !mCube.GetComponent<GhostBlock>().InPlayer)
                {
                    GameObject temp = Instantiate<GameObject>(Resources.Load<GameObject>("Cube"));
                    temp.transform.position = new Vector3(htp.x + x, htp.y + y, htp.z + z);
                    temp.GetComponent<MeshRenderer>().material.color = changeImage.color;
                    temp.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load<Texture>(changeImage.imageNum.ToString());

                    mBlocks[(int)htp.x + x, (int)htp.y + y, (int)htp.z + z] = temp;

                    mCube.MeshRenderer.enabled = false;
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    Destroy(mBlocks[(int)htp.x, (int)htp.y, (int)htp.z]);
                    mBlocks[(int)htp.x, (int)htp.y, (int)htp.z] = null;
                }

            }
        }
        else
            mCube.MeshRenderer.enabled = false;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.blue, 0.3f);
    }

    private bool IsBlockNone(int x, int y, int z, Vector3 htp)
    {
        if (mBlocks[x, y, z] != null)//블록이 이미 있는 곳에 고스트 블럭, 블럭 생성 불가하게
        {
            mCube.MeshRenderer.enabled = false;
            return false;
        }

        if (x - htp.x == 1 || x - htp.x == -1 || //맵끝 판별
           y - htp.y == 1 || y - htp.y == -1 ||
           z - htp.z == 1 || z - htp.z == -1)
        {
            if (x >= mapEnd || x <= -1 ||
                y >= mapEnd || y <= -1 ||
                z >= mapEnd || z <= -1)
            {
                mCube.MeshRenderer.enabled = false;
                return false;
            }
        }

        if (!mCube.InPlayer)
            mCube.MeshRenderer.enabled = true;
        return true;
    }
}
