using GameNetcodeStuff;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Wheel_of_Doom.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    
    internal class WheelOfDoomPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void TeleportTriggerPatch()
        {
            // Check for F2 key press
            if (Keyboard.current.f2Key.isPressed)
            {
                PlayerControllerB playerControllerB = null;
                GameObject[] allPlayerObjects = StartOfRound.Instance.allPlayerObjects;

                // Teleport position, random inside facility
                Vector3 teleportPos = RoundManager.Instance.insideAINodes[UnityEngine.Random.Range(0, RoundManager.Instance.insideAINodes.Length)].transform.position;

                foreach (GameObject gameObject in allPlayerObjects)
                {
                    // Find the PlayerControllerB component
                    PlayerControllerB component = gameObject.GetComponent<PlayerControllerB>();

                    // Assign the localPlayerController as target
                    if (component == GameNetworkManager.Instance.localPlayerController)
                    {
                        playerControllerB = component;
                        break;
                    }
                }

                // Perform teleport actions if playerControllerB is found
                if (playerControllerB != null && ItemIsKey(playerControllerB))
                {
                    if ((bool)UnityEngine.Object.FindObjectOfType<AudioReverbPresets>())
                    {
                        UnityEngine.Object.FindObjectOfType<AudioReverbPresets>().audioPresets[2]
                            .ChangeAudioReverbForPlayer(playerControllerB);
                    }

                    playerControllerB.isInElevator = false;
                    playerControllerB.isInHangarShipRoom = false;
                    playerControllerB.isInsideFactory = true;
                    playerControllerB.averageVelocity = 0f;
                    playerControllerB.velocityLastFrame = Vector3.zero;
                    playerControllerB.TeleportPlayer(teleportPos);
                    playerControllerB.beamOutParticle.Play();

                    if (playerControllerB == GameNetworkManager.Instance.localPlayerController)
                    {
                        HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
                    }
                }
            }
        }

        public static Boolean ItemIsKey(PlayerControllerB playerController)
        {
            KeyItem itemInHand = GetCurrentlySelectedKey(playerController);
                if (itemInHand)
                {
                    itemInHand.DestroyObjectInHand(playerController);
                    return true;
                }
            return false;
        }

        public static KeyItem GetCurrentlySelectedKey(PlayerControllerB playerController)
        {
            return ((((object)playerController != null) ? playerController.ItemSlots[playerController.currentItemSlot] : null) as KeyItem);
        }

    }
}
