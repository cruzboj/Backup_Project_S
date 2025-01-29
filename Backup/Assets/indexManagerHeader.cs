using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class indexManagerHeader
{
    static int counter = 0;
    public static int getindex()
    {
        return ++counter;
    }
}
