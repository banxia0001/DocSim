using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Sprite[] head;
    public SpriteRenderer head_;
    public Animator animC;
    public AudioSource[] sound;

    public float diff;

    public Animator[] barAnims;
    public Animator susAnim;
    public BarController susBar;
    public float susNow;

    public BarController[] BC;
    public Avatar_Controller[] player;
    
    public List<Tool> toolsAvaliable = new List<Tool>();
    public Tool bloodSpecial;
    public Tool boneSpecial;

    public Patient patient;
    public Organ[] organs;

    public GameObject[] circle;
    [Space]
    public bool isOnHeart;
    public bool isOnBrain;
    public bool isOnLung;


    public Organ[] organ_InMission;
    public Tool[] toolsInUse;

    public int  sawN_;



    public float[] barNow;
    public float[] barMax;


    [Space]
    public Bar_PointController[] BP;
    public GameObject[] Bar;
    public GameObject[] hitBox;
    public Image[] toolImage;
    public TMP_Text[] organText;


    public void Start()
    {
        TurnOffBools();
        susBar.SetValue(0, 100);

        NewPatient_Coming();
        diff = 0;
    }

    private void TurnOffBools()
    {
        //isOnHeart = false;
        //isOnBrain = false;
        //isOnLung = false;
        //isOnBlood = false;
    }


    private void NewPatient_Coming()
    {
        choose_Organ_Setup();
        ShuffleOrganList();
        head_.sprite = head[Random.Range(0, head.Length)];

        //Ëæ»ú²¡ÈË±¸×¢£¬ÏÔÊ¾ÔÚÆÁÄ»×ó²à
        //ÏÔÊ¾Í¼Æ¬
        giveOrderToDoctor(0, patient.organList[Random.Range(0, patient.organList.Count)]);
        giveToolToDoctor(0);
        giveOrderToDoctor(1, patient.organList[Random.Range(0, patient.organList.Count)]);
        giveToolToDoctor(1);

    }

    private void choose_Organ_Setup()
    {
        foreach (Organ organ in organs)
        {
            organ.loop = organ.loopMax;
        }
    }


    private void ShuffleOrganList()
    {
        patient.organList.Clear();
        foreach (Organ organ in organs)
        {
            patient.organList.Add(organ);
        }

        for (int i = 0; i < patient.organList.Count - 1; i++)
        {
            Organ oldO = patient.organList[i];
            int rand = Random.Range(i, patient.organList.Count);
            patient.organList[i] = patient.organList[rand];
            patient.organList[rand] = oldO;
        }

    }

    private void giveToolToDoctor(int playerNum)
    {
        if (organ_InMission[playerNum] == null) return;
        
        Bar[playerNum].SetActive(true);
        circle[playerNum].SetActive(true);

        Tool tool_Use = null;
        if (organ_InMission[playerNum].Name == "Blood")
        {
            tool_Use = bloodSpecial;
        }

        else if (organ_InMission[playerNum].Name == "Brain")
        {
            int i2 = Random.Range(0, 2);
            if (i2 == 0) 
            {
                tool_Use = boneSpecial;
                circle[playerNum].SetActive(false);
                Bar[playerNum].SetActive(false);
            }

            else
            {
                hitBox_Randoness(playerNum);
                int i = Random.Range(0, toolsAvaliable.Count);
                tool_Use = toolsAvaliable[i];
                toolsAvaliable.Remove(toolsAvaliable[i]);
            }

        }

        else
        {
            hitBox_Randoness(playerNum);
            int i = Random.Range(0, toolsAvaliable.Count);
            tool_Use = toolsAvaliable[i];
            toolsAvaliable.Remove(toolsAvaliable[i]); 
        }


        toolsInUse[playerNum] = tool_Use;
        toolImage[playerNum].sprite = tool_Use.image;
        toolImage[playerNum].gameObject.GetComponent<Animator>().SetTrigger("bigger");
    }


    private void giveOrderToDoctor(int playerNum, Organ organ)
    {
        Bar[playerNum].SetActive(true);
        circle[playerNum].SetActive(false);
        organ_InMission[playerNum] = organ;
        patient.organList.Remove(organ);
        barNow[playerNum] = 0;
        barMax[playerNum] = Random.Range(organ.workOnTimeMin, organ.workOnTimeMax);

        organText[playerNum].text = organ_InMission[playerNum].Name;
    }

    public void FixedUpdate()
    {
        if (organ_InMission[0] == null && organ_InMission[1] == null)
        {
            NewPatient_Coming();
        }


        if (organ_InMission[0] == null)
        {
            organText[0].text = "null";
            circle[0].SetActive(false);
            Bar[0].SetActive(false);
          
        }

        if (organ_InMission[1] == null)
        {
            organText[1].text = "null";
            circle[1].SetActive(false);
            Bar[1].SetActive(false);
        }


        TurnOffBools();
     
        for (int i = 0; i < 2; i++)
        {
            if(organ_InMission[i] == null) BC[i].SetValue(0, 1);
            else BC[i].SetValue(barNow[i], barMax[i]);
        }
    }

    public bool isOnLancet02;
    public bool isOnScissor;
    public bool isOnForceps;

    public void Update()
    {


        if (Input.GetKeyDown(KeyCode.F)) isOnScissor = true;
        if (Input.GetKeyUp(KeyCode.F)) isOnScissor = false;

        if (Input.GetKeyDown(KeyCode.D)) isOnForceps = true;
        if (Input.GetKeyUp(KeyCode.D)) isOnForceps = false;

        if (Input.GetKeyDown(KeyCode.RightArrow)) isOnLancet02 = true;
        if (Input.GetKeyUp(KeyCode.RightArrow)) isOnLancet02 = false;



        if (Input.GetKeyDown(KeyCode.UpArrow)) isOnHeart = true;
        if (Input.GetKeyUp(KeyCode.UpArrow)) isOnHeart = false;

        if (Input.GetKeyDown(KeyCode.DownArrow)) isOnLung = true;
        if (Input.GetKeyUp(KeyCode.DownArrow)) isOnLung = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) isOnBrain = true;
        if (Input.GetKeyUp(KeyCode.LeftArrow)) isOnBrain = false;



      

        if (Input.GetKeyDown(KeyCode.S)) working_on_Blood();


        ////¾â×Ó¾âÄÔ´ü µ¥¶ÀµÄÓÎÏ·
        if (Input.GetKeyDown(KeyCode.W)) working_on_Brain(0);
        if (Input.GetKeyDown(KeyCode.A)) working_on_Brain(1);

        if (Input.GetKeyDown(KeyCode.UpArrow)) workin_On_Organ_01("Heart");
        if (Input.GetKeyDown(KeyCode.DownArrow)) workin_On_Organ_01("Lung");
        if (Input.GetKeyDown(KeyCode.LeftArrow)) workin_On_Organ_01("Brain");

        //if (Input.GetKeyDown(KeyCode.RightArrow)) workin_On_Organ_01("Lancet02");
        //if (Input.GetKeyDown(KeyCode.F)) workin_On_Organ_01("Scissor");
        //if (Input.GetKeyDown(KeyCode.D)) workin_On_Organ_01("Forceps");
    }


    private void workin_On_Organ_01(string organName)
    {
        if (organ_InMission[0] != null)
            working_on_Organ_02(toolsInUse[0].toolName, 0, organName);

        if (organ_InMission[1] != null)
            working_on_Organ_02(toolsInUse[1].toolName, 1, organName);
    }


    private void working_on_Brain(int sawN)
    {
        if (organ_InMission[0] != null && toolsInUse[0] != null)
            if (organ_InMission[0].Name == "Brain" && toolsInUse[0].toolName == "Saw")
            working_on_Brain_02(0, sawN);
        if (organ_InMission[1] != null && toolsInUse[1] != null)
            if (organ_InMission[1].Name == "Brain" && toolsInUse[1].toolName == "Saw")
            working_on_Brain_02(1, sawN);
    }

    private void working_on_Blood()
    {
        if (organ_InMission[0] != null)
            if (organ_InMission[0].Name == "Blood")
                working_on_Organ_03(0);

        if (organ_InMission[1] != null)
            if (organ_InMission[1].Name == "Blood")
            working_on_Organ_03(1);
    }

    private void working_on_Brain_02(int playerNum, int sawN)
    {
        if (organ_InMission[playerNum] == null) return;
        if (isOnBrain == false) return;
        toolImage[playerNum].gameObject.GetComponent<Animator>().SetTrigger("bigger");


        barNow[playerNum]++;
        for (int i = 0; i < 4; i++)
        {
            GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
            blood.transform.parent = player[playerNum].bloodPoint.transform;
            blood.transform.localPosition = new Vector3(0, 0, 0);
            blood.transform.localRotation = Quaternion.Euler(0, 0, 0);

        }

        checkOrganGet(playerNum);


        //if (sawN == sawN_)
        //{
        //    barNow[playerNum]++;

        //    if (sawN_ == 0) sawN_ = 1;
        //    else sawN_ = 0;

        //    player[playerNum].anim.SetTrigger("saw");

        //    GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
        //    blood.transform.parent = player[playerNum].bloodPoint.transform;
        //    blood.transform.localPosition = new Vector3(0, 0, 0);
        //    blood.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //    checkOrganGet(playerNum);
        //}

        //else 
        //{

        //    player[playerNum].anim.SetTrigger("saw_bad");

        //    for (int i = 0; i < 4; i++)
        //    {
        //        GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
        //        blood.transform.parent = player[playerNum].bloodPoint.transform;
        //        blood.transform.localPosition = new Vector3(0, 0, 0);
        //        blood.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //    }

        //   // inceaseSUS();
        //}
    }


    private void inceaseSUS()
    {
        animC.SetTrigger("a");
        susNow++;
        susBar.SetValue(susNow, 20);
        susAnim.SetTrigger("haha");
     
        if (susNow == 20)
        {
            StartCoroutine(GameOver());
  
        }


    }

    public Animator gameOverAnim;
    public TMP_Text scoreText;
    public int score;

    IEnumerator GameOver()
    {
        gameOverAnim.SetTrigger("1");
        yield return new WaitForSeconds(0.5f);

        scoreText.text = "You are under arrest. Be a good person ina your next life.\n" + "Your Score:" + score;
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(0);
    }

    private void working_on_Organ_02(string toolName, int playerNum, string organName)
    {
        if(toolName == "Lancet02" && isOnLancet02 == false) return;
        if(toolName == "Scissor" && isOnScissor == false) return;
        if(toolName == "Forceps" && isOnForceps == false) return;

        if (toolName == "Saw") return;
        //if(organName == "Blood" && isOnBlood == false) return;

        if (organ_InMission[playerNum].Name == organName) working_on_Organ_03(playerNum);
    }



    private void hitBox_Randoness(int playerNum)
    {
        //Debug.Log("playerNum");
        hitBox[playerNum].transform.localPosition = new Vector3(Random.Range(-.3f,.3f), 0, 0);
        hitBox[playerNum].transform.localScale = new Vector3(Random.Range(0.3f, 0.45f), 0.1f, 1);
        BP[playerNum].speed = Random.Range(1f + diff, 1.3f + diff);
    }


    public GameObject blood_;
    private void working_on_Organ_03(int playerNum)
    {
        if (organ_InMission[playerNum] == null) return;
        toolImage[playerNum].gameObject.GetComponent<Animator>().SetTrigger("bigger");
        barAnims[playerNum].SetTrigger("a");
        bool CHECK_HIT = BP[playerNum].checkPointHit();

        GameObject blood_1 = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
        blood_1.transform.parent = player[playerNum].bloodPoint.transform;
        blood_1.transform.localPosition = new Vector3(0, 0, 0);
        blood_1.transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (CHECK_HIT == false)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
                blood.transform.parent = player[playerNum].bloodPoint.transform;
                blood.transform.localPosition = new Vector3(0, 0, 0);
                blood.transform.localRotation = Quaternion.Euler(0, 0, 0);

            }

            // Íæ¼Òì­Ñª£¬Ê§°Ü£¬¼Ó¿ÉÒÉ¶È
            inceaseSUS();
            return;
        }

        if (toolsInUse[playerNum].toolName == "Needle")
        {
            player[playerNum].anim.SetTrigger("blood");
        }
        else player[playerNum].anim.SetTrigger("a");


        barNow[playerNum]++;

        checkOrganGet(playerNum);
    }


    private void checkOrganGet(int playerNum)
    {
        if (organ_InMission[playerNum] == null) return;
        if (barNow[playerNum] >= barMax[playerNum])
        {
            organ_InMission[playerNum].loop--;
            sound[Random.Range(0, sound.Length)].Play();

            diff += 0.045f;


            if (toolsInUse[playerNum].toolName == "Needle" || toolsInUse[playerNum].toolName == "Saw")
            {
                toolsInUse[playerNum] = null;
            }

            else toolsAvaliable.Add(toolsInUse[playerNum]);

            giveToolToDoctor(playerNum);

            if (organ_InMission[playerNum].loop <= 0)
            {
                // StartCoroutine(checkOrganGet_1(playerNum, organ_InMission[playerNum].sprite));
                srn[playerNum].gameObject.SetActive(true);
                srn[playerNum].sprite = organ_InMission[playerNum].sprite;
                player[playerNum].anim.SetTrigger("getStuff");
          
                organ_InMission[playerNum] = null;

                for (int i = 0; i < 4; i++)
                {
                    GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
                    blood.transform.parent = player[playerNum].bloodPoint.transform;
                    blood.transform.localPosition = new Vector3(0, 0, 0);
                    blood.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }


                if (patient.organList != null)
                    if (patient.organList.Count > 0)
                    {
                        giveOrderToDoctor(playerNum, patient.organList[Random.Range(0, patient.organList.Count)]);
                        giveToolToDoctor(playerNum);
                    }
                        

                score += Random.Range(1, 3);

            }

            else
            {
                barNow[playerNum] = 0;
                barMax[playerNum] = Random.Range(organ_InMission[playerNum].workOnTimeMin, organ_InMission[playerNum].workOnTimeMax);
            }
        }


    }

    public SpriteRenderer[] srn;

    //IEnumerator checkOrganGet_1(int playerNum, Sprite organSprite)
    //{
    //    organ_InMission[playerNum] = null;
    //    player[playerNum].anim.SetTrigger("getStuff");
    //    yield return new WaitForSeconds(.05f);
    //    srn[playerNum].sprite = organSprite;

    //    for (int i = 0; i < 4; i++)
    //    {
    //        GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
    //        blood.transform.parent = player[playerNum].bloodPoint.transform;
    //        blood.transform.localPosition = new Vector3(0, 0, 0);
    //        blood.transform.localRotation = Quaternion.Euler(0, 0, 0);
    //    }

    //    yield return new WaitForSeconds(.05f);
    //    if (patient.organList != null)
    //        if (patient.organList.Count > 0)
    //            giveOrderToDoctor(playerNum, patient.organList[Random.Range(0, patient.organList.Count)]);
    //}

}
