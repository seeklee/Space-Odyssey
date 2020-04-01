using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



// 장애물 구조체 
public struct note
{
    public float time;   //등장시간
    public char pattern;  //패턴 a:등속직선 b:등가속도직선 c:왼쪽 곡선 d:오른쪽 곡선 e: 오른쪽 ㄹ자형 f: 왼쪽 ㄹ자형 g: 사라짐
    public float velocity; //속도
    public char direction; //방향  d:down r:right u:up l:left
    public float loc;    //위치 위 3.5 아래 -3.5  좌 -7.5 우 7.5
    public char color;    //색깔 w:white r:red g:green b:blue c:cyan m:magenta y:yellow u:unknown
    
    //장애물 생성자
    public note(float time, char pattern, float velocity, char direction, float loc, char color)
    {
        this.time = time;
        this.pattern = pattern;
        this.velocity = velocity;
        this.direction = direction;
        this.loc = loc;
        this.color = color; 
    }
}


// 현재 스테이지 관련 클래스
public class StageDirector : MonoBehaviour
{

    public GameObject unit_pf;    //장애물 프리팹
    public int noteIndex = 0;     //지금까지 생성된 노트의 개수 
    public AudioSource audioSource;    // 배경음악
    public SimpleHealthBar timebar;    // 음악 재생시간 관련 UI
    public AudioClip song;             // 음악
    public float songtime = 0;         //노래시간

