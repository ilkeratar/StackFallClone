using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;

    public GameObject settingBTN;
    public GameObject allBTN;

    public GameObject soundONBTN;
    public GameObject soundOFFBTN;
    public bool soundOffBo;

    public bool buttonSettingBo;
    public Material playerMat;

    private PlayerController player;
    [SerializeField] private GameObject homeUI;
    [SerializeField] private GameObject gameUI;

    void Start()
    {
        playerMat=FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        player=FindObjectOfType<PlayerController>();
        levelSlider.transform.GetComponent<Image>().color=playerMat.color + Color.gray;

        levelSlider.color=playerMat.color;
        currentLevelImg.color=playerMat.color;
        nextLevelImg.color=playerMat.color;

        soundONBTN.GetComponent<Button>().onClick.AddListener((() => SoundManager.instance.SoundOnOff()));
        soundOFFBTN.GetComponent<Button>().onClick.AddListener((() => SoundManager.instance.SoundOnOff()));   
        
        //PlayerPrefs.DeleteAll();
    }

    
    void Update()
    {

        if(Input.GetMouseButtonDown(0) && !ignoreUI() && player.playerState==PlayerController.PlayerState.PrePare)
        {
            player.playerState=PlayerController.PlayerState.Playing;
            homeUI.SetActive(false);
            gameUI.SetActive(true);
        }

        if(SoundManager.instance.sound)
        {
            soundONBTN.SetActive(true);
            soundOFFBTN.SetActive(false);
        }else{
            soundONBTN.SetActive(false);
            soundOFFBTN.SetActive(true);
        }
    }


    private bool ignoreUI()
    {
        PointerEventData pointerEventData=new PointerEventData(EventSystem.current);
        pointerEventData.position=Input.mousePosition;
        List<RaycastResult> raycastResults=new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData,raycastResults);

        for(int i=0;i<raycastResults.Count;i++){
            if(raycastResults[i].gameObject.GetComponent<IgnoreGameUI>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }
        return raycastResults.Count > 0;
    }
    public void levelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount=fillAmount;
    }

    public void settingShow()
    {
        buttonSettingBo=!buttonSettingBo;
        allBTN.SetActive(buttonSettingBo);
    }
}
