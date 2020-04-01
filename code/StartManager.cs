using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//시작화면 관리 
public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    //게임시작 버튼관련
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    //튜토리얼 버튼관련
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    //게임종료 버튼관련
    public void GameExit()
    {
        Application.Quit();
    }
}
