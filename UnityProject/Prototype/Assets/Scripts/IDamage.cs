using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void takeDamage(float amount, bool slowFlash = false, bool isMushroom = false);
}
