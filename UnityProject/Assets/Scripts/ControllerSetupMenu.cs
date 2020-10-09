using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

/// <summary>
/// Used to manage the controller setup menu.
/// </summary>
public class ControllerSetupMenu : MonoBehaviour
{
    /// <summary>
    /// The default message to be shown in each status box when a player is not in its slot
    /// </summary>
    private const string DefaultMessage = "Press A to join...";
    /// <summary>
    /// A list containing all the status boxes that are to be used to indicate how many players are connected
    /// </summary>
    public Text[] StatusBoxes = new Text[GameNFO.PlayerLimit];

    private void Start()
    {
        // Make sure there is an indicator available for each player, or disable this script.
        if (StatusBoxes.Length != GameNFO.PlayerLimit || StatusBoxes.Any(x => x == null)) 
        { 
            Debug.LogError($"{GameNFO.PlayerLimit} status boxes must be provided! This script will now be disabled.");
            enabled = false;
            return;
        }
        // Add an event listener to detect when a controller is removed
        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
    }

    private void Update()
    {
        int joinedPlayers = GameNFO.PlayerControllers.Count(x=>x!=null);

        // Check that there are player slots free
        if (joinedPlayers < GameNFO.PlayerLimit) {
            ReadOnlyArray<Gamepad> pads = Gamepad.all;
            // Check each pad to see if they have pressed the A button on this frame
            for (int p = 0; p < pads.Count; ++p)
            {
                Gamepad pad = pads[p];
                if (pad.aButton.wasPressedThisFrame && GameNFO.PlayerControllers.Count(x=>x?.deviceId == pad.deviceId) == 0)
                {
                    // Figure out what slot is free
                    int nextFree = Array.IndexOf(GameNFO.PlayerControllers, null);
                    GameNFO.PlayerControllers[nextFree] = pad;
                    // Update the corresponding status box
                    StatusBoxes[nextFree].text = $"(ID:{pad.deviceId}) {pad.displayName}";
                }
            }
        }

        if (GameNFO.PlayerControllers[0] != null)
        {
            Gamepad player1Pad = GameNFO.PlayerControllers[0];
            if (player1Pad.startButton.wasPressedThisFrame)
            {
                // TODO: Fire off an event here. (e.g. start the game from the menu, or resume the game after a controller disconnects)
                Debug.Log("Event fired!");
            }
        }
    }

    private void InputSystem_onDeviceChange(InputDevice device, InputDeviceChange action)
    {
        if (action == InputDeviceChange.Removed)
        {
            // "x => x?.device == device" explained:
            // "x => <function>" is a lambda expression, essentially a small function called as a parameter within a method
            //
            // In this example, the parameter is of type Func<Gamepad, bool>, which means it needs a parameter that is of type Gamepad (thats 'x'!)
            // and it must return a boolean (hence, ==)
            //
            // "x?.device" gives the value of 'x.device' if 'x != null', otherwise it will simply be null. Since "null == device" will never be true, this works. 
            if (GameNFO.PlayerControllers.Count(x => x?.device == device) > 0)
            {
                Gamepad pad = GameNFO.PlayerControllers.First(x => x?.device == device); // Don't forget to do it again here!!!
                int index = Array.IndexOf(GameNFO.PlayerControllers, pad);
                StatusBoxes[index].text = DefaultMessage;
                GameNFO.PlayerControllers[index] = null;
            }
        }
    }
}
