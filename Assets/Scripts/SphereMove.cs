using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SphereMove : MonoBehaviour
{
    [SerializeField, Range(1.0f, 10.0f)] float timeSpan = 1.0f;
    //[SerializeField, Range(1.0f, 100.0f)] float moveSpeed = 10.0f;

    private Rigidbody rb;
    private float currentTime = 0.0f;

    public int envy;
    public int grid;
    public int pride;
    public int wrath;
    public int gluttony;
    public int sloth;
    public int lust;

    public int life;
    public int token;
    private int sex;

    private int age;

    public void InitStatus()
    {
        rb = GetComponent<Rigidbody>();

        // 初期パラメータ設定
        envy = Random.Range(0, 10);
        grid = Random.Range(0, 10);
        pride = Random.Range(0, 10);
        wrath = Random.Range(0, 10);
        gluttony = Random.Range(0, 10);
        sloth = Random.Range(0, 10);
        lust = Random.Range(0, 10);

        life = Random.Range(0, 100);
        token = Random.Range(0, 100);
        sex = Random.Range(0, 1);

        age = 0;
    }


    void Update()
    {
        // ランダムな方向に力を加えて動かす
        //float x = Random.Range(-5.0f, 5.0f);
        //float y = Random.Range(-5.0f, 5.0f);
        //float z = Random.Range(-5.0f, 5.0f);
        //var movement = new Vector3(x, y, z);
        //rb.AddForce(movement);

        // timeSpan 毎にこれより以下の処理を実行
        currentTime += Time.deltaTime;
        if (currentTime < timeSpan)
        {
            return;
        }
        currentTime = 0.0f;

        // 生存のために life を消費する
        life--;
        age++;

        // -10 以下まで落下したら死ぬ
        if (this.gameObject.transform.position.y <= -10)
        {
            Destroy(this.gameObject);
        }

        // life が 0 以下になったら死ぬ
        if (life <= 0)
        {
            Destroy(this.gameObject);
        }

        lust += 10;

        // 行動指針を決める (ルーレット選択)
        var enemyActionProbs = new Dictionary<Action, int>()
        {
            { EnvyMove, envy },
            { GridMove, grid },
            { PrideMove, pride },
            { WrathMove, wrath },
            { GluttonyMove, gluttony },
            { SlothMove, sloth },
            { LustMove, lust },
        };
        var move = enemyActionProbs.GetByRouletteSelection();
        move();
        // Debug.Log(enemyActionProbs.GetByRouletteSelection());
    }

    // 嫉妬行動：目についた Gene の token が自分より多ければ、 envy 分だけ奪う
    void EnvyMove()
    {
        Ray ray = new Ray(this.transform.position, Vector3.forward);
        if (!Physics.Raycast(ray, out var target, 100.0f)) return; // hit してなかったら return

        // 目標が Gene でなければ動作を終了する
        if (!target.transform.CompareTag("Gene"))
        {
            return;
        }

        // target の元まで移動する
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
        rb.AddForce(target.transform.position);

        // 目標が Gene であれば、 token 量を比較して相手が多ければ略奪する。少なければ何もしない
        var targetClass = target.transform.GetComponent<SphereMove>();
        if (token <= targetClass.token)
        {
            // 相手の token が少なすぎて 0 をわる場合は、 0 にする
            if (targetClass.token < envy)
            {
                token += targetClass.token;
                targetClass.token = 0;
            }
            else
            {
                token += envy;
                targetClass.token -= envy;
            }
            // 殴られた相手は負の感情を貯める
            targetClass.envy++;
            targetClass.grid++;
            targetClass.pride++;
            targetClass.wrath++;
            envy++; // envy の選択可能性を増やす
        }
    }

    // 強欲行動：grid 分の life を消費し、 token を生産する (マジメ……)
    void GridMove()
    {
        token += grid;
        life -= grid;
        grid++;
    }

    // 傲慢行動：目についた Gene の token が自分より少なければ、 pride 分だけ奪う
    void PrideMove()
    {
        Ray ray = new Ray(this.transform.position, Vector3.forward);
        if (!Physics.Raycast(ray, out var target, 100.0f)) return; // hit してなかったら return

        // 目標が Gene でなければ動作を終了する
        if (!target.transform.CompareTag("Gene"))
        {
            return;
        }

        // target の元まで移動する
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
        rb.AddForce(target.transform.position);

        // 目標が Gene であれば、 token 量を比較して相手が多ければ略奪する。少なければ何もしない
        var targetClass = target.transform.GetComponent<SphereMove>();
        if (token > targetClass.token)
        {
            // 相手の token が少なすぎて 0 をわる場合は、 0 にする
            if (targetClass.token < pride)
            {
                token += targetClass.token;
                targetClass.token = 0;
            }
            else
            {
                token += pride;
                targetClass.token -= pride;
            }
            // 殴られた相手は負の感情を貯める
            targetClass.envy++;
            targetClass.grid++;
            targetClass.pride++;
            targetClass.wrath++;
            pride++; // pride の選択可能性を増やす
        }
    }

    // 憤怒行動：目についた Gene に wrath 分だけダメージを与えて life を奪う
    void WrathMove()
    {
        Ray ray = new Ray(this.transform.position, Vector3.forward);
        if (!Physics.Raycast(ray, out var target, 100.0f)) return; // hit してなかったら return

        // 目標が Gene でなければ動作を終了する
        if (!target.transform.CompareTag("Gene"))
        {
            return;
        }

        // target の元まで移動する
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
        rb.AddForce(target.transform.position);

        // 目標が Gene であれば、ぶっ殺して life を奪う
        var targetClass = target.transform.GetComponent<SphereMove>();
        // 相手の life が少なすぎて 0 をわる場合は、 0 にする
        if (targetClass.life < wrath)
        {
            life += targetClass.life;
            targetClass.life = 0;
        }
        else
        {
            life += wrath;
            targetClass.life -= wrath;
        }
        // 殴られた相手は負の感情を貯める
        targetClass.envy++;
        targetClass.grid++;
        targetClass.pride++;
        targetClass.wrath++;
        wrath++; // wrath の選択可能性を増やす
    }

    // 貪食行動：gluttony 分の token を消費して life に変える
    void GluttonyMove()
    {
        // token が少なすぎて 0 をわる場合は、 0 にする
        if (token < gluttony)
        {
            life += token;
            token = 0;
        }
        else
        {
            life += gluttony;
            token -= gluttony;
        }
        gluttony++; // gluttony の選択可能性を増やす
    }

    // 怠惰行動：何もしない
    void SlothMove()
    {
        sloth++; // sloth の選択可能性を増やす
    }

    // 色欲行動：目についた Gene と交配して子を産む
    void LustMove()
    {
        Ray ray = new Ray(this.transform.position, Vector3.forward);
        if(!Physics.Raycast(ray, out var target, 100.0f)) return; // hit してなかったら return

        // 目標が Gene でなければ動作を終了する
        if (!target.transform.CompareTag("Gene"))
        {
            return;
        }

        // target の元まで移動する
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
        rb.AddForce(target.point);
        var targetClass = target.transform.GetComponent<SphereMove>();

        // 目標が同性であったら動作を終了する
        //if (targetClass.sex == sex)
        //{
        //    return;
        //}

        // 子を作成する
        GameObject obj = (GameObject)Resources.Load("Gene");
        // 向きはランダムで設定
        var randomQ = Random.rotation;
        // プレハブ (子) を生成
        GameObject lustChild = Instantiate(obj, transform.position, randomQ);

        // 各々のパラメータを半々の確立で受け継いだ子供を作る
        var lustChildClass = lustChild.transform.GetComponent<SphereMove>();

        lustChildClass.envy = (Random.Range(1, 10) < 5) ? targetClass.envy : envy;
        lustChildClass.grid = (Random.Range(1, 10) < 5) ? targetClass.grid : grid;
        lustChildClass.pride = (Random.Range(1, 10) < 5) ? targetClass.pride : pride;
        lustChildClass.wrath = (Random.Range(1, 10) < 5) ? targetClass.wrath : wrath;
        lustChildClass.gluttony = (Random.Range(1, 10) < 5) ? targetClass.gluttony : gluttony;
        lustChildClass.sloth = (Random.Range(1, 10) < 5) ? targetClass.sloth : sloth;
        lustChildClass.lust = (Random.Range(1, 10) < 5) ? targetClass.lust : lust;

        lustChildClass.life = (int)((targetClass.life / 2) + (life / 2));
        lustChildClass.token = (int)((targetClass.token / 2) + (token / 2));
        lustChildClass.sex = Random.Range(0, 1);

        lust++; // lust の選択可能性を増やす
    }
}