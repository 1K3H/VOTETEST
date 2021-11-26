using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class VoteManager : MonoBehaviour
{
    public static VoteManager VM;

    public enum Mode { CLOSED, LIST, CREATE, VOTE, VOTE_RESULT }
    Mode mode = Mode.CLOSED;

    public GameObject voteListPanel;
    public GameObject voteListPanelContent;
    public GameObject voteListBlock;
    public GameObject voteCreatePanel;
    public GameObject votePanel;
    public int SelectedVoteID;
    public int SelectedSelectionNum;

    public Text inputTitle;
    public Text inputSelection1;
    public Text inputSelection2;

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

    void Awake()
    {
        VM = this;
    }

    void Start()
    {
        LoadAllData();
        SetVoteList();
    }

    public void LoadAllData()
    {
        // JSON 파일 불러오기
        voteDataText = Resources.Load("VoteInfo") as TextAsset;
        myVoteData = JsonUtility.FromJson<MyVoteDataArray>(voteDataText.ToString());
        Debug.Log("LoadAllData");
    }

    public void SetVoteList()
    {
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
            voteBlock.GetComponent<Button>().onClick.AddListener(() => OpenVotePanel(myVoteData.data[idIndex].ID));
        }
        Debug.Log("SetVoteList");
    }

    public void DeleteVoteList()
    {
        for (int i = 0; i < voteListPanelContent.transform.childCount; i++)
        {
            Destroy(voteListPanelContent.transform.GetChild(i).gameObject);
        }
    }

    public void InsertData(string title, string selection1, string selection2)
    {
        VoteData newVoteData = new VoteData();
        newVoteData.ID = myVoteData.data.Length;
        newVoteData.TITLE = title;
        newVoteData.USER_NAME = "김승환";
        newVoteData.CREATE_DATE = "2021-11-23";
        Selection newSelectionData1 = new Selection();
        newSelectionData1.COUNT = 0;
        newSelectionData1.DESC = selection1;
        Selection newSelectionData2 = new Selection();
        newSelectionData2.COUNT = 0;
        newSelectionData2.DESC = selection2;
        Selection[] newSelections = new Selection[] { newSelectionData1, newSelectionData2 };
        newVoteData.SELECTION = newSelections;

        List<VoteData> intermediate_list = new List<VoteData>();
        for (int i = 0; i < myVoteData.data.Length; i++)
        {
            intermediate_list.Add(myVoteData.data[i]);
        }
        intermediate_list.Add(newVoteData);
        myVoteData.data = intermediate_list.ToArray();

        string json = JsonUtility.ToJson(myVoteData);
        string path = Application.dataPath + "/Resources/VoteInfo.json";

        // File.WriteAllText(path, json);
        Debug.Log("InsertData");
    }


    // Button Events

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
        string title = inputTitle.text;
        string selection1 = inputSelection1.text;
        string selection2 = inputSelection2.text;
        InsertData(title, selection1, selection2);
        DeleteVoteList();
        SetVoteList();
        BackToVoteListPanel();
    }

    public void OpenVotePanel(int ID)
    {
        SelectedVoteID = ID;
        // Set selection
        for (int i = 0; i < myVoteData.data[ID].SELECTION.Length; i++)
        {
            int num = i;
            // Set title
            votePanel.transform.Find("Text VoteTitle").GetComponent<Text>().text = myVoteData.data[ID].TITLE;
            // Set descript
            votePanel.transform.Find($"Panel Selection{num + 1}/Button Selection{num + 1}/Text Selection{num + 1}").GetComponent<Text>().text = myVoteData.data[ID].SELECTION[num].DESC;
        }

        voteListPanel.SetActive(false);
        votePanel.SetActive(true);
        mode = Mode.VOTE;
    }

    public void SelectSelection(int selectionNum)
    {
        SelectedSelectionNum = selectionNum;
        Debug.Log(SelectedSelectionNum);
    }

    public void FinishVote()
    {
        // 투표 정보 DB에 업데이트하기
        myVoteData.data[SelectedVoteID].SELECTION[SelectedSelectionNum].COUNT++;
        // DeleteVoteList();
        // SetVoteList();

        int allCount = 0;

        for(int i = 0; i < myVoteData.data[SelectedVoteID].SELECTION.Length; i++)
        {
            allCount += myVoteData.data[SelectedVoteID].SELECTION[i].COUNT;
        }

        for(int i = 0; i < myVoteData.data[SelectedVoteID].SELECTION.Length; i++)
        {
            float per = 100 * ((float)myVoteData.data[SelectedVoteID].SELECTION[i].COUNT / (float)allCount);

            votePanel.transform.Find($"Panel Selection{i+1}/Text Count").GetComponent<Text>().text = $"{myVoteData.data[SelectedVoteID].SELECTION[i].COUNT} / {per}%";

            // Debug.Log(allCount);
            // Debug.Log(myVoteData.data[SelectedVoteID].SELECTION[i].COUNT);

            votePanel.transform.Find($"Panel Selection{i+1}/Text Count").GetComponent<Text>().gameObject.SetActive(true);
        }
        mode = Mode.VOTE_RESULT;
    }

    public void BackToVoteListPanel()
    {
        if (mode == Mode.CREATE) voteCreatePanel.SetActive(false);
        else if (mode == Mode.VOTE) votePanel.SetActive(false);

        voteListPanel.SetActive(true);
        mode = Mode.LIST;
    }
}