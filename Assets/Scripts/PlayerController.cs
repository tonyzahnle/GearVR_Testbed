using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour {
    Player player;
    InputDevice inputDevice;
	// Use this for initialization
	void Start () {
        if (player == null)
            player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        DetectInput();
	}

    void DetectInput()
    {
        if (inputDevice == null)
            if (InputManager.ActiveDevice != null)
                inputDevice = InputManager.ActiveDevice;

        if(inputDevice != null)
        {
            if (inputDevice.AnyButton) Debug.Log(inputDevice.GetControl(InputControlType.Action1).ToString());

            MovePlayer(inputDevice.LeftStick.Vector);
        }


    }

    void MovePlayer(Vector2 dir)
    {
        player.transform.position += new Vector3(dir.x, dir.y, 0);
    }

}
