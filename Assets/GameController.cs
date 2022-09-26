using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_Text workingText_P0;
    public TMP_Text workingText_P1;


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
 
    //public enum OrgansOn { NA, Heart, Lung }
    public enum correctTool { NA, Lancet02, Lancet07, Scissor, Forceps }

    //public OrgansOn Mission_P0;
    public correctTool Tool_P0;


    //public OrgansOn Mission_P1;
    public correctTool Tool_P1;

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
        isOnLung = false;
    }


    private void NewPatient_Coming()
    {
        choose_Organ_Setup();
        ShuffleOrganList();

        //随机病人备注，显示在屏幕左侧
        //显示图片
        giveOrderToDoctor(0, patient.organList[Random.Range(0, patient.organList.Count)]);
        giveOrderToDoctor(1, patient.organList[Random.Range(0, patient.organList.Count)]);

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

    private void giveOrderToDoctor(int playerNum, Organ organ)
    {
        organ_InMission[playerNum] = organ;
        patient.organList.Remove(organ);
        barNow[playerNum] = 0;
        barMax[playerNum] = Random.Range(organ.workOnTimeMin, organ.workOnTimeMax);

        int i = Random.Range(0, 4);
        if(i == 0) Tool_P0 = correctTool.Forceps;
        if(i == 1) Tool_P0 = correctTool.Lancet02;
        if(i == 2) Tool_P0 = correctTool.Lancet07;
        if(i == 3) Tool_P0 = correctTool.Scissor;

    }

    public void FixedUpdate()
    {
        TurnOffBools();

        //if (isWorking_P0) barNow_P0 += 1 * Time.fixedDeltaTime;
        //if (isWorking_P1) barNow_P1 += 1 * Time.fixedDeltaTime;
    }



    public void Update()
    {
        if (organ_InMission[0] == null && organ_InMission[1] == null)
        {
            NewPatient_Coming();
        }

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
        working_on_Organ_02(organ_InMission[0].Name, 0, toolName);
        working_on_Organ_02(organ_InMission[1].Name, 1, toolName);
    }

    private void working_on_Organ_02(string organName, int playerNum, string toolName)
    {
       
        if (organName == "Heart" && isOnHeart == false) return;
        if(organName == "Lung" && isOnLung == false) return;
        if(organName == "Kidney" && isOnKidney == false) return;

        if (organ_InMission[playerNum] == null) return;

        if (playerNum == 0)
        {
           
            if (Tool_P0 == correctTool.Lancet02 && toolName == "Lancet02") working_on_Organ_03(0);
            if (Tool_P0 == correctTool.Lancet07 && toolName == "Lancet07") working_on_Organ_03(0);
            if (Tool_P0 == correctTool.Forceps && toolName == "Forceps") working_on_Organ_03(0);
            if (Tool_P0 == correctTool.Scissor && toolName == "Scissor") working_on_Organ_03(0);
        }

        else
        {
            if (Tool_P1 == correctTool.Lancet02 && toolName == "Lancet02") working_on_Organ_03(1);
            if (Tool_P1 == correctTool.Lancet07 && toolName == "Lancet07") working_on_Organ_03(1);
            if (Tool_P1 == correctTool.Forceps && toolName == "Forceps") working_on_Organ_03(1);
            if (Tool_P1 == correctTool.Scissor && toolName == "Scissor") working_on_Organ_03(1);
        }
       
    }


    private void working_on_Organ_03(int playerNum)
    {
        barNow[playerNum]++;

        if (barNow[playerNum] >= barMax[playerNum])
        {
            organ_InMission[playerNum].loop--;
            if (organ_InMission[playerNum].loop <= 0)
            {
                organ_InMission[playerNum] = null;

                if (patient.organList != null)
                    if (patient.organList.Count <= 0)
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
