using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라가 벽을 뚫음

public class BlockManager : MonoBehaviour
{
    public ChangeImage changeImage;
    public int startingPoint;

    private Shader glassShader;
    private block hitBlock;
    private Transform map;
    private Animator animator;

    private void Awake()
    {
        glassShader = Shader.Find("Unlit/Transparent");
        map = GameObject.FindGameObjectWithTag("Map").transform;
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;//마우스 커서 고정
        Cursor.visible = false;

        //start1, coroutine1, start2, update1, update1, coroutine2
    }

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/6"));
                temp.transform.position = new Vector3(startingPoint + i, 20, startingPoint + j);
                temp.transform.parent = map;
            }
        }

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/2"));
                temp.transform.position = new Vector3(startingPoint + i, 10, startingPoint + j);
                temp.transform.parent = map;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            MakeBlock();

        if (Input.GetMouseButton(0))
            DestroyBlock();

        if (Input.GetMouseButtonUp(0))
            StopDestroyBlock();

    }

    private bool IsRayHitBlock(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 5.5f))
            if (hit.transform.gameObject.CompareTag("Player"))
                return true;
        
        return false;
    }

    private void MakeBlock()
    {
        if (IsRayHitBlock(out RaycastHit hit))
        {
            Vector3 installPos = hit.transform.position;
            installPos += hit.normal;

            if (Vector3.Distance(transform.position, installPos) <= 1.05f)
                return;

            foreach (Transform child in map)
                if (child.position.Equals(installPos))
                    return;

            GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/" + changeImage.imageNum.ToString()));
            temp.transform.position = installPos;
            temp.transform.parent = map;
            temp.GetComponent<MeshRenderer>().material.color = changeImage.color;
            
            animator.SetBool("isHit", true);
        }

    }

    private void DestroyBlock()
    {
        if (IsRayHitBlock(out RaycastHit hit))
        {
            block block = hit.transform.GetComponent<block>();
            if (hitBlock == null)
                hitBlock = block;
            if (hitBlock != block)
            {
                StopDestroyBlock();
                hitBlock = block;
            }
            Debug.Log("update");
            if (!animator.GetBool("isHit"))
            {
                Debug.Log("true");
                animator.SetBool("isHit", true);
            }

            hitBlock.healthPoint--;
            if (hitBlock.healthPoint <= 0f)
                Destroy(hitBlock.gameObject);

        }
    }

    private void StopDestroyBlock()
    {
        if (hitBlock == null)
            return;

        hitBlock.healthPoint = hitBlock.maxHealthPoint;
    }

    public void HitAnimationDone() { StartCoroutine("Late2"); }
    private IEnumerator Late2()
    {
        Debug.Log("call");
        yield return null;//다음 프레임 업데이트가 끝날때 까지 대기
        Debug.Log("stop");
        animator.SetBool("isHit", false);
    }
    //private void ClickDestroyBlock(block block, Vector3 htp)
    //{
    //    if (hitBlock == block)
    //        Debug.Log("same block");
    //    else
    //    {
    //        if (hitBlock == null)
    //            hitBlock = block;

    //        hitBlock.healthPoint = hitBlock.maxHealthPoint;
    //    }

    //    hitBlock = block;
    //    hitBlock.healthPoint--;

    //    if (hitBlock.healthPoint <= 0f)
    //        Destroy(block);

    //}
}

//int layerMask = 1 << LayerMask.NameToLayer("TranslucentBlock");// TranslucentBlock레이어만 무시하고 충돌
//layerMask = ~layerMask;

//Ray ray = new Ray(transform.position, transform.forward);
//Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));//Ray를 쏠 지점을 화면의 정가운데로 한다
//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//ray.direction += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)); //ray.direction의 값은 normalized 되어 있기 때문에 쏘는 방향을 다르게 해주려면 저렇게 해줘야됨

//if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 7f, layerMask))
//{
//    if (hit.transform.gameObject.CompareTag("Player"))
//    {
//        Vector3 installPos = hit.transform.position;
//        installPos += hit.normal;

//        if (Input.GetMouseButtonDown(1))
//            ClickMakeBlock(installPos);

//        if (Input.GetMouseButton(0))
//            DestroyBlock(hit.transform.GetComponent<block>());
//        if (Input.GetMouseButtonUp(0))
//            StopDestroyBlock(hit.transform.GetComponent<block>());
//    }
//}