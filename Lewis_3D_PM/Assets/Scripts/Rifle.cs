using UnityEngine;

public class Rifle : Weapon
{
    [Header("Rifle Stats")]
    public float semiAutoROF = 1;
    public float fullAutoROF = .25f;

    public void changeFireMode()
    {
        if (fireModes >= 2 && canFire)
        {
            currentFireMode++;

            if (currentFireMode >= fireModes)
                currentFireMode = 0;

            if (currentFireMode == 0)
            {
                holdToAttack = false;
                rof = semiAutoROF;
            }

            else if (currentFireMode == 1)
            {
                holdToAttack = true;
                rof = fullAutoROF;
            }
        }
    }
}
