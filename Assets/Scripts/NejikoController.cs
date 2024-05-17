using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;

    public float gravity;
    public float speedZ;
    public float speedX;        //横方向スピード
    public float speedJump;
    public float accelerationZ; //前進加速度

    void Start()
    {
        //必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown("left")) MoveToleft();
        if (Input.GetKeyDown("right")) MoveToRight();
        if (Input.GetKeyDown("space")) Jump();
        if (Input.GetKey("s")) Stop();

        //徐々に加速し、Z方向に常に前進
        float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
        //Mathf.ClampでsppedZ以上の加速制限をかけている
        moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);
        // Debug.Log(acceleratedZ);

        //x方向は目標のポジションまでの差分の配合で速度を計算
        float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
        moveDirection.x = ratioX * speedX;


        // //接地しているかの判定
        // if (controller.isGrounded)
        // {
        //     //前進プロパティの設定
        //     if (Input.GetAxis("Vertical") > 0.0f)
        //     {
        //         moveDirection.z = Input.GetAxis("Vertical") * speedZ;
        //     }
        //     else
        //     {
        //         moveDirection.z = 0;
        //     }

        //     transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);
        //     // moveDirection.x = Input.GetAxis("Horizontal") * speedZ;

        //     //ジャンプ処理
        //     if (Input.GetButton("Jump"))
        //     {
        //         moveDirection.y = speedJump;
        //         animator.SetTrigger("jump");
        //     }
        // }

        //重力分の毎フレーム追加 重力加算　ジャンプしたあとの処理として徐々に
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        //ここを変えないと、ネジコの向きを考慮しない　globalDirectionをしないとどこの向きをみていてもZ軸に進む
        //ネジコの向きを考慮したベクトルに変換
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地してたらY方向の速度はリセットする
        //Y軸下に重力加算しているので、地面から落ちた時、すごい速度で落ちていく
        if (controller.isGrounded) moveDirection.y = 0;

        //速度が0以上なら走っているフラグをtrueにする. IdleとRunアニメーションの制御
        animator.SetBool("run", moveDirection.z > 0.0f);
    }

    //左のレーンに移動を開始
    public void MoveToleft()
    {
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    //右のレーンに移動を開始
    public void MoveToRight()
    {
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;
            animator.SetTrigger("jump");
        }
    }
    public void Stop()
    {
        Debug.Log("On");
        moveDirection.z = 0;


    }

}
