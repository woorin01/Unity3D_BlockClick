using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//벽을 뚫어서 설치됨
//그래서 중복 설치도 됨
public class BlockManager : MonoBehaviour
{
    public ChangeImage changeImage;
    public int startingPoint;

    private Shader glassShader;
    private block hitBlock;

    private void Awake()
    {
        glassShader = Shader.Find("Unlit/Transparent");

        Cursor.lockState = CursorLockMode.Locked;//마우스 커서 고정
        Cursor.visible = false;
    }

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                GameObject temp = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/0"));
                temp.transform.position = new Vector3(startingPoint + i, 20, startingPoint + j);
                temp.transform.parent = GameObject.Find("Map").transform;
            }
        }
    }

    void Update()
    {
        int layerMask = 1 << LayerMask.NameToLayer("TranslucentBlock");// TranslucentBlock레이어만 무시하고 충돌
        layerMask = ~layerMask;

        //Ray ray = new Ray(transform.position, transform.forward);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));//Ray를 쏠 지점을 화면의 정가운데로 한다
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //ray.direction += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)); //ray.direction의 값은 normalized 되어 있기 때문에 쏘는 방향을 다르게 해주려면 저렇게 해줘야됨

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 7f, layerMask))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Vector3 installPos = hit.transform.position;
                installPos += hit.normal;

                if (Input.GetMouseButtonDown(1))
                    ClickMakeBlock(installPos);

                if (Input.GetMouseButton(0))
                    DestroyBlock(hit.transform.GetComponent<block>());
                if (Input.GetMouseButtonUp(0))
                    StopDestroyBlock(hit.transform.GetComponent<block>());
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 5f, Color.blue, 0.3f);
    }

    private void ClickMakeBlock(Vector3 installPos)
    {
        if (Vector3.Distance(transform.position, installPos) <= 1.05f)
            return;

        Debug.Log(Vector3.Distance(transform.position, installPos));
        GameObject temp = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/" + changeImage.imageNum.ToString()));
        temp.transform.position = installPos;
        temp.GetComponent<MeshRenderer>().material.color = changeImage.color;
    }

    private void DestroyBlock(block block)
    {
        if (hitBlock == null)
            hitBlock = block;
        if (hitBlock != block)
        {
            StopDestroyBlock(hitBlock);
            hitBlock = block;
        }

        hitBlock.healthPoint--;
        Debug.Log(hitBlock.healthPoint);
        if (hitBlock.healthPoint <= 0f)
            Destroy(hitBlock.gameObject);
    }

    private void StopDestroyBlock(block block)
    {
        block.healthPoint = block.maxHealthPoint;
    }

    private void ClickDestroyBlock(block block, Vector3 htp)
    {
        if (hitBlock == block)
            Debug.Log("same block");
        else
        {
            if (hitBlock == null)
                hitBlock = block;

            hitBlock.healthPoint = hitBlock.maxHealthPoint;
        }

        hitBlock = block;
        hitBlock.healthPoint--;

        if (hitBlock.healthPoint <= 0f)
            Destroy(block);

    }
}
