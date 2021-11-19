using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEvent : MonoBehaviour
{
    public GameObject voteListPanel;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenVotePanel()
    {
        voteListPanel.SetActive(!voteListPanel.activeSelf);
    }
}
