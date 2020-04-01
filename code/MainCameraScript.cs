using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 카메라 관리 스크립트.
public class MainCameraScript : MonoBehaviour  //
{
    public Vector3 oripos;  // 카메라 처음 위치
    public float timer = 0;  // 카메라 흔들리는 시간 
    public float power = 0.1f;  //카메라 흔들리는 강도 
    // Start is called before the first frame update
    void Start()
    {
        oripos = transform.position;  // 카메라 처음 위치 저장
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라 흔들림 구현부분
        if (timer >0.0f)
        {
            timer -= Time.deltaTime;
            transform.position = new Vector3(Random.insideUnitCircle.x * power + oripos.x,Random.insideUnitCircle.y * power + oripos.y,transform.position.z);
        }
        else
        {
            timer = 0.0f;
            transform.position = oripos;
        }
    }

    public void CameraEffect(float term)
    {
        timer = term;
    }
}