    public note[] stage1 = new note[]         //노래에 시간 맞춘 장애물 생성정보
    {
        // 등장시간, 패턴, 속도, 방향, 위치, 색깔
        new note(0.5f,'a',0.1f,'d',-6.0f,'u'),  //삐 삐 0.5f
        new note(0.8f,'a',0.1f,'d',3.0f,'u'),  
        new note(1.0f,'a',0.1f,'d',-4.0f,'u'),  //삐 삐 1.0f
        new note(1.2f,'a',0.1f,'d',-1.0f,'u'),  
        new note(1.4f,'a',0.1f,'d',7.0f,'u'),  //삐 삐 1.4f
        new note(1.6f,'a',0.1f,'d',0.0f,'u'),
        new note(1.85f,'a',0.1f,'d',3.0f,'u'), //삐 삐  1.85f
        new note(2.0f,'a',0.1f,'d',-7.0f,'u'),
        new note(3.0f,'a',0.07f,'r',-3.5f,'u'),  //P   3.0f
        new note(3.0f,'a',0.07f,'r',-1.0f,'y'),
        new note(3.0f,'a',0.07f,'r',1.0f,'u'),
        new note(3.0f,'a',0.07f,'r',3.5f,'u'),
        new note(3.4f,'a',0.07f,'u',7.0f,'u'),  //P   3.4f
        new note(3.4f,'a',0.07f,'u',2.0f,'g'),
        new note(3.4f,'a',0.07f,'u',-2.0f,'u'),
        new note(3.4f,'a',0.07f,'u',-7.0f,'u'),
        new note(3.6f,'a',0.07f,'l',-3.5f,'u'),  //A     3.6f
        new note(3.6f,'a',0.07f,'l',-1.0f,'u'),
        new note(3.6f,'a',0.07f,'l',1.0f,'c'),
        new note(3.6f,'a',0.07f,'l',3.5f,'u'),
        new note(3.8f,'a',0.07f,'d',7.0f,'u'),  //P    3.8f
        new note(3.8f,'a',0.07f,'d',2.0f,'u'),
        new note(3.8f,'a',0.07f,'d',-2.0f,'m'),
        new note(3.8f,'a',0.07f,'d',-7.0f,'u'),
        new note(4.5f,'c',1.5f,'r',3.0f,'u'),  //뚠      4.5f
        new note(5.3f,'c',1.5f,'u',-3.0f,'u'),  //뚠     5.3f
        new note(6.3f,'d',1.5f,'u',3.0f,'u'),  //뚠      6.3f
        new note(7.1f,'c',1.5f,'l',-3.0f,'u'),  //뚠     7.1f
        new note(8.0f,'c',1.5f,'d',3.0f,'u'),  //뚠      8.0f
        new note(8.8f,'d',1.5f,'d',-3.0f,'u'),  //뚠     8.8f
        new note(9.8f,'c',1.5f,'r',3.0f,'b'),  //뚠      9.8f
        new note(11.2f,'a',0.1f,'d',-6.0f,'y'),  //I have a pen  11.2f,11.5f,11.7f,12.0f
        new note(11.2f,'a',0.1f,'d',6.0f,'y'),
        new note(11.5f,'a',0.1f,'d',-3.0f,'m'),
        new note(11.5f,'a',0.1f,'d',3.0f,'m'),
        new note(11.7f,'a',0.1f,'d',-1.0f,'c'),
        new note(11.7f,'a',0.1f,'d',1.0f,'c'),
        new note(12.0f,'b',0.01f,'r',-3.0f,'u'),
        new note(12.0f,'b',0.01f,'r',-1.0f,'u'),
        new note(12.0f,'b',0.01f,'r',1.0f,'u'),
        new note(12.0f,'b',0.01f,'r',3.0f,'u'),
        new note(13.0f,'a',0.1f,'u',1.0f,'r'),  //I have an apple  13.0f,13.3f,13.7f,14.0f
        new note(13.0f,'a',0.1f,'u',-1.0f,'r'),
        new note(13.3f,'a',0.1f,'u',3.0f,'g'),
        new note(13.3f,'a',0.1f,'u',-3.0f,'g'),
        new note(13.7f,'a',0.1f,'u',6.0f,'b'),
        new note(13.7f,'a',0.1f,'u',-6.0f,'b'),
        new note(14.0f,'b',0.01f,'l',-3.0f,'u'),
        new note(14.0f,'b',0.01f,'l',-1.0f,'u'),
        new note(14.0f,'b',0.01f,'l',1.0f,'u'),
        new note(14.0f,'b',0.01f,'l',3.0f,'u'),
        new note(15.5f,'b',0.01f,'r',-3.0f,'m'),  //ahh 15.5f
        new note(15.5f,'b',0.01f,'r',-1.0f,'m'),
        new note(15.5f,'b',0.01f,'r',1.0f,'m'),
        new note(15.5f,'b',0.01f,'l',3.0f,'c'),
        new note(15.5f,'b',0.01f,'l',1.0f,'c'),
        new note(15.5f,'b',0.01f,'l',-1.0f,'c'),
        new note(16.5f,'a',0.05f,'r',-3.0f,'r'),   //apple 16.5f
        new note(16.5f,'a',0.05f,'r',0.0f,'r'),
        new note(16.5f,'a',0.05f,'r',3.0f,'r'),
        new note(16.5f,'a',0.05f,'l',-3.0f,'y'),
        new note(16.5f,'a',0.05f,'l',0.0f,'y'),
        new note(16.5f,'a',0.05f,'l',3.0f,'y'),
        new note(17.2f,'a',0.05f,'r',-1.5f,'y'),   //peeen 17.2f
        new note(17.2f,'a',0.05f,'r',1.5f,'y'),
        new note(17.2f,'a',0.05f,'l',1.5f,'r'),
        new note(17.2f,'a',0.05f,'l',-1.5f,'r'),
        new note(18.3f,'a',0.1f,'d',-7.0f,'u'),   //i have a pen 18.3f,18.5f,18.8f,19.3f
        new note(18.3f,'a',0.1f,'d',7.0f,'u'),
        new note(18.5f,'a',0.1f,'d',-4.0f,'u'),
        new note(18.5f,'a',0.1f,'d',4.0f,'u'),
        new note(18.8f,'a',0.1f,'d',-1.0f,'u'),
        new note(18.8f,'a',0.1f,'d',1.0f,'u'),
        new note(19.3f,'b',0.01f,'u',-6.0f,'g'),
        new note(19.3f,'b',0.01f,'u',3.0f,'g'),
        new note(19.3f,'b',0.01f,'u',-3.0f,'g'),
        new note(19.3f,'b',0.01f,'u',6.0f,'g'),
        new note(20.0f,'a',0.1f,'r',3.0f,'u'),  //i have pineapple 20.0f,20.3f,20.47f
        new note(20.3f,'a',0.1f,'l',-3.0f,'u'),
        new note(20.47f,'a',0.1f,'d',-6.0f,'b'),
        new note(20.47f,'a',0.1f,'d',7.0f,'b'),
        new note(20.75f,'a',0.1f,'d',3.0f,'b'),
        new note(20.75f,'a',0.1f,'d',-7.0f,'b'),
        new note(20.9f,'a',0.1f,'d',-1.0f,'b'),
        new note(20.9f,'a',0.1f,'d',2.0f,'b'),
        new note(21.2f,'a',0.1f,'d',-3.0f,'b'),
        new note(21.2f,'a',0.1f,'d',4.0f,'b'),
        new note(22.5f,'b',0.03f,'r',-3.0f,'u'),  //ahh  22.5f
        new note(22.5f,'b',0.03f,'r',0.0f,'m'),
        new note(22.5f,'b',0.03f,'r',3.0f,'u'),
        new note(22.5f,'b',0.03f,'l',3.0f,'u'),
        new note(22.5f,'b',0.03f,'l',0.0f,'c'),
        new note(22.5f,'b',0.03f,'l',-3.0f,'u'),
        new note(23.3f,'a',0.1f,'d',-6.0f,'u'),   //pineapple pen  23.3f,23.6f,23.8f,24.3f
        new note(23.3f,'a',0.1f,'d',0.0f,'u'),
        new note(23.6f,'a',0.1f,'d',7.0f,'u'),
        new note(23.6f,'a',0.1f,'d',3.0f,'u'),
        new note(23.8f,'a',0.1f,'d',-3.0f,'u'),
        new note(23.8f,'a',0.1f,'d',5.0f,'u'),
        new note(24.3f,'a',0.1f,'d',1.0f,'u'),
        new note(24.3f,'a',0.1f,'d',-7.0f,'u'),
        new note(25.0f,'a',0.1f,'r',-3.0f,'r'),  //apple pen 25.1f 25.5f 26.0f
        new note(25.0f,'a',0.1f,'r',-1.5f,'b'),
        new note(25.0f,'a',0.1f,'r',0f,'r'),
        new note(25.0f,'a',0.1f,'r',1.5f,'b'),
        new note(25.0f,'a',0.1f,'r',3f,'r'),
        new note(25.5f,'a',0.1f,'r',-3.0f,'r'),  
        new note(25.5f,'a',0.1f,'r',-1.5f,'b'),
        new note(25.5f,'a',0.1f,'r',0f,'r'),
        new note(25.5f,'a',0.1f,'r',1.5f,'b'),
        new note(25.5f,'a',0.1f,'r',3f,'r'),
        new note(26.0f,'a',0.1f,'r',-3.0f,'r'),  
        new note(26.0f,'a',0.1f,'r',-1.5f,'b'),
        new note(26.0f,'a',0.1f,'r',0f,'r'),
        new note(26.0f,'a',0.1f,'r',1.5f,'b'),
        new note(26.0f,'a',0.1f,'r',3f,'r'),
        new note(27.0f,'a',0.1f,'l',-3.0f,'m'),   //pineapple pen 26.9f 27.2f 27.4f 28.7f (29.1f
        new note(27.0f,'a',0.1f,'l',-1.5f,'g'),
        new note(27.0f,'a',0.1f,'l',0f,'m'),
        new note(27.0f,'a',0.1f,'l',1.5f,'g'),
        new note(27.0f,'a',0.1f,'l',3f,'m'),
        new note(27.5f,'a',0.1f,'l',-3.0f,'m'),
        new note(27.5f,'a',0.1f,'l',-1.5f,'g'),
        new note(27.5f,'a',0.1f,'l',0f,'m'),
        new note(27.5f,'a',0.1f,'l',1.5f,'g'),
        new note(27.5f,'a',0.1f,'l',3f,'m'),
        new note(28.0f,'a',0.1f,'l',-3.0f,'m'),
        new note(28.0f,'a',0.1f,'l',-1.5f,'g'),
        new note(28.0f,'a',0.1f,'l',0f,'m'),
        new note(28.0f,'a',0.1f,'l',1.5f,'g'),
        new note(28.0f,'a',0.1f,'l',3f,'m'),
        new note(29.6f,'b',0.03f,'r',-3.5f,'u'), //ahh 29.6f
        new note(29.6f,'b',0.03f,'r',-1.5f,'u'),
        new note(29.6f,'b',0.03f,'r',0.5f,'c'),
        new note(29.6f,'b',0.03f,'l',3.5f,'u'),
        new note(29.6f,'b',0.03f,'l',1.5f,'u'),
        new note(29.6f,'b',0.03f,'l',-0.5f,'c'),
        new note(30.5f,'a',0.07f,'r',-3.0f,'r'),  //pen pineapple apple pen 
        new note(30.5f,'a',0.07f,'r',-1.0f,'r'),
        new note(30.5f,'a',0.07f,'r',1.0f,'r'),
        new note(30.5f,'a',0.07f,'r',3.0f,'r'),
        new note(30.7f,'a',0.07f,'u',1.0f,'g'), 
        new note(30.7f,'a',0.07f,'u',3.0f,'g'),
        new note(30.7f,'a',0.07f,'u',5.0f,'g'),
        new note(30.7f,'a',0.07f,'u',7.0f,'g'),
        new note(30.9f,'a',0.07f,'l',3.0f,'b'),  
        new note(30.9f,'a',0.07f,'l',1.0f,'b'),
        new note(30.9f,'a',0.07f,'l',-1.0f,'b'),
        new note(30.9f,'a',0.07f,'l',-3.0f,'b'),
        new note(31.17f,'a',0.07f,'d',-7.0f,'c'),  
        new note(31.17f,'a',0.07f,'d',-5.0f,'c'),
        new note(31.17f,'a',0.07f,'d',-3.0f,'c'),
        new note(31.17f,'a',0.07f,'d',-1.0f,'c'),
        new note(31.4f,'a',0.07f,'r',-3.0f,'m'),  
        new note(31.4f,'a',0.07f,'r',-1.0f,'m'),
        new note(31.4f,'a',0.07f,'r',1.0f,'m'),
        new note(31.4f,'a',0.07f,'r',3.0f,'m'),
        new note(31.6f,'a',0.07f,'u',1.0f,'y'), 
        new note(31.6f,'a',0.07f,'u',3.0f,'y'),
        new note(31.6f,'a',0.07f,'u',5.0f,'y'),
        new note(31.6f,'a',0.07f,'u',7.0f,'y'),
        new note(31.85f,'a',0.07f,'l',3.0f,'r'),  
        new note(31.85f,'a',0.07f,'l',1.0f,'r'),
        new note(31.85f,'a',0.07f,'l',-1.0f,'r'),
        new note(31.85f,'a',0.07f,'l',-3.0f,'r'),
        new note(32.65f,'e',0.1f,'d',-5.0f,'u'),  //뚠      
        new note(33.5f,'e',0.1f,'r',-3.0f,'u'),  //뚠     
        new note(34.4f,'e',0.1f,'u',5.0f,'u'),  //뚠      
        new note(35.3f,'e',0.1f,'l',3.0f,'u'),  //뚠     
        new note(36.2f,'f',0.1f,'r',3.0f,'u'),  //뚠      
        new note(37.1f,'c',1.5f,'r',3.0f,'u'),  //칭
        new note(37.1f,'c',1.5f,'r',1.0f,'u'),  
        new note(37.1f,'c',1.5f,'r',-1.0f,'u'),  
        new note(37.1f,'c',1.5f,'l',-3.0f,'u'),    
        new note(37.1f,'c',1.5f,'l',-1.0f,'u'),   
        new note(37.1f,'c',1.5f,'l',1.0f,'u'),   
        new note(37.5f,'a',0.07f,'r',-3.0f,'g'),  //pen pineapple apple pen
        new note(37.5f,'a',0.07f,'r',-1.5f,'g'),
        new note(37.5f,'a',0.07f,'r',0.5f,'g'),
        new note(37.5f,'a',0.07f,'r',2.0f,'g'),
        new note(37.7f,'a',0.07f,'u',1.0f,'b'),
        new note(37.7f,'a',0.07f,'u',3.0f,'b'),
        new note(37.7f,'a',0.07f,'u',5.0f,'b'),
        new note(37.7f,'a',0.07f,'u',7.0f,'b'),
        new note(38.0f,'a',0.07f,'l',3.0f,'c'),
        new note(38.0f,'a',0.07f,'l',1.5f,'c'),
        new note(38.0f,'a',0.07f,'l',0.0f,'c'),
        new note(38.0f,'a',0.07f,'l',-1.5f,'c'),
        new note(38.2f,'a',0.07f,'d',-7.0f,'m'),
        new note(38.2f,'a',0.07f,'d',-5.0f,'m'),
        new note(38.2f,'a',0.07f,'d',-3.0f,'m'),
        new note(38.2f,'a',0.07f,'d',-1.0f,'m'),
        new note(38.5f,'a',0.07f,'r',-3.0f,'y'),
        new note(38.5f,'a',0.07f,'r',-1.5f,'y'),
        new note(38.5f,'a',0.07f,'r',0.5f,'y'),
        new note(38.5f,'a',0.07f,'r',2.0f,'y'),
        new note(38.7f,'a',0.07f,'u',1.0f,'r'),
        new note(38.7f,'a',0.07f,'u',3.0f,'r'),
        new note(38.7f,'a',0.07f,'u',5.0f,'r'),
        new note(38.7f,'a',0.07f,'u',7.0f,'r'),
        new note(38.9f,'a',0.07f,'l',3.0f,'g'),
        new note(38.9f,'a',0.07f,'l',1.5f,'g'),
        new note(38.9f,'a',0.07f,'l',0.0f,'g'),
        new note(38.9f,'a',0.07f,'l',-1.5f,'g'),
        new note(39.3f,'a',0.2f,'d',-4.5f,'r'),  // 뚜루 뚜루루루룽
        new note(39.4f,'a',0.2f,'d',-1f,'g'),
        new note(39.5f,'a',0.2f,'d',7.0f,'b'),
        new note(39.7f,'a',0.2f,'d',3.0f,'c'),
        new note(40.0f,'a',0.2f,'d',-7.0f,'m'),
        new note(40.3f,'a',0.2f,'d',-2.0f,'y'),
        new note(40.7f,'a',0.2f,'d',2f,'r'),
        new note(41.0f,'a',0.2f,'d',0f,'g'),
        new note(41.1f,'a',0.2f,'d',-4f,'b'),
        new note(41.3f,'a',0.2f,'d',6f,'c'),
        new note(41.5f,'a',0.2f,'d',3f,'m'),
        new note(41.7f,'a',0.2f,'d',-5.0f,'y')
    };

