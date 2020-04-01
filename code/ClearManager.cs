using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 게임 결과를 알려주는 씬 관리
public class ClearManager : MonoBehaviour    
{
    const int SSCORE = 50000;    // S등급 최소점수 
    const int ASCORE = 30000;    // A등급 최소점수
    const int BSCORE = 15000;    // B등급 최소점수
    const int CSCORE = 5000;     // C등급 최소점수

    // 게임점수와 콤보 클리어 여부를 출력
    void Start()
    {
        GameObject scoreNum = GameObject.Find("ScoreNum");   //score 문자열 텍스트
        GameObject comboNum = GameObject.Find("ComboNum");   //combo 문자열 텍스트
        GameObject grade = GameObject.Find("Grade");         //grade  
        GameObject message = GameObject.Find("Clear");       //fail message
        scoreNum.GetComponent<Text>().text = PlayerScript2.score.ToString();
        comboNum.GetComponent<Text>().text = PlayerScript2.maxcombo.ToString();
        if(PlayerScript2.score >= SSCORE)
        {
            grade.GetComponent<Text>().text = "S";
            message.GetComponent<Text>().text = "Clear";
        }
        else if(PlayerScript2.score >= ASCORE)
        {
            grade.GetComponent<Text>().text = "A";
            message.GetComponent<Text>().text = "Clear";
        }
        else if (PlayerScript2.score >= BSCORE)
        {
            grade.GetComponent<Text>().text = "B";
            message.GetComponent<Text>().text = "Clear";
        }
        else if (PlayerScript2.score >= CSCORE)
        {
            grade.GetComponent<Text>().text = "C";
            message.GetComponent<Text>().text = "Clear";
        }
        else 
        {
            grade.GetComponent<Text>().text = "F";
            message.GetComponent<Text>().text = "Failed";
        }
    }

    //다시 처음 화면으로 로드
    public void ToStartGame()
    {
        SceneManager.LoadScene("StartScene");
    }
}
