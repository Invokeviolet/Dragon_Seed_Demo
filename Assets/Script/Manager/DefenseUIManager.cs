using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DefenseUIManager : MonoSingleTon<DefenseUIManager>
{
    #region Click Object Value
    [SerializeField] public bool BUILDINGMODE { get; set; } 
    [SerializeField] public bool onPOTATO     { get; set; }
    [SerializeField] public bool onAPPLE      { get; set; }
    [SerializeField] public bool onCABBAGE    { get; set; }
    [SerializeField] public bool onCARROT     { get; set; }
    [SerializeField] public bool onEEGPLANT   { get; set; }
    [SerializeField] public bool onWATER      { get; set; }
    [SerializeField] public bool onHOUSE      { get; set; }
    [SerializeField] public bool onWELL       { get; set; }
    [SerializeField] public bool WATERRAY     { get; set; }
    #endregion

    #region Ui
    [Header("[ UI ]")]
    [SerializeField] GameObject GardeningMenu;
    [SerializeField] GameObject StorePage;
    [SerializeField] GameObject BulidingModeMenu;

    [Header("[Vegetable Menu]")]
    [SerializeField] GameObject VegetableScroll;
    [SerializeField] GameObject VegetableScrollOpenButton;
    [SerializeField] GameObject VegetableScrollCloseButton;

    [Header("[Building Menu]")]
    [SerializeField] GameObject BuildingScroll;
    [SerializeField] GameObject BuildingScrollOpenButton;
    [SerializeField] GameObject BuildingScrollCloseButton;

    #endregion

    CursorChange myCursor;

    Vector3 VegetableMenuOriginPos;
    Vector3 BuildingMenuOriginPos;
    Vector3 originMenuPos;

    bool onMenu;

    private void Awake()
    {
        //button is clicked Permit
        onMenu = false;
        WATERRAY = false;

        //Cursor Change Image 
        myCursor = FindObjectOfType<CursorChange>();

        //Reset Value
        Initializing();

        //Back to the Defense Scene transform initializing Scroll
        VegetableMenuOriginPos = VegetableScroll.transform.position;
        BuildingMenuOriginPos = BuildingScroll.transform.position;

    }

    //Value Reset 
    private void Initializing()
    {
        DefenseUIManager.INSTANCE.onWELL = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onHOUSE = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onAPPLE = false;
        DefenseUIManager.INSTANCE.onCABBAGE = false;
        DefenseUIManager.INSTANCE.onCARROT = false;
        DefenseUIManager.INSTANCE.onEEGPLANT = false;
    }
    public void Go_buildingMode()
    {
        if (onMenu) return;
        onMenu = true;

        BUILDINGMODE = true;
        GardeningMenu.SetActive(false);
        BulidingModeMenu.SetActive(true);
    }

    #region BuildingMode Func

    #region BuildingMode Scroll Func

    //Vegetable Scroll Open&Close
    /////////////////////////////////////////////////////////////////
    public void OpenScrollVegetable()
    {
        BuildingScrollOpenButton.GetComponent<Button>().interactable = false;

        StartCoroutine(OpenScroll(VegetableScroll));

        SwitchBackButton(VegetableScrollOpenButton);
    }
    public void CloseScrollVegetable()
    {
        BuildingScrollOpenButton.GetComponent<Button>().interactable = true;
        StartCoroutine(CloseScroll(VegetableScroll));
        SwitchOpenButton(VegetableScrollCloseButton);
    }
    /////////////////////////////////////////////////////////////////

    //Building Scroll Open&Close
    /////////////////////////////////////////////////////////////////
    public void OpenScrollBuilding()
    {
        VegetableScrollOpenButton.GetComponent<Button>().interactable = false;

        StartCoroutine(OpenScroll(BuildingScroll));

        SwitchBackButton(BuildingScrollOpenButton);
    }
    public void CloseScrollBuilding()
    {
        VegetableScrollOpenButton.GetComponent<Button>().interactable = true;
        StartCoroutine(CloseScroll(BuildingScroll));
        SwitchOpenButton(BuildingScrollCloseButton);
    }
    /////////////////////////////////////////////////////////////////

    //Scroll Button Chane (Switch Go & Back)
    /////////////////////////////////////////////////////////////////
    void SwitchBackButton(GameObject button)
    {
        if(button == VegetableScrollOpenButton)
        {
            VegetableScrollOpenButton.SetActive(false);
            VegetableScrollCloseButton.SetActive(true);
        }
        else if (button == BuildingScrollOpenButton)
        {
            BuildingScrollOpenButton.SetActive(false);
            BuildingScrollCloseButton.SetActive(true);
        }
    }
    void SwitchOpenButton(GameObject button)
    {
        if (button == VegetableScrollCloseButton)
        {
            VegetableScrollOpenButton.SetActive(true);
            VegetableScrollCloseButton.SetActive(false);
        }
        else if (button == BuildingScrollCloseButton)
        {
            BuildingScrollOpenButton.SetActive(true);
            BuildingScrollCloseButton.SetActive(false);
        }
    }
    /////////////////////////////////////////////////////////////////

    #endregion

    #region BuildingMenu Open&Close Corutine

    IEnumerator OpenScroll(GameObject scroll)
    {
        originMenuPos = scroll.transform.position;
        while(true)
        {
            scroll.transform.position = 
                Vector3.Lerp(scroll.transform.position, scroll.transform.position + new Vector3(100f, 0, 0), Time.deltaTime * 15f);
            yield return null;

            if(scroll.transform.position.x>=450f)
            {
                yield break;
            }
        }
        
    }
    IEnumerator CloseScroll(GameObject scroll)
    {
        while (true)
        {
            scroll.transform.position = 
                Vector3.Lerp(scroll.transform.position, scroll.transform.position + new Vector3(-100f, 0, 0), Time.deltaTime * 25f);
            yield return null;

            if(scroll.transform.position.x<=-350f)
            {
                scroll.transform.position = originMenuPos;
                yield break;
            }
        }
        
    }

    #endregion

    //Back to the Defense Scene
    /////////////////////////////////////////////////////
    public void Back_buildingMode()
    {
        if (!onMenu) return;
        if (onMenu)
            onMenu = false;

        VegetableScroll.transform.position = VegetableMenuOriginPos;
        BuildingScroll.transform.position = BuildingMenuOriginPos;
        SwitchOpenButton(VegetableScrollCloseButton);
        SwitchOpenButton(BuildingScrollCloseButton);

        BuildingScrollOpenButton.GetComponent<Button>().interactable = true;
        VegetableScrollOpenButton.GetComponent<Button>().interactable = true;

        BUILDINGMODE = false;
        GardeningMenu.SetActive(true);
        BulidingModeMenu.SetActive(false);
        Initializing();
        ObjectPoolingManager.inst.ObjectDisappear();
        myCursor.BasicCursor();
    }
    /////////////////////////////////////////////////////

    #endregion

    #region UI Transform Up&Down Corutine
    IEnumerator Uptrans(GameObject page)
    {
        while (true)
        {
            page.transform.position =
                Vector3.Lerp(page.transform.position, page.transform.position + new Vector3(0, 100f, 0), Time.deltaTime * 20f);
            yield return null;

            if (page.transform.position.y >= 540f)
            {
                yield break;
            }
        }

    }
    IEnumerator Downtrans(GameObject page)
    {
        while (true)
        {
            page.transform.position =
                Vector3.Lerp(page.transform.position, page.transform.position + new Vector3(0, -100f, 0), Time.deltaTime * 30f);
            yield return null;

            if (page.transform.position.y <= -540f)
            {
                yield break;
            }
        }

    }
    #endregion

    #region StorePage Func
    public void UpStorePage()
    {
        if (onMenu) return;
        onMenu = true;

        StartCoroutine(Uptrans(StorePage));
    }
    public void DownStorePage()
    {
        if (!onMenu) return;
        if(onMenu)
        onMenu = false;
        StartCoroutine(Downtrans(StorePage));
    }
    #endregion

    #region Select Building&Vegetable Button
    public void SelectWell()
    {
        CloseScrollBuilding();
        //myCursor.BuildingCursor();
        DefenseUIManager.INSTANCE.onWELL = true;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onHOUSE = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onAPPLE = false;
        DefenseUIManager.INSTANCE.onCABBAGE = false;
        DefenseUIManager.INSTANCE.onCARROT = false;
        DefenseUIManager.INSTANCE.onEEGPLANT = false;
        ObjectPoolingManager.inst.ObjectDisappear();
    }
    public void SelectHouse()
    {
        CloseScrollBuilding();
        //myCursor.BuildingCursor();
        DefenseUIManager.INSTANCE.onHOUSE = true;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onWELL = false;
        DefenseUIManager.INSTANCE.onAPPLE = false;
        DefenseUIManager.INSTANCE.onCABBAGE = false;
        DefenseUIManager.INSTANCE.onCARROT = false;
        DefenseUIManager.INSTANCE.onEEGPLANT = false;
        ObjectPoolingManager.inst.ObjectDisappear();
    }
    public void SelectPotato()
    {
        CloseScrollVegetable();
        //myCursor.VegetableCursor();
        DefenseUIManager.INSTANCE.onPOTATO = true;
        DefenseUIManager.INSTANCE.onWELL = false;
        DefenseUIManager.INSTANCE.onHOUSE = false;
        DefenseUIManager.INSTANCE.onAPPLE = false;
        DefenseUIManager.INSTANCE.onCABBAGE = false;
        DefenseUIManager.INSTANCE.onCARROT = false;
        DefenseUIManager.INSTANCE.onEEGPLANT = false;
        ObjectPoolingManager.inst.ObjectDisappear();
    }
    public void SelectApple()
    {
        CloseScrollVegetable();
        //myCursor.VegetableCursor();
        DefenseUIManager.INSTANCE.onAPPLE = true;
        DefenseUIManager.INSTANCE.onWELL = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onHOUSE = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onCABBAGE = false;
        DefenseUIManager.INSTANCE.onCARROT = false;
        DefenseUIManager.INSTANCE.onEEGPLANT = false;
        ObjectPoolingManager.inst.ObjectDisappear();
    }
    public void SelectCabbage()
    {
        CloseScrollVegetable();
        //myCursor.VegetableCursor();
        DefenseUIManager.INSTANCE.onCABBAGE = true;
        DefenseUIManager.INSTANCE.onWELL = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onHOUSE = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onAPPLE = false;
        DefenseUIManager.INSTANCE.onCARROT = false;
        DefenseUIManager.INSTANCE.onEEGPLANT = false;
        ObjectPoolingManager.inst.ObjectDisappear();
    }
    public void SelectCarrot()
    {
        CloseScrollVegetable();
        //myCursor.VegetableCursor();
        DefenseUIManager.INSTANCE.onCARROT = true;
        DefenseUIManager.INSTANCE.onWELL = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onHOUSE = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onAPPLE = false;
        DefenseUIManager.INSTANCE.onCABBAGE = false;
        DefenseUIManager.INSTANCE.onEEGPLANT = false;
        ObjectPoolingManager.inst.ObjectDisappear();
    }
    public void SelectEggplant()
    {
        CloseScrollVegetable();
        //myCursor.VegetableCursor();
        DefenseUIManager.INSTANCE.onEEGPLANT = true;
        DefenseUIManager.INSTANCE.onWELL = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onHOUSE = false;
        DefenseUIManager.INSTANCE.onPOTATO = false;
        DefenseUIManager.INSTANCE.onAPPLE = false;
        DefenseUIManager.INSTANCE.onCABBAGE = false;
        DefenseUIManager.INSTANCE.onCARROT = false;
        ObjectPoolingManager.inst.ObjectDisappear();
    }

    #endregion

   


}
