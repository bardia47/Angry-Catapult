using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationUtils
{
 public static bool   isPhoneMode() {
        return Application.isMobilePlatform || UnityEngine.Device.Application.isMobilePlatform;
        
        //  return
    }
}
