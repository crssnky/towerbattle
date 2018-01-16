using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushTitle:MonoBehaviour {
    Text push;
    string pushtext;

    void Start() {
        push = GetComponent<Text>();
        pushtext = push.text;
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine() {
        while (true) {
            push.text = push.text == "" ? pushtext : "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
