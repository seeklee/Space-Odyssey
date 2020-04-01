using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//플레이어와의 작용에 관련된 클래스

public class PlayerScript2 : MonoBehaviour
{
    Rigidbody2D rigid2D;
    float moveForce = 100.0f;   // 플레이어 움직이는 정도 
    public GameObject mousepf;   // 클릭된 위치를 알려주시 위한 프리펫 
    public AudioClip[] ad;      // 충돌시 효과음 리스트
    public Vector3 mposition;   // 클릭된 위치의 좌표를 스크린기준으로 변환 시킨 좌표
    public Vector3 playerDirection;   // 플레이어의 이동 방향
    static public float godTime;    //무적 시간
    static public bool godmod;     //무적 여부
    public SimpleHealthBar hpbar;   //hp ui
    public int hp = 100;            //hp
    public GameObject collisionEffect;   //충돌시 생기는 이펙트
    public static int score = 0;      // 게임 점수
    public static int combo = 0;      // 현재 게임 콤보수
    public static int maxcombo = 0;   // 게임 최대 콤보수
    const int  GLITER = 2;      // 충돌시 반짝임 횟수 
    const float GLITERTERM = 0.02f;    // 충돌시 반짝임 사이시간
    const float MOUSETERM = 0.1f;    // 클릭위치 표시 지속시간


    // 초기화
    void Start()
    {
        godTime = 0.0f;
        godmod = false;
        this.rigid2D = GetComponent<Rigidbody2D>();
        playerDirection = transform.position;
        mposition = transform.position;
        
    }

    void Update()
    {
        // 화면 클릭시 플레이어 이동 및 클릭 UI 관련 코드
        if (Input.GetMouseButtonDown(0))
        {
            mposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(mouseEffect(mousepf,transform.GetComponent<SpriteRenderer>().color, mposition));
            playerDirection = new Vector3(mposition.x - transform.position.x, mposition.y - transform.position.y, transform.position.z);
            playerDirection = playerDirection.normalized;
            this.rigid2D.AddForce(playerDirection  * this.moveForce);
        }

        // 무적 해제 관련 코드
        godTime -= Time.deltaTime;
        if(godTime < 0)
        {
            godmod = false;
        }
    }

    // 플레이어가 커지는 함수
    public void GrowPlayer(bool grow)
    {
        if(grow == true)
        {
            if(this.transform.localScale.x <0.3f)
            {
                this.transform.localScale += new Vector3(0.02f, 0.02f, 0.0f);
                this.rigid2D.mass += 0.015f;
            }   
        }
        else
        {
            if (this.transform.localScale.x > 0.2f)
            {
                this.transform.localScale -= new Vector3(0.02f, 0.02f, 0.0f);
                this.rigid2D.mass -= 0.015f;
            }
        }
    }

    //충돌시 소리나는 함수
    public void CollisionSound(bool collision)
    {
        if (collision == true)
        {
            this.GetComponent<AudioSource>().clip = ad[2];
            this.GetComponent<AudioSource>().Play();
        }
        else
        {
            this.GetComponent<AudioSource>().clip = ad[3];
            this.GetComponent<AudioSource>().Play();
        }
    }

    //플레이어 충돌 함수
    public void GetCollision(Color unitColor)
    {
        Color oriColor = transform.GetComponent<SpriteRenderer>().color;
        GameObject scoreUI = GameObject.Find("score");
        GameObject comboUI = GameObject.Find("combo");

        StartCoroutine(ParticleEffect(oriColor, unitColor));   // 파티클 효과
        // 충돌시 장애물 색깔에 따른 효과
        if (oriColor != unitColor)   
        {
            CollisionSound(true);   //충돌 사운드 효과
            GrowPlayer(true);       // 충돌시 크기변화
            StartCoroutine(glitter(oriColor,unitColor));   // 충돌시 플레이어 반짝임
            combo++;                                   //충돌시 콤보, 점수변화
            if (combo > maxcombo)
            {
                maxcombo = combo;
            }
            score = score + 300 + combo * 100;
            if (hp < 100)                              //충돌시 체력변화
            {
                HillDamage(20);           
            }
        }
        else
        {
            CollisionSound(false);           //충돌 사운드 효과
            GrowPlayer(false);               // 충돌시 크기변화
           
            TakeDamage(20);                //충돌시 체력변화
            combo = 0;                     //충돌시 콤보, 점수변화
            score += 100;
            if(hp<=0)                      //체력이 0일경우 게임종료
            {
                maxcombo = 0;
                score = 0;
                SceneManager.LoadScene("ClearScene");
            }
            
        }
        scoreUI.GetComponent<Text>().text = score.ToString();
        comboUI.GetComponent<Text>().text = combo.ToString();
    }


    //충돌시 반짝이는 효과
    public IEnumerator glitter(Color oriColor,Color unitColor)
    {
        int gliterNum = GLITER;
        
        while(gliterNum > 0)
        {
            gliterNum--;
            transform.GetComponent<SpriteRenderer>().color = unitColor;
            yield return new WaitForSeconds(GLITERTERM);
            transform.GetComponent<SpriteRenderer>().color = oriColor;
            yield return new WaitForSeconds(GLITERTERM);
        }
        transform.GetComponent<SpriteRenderer>().color = unitColor;
    }

    //마우스 클릭시 클릭한곳 표시 효과

    public IEnumerator mouseEffect(GameObject pf, Color oriColor,Vector3 eposition)
    {
        GameObject unit = Instantiate(pf, new Vector3(eposition.x, eposition.y, 0.0f), Quaternion.identity);
        unit.GetComponent<SpriteRenderer>().color = new Color(oriColor.r, oriColor.g, oriColor.b, 1.0f);
        yield return new WaitForSeconds(MOUSETERM);
        Destroy(unit);
    }

    // 충돌시 생성되는 파티클 효과
    public IEnumerator ParticleEffect(Color oriColor,Color changedColor)
    {
        ParticleSystem ps = collisionEffect.GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(oriColor, 0.0f), new GradientColorKey(changedColor, 0.5f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        col.color = new ParticleSystem.MinMaxGradient(grad);
        GameObject unit = Instantiate(collisionEffect, new Vector3(transform.position.x, transform.position.y, 0.0f), Quaternion.identity);
        yield return new WaitForSeconds(2.0f);
        Destroy(unit);
    }

    //데미지로 인한 체력 감소 함수
    public void TakeDamage(int damage)
    {
        hp -= damage;
        // Now is where you will want to update the Simple Health Bar. Only AFTER the value has been modified.
        hpbar.UpdateBar(hp, 100);
    }

    //데미지로 인한 체력 회복 함수
    public void HillDamage(int hill)
    {
        hp += hill;
        // Now is where you will want to update the Simple Health Bar. Only AFTER the value has been modified.
        hpbar.UpdateBar(hp, 100);
    }
}
