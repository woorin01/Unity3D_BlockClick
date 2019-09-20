﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GhostBlock ghostBlock;
    public GameObject[,,] blocks;
    public ChangeImage changeImage;
    public int startingPoint;

    private readonly int mapEnd = 400;
    private readonly int mapStart = 0;
    private Shader glassShader;
    private block hitBlock;

    private void Awake()
    {
        glassShader = Shader.Find("Unlit/Transparent");
        blocks = new GameObject[mapEnd, mapEnd, mapEnd];

        Cursor.lockState = CursorLockMode.Locked;//마우스 커서 고정
        Cursor.visible = false;

    }

    void Start()
    {
        ghostBlock.MeshRenderer.enabled = false;
        ghostBlock.InPlayer = false;

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                blocks[startingPoint + i, 20, startingPoint + j] = Instantiate<GameObject>(Resources.Load<GameObject>("Cube"));
                blocks[startingPoint + i, 20, startingPoint + j].transform.position = new Vector3(startingPoint + i, 20, startingPoint + j);
                blocks[startingPoint + i, 20, startingPoint + j].transform.parent = GameObject.Find("Map").transform;
                blocks[startingPoint + i, 20, startingPoint + j].GetComponent<block>().healthPoint = 100f;
            }
        }
    }

    void Update()
    {
        int layerMask = 1 << LayerMask.NameToLayer("TranslucentBlock");// TranslucentBlock레이어만 무시하고 충돌
        layerMask = ~layerMask;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));//Ray를 쏠 지점을 화면의 정가운데로 한다
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 5f, layerMask))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Vector3 htp = hit.transform.position;
                Vector3 installBlockPos = GetInstallBlockPos(htp, hit.point);

                ghostBlock.transform.position = installBlockPos;

                if (Input.GetMouseButtonDown(1))
                    ClickMakeBlock(installBlockPos, htp);

                else if (Input.GetMouseButton(0))
                    ClickDestroyBlock(htp);
            }
        }
        else
            ghostBlock.MeshRenderer.enabled = false;

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.blue, 0.3f);
    }

    private void ClickMakeBlock(Vector3 installBlockPos, Vector3 htp)
    {
        if (!IsBlockNone((int)installBlockPos.x, (int)installBlockPos.y, (int)installBlockPos.z, htp) || ghostBlock.GetComponent<GhostBlock>().InPlayer)
            return;

        GameObject temp = Instantiate<GameObject>(Resources.Load<GameObject>("Cube"));
        temp.transform.position = installBlockPos;
        temp.GetComponent<block>().healthPoint = 100f;
        temp.GetComponent<MeshRenderer>().material.color = changeImage.color;
        temp.GetComponent<MeshRenderer>().material.mainTexture = changeImage.ghostBlockMaterial.mainTexture;
        if (changeImage.imageNum.Equals(8))
            temp.GetComponent<MeshRenderer>().material.shader = glassShader;

        blocks[(int)installBlockPos.x, (int)installBlockPos.y, (int)installBlockPos.z] = temp;

        ghostBlock.MeshRenderer.enabled = false;
    }

    private void ClickDestroyBlock(Vector3 htp)
    {
        if (hitBlock == blocks[(int)htp.x, (int)htp.y, (int)htp.z].GetComponent<block>())
            Debug.Log("same block");
        else
            Debug.Log("different block");

        hitBlock = blocks[(int)htp.x, (int)htp.y, (int)htp.z].GetComponent<block>();
        hitBlock.healthPoint--;

        if (hitBlock.healthPoint <= 0f)
        {
            Destroy(blocks[(int)htp.x, (int)htp.y, (int)htp.z]);
            blocks[(int)htp.x, (int)htp.y, (int)htp.z] = null;
        }
    }

    private Vector3 GetInstallBlockPos(Vector3 htp, Vector3 hitPoint)
    {
        if (htp.x + 0.5f <= hitPoint.x && IsBlockNone((int)htp.x + 1, (int)htp.y, (int)htp.z, htp)) { return new Vector3(htp.x + 1, htp.y, htp.z); }
        else if (htp.x - 0.5f >= hitPoint.x && IsBlockNone((int)htp.x - 1, (int)htp.y, (int)htp.z, htp)) { return new Vector3(htp.x - 1, htp.y, htp.z); }

        if (htp.y + 0.5f <= hitPoint.y && IsBlockNone((int)htp.x, (int)htp.y + 1, (int)htp.z, htp)) { return new Vector3(htp.x, htp.y + 1, htp.z); }
        else if (htp.y - 0.5f >= hitPoint.y && IsBlockNone((int)htp.x, (int)htp.y - 1, (int)htp.z, htp)) { return new Vector3(htp.x, htp.y - 1, htp.z); }

        if (htp.z + 0.5f <= hitPoint.z && IsBlockNone((int)htp.x, (int)htp.y, (int)htp.z + 1, htp)) { return new Vector3(htp.x, htp.y, htp.z + 1); }
        else if (htp.z - 0.5f >= hitPoint.z && IsBlockNone((int)htp.x, (int)htp.y, (int)htp.z - 1, htp)) { return new Vector3(htp.x, htp.y, htp.z - 1); }

        return new Vector3(htp.x, htp.y, htp.z);
    }


    private bool IsBlockNone(int x, int y, int z, Vector3 htp)
    {
        if (blocks[x, y, z] != null)//블록이 이미 있는 곳에 고스트 블럭, 블럭 생성 불가하게
        {
            ghostBlock.MeshRenderer.enabled = false;
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
                ghostBlock.MeshRenderer.enabled = false;
                return false;
            }
        }

        if (!ghostBlock.InPlayer)
            ghostBlock.MeshRenderer.enabled = true;
        return true;
    }
}
