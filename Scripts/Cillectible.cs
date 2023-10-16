using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 草莓被玩家碰撞到时检测的相关类
/// </summary>
public class Cillectible : MonoBehaviour
{
    public ParticleSystem collectEffect;//拾取特效

    public AudioClip collectClip;//拾取音效

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 碰撞检测相关
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other) {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            if (pc.MyCurrentHealth < pc.MyMaxHealth) {
                pc.ChangeHealth(1);
                Instantiate(collectEffect, transform.position, Quaternion.identity);
                AudioManager.instance.AudioPlay(collectClip);//播放音效
                Destroy(this.gameObject);
            }
        }
    }
}
