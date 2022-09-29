using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
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
   

    [Space]
    public bool isOnHeart;
    public bool isOnBrain;
    //public bool isOnBlood;


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

    }

    private void TurnOffBools()
    {
        isOnHeart = false;
        isOnBrain = false;
        //isOnBlood = false;
    }


    private void NewPatient_Coming()
    {
        choose_Organ_Setup();
        ShuffleOrganList();

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


        TurnOffBools();
     
        for (int i = 0; i < 2; i++)
        {
            if(organ_InMission[i] == null) BC[i].SetValue(0, 1);
            else BC[i].SetValue(barNow[i], barMax[i]);
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) isOnHeart = true;
        //if (Input.GetKey(KeyCode.DownArrow)) isOnBlood = true;
        if (Input.GetKey(KeyCode.LeftArrow)) isOnBrain = true;

        //if (Input.GetKeyDown(KeyCode.A)) workin_On_Organ_01("Lancet02");

        if (Input.GetKeyDown(KeyCode.S)) working_on_Blood();


        //¾â×Ó¾âÄÔ´ü µ¥¶ÀµÄÓÎÏ·
        if (Input.GetKeyDown(KeyCode.W)) working_on_Brain(0);
        if (Input.GetKeyDown(KeyCode.A)) working_on_Brain(1);


        if (Input.GetKeyDown(KeyCode.RightArrow)) workin_On_Organ_01("Lancet02");
        if (Input.GetKeyDown(KeyCode.DownArrow)) workin_On_Organ_01("Scissor");
        if (Input.GetKeyDown(KeyCode.D)) workin_On_Organ_01("Forceps");
    }


    private void workin_On_Organ_01(string toolName)
    {
        if (organ_InMission[0] != null)
            working_on_Organ_02(organ_InMission[0].Name, 0, toolName);

        if (organ_InMission[1] != null)
            working_on_Organ_02(organ_InMission[1].Name, 1, toolName);
    }


    private void working_on_Brain(int sawN)
    {
        if (organ_InMission[0].Name == "Brain" && toolsInUse[0].toolName == "Saw")
            working_on_Brain_02(0, sawN);

        if (organ_InMission[1].Name == "Brain" && toolsInUse[1].toolName == "Saw")
            working_on_Brain_02(1, sawN);
    }

    private void working_on_Blood()
    {
        if (organ_InMission[0].Name == "Blood")
            working_on_Organ_03(0);

        if (organ_InMission[1].Name == "Blood")
            working_on_Organ_03(1);
    }

    private void working_on_Brain_02(int playerNum, int sawN)
    {
        if(isOnBrain == false) return;
       
        if (sawN == sawN_)
        {
            barNow[playerNum]++;

            if (sawN_ == 0) sawN_ = 1;
            else sawN_ = 0;

            player[playerNum].anim.SetTrigger("saw");

            GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
            blood.transform.parent = player[playerNum].bloodPoint.transform;
            blood.transform.localPosition = new Vector3(0, 0, 0);
            blood.transform.localRotation = Quaternion.Euler(0, 0, 0);

            checkOrganGet(playerNum);
        }

        else 
        {

            player[playerNum].anim.SetTrigger("saw_bad");

            for (int i = 0; i < 4; i++)
            {
                GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
                blood.transform.parent = player[playerNum].bloodPoint.transform;
                blood.transform.localPosition = new Vector3(0, 0, 0);
                blood.transform.localRotation = Quaternion.Euler(0, 0, 0);

            }

            inceaseSUS();
        }

      
    }


    private void inceaseSUS()
    {
        susNow++;
        susBar.SetValue(susNow, 100);
        susAnim.SetTrigger("haha");
     
        if (susNow == 100)
        {
            
            SceneManager.LoadScene(0);
        }
    }
    private void working_on_Organ_02(string organName, int playerNum, string toolName)
    {
        if (organName == "Heart" && isOnHeart == false) return;
        if(organName == "Brain" && isOnBrain == false) return;
        //if(organName == "Blood" && isOnBlood == false) return;

        if (toolsInUse[playerNum].toolName == toolName) working_on_Organ_03(playerNum);
    }



    private void hitBox_Randoness(int playerNum)
    {
        //Debug.Log("playerNum");
        hitBox[playerNum].transform.localPosition = new Vector3(Random.Range(-.3f,.3f), 0, 0);
        hitBox[playerNum].transform.localScale = new Vector3(Random.Range(0.2f, 0.4f), 0.1f, 1);
        BP[playerNum].speed = Random.Range(0.9f, 2f);
    }


    public GameObject blood_;
    private void working_on_Organ_03(int playerNum)
    {
        bool CHECK_HIT = BP[playerNum].checkPointHit();

        if (CHECK_HIT == false)
        {
            GameObject blood = Instantiate(blood_, player[playerNum].bloodPoint.transform.position, Quaternion.identity) as GameObject;
            blood.transform.parent = player[playerNum].bloodPoint.transform;
            blood.transform.localPosition = new Vector3(0,0,0);
            blood.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
        if (barNow[playerNum] >= barMax[playerNum])
        {
            organ_InMission[playerNum].loop--;

            if (toolsInUse[playerNum].toolName == "Needle" || toolsInUse[playerNum].toolName == "Saw")
            {
                toolsInUse[playerNum] = null;
            }

            else toolsAvaliable.Add(toolsInUse[playerNum]);

            giveToolToDoctor(playerNum);

            if (organ_InMission[playerNum].loop <= 0)
            {
                organ_InMission[playerNum] = null;

                if (patient.organList != null)
                    if (patient.organList.Count > 0)
                        giveOrderToDoctor(playerNum, patient.organList[Random.Range(0, patient.organList.Count)]);
            }

            else
            {
                barNow[playerNum] = 0;
                barMax[playerNum] = Random.Range(organ_InMission[playerNum].workOnTimeMin, organ_InMission[playerNum].workOnTimeMax);
            }
        }


    }
}
