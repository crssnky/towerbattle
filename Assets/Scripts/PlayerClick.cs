using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClick:MonoBehaviour {
    public GameObject[] prefabs;
    public Button RotateButton, RotateButton2;
    int count = 0;
    GameObject currentObj = null;
    bool readyObj = false;
    bool end = true;

    void Start() {
        GameObject.Find("SpreadSheetSetting").GetComponent<GameManaging>().setRotateButton(RotateButton, RotateButton2);

        StartCoroutine(FirstObject());
    }

    IEnumerator FirstObject() {
        yield return new WaitForSeconds(1.0f);
        NextObject();
    }

    void Update() {
        if (readyObj) {
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -3) {
                if (Input.GetMouseButtonUp(0)) {
                    var rdb = currentObj.GetComponent<Rigidbody2D>();
                    rdb.gravityScale = 0.5f;
                    readyObj = false;
                } else if (Input.GetMouseButton(0)) {
                    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    currentObj.transform.position = new Vector3(mousePos.x, currentObj.transform.position.y, currentObj.transform.position.z);
                }
            }
        } else if (currentObj != null) {
            if (currentObj.GetComponent<Rigidbody2D>().IsSleeping()) {
                if (Vector3.Distance(transform.position, currentObj.transform.position) < 2) {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + transform.position.y - currentObj.transform.position.y, Camera.main.transform.position.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y * 2.0f - currentObj.transform.position.y, transform.position.z);
                }
                NextObject();
            }
        }
    }

    void NextObject() {
        Vector2 spawnPos = gameObject.transform.position;
        var spawnObj = prefabs[Random.Range(0, prefabs.Length)];
        currentObj = Instantiate(spawnObj, spawnPos, spawnObj.transform.rotation) as GameObject;
        currentObj.name = (count++).ToString();
        currentObj.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        //currentObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        var rdb = currentObj.GetComponent<Rigidbody2D>();
        rdb.mass = 50;
        rdb.gravityScale = 0;
        rdb.drag = 0.5f;

        readyObj = true;
    }

    public void RotateObj() {
        currentObj.transform.Rotate(new Vector3(0, 0, 30));
    }
    public void RotateObj2() {
        currentObj.transform.Rotate(new Vector3(0, 0, -30));
    }

    public void EndGame() {
        if (end) {
            GameObject.Find("SpreadSheetSetting").GetComponent<GameManaging>().EndGame(count);
            end = false;
        }
    }
}
