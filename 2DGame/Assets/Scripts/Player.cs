﻿using UnityEngine;

public class Player : MonoBehaviour
{
    // 可讀性、維護性、擴充性
    [Header("移動速度"), Range(0, 1000)]
    public float speed = 10.5f;
    [Header("跳躍高度"), Range(0, 3000)]
    public int jump = 100;
    [Header("是否在地板上"), Tooltip("用來儲存玩家是否站在地板上")]
    public bool isGrounded = false;
    [Header("子彈"), Tooltip("存放要生成的子彈預製物")]
    public GameObject bullet;
    [Header("子彈生成點"), Tooltip("子彈要生成的起始位置")]
    public Transform point;
    [Header("子彈速度"), Range(0, 5000)]
    public int speedBullet = 800;
    [Header("開槍音效")]
    public AudioClip soundFire;
    [Header("生命數量"), Range(0, 10)]
    public int live = 3;

    private int score;
    private AudioSource aud;
    private Rigidbody2D rig;
    private Animator ani;

    // 事件：喚醒 - 在 Start 之前執行一次
    private void Awake()
    {
        // 剛體 = 取得元件<剛體元件>()；
        // 抓到角色身上的剛體元件存放到 rig 欄位內
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
        Fire();
    }

    /// <summary>
    /// 移動功能
    /// </summary>
    private void Move()
    {
        // 水平浮點數 = 輸入 的 取得軸向("水平") - 左右AD
        float h = Input.GetAxis("Horizontal");
        // 剛體 的 加速度 = 新 二維向量(水平浮點數 * 速度，剛體的加速度的Y)
        rig.velocity = new Vector2(h * speed, rig.velocity.y);
        // 動畫 的 設定布林值(參數名稱，水平 不等於 零時勾選)
        // != 不等於，傳回布林值
        ani.SetBool("跑步開關", h != 0);

        // KeyCode 列舉(下拉式選單) - 所有輸入的項目 滑鼠、鍵盤、搖桿
        if (Input.GetKeyDown(KeyCode.D))
        {
            // transform 此物件的變形元件
            // eulerAngles 歐拉角度 0 - 180 - 270 - 360...
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    /// <summary>
    /// 跳躍功能
    /// </summary>
    private void Jump()
    {

    }

    /// <summary>
    /// 開槍功能
    /// </summary>
    private void Fire()
    {
        // 按下左鍵之後
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // 音源 的 播放一次音效(音效，隨機大小聲)
            aud.PlayOneShot(soundFire, Random.Range(0.8f, 1.5f));
            // 生成 子彈在槍口
            // 生成(物件，座標，角度)
            GameObject temp = Instantiate(bullet, point.position, point.rotation);
            // 讓子彈飛
            // 上 綠 transform.up
            // 右 紅 transform.right
            // 前 藍 transform.forward
            temp.GetComponent<Rigidbody2D>().AddForce(transform.right * speedBullet + transform.up * 100);
        }
    }

    /// <summary>
    /// 死亡功能
    /// </summary>
    /// <param name="obj">碰到物件的名稱</param>
    private void Dead(string obj)
    {
        // 如果 物件名稱 等於 死亡區域
        // 等於 ==
        if (obj == "死亡區域")
        {
            //this.enabled = false;
            enabled = false;                    // 此腳本 關閉
            ani.SetBool("死亡開關", true);
        }
    }

    // OCE 碰撞時執行一次的事件
    // collision 碰到物件的資訊
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Dead(collision.gameObject.name);
    }
}
