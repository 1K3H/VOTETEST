using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class VoteManager : MonoBehaviour
{
    public enum Mode { CLOSED, LIST, CREATE, VOTE }
    public Mode mode = Mode.CLOSED;

    public GameObject voteListPanel;
    public GameObject voteListPanelContent;
    public GameObject voteListBlock;
    public GameObject voteCreatePanel;
    public GameObject votePanel;

    TextAsset voteDataText;
    MyVoteDataArray myVoteData;

    [System.Serializable]
    public class Selection
    {
        public int COUNT;
        public string DESC;
    }

    [System.Serializable]
    public class VoteData
    {
        public int ID;
        public string TITLE;
        public string USER_NAME;
        public string CREATE_DATE;
        public Selection[] SELECTION;
    }

    [System.Serializable]
    public class MyVoteDataArray
    {
        public VoteData[] data;
    }

    void Start()
    {
        voteDataText = Resources.Load("VoteInfo") as TextAsset;
        myVoteData = JsonUtility.FromJson<MyVoteDataArray>(voteDataText.ToString());

        VoteData newVoteData = new VoteData();
        newVoteData.ID = 4;
        newVoteData.TITLE = "제발 데이터에 제대로 들어가 주세요 제발~";
        newVoteData.USER_NAME = "김승환";
        newVoteData.CREATE_DATE = "2021-11-23";
        Selection newSelectionData1 = new Selection();
        newSelectionData1.COUNT = 200;
        newSelectionData1.DESC = "왼쪽";
        Selection newSelectionData2 = new Selection();
        newSelectionData2.COUNT = 300;
        newSelectionData2.DESC = "오른쪽";
        Selection[] newSelections = new Selection[]{newSelectionData1, newSelectionData2};
        newVoteData.SELECTION = newSelections;

        List<VoteData> intermediate_list = new List<VoteData>();
        for(int i = 0; i < myVoteData.data.Length; i++)
        {
            intermediate_list.Add(myVoteData.data[i]);
        }
        intermediate_list.Add(newVoteData);
        myVoteData.data = intermediate_list.ToArray();
        
        Debug.Log(myVoteData.data[myVoteData.data.Length-1].SELECTION[0].DESC);

        string json = JsonUtility.ToJson(myVoteData);
        string path = Application.dataPath + "/Resources/VoteInfo.json";

        File.WriteAllText(path, json);

        // Debug.Log(myVoteData.data[0].SELECTION[0].COUNT);
        // Debug.Log(myVoteData.data[1].SELECTION[0].COUNT);

        // Set list
        for (int i = 0; i < myVoteData.data.Length; i++)
        {
            int idIndex = i;
            GameObject voteBlock = Instantiate(voteListBlock, voteListPanelContent.transform, false);
            // Set PosY
            voteBlock.GetComponent<RectTransform>().anchoredPosition = new Vector2(voteBlock.GetComponent<RectTransform>().anchoredPosition.x, voteBlock.GetComponent<RectTransform>().anchoredPosition.y - 80 * i);
            // Set Title
            voteBlock.transform.Find("Text Title").GetComponent<Text>().text = myVoteData.data[i].TITLE;
            // Set Info
            voteBlock.transform.Find("Text Info").GetComponent<Text>().text = $"제작 : {myVoteData.data[i].USER_NAME}   참여 : {myVoteData.data[i].SELECTION[0].COUNT + myVoteData.data[i].SELECTION[1].COUNT}";
            // Set Date
            voteBlock.transform.Find("Text Date").GetComponent<Text>().text = myVoteData.data[i].CREATE_DATE;
            // Set button event
            voteBlock.GetComponent<Button>().onClick.AddListener(()=> OpenVotePanel(myVoteData.data[idIndex].ID));
        }
    }

    void Update()
    {

    }

    public void ToggleVoteListPanel()
    {
        if (mode == Mode.CLOSED)
        {
            voteListPanel.SetActive(true);
            mode = Mode.LIST;
        }
        else if (mode == Mode.LIST)
        {
            voteListPanel.SetActive(false);
            mode = Mode.CLOSED;
        }
    }

    public void OpenVoteCreatePanel()
    {
        voteListPanel.SetActive(false);
        voteCreatePanel.SetActive(true);
        mode = Mode.CREATE;
    }

    public void FinishVoteCreate()
    {
        // 생성한 투표 데이터 DB로 전송하기
        // ListPanel로 돌아가기
        BackToVoteListPanel();
    }

    public void OpenVotePanel(int ID)
    {
        // Debug.Log(ID);

        // Set selection
        for(int i = 0; i < myVoteData.data[ID].SELECTION.Length; i++)
        {
            int num = i;
            // Set title
            votePanel.transform.Find("Text VoteTitle").GetComponent<Text>().text = myVoteData.data[ID].TITLE;
            // Set descript
            votePanel.transform.Find($"Panel Selection{num+1}/Button Selection{num+1}/Text Selection{num+1}").GetComponent<Text>().text = myVoteData.data[ID].SELECTION[num].DESC;
        }

        voteListPanel.SetActive(false);
        votePanel.SetActive(true);
        mode = Mode.VOTE;
    }

    public void FinishVote()
    {
        // 투표 정보 DB에 업데이트하기
        // ListPanel로 돌아가기
        BackToVoteListPanel();
    }

    public void BackToVoteListPanel()
    {
        if (mode == Mode.CREATE) voteCreatePanel.SetActive(false);
        else if (mode == Mode.VOTE) votePanel.SetActive(false);

        voteListPanel.SetActive(true);
        mode = Mode.LIST;
    }
}