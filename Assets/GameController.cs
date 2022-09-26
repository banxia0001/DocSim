using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_Text workingText_P0;
    public TMP_Text workingText_P1;

    public BarController[] BC;
    public Bar_PointController[] BP;

    public Patient patient;
    public Organ[] organs; 
    // just for test
    //public bool isOnLancet02;//2号手术刀
    //public bool isOnLancet07;//7号手术刀
    //public bool isOnScissor;//剪刀
    //public bool isOnForceps;//镊子
    [Space]
    public bool isOnHeart;
    public bool isOnLung;
    public bool isOnKidney;


    public Organ[] organ_InMission;
    public List<string> toolsAvaliable = new List<string>();

    public string[] toolsInUse;
 
    //public enum OrgansOn { NA, Heart, Lung }
    //public enum correctTool { NA, Lancet02, Lancet07, Scissor, Forceps }

    //public OrgansOn Mission_P0;
    //public correctTool Tool_P0;


    //public OrgansOn Mission_P1;
    //public correctTool Tool_P1;

    public float[] barNow;
    public float[] barMax;

    public void Start()
    {
        TurnOffBools();


        NewPatient_Coming();

    }

    private void TurnOffBools()
    {
        isOnHeart = false;
        isOnKidney = false;
        isOnLung = false;
    }


    private void NewPatient_Coming()
    {
        choose_Organ_Setup();
        ShuffleOrganList();

        //随机病人备注，显示在屏幕左侧
        //显示图片
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
        int i = Random.Range(0, toolsAvaliable.Count);
        toolsInUse[playerNum] = toolsAvaliable[i];
        toolsAvaliable.Remove(toolsAvaliable[i]);
    }


    private void giveOrderToDoctor(int playerNum, Organ organ)
    {
        organ_InMission[playerNum] = organ;
        patient.organList.Remove(organ);
        barNow[playerNum] = 0;
        barMax[playerNum] = Random.Range(organ.workOnTimeMin, organ.workOnTimeMax);
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
        if (Input.GetKey(KeyCode.DownArrow)) isOnLung = true;
        if (Input.GetKey(KeyCode.LeftArrow)) isOnKidney = true;

        if (Input.GetKeyDown(KeyCode.A)) workin_On_Organ_01("Lancet02");
        if (Input.GetKeyDown(KeyCode.S)) workin_On_Organ_01("Lancet07");
        if (Input.GetKeyDown(KeyCode.W)) workin_On_Organ_01("Scissor");
        if (Input.GetKeyDown(KeyCode.D)) workin_On_Organ_01("Forceps");
    }


    private void workin_On_Organ_01(string toolName)
    {
        if (organ_InMission[0] != null)
            working_on_Organ_02(organ_InMission[0].Name, 0, toolName);

        if (organ_InMission[1] != null)
            working_on_Organ_02(organ_InMission[1].Name, 1, toolName);
    }

    private void working_on_Organ_02(string organName, int playerNum, string toolName)
    {
       

        if (organName == "Heart" && isOnHeart == false) return;
        if(organName == "Lung" && isOnLung == false) return;
        if(organName == "Kidney" && isOnKidney == false) return;


        if (toolsInUse[playerNum] == toolName) working_on_Organ_03(playerNum);
    }


    private void working_on_Organ_03(int playerNum)
    {
        bool CHECK_HIT = BP[playerNum].checkPointHit();

        if (CHECK_HIT == false)
        {
            // 玩家飙血，失败，加可疑度
            return;
        }
        barNow[playerNum]++;

        if (barNow[playerNum] >= barMax[playerNum])
        {
            organ_InMission[playerNum].loop--;

            toolsAvaliable.Add(toolsInUse[playerNum]);
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
