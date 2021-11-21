using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteManager : MonoBehaviour
{
    public static VoteManager VM;
    public enum Mode { CLOSED, LIST, CREATE, VOTE }
    public Mode mode = Mode.CLOSED;

    public GameObject voteListPanel;
    public GameObject voteCreatePanel;
    public GameObject votePanel;

    void Awake() {
        VM = this;    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ToggleVoteListPanel()
    {
        if(mode == Mode.CLOSED)
        {
            voteListPanel.SetActive(true);
            mode = Mode.LIST;
        }
        else if(mode == Mode.LIST)
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

    public void OpenVotePanel()
    {
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
        if(mode == Mode.CREATE) voteCreatePanel.SetActive(false);
        else if(mode == Mode.VOTE) votePanel.SetActive(false);

        voteListPanel.SetActive(true);
        mode = Mode.LIST;
    }
}
