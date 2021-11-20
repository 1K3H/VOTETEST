using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEvent : MonoBehaviour
{
    public static BtnEvent BE;

    // 상태 관리
    // 투표 프로세스의 어떤 창이 떠있는지 알려줄 수 있는 상태가 필요하다.

    public GameObject voteListPanel;
    public GameObject votePanel;

    void Awake() {
        BE = this;    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenVoteListPanel()
    {
        // 투표와 관련된 어떤 창도 떠있지 않은 경우와 voteListPanel이 떠있는 경우에만 작동한다. (조건)
        voteListPanel.SetActive(!voteListPanel.activeSelf);
    }

    public void OpenVotePanel()
    {
        voteListPanel.SetActive(false);
        votePanel.SetActive(true);
    }
}
