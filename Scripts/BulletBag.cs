using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 子弹补给包
/// </summary>
public class BulletBag : MonoBehaviour
{
    public int bulletCount = 10;//包里含有的子弹数量

    public ParticleSystem collectEffect;//拾取特效

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if(pc != null)
        {
            if(pc.MyCurBulletCount < pc.MyMaxBulletCount)
            {
                pc.ChangeBulletCount(bulletCount);//增加玩家的子弹数
                Instantiate(collectEffect, transform.position, Quaternion.identity);//添加拾取特效
                Destroy(this.gameObject);
            }
        }
    }
}
