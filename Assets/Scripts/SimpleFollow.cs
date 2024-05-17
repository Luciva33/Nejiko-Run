using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    Vector3 diff;

    //追従ターゲットプロパティ
    public GameObject target;
    public float followSpeed;

    void Start()
    {
        //追従距離の計算 スタート時のカメラ位置で距離を計算
        diff = target.transform.position - transform.position;
    }

    // LateUpdateは通常のUpdate関数と同様に毎フレーム呼ばれるがすべてのUpdate関数の処理が終わった後に動作する
    void LateUpdate()
    {
        //線形補間関数によるスムージング
        /*
        Lerp関数は、線型補間関数と呼ばれるもの
        線型補間関数は、第一引数と第二引数間の値で、第三引数の0.0~1.0の割合いに相当するものを返す
        現在のポジションと目標のポジションの間の距離を一定の割合で縮めていく処理を行っている
        これにより、距離が離れているほど素早く近づき、近いほどゆっくりちかづくことになって
        カメラが目標物に滑らかに追従する動きを表現できる
        */
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position - diff,
            //followSpeedを1にするとダイレクトカメラになって滑らかな動きがない。
            //followSpeedを遅らせるほど、滑らかに追従する
            Time.deltaTime * followSpeed
        );
    }
}