    // 초기화
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        songtime = song.length;
        PlayerScript2.combo = 0;
        PlayerScript2.score = 0;
        PlayerScript2.maxcombo = 0;
    }

    void Update()
    {
        timebar.UpdateBar(songtime - audioSource.time, songtime);               //시간바 업데이트

        //장애물 생성 코드 
        while (noteIndex < stage1.Length && stage1[noteIndex].time < audioSource.time)
        {
            float px;
            float py;
            //장애물 방향 설정 
            if (stage1[noteIndex].direction == 'd' )
            {
                px = stage1[noteIndex].loc;
                py = 4f;
            }
            else if(stage1[noteIndex].direction == 'r')
            {
                px = -8f;
                py = stage1[noteIndex].loc; 
            }
            else if (stage1[noteIndex].direction == 'u')
            {
                px = stage1[noteIndex].loc;
                py = -4.0f;
            }
            else
            {
                px = 8f;
                py = stage1[noteIndex].loc;
            }

            //장애물 생성 및 속성 변환
            GameObject unit = Instantiate(unit_pf, new Vector3(px, py, 0.0f), Quaternion.identity);
            unit.GetComponent<Unit>().pattern = stage1[noteIndex].pattern;
            unit.GetComponent<Unit>().speed = stage1[noteIndex].velocity;
            unit.GetComponent<Unit>().unitDir = stage1[noteIndex].direction;

            if (stage1[noteIndex].color == 'u')
            {
                unit.GetComponent<SpriteRenderer>().color = GameObject.Find("Player").GetComponent<SpriteRenderer>().color;
            }
            else if (stage1[noteIndex].color == 'r')
            {
                unit.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (stage1[noteIndex].color == 'g')
            {
                unit.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (stage1[noteIndex].color == 'b')
            {
                unit.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            else if (stage1[noteIndex].color == 'c')
            {
                unit.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            else if (stage1[noteIndex].color == 'm')
            {
                unit.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
            else if (stage1[noteIndex].color == 'y')
            {
                unit.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                unit.GetComponent<SpriteRenderer>().color = Color.white;
            }
            noteIndex++;
        }

        //노래 재생완료후 결과화면 이동 
        if(audioSource.isPlaying == false)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
}
