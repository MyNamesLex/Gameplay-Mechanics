using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;
using WiimoteApi.Util;
using WiimoteApi.Internal;

public class FindWiiRemotes : MonoBehaviour
{
    public Wiimote remote;

    private void Start()
    {
        InitWiimotes();
    }
    public void InitWiimotes()
    {
        WiimoteManager.FindWiimotes(); // Poll native bluetooth drivers to find Wiimotes

        foreach (Wiimote remote in WiimoteManager.Wiimotes)
        {
            remote.SetupIRCamera(IRDataType.EXTENDED);
            StartCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        while(true)
        {

            foreach (Wiimote remote in WiimoteManager.Wiimotes)
            {
                remote.SendPlayerLED(true, false, false, false);

                yield return new WaitForSeconds(1);

                remote.SendPlayerLED(false, true, false, false);

                yield return new WaitForSeconds(1);

                remote.SendPlayerLED(false, false, true, false);

                yield return new WaitForSeconds(1);

                remote.SendPlayerLED(false, false, false, true);

                StartCoroutine(Flash());
                break;
            }
        }
    }


    public void FinishedWithWiimotes()
    {
        foreach (Wiimote remote in WiimoteManager.Wiimotes)
        {
            remote.SendPlayerLED(false, true, false, false);
            remote.RumbleOn = false;

            WiimoteManager.Cleanup(remote);
        }
    }
}
