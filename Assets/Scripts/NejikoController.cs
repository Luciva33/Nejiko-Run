using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NejikoController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public float speedZ;
    public float speedJump;

    void Start()
    {
        //必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //接地しているかの判定
        if (controller.isGrounded)
        {
            //前進プロパティの設定
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }

            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);
            // moveDirection.x = Input.GetAxis("Horizontal") * speedZ;

            //ジャンプ処理
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump;
                animator.SetTrigger("jump");
            }
        }

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
}
