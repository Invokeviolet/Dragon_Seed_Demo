using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayClick : MonoBehaviour
{
    [Header("Vegetable_Prefab")]
    [SerializeField] GameObject potatoPrefab;
    [SerializeField] GameObject ApplePrefab;
    [SerializeField] GameObject CarrotPrefab;
    [SerializeField] GameObject CabbagePrefab;
    [SerializeField] GameObject EggplantPrefab;

    [Header("Building_Prefab")]
    [SerializeField] GameObject housePrefab;
    [SerializeField] GameObject WellPrefab;
    [Header("AlphaPrefab_List")]
    [SerializeField] GameObject[] all_Alpha_Prefab;
    CursorChange myCursor;

    private int mask; //cullingMask plag Save Value
    private int mask1;
    private Vector3 FirstRayPosition; //FirstRay hit point Position

    private void Awake()
    {
        myCursor = FindObjectOfType<CursorChange>();
        mask = Camera.main.cullingMask = (1 << 9);
        mask1= Camera.main.cullingMask = (1 << 7);
        ObjectPoolingManager.inst.inst_AlphaPrefab(all_Alpha_Prefab);
    }

    void Update()
    {
       
        if (DefenseUIManager.INSTANCE.BUILDINGMODE == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform.name == "NoBuildingZone" || hit.transform.tag == "OnGround")
                {
                    myCursor.noBuildingZoneCursor();
                }
                if(hit.transform.name == "UiZone")
                {
                    myCursor.BasicCursor();
                }
                if (hit.transform.tag == "GroundEmpty")
                {
                    myCursor.BasicCursor();
                    if (DefenseUIManager.INSTANCE.onHOUSE == true || DefenseUIManager.INSTANCE.onWELL == true)
                    {
                        myCursor.BuildingCursor();
                    }
                    if (DefenseUIManager.INSTANCE.onAPPLE == true || DefenseUIManager.INSTANCE.onCABBAGE == true ||
                                DefenseUIManager.INSTANCE.onCARROT == true || DefenseUIManager.INSTANCE.onEEGPLANT == true ||
                                DefenseUIManager.INSTANCE.onPOTATO == true)
                    {
                        myCursor.VegetableCursor();
                    }
                }
            }
        }


        if ( DefenseUIManager.INSTANCE.BUILDINGMODE == true     &&
           ( DefenseUIManager.INSTANCE.onHOUSE == true       ||
             DefenseUIManager.INSTANCE.onWELL == true        ||
             DefenseUIManager.INSTANCE.onAPPLE == true       ||
             DefenseUIManager.INSTANCE.onCABBAGE == true     ||
             DefenseUIManager.INSTANCE.onCARROT == true      ||
             DefenseUIManager.INSTANCE.onEEGPLANT == true    ||
             DefenseUIManager.INSTANCE.onPOTATO == true))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                

                if (FirstRayPosition == Vector3.zero)
                {
                    FirstRayPosition = hit.transform.position;
                }
                if (FirstRayPosition != hit.transform.position && hit.transform.gameObject.tag == "GroundEmpty")
                {
                    ObjectPoolingManager.inst.Objectapperear(hit.transform.position);
                    FirstRayPosition = hit.transform.position;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "OnGround") return;

                    if (DefenseUIManager.INSTANCE.onHOUSE == true)
                    {
                        DefenseUIManager.INSTANCE.onHOUSE = false;
                        ObjectPoolingManager.inst.Instantiate(housePrefab, hit.transform.position, Quaternion.identity, ObjectPoolingManager.inst.PoolingZone);
                        ObjectPoolingManager.inst.ObjectDisappear();
                    }
                    else if (DefenseUIManager.INSTANCE.onPOTATO == true)
                    {
                        DefenseUIManager.INSTANCE.onPOTATO = false;
                        ObjectPoolingManager.inst.Instantiate(potatoPrefab, hit.transform.position + new Vector3(0, 0.33f, 0), Quaternion.identity, ObjectPoolingManager.inst.PoolingZone);
                        ObjectPoolingManager.inst.ObjectDisappear();
                    }
                    else if (DefenseUIManager.INSTANCE.onWELL == true )
                    {
                        DefenseUIManager.INSTANCE.onWELL = false;
                        ObjectPoolingManager.inst.Instantiate(WellPrefab, hit.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity, ObjectPoolingManager.inst.PoolingZone);
                        ObjectPoolingManager.inst.ObjectDisappear();
                    }
                    else if (DefenseUIManager.INSTANCE.onAPPLE == true)
                    {
                        DefenseUIManager.INSTANCE.onAPPLE = false;
                        ObjectPoolingManager.inst.Instantiate(ApplePrefab, hit.transform.position+new Vector3(0,0.33f,0), Quaternion.identity, ObjectPoolingManager.inst.PoolingZone);
                        ObjectPoolingManager.inst.ObjectDisappear();
                    }
                    else if (DefenseUIManager.INSTANCE.onCABBAGE == true)
                    {
                        DefenseUIManager.INSTANCE.onCABBAGE = false;
                        ObjectPoolingManager.inst.Instantiate(CabbagePrefab, hit.transform.position + new Vector3(0, 0.33f, 0), Quaternion.identity, ObjectPoolingManager.inst.PoolingZone);
                        ObjectPoolingManager.inst.ObjectDisappear();
                    }
                    else if (DefenseUIManager.INSTANCE.onCARROT == true )
                    {
                        DefenseUIManager.INSTANCE.onCARROT = false;
                        ObjectPoolingManager.inst.Instantiate(CarrotPrefab, hit.transform.position + new Vector3(0, 0.33f, 0), Quaternion.identity, ObjectPoolingManager.inst.PoolingZone);
                        ObjectPoolingManager.inst.ObjectDisappear();
                    }
                    else if (DefenseUIManager.INSTANCE.onEEGPLANT == true)
                    {
                        DefenseUIManager.INSTANCE.onEEGPLANT = false;
                        ObjectPoolingManager.inst.Instantiate(EggplantPrefab, hit.transform.position + new Vector3(0, 0.33f, 0), Quaternion.identity, ObjectPoolingManager.inst.PoolingZone);
                        ObjectPoolingManager.inst.ObjectDisappear();
                    }
                }
            }
        }

        if (Input.GetMouseButton(0) && DefenseUIManager.INSTANCE.BUILDINGMODE == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.tag=="well")
                {
                    DefenseUIManager.INSTANCE.WATERRAY = true;
                    myCursor.WarterCursor();
                }
                if (FirstRayPosition == Vector3.zero)
                {
                    FirstRayPosition = hit.transform.position;
                }
                if (FirstRayPosition != hit.transform.position &&
                hit.transform.gameObject.tag == "GroundEmpty" || hit.transform.gameObject.tag == "OnGround")
                {
                    ObjectPoolingManager.inst.Objectapperear(hit.transform.position);
                    FirstRayPosition = hit.transform.position;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && DefenseUIManager.INSTANCE.BUILDINGMODE == false && DefenseUIManager.INSTANCE.WATERRAY == true)
        {
            myCursor.BasicCursor();
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000,mask1))
            {
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag=="plant")
                {
                    if(hit.transform.gameObject.GetComponent<Vegetable>().onWater == false)
                    {
                        hit.transform.gameObject.GetComponent<Vegetable>().startGrowth();
                    }
                    DefenseUIManager.INSTANCE.WATERRAY = false;
                    ObjectPoolingManager.inst.ObjectDisappear();
                }
            }
            else
            {
                DefenseUIManager.INSTANCE.WATERRAY = false;
                ObjectPoolingManager.inst.ObjectDisappear();
            }
        }
    }
   

}