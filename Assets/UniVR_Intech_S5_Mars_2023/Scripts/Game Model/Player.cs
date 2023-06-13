using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public SyncedInt gold;
    private Inventory inventory;

    private void Start()
    {
        inventory = new Inventory();
    }
}
