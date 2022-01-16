using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    bool carpma;
    float currentTime;
    bool invincible;
    public GameObject fireShield;
    [SerializeField]
    AudioClip win,death,idestroy,destroy,bounce;
    public int currentObstacleNumber;
    public int totalObstacleNumber;
    public Image InvincibleSlider;
    public GameObject InvincibleOBJ;
    public GameObject gameOverUI;
    public GameObject finishUI;
    public enum PlayerState{
        PrePare,
        Playing,
        Died,
        Finish
    }
    [HideInInspector]
    public PlayerState playerState=PlayerState.PrePare;

    private void Awake() 
    {
        rb=GetComponent<Rigidbody>();
        currentObstacleNumber=0;
    }
    void Start()
    {
        totalObstacleNumber=FindObjectsOfType<ObstacleController>().Length;
    }

    void Update()
    {
        if(playerState==PlayerState.Playing)
        {
            if(Input.GetMouseButtonDown(0)){
            carpma=true;
        }
        if(Input.GetMouseButtonUp(0)){
            carpma=false;
        }

        if(invincible){
            currentTime -=Time.deltaTime* .35f;
            if(!fireShield.activeInHierarchy){
                fireShield.SetActive(true);
            }
        }else{
             if(fireShield.activeInHierarchy){
                fireShield.SetActive(false);
            }
            if(carpma){
            currentTime += Time.deltaTime* 0.8f;
            }
            else{
            currentTime -= Time.deltaTime* 0.5f;
             }
        }

        if(currentTime>=0.15f || InvincibleSlider.color==Color.red)
        {
            InvincibleOBJ.SetActive(true);
        }else{
            InvincibleOBJ.SetActive(false);
        }

        if(currentTime >=1){
            currentTime= 1;
            invincible=true;
            Debug.Log("invincible");
            InvincibleSlider.color=Color.red;
        }else if (currentTime<=0)
            {
            currentTime=0;
            invincible=false;
            Debug.Log("normal");
            InvincibleSlider.color=Color.white;
            }

            if(InvincibleOBJ.activeInHierarchy)
            {
                InvincibleSlider.fillAmount=currentTime/1;
            }
            
        }

        

        if(playerState==PlayerState.Finish)
        {
            if(Input.GetMouseButtonDown(0)){
                FindObjectOfType<LevelSpawner>().NextLevel();
            }
        }
        if(playerState==PlayerState.Died)
        {
            if(Input.GetMouseButtonDown(0)){
                SceneManager.LoadScene(0);
            }
        }
        
    }
    public void shatterObstacles(){
        if(invincible){
            ScoreManager.instance.addScore(2);
        }else{
            ScoreManager.instance.addScore(1);
        }
        
    }
    private void FixedUpdate() {
        if(playerState==PlayerState.Playing){
            if(carpma){
            rb.velocity=new Vector3(0,-100 * Time.fixedDeltaTime*7,0);
        }
        }
        
    }
    private void OnCollisionEnter(Collision collision) {
        if(!carpma){
            rb.velocity=new Vector3(0,50*Time.deltaTime*5,0);
        }else
        {
            if(invincible){
                if(collision.gameObject.tag=="enemy" || collision.gameObject.tag=="plane"){
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    shatterObstacles();
                    SoundManager.instance.playSoundFX(idestroy,0.5f);
                    currentObstacleNumber++;
                    //Destroy(collision.transform.parent.gameObject);
                }
            }else{
                if(collision.gameObject.tag=="enemy"){
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    shatterObstacles();
                    SoundManager.instance.playSoundFX(destroy,0.5f);
                    currentObstacleNumber++;
                    //Destroy(collision.transform.parent.gameObject);
                }
                else if(collision.gameObject.tag=="plane"){
                Debug.Log("Game Over");
                gameOverUI.SetActive(true);
                gameOverUI.transform.GetChild(2).GetComponent<Text>().text="Score "+ScoreManager.instance.score;
                gameOverUI.transform.GetChild(3).GetComponent<Text>().text="Best "+PlayerPrefs.GetInt("HighScore");
                playerState=PlayerState.Died;
                gameObject.GetComponent<Rigidbody>().isKinematic=true;
                ScoreManager.instance.ResetScore();
                SoundManager.instance.playSoundFX(death,0.5f);
                }
            }
        }


        FindObjectOfType<GameUI>().levelSliderFill((float) currentObstacleNumber / (float) totalObstacleNumber);

        if(collision.gameObject.tag=="Finish" && playerState==PlayerState.Playing)
        {
            playerState=PlayerState.Finish;
            SoundManager.instance.playSoundFX(win,0.5f);
            finishUI.SetActive(true);
            finishUI.transform.GetChild(0).GetComponent<Text>().text="Level "+PlayerPrefs.GetInt("Level",1);
        }
    }
    private void OnCollisionStay(Collision collision) {
        if(!carpma || collision.gameObject.tag=="Finish"){
            rb.velocity=new Vector3(0,50*Time.deltaTime*5,0);
            SoundManager.instance.playSoundFX(bounce,0.5f);
        }
    }

}
