using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedZ;
    public float speedX;        //横方向スピード
    public float speedJump;
    public float accelerationZ; //前進加速度

    //ライフ取得用関数
    public int Life()
    {
        return life;
    }
    //きぜつはんてい

    bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }

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

        //気絶時の行動
        if (IsStun())
        {
            //動きを止め、気絶状態からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {

            //徐々に加速し、Z方向に常に前進
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            //Mathf.ClampでsppedZ以上の加速制限をかけている
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);
            // Debug.Log(acceleratedZ);

            //x方向は目標のポジションまでの差分の配合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }

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
        if (IsStun()) return; //気絶時の入力キャンセル
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    //右のレーンに移動を開始
    public void MoveToRight()
    {
        if (IsStun()) return; //気絶時の入力キャンセル
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump()
    {
        if (IsStun()) return; //気絶時の入力キャンセル
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;
            animator.SetTrigger("jump");
        }
    }
    public void Stop()
    {
        if (IsStun()) return; //気絶時の入力キャンセル
        Debug.Log("On");
        moveDirection.z = 0;
    }

    //衝突判定が生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return;

        //ヒット処理
        if (hit.gameObject.tag == "Robo")
        {
            //ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;

            //ダメージトリガーを認定
            animator.SetTrigger("damage");

            //ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
    }

}
