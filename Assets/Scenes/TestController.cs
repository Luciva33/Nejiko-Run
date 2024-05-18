using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


public class TestController : MonoBehaviour
{
    public Image img;
    public Transform obj;
    void Start()
    {
        // img.DOColor(Color.red, 1.5f);
        // transform.DOMove(new Vector3(10f, 0, 0), 2f)
        // .SetEase(Ease.Linear);
        //回転 コルーチンと違い終わった後の指定ができる

        //ターゲットと同じ向きにする
        // transform.DOLocalRotate(new Vector3(0, 180f, 0), 1f);
        // transform.DOLookAt(obj.localPosition, 1f)
        // .SetDelay(2f);

        //     transform.DOLocalMove(new Vector3(10f, 0, 0), 1f)
        //     .SetLoops(-1, LoopType.Yoyo)
        //     .SetEase(Ease.InOutQuart);
        img.DOFade(0.3f, 1f)
        .SetLoops(-1, LoopType.Yoyo);

        DOTween.Sequence()
            .Append(transform.DOLocalMoveX(10f, 1f))
            .Append(transform.DOLocalMoveY(1f, 5f))
            .Append(transform.DOLocalMoveZ(5f, 1f))
            .Append(transform.DOScale(3.5f, 0.3f));
    }

    void Update()
    {

    }
}
