using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public float textReadingTime = 5f;
    public Image phoneScreen;
    public TextMeshProUGUI time;
    public GameObject chatPrefab;
    public List<string> dialogs = new List<string>();
    public List<float> waitTime = new List<float>();

    float curTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        checkDialogLength();
        StartCoroutine(UberStartWaiting());
    }

    private void Update()
    {
        curTime += Time.deltaTime;
    }

    private void checkDialogLength()
    {
        if (dialogs.Count != waitTime.Count)
        {
            Debug.LogError("Dialogs does not have the same length as waitTime!");
        }
    }

    IEnumerator UberStartWaiting()
    {
        for (int i = 0; i < dialogs.Count; i++)
        {
            yield return new WaitForSeconds(waitTime[i]);

            // Change the time
            if (curTime > 600)
            {
                time.text = "2:" + (int)Mathf.Floor(curTime / 60f);
            }
            else
            {
                time.text = "2:0" + (int)Mathf.Floor(curTime / 60f);
            }

            // Get phone up
            GetComponent<AudioSource>().Play();
            phoneScreen.gameObject.GetComponent<Animator>().SetBool("show", true);
            yield return new WaitForSeconds(0.5f);

            

            // Show the new text
            GameObject newChat = Instantiate(chatPrefab, phoneScreen.rectTransform.anchoredPosition, Quaternion.identity);
            newChat.transform.parent = phoneScreen.transform;
            newChat.GetComponent<RectTransform>().position = new Vector2(phoneScreen.rectTransform.position.x + 97, phoneScreen.rectTransform.position.y -330 - 15 * i);
            newChat.GetComponentInChildren<TextMeshProUGUI>().text = dialogs[i];

            // Put phone down
            yield return new WaitForSeconds(textReadingTime);
            phoneScreen.gameObject.GetComponent<Animator>().SetBool("show", false);
        }
    }
}
