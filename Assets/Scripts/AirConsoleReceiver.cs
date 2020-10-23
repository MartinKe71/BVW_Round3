using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AirConsoleReceiver : MonoBehaviour
{
    public MovementController movementCtrl;
    public GameObject pauseScreen;

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
        movementCtrl = GetComponent<MovementController>();
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
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                switch (data["dpad"]["directionchange"]["key"].ToString())
                {
                    case "up":
                        if (active_player != -1)
                        {
                            if (active_player == 0)
                            {
                                movementCtrl.MovePlayer(MovingDirection.front, movementCtrl.front);
                            }
                            if (active_player == 1)
                            {
                                movementCtrl.MovePlayer(MovingDirection.front, movementCtrl.back);
                            }
                        }
                        break;
                    case "down":
                        if (active_player != -1)
                        {
                            if (active_player == 0)
                            {
                                movementCtrl.MovePlayer(MovingDirection.back, movementCtrl.front);
                            }
                            if (active_player == 1)
                            {
                                movementCtrl.MovePlayer(MovingDirection.back, movementCtrl.back);
                            }
                        }
                        Debug.Log("DOWN");
                        break;
                    case "left":
                        if (active_player != -1)
                        {
                            if (active_player == 0)
                            {
                                movementCtrl.MovePlayer(MovingDirection.left, movementCtrl.front);
                            }
                            if (active_player == 1)
                            {
                                movementCtrl.MovePlayer(MovingDirection.left, movementCtrl.back);
                            }
                        }
                        Debug.Log("LEFT");
                        break;
                    case "right":
                        if (active_player != -1)
                        {
                            if (active_player == 0)
                            {
                                movementCtrl.MovePlayer(MovingDirection.right, movementCtrl.front);
                            }
                            if (active_player == 1)
                            {
                                movementCtrl.MovePlayer(MovingDirection.right, movementCtrl.back);
                            }
                        }
                        Debug.Log("RIGHT");
                        break;
                    default:
                        break;
                }
            }
            else
            {

            }
            //int active_player = fromDevice.ID % 2;
            
        }

        // Jump
        if (data["jump"] != null && data["jump"].ToString() == "up")
        {
            int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(fromDeviceID);
            //int active_player = fromDeviceID % 2;
            if (active_player != -1)
            {
                if (active_player == 0)
                {
                    movementCtrl.JumpCheckForAC(movementCtrl.front);
                }
                if (active_player == 1)
                {
                    movementCtrl.JumpCheckForAC(movementCtrl.back);
                }
            }
            Debug.Log("Jump!");
        }

        // Menu buttons
        // Start game
        if (data["start"] != null && data["start"].ToString() == "up")
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        // Pause game
        if (data["pause"] != null && data["pause"].ToString() == "up")
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }
        // Restart game
        if (data["resume"] != null && data["resume"].ToString() == "up")
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
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
