using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpChangeColor : BasePowerUp
{
    protected override bool ApplyToPlayer(Player thePickerUpper)
    {
        thePickerUpper.playerColor.Value = Color.red;
        return true;
    }
}
