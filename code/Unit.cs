using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//장애물 관련 코드 
public class Unit : MonoBehaviour
{
    public char pattern = 'a';    // 장애물 이동 패턴
    public float unitTime = 0.0f;    //장애물 생성후 시간 
    public Vector3 startPos;          //생성 위치
    public float speed = 0.1f;        // 장애물 속도
    public char unitDir = 'd';       // 장애물 이동 방향
    public Vector3 velocity;         //장애물 속도
    public Quaternion v3rotation;   //장애물 이동 방향 벡터

    // 장애물 이동설정 
    void Start()
    {
        if (unitDir == 'd')
        {
            v3rotation = Quaternion.Euler(0f, 0f, 0.0f);
        }
        else if (unitDir == 'r')
        {
            v3rotation = Quaternion.Euler(0f, 0f, 90.0f);
        }
        else if (unitDir == 'u')
        {
            v3rotation = Quaternion.Euler(0f, 0f, 180.0f);
        }
        else if (unitDir == 'l')
        {
            v3rotation = Quaternion.Euler(0f, 0f, 270.0f);
        }
        velocity = new Vector3(0, -1 * speed, 0);
        startPos = transform.position;
        velocity = v3rotation * velocity;
    }

    // 장애물 패턴에 따른 이동설정 
    void Update()
    {
        unitTime += Time.deltaTime;
        if (pattern == 'a') //일정한 속도의 직선
        {
            transform.Translate(velocity);
        }
        else if(pattern == 'b')   //가속하는 직선
        {
            velocity = velocity * 1.1f;
            transform.Translate(velocity);
        }
        else if(pattern == 'c')  //곡선
        {
            transform.position =  GetPointOnBezierCurve(startPos, startPos + v3rotation * new Vector3(2, -1, 0), startPos + v3rotation * new Vector3(2, -5, 0), startPos + v3rotation * new Vector3(0,-7,0), unitTime * speed);
        }
        else if (pattern == 'd')  //곡선2
        {
            transform.position = GetPointOnBezierCurve(startPos, startPos + v3rotation * new Vector3(-2, -1, 0), startPos + v3rotation * new Vector3(-2, -5, 0), startPos + v3rotation * new Vector3(0, -7, 0), unitTime* speed);
        }
        else if(pattern == 'e')  //ㄹ자형
        {
            if (unitTime < 0.5f)
            {
                transform.Translate(velocity);
            }
            else if (unitTime >= 0.5f && unitTime < 1.0f)
            {
                transform.Translate(Quaternion.Euler(0f, 0f, 90.0f) * velocity);
            }
            else if (unitTime >= 1.0f && unitTime < 1.5f)
            {
                transform.Translate(velocity);
            }
            else if (unitTime >= 1.5f && unitTime < 2.0f)
            {
                transform.Translate(Quaternion.Euler(0f, 0f, -90.0f) * velocity);
            }
            else
            {
                velocity = velocity * 1.2f;
                transform.Translate(velocity);
            }
        }
        else if (pattern == 'f')  //ㄹ자형2
        {
            if (unitTime < 0.5f)
            {
                transform.Translate(velocity);
            }
            else if (unitTime >= 0.5f && unitTime < 1.0f)
            {
                transform.Translate(Quaternion.Euler(0f, 0f, -90.0f) * velocity);
            }
            else if (unitTime >= 1.0f && unitTime < 1.5f)
            {
                transform.Translate(velocity);
            }
            else if (unitTime >= 1.5f && unitTime < 2.0f)
            {
                transform.Translate(Quaternion.Euler(0f, 0f, 90.0f) * velocity);
            }
            else
            {
                velocity = velocity * 1.2f;
                transform.Translate(velocity);
            }
        }
        else if(pattern == 'g')  // 안보이는 유형
        {
            transform.Translate(velocity);

            if (unitTime >= 0.3f && unitTime < 0.6f)
            {
                transform.Translate(velocity);
                this.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else
            {
                this.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
        }


        //화면을 벗어날경우 없앰 
        if(transform.position.x > 11 || transform.position.x < -11 || transform.position.y > 7 || transform.position.y < -7)
        {
            Destroy(gameObject);
        }
    }

    // 장애물 충돌 관리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player" )
        {
            if(PlayerScript2.godmod == false)
            {
                collision.GetComponent<PlayerScript2>().GetCollision(transform.GetComponent<SpriteRenderer>().color);
                GameObject camera = GameObject.Find("Main Camera");
                camera.GetComponent<MainCameraScript>().CameraEffect(0.2f);
                PlayerScript2.godmod = true;
                PlayerScript2.godTime = 0.2f;
            }
            Destroy(gameObject);
        }
    }

    //곡선이동 함수
    Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result =
            (u3) * p0 +
            (3f * u2 * t) * p1 +
            (3f * u * t2) * p2 +
            (t3) * p3;

        return result;
    }
}
