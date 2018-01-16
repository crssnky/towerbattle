﻿using System.Collections;using UnityEngine;using GSSA;using System.Linq;using UnityEngine.UI;using UnityEngine.SceneManagement;public class GameManaging:MonoBehaviour {    string UniqueID;    SpreadSheetQuery query = new SpreadSheetQuery("Ranking");    Text rankText;    Button RotateButton, RotateButton2;    int currentRank = 0;    int clearRank = 0;    int orders = 0;    void Awake() {        DontDestroyOnLoad(gameObject);        UniqueID = SpreadSheetSetting.Instance.UniqueID;        StartCoroutine(InitializeManager());    }    void Start() {        rankText = GameObject.Find("RankText").GetComponent<Text>();    }    IEnumerator InitializeManager() {        query.Where("id", "=", UniqueID);        yield return query.FindAsync();        var result = query.Result.FirstOrDefault();        if (result == null) {            result = new SpreadSheetObject("Ranking");            result["id"] = UniqueID;            result["Rank"] = 0;            rankText.text = "高く積み上げて点数を稼ごう！";            yield return result.SaveAsync();        } else {            Debug.Log("result[\"Rank\"].ToString()=>" + result["Rank"].ToString());            currentRank = int.Parse(result["Rank"].ToString());            rankText.text = "最高点は" + currentRank + "点です。";            query.Where("Rank", ">", currentRank);            yield return query.CountAsync();            orders = (query.Count < 1 ? 1 : query.Count);            rankText.text += "\n順位は" + orders + "位 です。";        }    }    public void GameScene() {        SceneManager.LoadScene("2");    }    public void setRotateButton(Button btn, Button btn2) {        RotateButton = btn;        RotateButton2 = btn2;    }    public void EndGame(int clear) {        RotateButton.gameObject.SetActive(false);        RotateButton2.gameObject.SetActive(false);        clearRank = clear - 1;        if (SceneManager.GetActiveScene().name != "result") {            SceneManager.LoadScene("result", LoadSceneMode.Additive);        }    }    public void setRank(Text text) {        text.text = "得点：" + clearRank + "点！\n(ハイスコア：" + currentRank + ")";        StartCoroutine(rankinging(text));    }    IEnumerator rankinging(Text text) {        if (currentRank < clearRank) {            text.text += "\nハイスコアを更新！";            currentRank = clearRank;            query.Where("id", "=", UniqueID);            yield return query.FindAsync();            var so = query.Result.FirstOrDefault();            if (so != null) {                so["Rank"] = clearRank;                yield return so.SaveAsync();            } else {                so = new SpreadSheetObject("Ranking");                so["id"] = UniqueID;                so["Rank"] = clearRank;                yield return so.SaveAsync();            }        }        query.Where("Rank", ">", clearRank);        yield return query.CountAsync();        orders = (query.Count < 1 ? 1 : query.Count);        text.text += "\n順位は" + orders + "位 です。";    }    public void Tweet_btn() {        //Application.OpenURL("https://twitter.com/intent/tweet?text=「SonicTowerBattle」得点:" + clearRank + "点(ハイスコア:" + currentRank + ",現在第" + orders + "位)&hashtags=sonictowerbattle&url=https%3a%2f%2fcrssnky%2exyz%2fHappyNewYear%2f2018%2fSonicTowerBattle%2f");        Application.OpenURL("https://twitter.com/intent/tweet?text=「DogTowerBattle」得点:" + clearRank + "点(ハイスコア:" + currentRank + ",現在第" + orders + "位)&hashtags=dogtowerbattle&url=https%3a%2f%2fcrssnky%2exyz%2fHappyNewYear%2f2018%2fDogTowerBattle%2f");    }}