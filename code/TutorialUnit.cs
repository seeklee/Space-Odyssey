using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//튜토리얼 전용 장애물 관리 코드 
public class TutorialUnit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //플레이어와 출돌시 이벤트 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "player")
        {
            collision.GetComponent<PlayerScript2>().GetCollision(transform.GetComponent<SpriteRenderer>().color);
            GameObject camera = GameObject.Find("Main Camera");
            camera.GetComponent<MainCameraScript>().CameraEffect(0.2f); //카메라 흔들림 
            TutorialScript.breakcount++;   //장애물 충돌 카운트
            Destroy(gameObject);  //파괴
        }
    }
}
