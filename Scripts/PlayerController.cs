using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制角色移动、生命、动画等
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;//移动速度

    private int maxHealth = 5;//最大生命值

    private int currentHealth;//当前生命值

    public int MyMaxHealth { get { return maxHealth; } }

    public int MyCurrentHealth { get { return currentHealth; } }

    private float invincibleTime = 2f;//无敌时间2秒

    private float invincibleTimer;//无敌计时器

    private bool isInvincible;//是否处于无敌状态

    public GameObject bulletPrefab;//子弹

    //=====玩家的音效=============================

    public AudioClip hitClip;//受伤音效
    public AudioClip launchClip;//发射齿轮音效

    //=====玩家的朝向=============================
    private Vector2 lookDirection = new Vector2(1, 0);//默认朝向右方

    //=====玩家的子弹数量=========================
    [SerializeField]
    private int maxBulletCount = 99;//最大子弹数量
    private int curBulletCount;//当前子弹数量

    public int MyCurBulletCount { get { return curBulletCount; } }

    public int MyMaxBulletCount { get { return maxBulletCount; } }

    Rigidbody2D rbody;//刚体组件
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 2;
        curBulletCount = 2;//当前子弹数量初始为2
        invincibleTimer = 0;
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        UImanager.instance.UpdatellealthBar(currentHealth, maxHealth);
        UImanager.instance.UpdateBulletCount(curBulletCount, maxBulletCount);
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");//控制水平移动方向 A:-1  D:+1
        float moveY = Input.GetAxisRaw("Vertical");//控制垂直移动方向 W:1  S: -1

        Vector2 moveVector = new Vector2(moveX, moveY);
        if (moveVector.x != 0 || moveVector.y != 0) {
            lookDirection = moveVector;
        }
        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);
        anim.SetFloat("Speed", moveVector.magnitude);

        //=========移动====================================================
        Vector2 position = rbody.position;
        position += moveVector * speed * Time.deltaTime;
        rbody.MovePosition(position);

        //=========无敌计时=================================================
        if (isInvincible) {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0) {
                isInvincible = false;//倒计时结束后(2秒)，取消无敌状态
            }
        }
        //=========按下 J 键 并且子弹数量 >0 进行攻击=======================================
        if (Input.GetKeyDown(KeyCode.J) && curBulletCount > 0) {
            ChangeBulletCount(-1);//每次攻击减 1 个子弹
            anim.SetTrigger("Launch");//播放攻击动画
            AudioManager.instance.AudioPlay(launchClip);//播放攻击音效
            GameObject bullet = Instantiate(bulletPrefab, rbody.position + Vector2.up * 0.5f, Quaternion.identity);
            BulletController bc = bullet.GetComponent<BulletController>();
            if (bc != null) {
                bc.Move(lookDirection, 300);
            }
        }
        //=========按下 E 键 进行NPC互动
        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(rbody.position, lookDirection, 2f, LayerMask.GetMask("NPC"));
            if(hit.collider != null)
            {
                NPCmanager npc = hit.collider.GetComponent<NPCmanager>();
                if (npc != null)
                {
                    npc.ShowDialog();//显示对话框
                }
            }
        }

    }

    /// <summary>
    /// 改变玩家的生命值
    /// </summary>
    public void ChangeHealth(int amount) {
        //如果玩家收到伤害
        if (amount < 0) {
            if (isInvincible == true) {
                return;
            }
            isInvincible = true;
            anim.SetTrigger("Hit");
            AudioManager.instance.AudioPlay(hitClip);//播放受伤音效
            invincibleTimer = invincibleTime;
        }


        Debug.Log(currentHealth + "/" + maxHealth);

        //把玩家的生命值约束在 0 和 最大值 之间
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UImanager.instance.UpdatellealthBar(currentHealth, maxHealth);//更新血条
        Debug.Log(currentHealth + "/" + maxHealth);
    }
    /// <summary>
    /// 改变子弹数量
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeBulletCount(int amount)
    {
        curBulletCount = Mathf.Clamp(curBulletCount + amount, 0, maxBulletCount);//限制子弹数量在0-最大值之间
        UImanager.instance.UpdateBulletCount(curBulletCount, maxBulletCount);
    }
}
