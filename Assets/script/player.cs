using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    Rigidbody2D rigid;
    SpriteRenderer rend;
    Animator anim;

    bool isJumping = false;
    bool isAttack = false;

    int SpeakCount = 0;
    public int JumpCount = 0;

    public GameObject guide;
    public GameObject speak;
    public GameObject[] AttackEffect;
    public bool Freeze = false;
    public int JumpPower = 5;
    public int Speed = 5;


    private void Awake()
    {
        Instance = this;

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Freeze||GameManager.Instance.Pause.isOn) return;
        
        if (Input.GetButtonDown("Jump")) isJumping = true;

        if (Input.GetButtonDown("Attack")) isAttack = true;

        Jump();

        Attack();

        Move();
    }

    void Jump()
    {
        if (!isJumping||JumpCount >= GameManager.Instance.JumpLimt) { 
            isJumping = false;
            return;
        }
        isJumping = false;

        JumpCount++;

        rigid.velocity = Vector3.zero;

        rigid.AddForce (Vector3.up*JumpPower, ForceMode2D.Impulse);
    }

    void Attack()
    {
        if (!isAttack || GameManager.Instance.AttackDelay > GameManager.Instance.Delay)
        {
            isAttack = false;
            return;
        }

        int LR = (rend.flipX) ? 1 : 0;
        AttackEffect[LR].SetActive(true);
        anim.SetTrigger("isAttack");

        StartCoroutine(delete(0.2f, LR));

        GameManager.Instance.Delay = 0f;
        isAttack = false;
    }

    void Move()
    {
        Vector3 MoveVelocity = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            MoveVelocity = Vector3.left;
            rend.flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            MoveVelocity = Vector3.right;
            rend.flipX = false;
        }

        int slow = 0;

        if (GameManager.Instance.isHard) slow = 2;
        else slow = 0;

        transform.position += MoveVelocity * (Speed - slow) * Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("floor")&& JumpCount != 0) JumpCount = 0;

        if (other.gameObject.CompareTag("Monster") && GameManager.Instance.HitTime <= 0 && !Freeze)
        {
            GameManager.Instance.Hp--;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
            Freeze = true;

            if (GameManager.Instance.Hp <= 0)
            {
                StartCoroutine(Dead());
            }
            else
            {
                anim.SetTrigger("isHit");
                StartCoroutine(Freezecancel(0.6f));
                gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.Hurt;
                GameManager.Instance.HitTime = 2;
            }
        }
        else if ((other.gameObject.name.Equals("lava")|| other.gameObject.name.Equals("Spike"))&& GameManager.Instance.HitTime <= 0)
        {
            GameManager.Instance.Hp--;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

            if (GameManager.Instance.Hp <= 0)
            {
                StartCoroutine(Dead());
            }
            else
            {
                isJumping = true;
                JumpCount = 0;
                gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.Hurt;
                GameManager.Instance.HitTime = 0.5f;
                Jump();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (GameManager.Instance.Pause.isOn)
        {
            Guide(false, 0);
            return;
        }
        if (other.gameObject.name.Equals("hidden")) other.gameObject.GetComponent<Tilemap>().color = new Color(1, 1, 1, 1 / 2f);

        if (guide.activeSelf) return;
        if (other.gameObject.CompareTag("Items")||other.gameObject.CompareTag("Coin")) Guide(true, 0);
        if (other.gameObject.name.Equals("Store") || other.gameObject.name.Equals("enter")) Guide(true, 1);
        if (other.gameObject.name.Equals("exit")) Guide(true, 2);
        if (other.gameObject.name.Equals("door")) Guide(true, 3);
        if (other.gameObject.name.Equals("runed_door")) Guide(true, 4);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Guide(false, 0);
        if (other.gameObject.name.Equals("hidden")) other.gameObject.GetComponent<Tilemap>().color = Color.white;
    }

    public void Guide(bool turning, int i)
    {
        if (!turning) guide.SetActive(false);
        else
        {
            guide.SetActive(true);

            GameObject guide_text = guide.transform.GetChild(0).gameObject;

            switch (i)
            {
                case 0:
                    guide_text.GetComponent<Text>().text = "Press F to get";
                    break;
                case 1:
                    guide_text.GetComponent<Text>().text = "Press F to enter";
                    break;
                case 2:
                    guide_text.GetComponent<Text>().text = "Press F to exit";
                    break;
                case 3:
                    guide_text.GetComponent<Text>().text = "Press F to lock on";
                    break;
                case 4:
                    guide_text.GetComponent<Text>().text = "Press F to solve";
                    break;
            }
        }
    }
    public IEnumerator Speak(int i)
    {
        speak.SetActive(true);
        SpeakCount++;
        if (SpeakCount >= 100) SpeakCount = 0;

        int nowspeak = SpeakCount;

        switch (i)
        {
            case 0:
                speak.GetComponent<Text>().text = "Bag is full";
                break;
            case 1:
                speak.GetComponent<Text>().text = "Bag is too weight";
                break;
            case 2:
                speak.GetComponent<Text>().text = "I don't have hand to hold it";
                break;
            case 3:
                speak.GetComponent<Text>().text = "How?";
                break;
            case 4:
                speak.GetComponent<Text>().text = "This floor have no more tresure.";
                break;
            case 5:
                speak.GetComponent<Text>().text = "Have key no one.";
                break;
            case 6:
                speak.GetComponent<Text>().text = "Wow. taste good."; // æ∆¿Ã≈€ »πµÊΩ√ ¥ÎªÁ
                break;
            case 7:
                speak.GetComponent<Text>().text = "Fresh air.";
                break;
            case 8:
                speak.GetComponent<Text>().text = "There is it.";
                break;
            case 9:
                speak.GetComponent<Text>().text = "Catch me if you can.";
                break;
            case 10:
                speak.GetComponent<Text>().text = "Wait! too fast!";
                break;
            case 11:
                speak.GetComponent<Text>().text = "I'm dark templor.";
                break;
            case 12:
                speak.GetComponent<Text>().text = "Lumos!!!......Just wish list.";
                break;
            case 13:
                speak.GetComponent<Text>().text = "Feel like the moon."; // æ∆¿Ã≈€ »πµÊΩ√ ¥ÎªÁ
                break;
            case 14:
                speak.GetComponent<Text>().text = "I think no more tresure is in this floor.";
                break;
            case 15:
                speak.GetComponent<Text>().text = "Feel like the moon.";
                break;
        }

        yield return new WaitForSeconds(5f);

        if (nowspeak == SpeakCount) {speak.SetActive(false); }
    }
    IEnumerator Freezecancel(float time)
    {
        yield return new WaitForSeconds(time);
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        Freeze = false;
        Debug.Log("∂Ø");
    }

    public IEnumerator Dead()
    {
        Freeze = true;
        rigid.rotation = 90f;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        anim.SetBool("isDead", true);
        GameManager.Instance.selling(false);
        speak.SetActive(false);
        guide.SetActive(false);
        Debug.Log("æÛ¿Ω");
        yield return new WaitForSeconds(5f);

        GameManager.Instance.StageLoad(GameManager.Instance.mapId);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        StartCoroutine(Freezecancel(2f));
        yield return new WaitForSeconds(2f);
        anim.SetBool("isDead", false);
        rigid.rotation = 0;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    IEnumerator delete(float time, int i)
    {
        yield return new WaitForSeconds(time);

        AttackEffect[i].SetActive(false);
    }
}
