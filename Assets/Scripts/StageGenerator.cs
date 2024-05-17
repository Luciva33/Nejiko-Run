using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30;

    //作成済みチップの最大値　
    int CurrentChipIndex;

    //ターゲットキャラクターの指定
    public Transform character;
    //ステージチッププレファブの設定
    //この配列の中から生成されるステージチップがランダムに選ばれる
    public GameObject[] stageChips;
    //自動生成開始インデックス
    public int startChipIndex;
    //生成先読み個数
    public int preInstantiate;
    //生成済みステージチップ保持リスト
    public List<GameObject> generatedStageList = new List<GameObject>();


    //初期化処理
    void Start()
    {
        //現在0
        CurrentChipIndex = startChipIndex - 1;
        //デフォルトで5の値を入れている
        UpdateStage(preInstantiate);

    }

    //ステージの更新タイミングの監視
    void Update()
    {
        //キャラクターの位置から現在のステージチップのインデックスを計算 Z軸で計算している
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        //次のステージチップに入ったらステージの更新処理を行う
        if (charaPositionIndex + preInstantiate > CurrentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    //ステージの更新処理
    //指定のIndexまでのステージチップを生成して、監視下に置く
    void UpdateStage(int toChipIndex)
    {
        if (toChipIndex <= CurrentChipIndex) return;

        //次のステージチップまでを作成
        for (int i = CurrentChipIndex + 1; i <= toChipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            //生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);

        }

        //ステージ保持上限内になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldStage();

        CurrentChipIndex = toChipIndex;
    }

    //ステージの生成処理。指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, 0, chipIndex * StageChipSize),
            Quaternion.identity
        );
        return stageObject;
    }

    //ステージの削除処理。一番古いオブジェクトを破壊
    void DestroyOldStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0); //リストからの削除
        Destroy(oldStage);
    }


}
