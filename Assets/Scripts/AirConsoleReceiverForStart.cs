using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AirConsoleReceiverForStart : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject pg1;
    public GameObject highlight1;
    public GameObject pg2;
    public GameObject highlight2;

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;

        pg1.SetActive(false);
        pg2.SetActive(false);
        highlight1.SetActive(false);
        highlight2.SetActive(false);
}

    private void OnConnect(int device_id)
    {
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
            {
                AirConsole.instance.SetActivePlayers(2);
                Debug.Log("We have enough players!");
            }
            else
            {
                Debug.Log(AirConsole.instance.GetControllerDeviceIds().ToArray());
                Debug.Log("id arrays: " + string.Join(",", AirConsole.instance.GetControllerDeviceIds().ToArray()));
                Debug.Log("We need more players");
            }
        }
    }

    private void OnDisconnect(int device_id)
    {
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        if (active_player != -1)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
            {
                AirConsole.instance.SetActivePlayers(2);
            }
            else
            {
                AirConsole.instance.SetActivePlayers(0);
            }
        }
    }

    private void OnMessage(int fromDeviceID, JToken data)
    {
        // Moving in four directions
        if (data["dpad"] != null && data["dpad"]["directionchange"]!=null && data["dpad"]["directionchange"]["pressed"].ToObject<bool>())
        {
            int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(fromDeviceID);
            switch (data["dpad"]["directionchange"]["key"].ToString())
            {
                
                case "right":
                    if (active_player != -1)
                    {
                        StartCoroutine(GoToNextPageTutorial());
                    }
                    Debug.Log("RIGHT");
                    break;
                default:
                    break;
            }
        }
           
        // Menu buttons
        // Start game
        if (data["start"] != null && data["start"].ToString() == "up")
        {
            pg1.SetActive(true);
        }
    }

    IEnumerator GoToNextPageTutorial()
    {
        if (pg1.activeSelf)
        {
            highlight1.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            pg1.SetActive(false);
            pg2.SetActive(true);
        }
        else
        {
            highlight2.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            audioSource.Stop();
            SceneManager.LoadScene(1);
        }
    }

    private void OnDestroy()
    {
        if (AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
        }
    }
}
