using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//튜토리얼 관리 코드 
public class TutorialScript : MonoBehaviour
{

    public GameObject unit_pf;     //장애물 프리팹
    public GameObject message;     // 튜토리얼 설명 오브젝트 
    public int stage = 1;          //튜토리얼 단계
    float stageTerm = 20.0f;       // 각 단계 최소 유지 시간
    static public int breakcount;   // 튜토리얼 유닛 충돌 횟수 카운드
    Text text;                      // 튜토리얼 설명 텍스트
    public string[] script;        //튜토리얼 설명 텍스트 저장


    //초기화
    void Start()
    {
        text = message.gameObject.GetComponent<Text>();
        StartCoroutine(FadeInOut());
        PlayerScript2.score = 0;
        PlayerScript2.combo = 0;
        PlayerScript2.maxcombo = 0;
        breakcount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        stageTerm -= Time.deltaTime;   //스테이지 경과시간 업데이트 


        //각 스테이지별 코드 
        if (stage == 1)  //stage1
        {
            stageTerm = 5.0f;
            stage = 2;
            message.GetComponent<Text>().text = script[0];
        }
        else if (stage == 2 && stageTerm < 0.0f)     //stage2
        {
            GameObject unit = Instantiate(unit_pf, new Vector3(7.0f, 3.0f, 0.0f), Quaternion.identity);
            unit.GetComponent<SpriteRenderer>().color = Color.blue;
            GameObject unit2 = Instantiate(unit_pf, new Vector3(7.0f, -3.0f, 0.0f), Quaternion.identity);
            unit2.GetComponent<SpriteRenderer>().color = Color.green;
            GameObject unit3 = Instantiate(unit_pf, new Vector3(-7.0f, -3.0f, 0.0f), Quaternion.identity);
            unit3.GetComponent<SpriteRenderer>().color = Color.cyan;
            stageTerm = 0f;
            message.GetComponent<Text>().text = script[1];
            stage = 3;
        }
        else if (stage == 3 && breakcount == 3)     //stage3
        {
            GameObject unit = Instantiate(unit_pf, new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
            unit.GetComponent<SpriteRenderer>().color = Color.yellow;
            GameObject unit2 = Instantiate(unit_pf, new Vector3(0.0f, -3.0f, 0.0f), Quaternion.identity);
            unit2.GetComponent<SpriteRenderer>().color = Color.yellow;
            GameObject unit3 = Instantiate(unit_pf, new Vector3(-7.0f, -0.0f, 0.0f), Quaternion.identity);
            unit3.GetComponent<SpriteRenderer>().color = Color.yellow;
            GameObject unit4 = Instantiate(unit_pf, new Vector3(7.0f, -0.0f, 0.0f), Quaternion.identity);
            unit4.GetComponent<SpriteRenderer>().color = Color.yellow;
            breakcount = 0;
            stage = 4;
            message.GetComponent<Text>().text = script[2];
        }
        else if (stage == 4 && breakcount == 4)     //stage4
        {
            GameObject unit = Instantiate(unit_pf, new Vector3(7.0f, 3.0f, 0.0f), Quaternion.identity);
            unit.GetComponent<SpriteRenderer>().color = Color.cyan;
            GameObject unit2 = Instantiate(unit_pf, new Vector3(7.0f, -3.0f, 0.0f), Quaternion.identity);
            unit2.GetComponent<SpriteRenderer>().color = Color.magenta;
            GameObject unit3 = Instantiate(unit_pf, new Vector3(-7.0f, -3.0f, 0.0f), Quaternion.identity);
            unit3.GetComponent<SpriteRenderer>().color = Color.green;
            GameObject unit4 = Instantiate(unit_pf, new Vector3(-7.0f, 3.0f, 0.0f), Quaternion.identity);
            unit4.GetComponent<SpriteRenderer>().color = Color.blue;
            breakcount = 0;
            stage = 5;
            message.GetComponent<Text>().text = script[3];
        }
        else if (stage == 5 && breakcount == 4)     //stage5
        {
            GameObject unit = Instantiate(unit_pf, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            unit.GetComponent<SpriteRenderer>().color = Color.red;
            breakcount = 0;
            stage = 6;
            stageTerm = 5.0f;
            message.GetComponent<Text>().text = script[4];
        }
        else if (stage == 6 && breakcount == 1 && stageTerm < 0)     //stage6
        {
            SceneManager.LoadScene("StartScene");
            breakcount = 0;
        }
    }

    // 튜토리얼 설명 깜박임 효과 
    IEnumerator FadeInOut()
    {
        while(true)
        {
            for (float i = 1f; i >= 0; i -= 0.02f)
            {
                Color tempColor = text.color;
                tempColor.a = i;
                text.color = tempColor;
                yield return 0;
            }
            for (float i = 0.0f; i <= 1; i += 0.02f)
            {
                Color tempColor = text.color;
                tempColor.a = i;
                text.color = tempColor;
                yield return 0;
            }
        }
    }


}
