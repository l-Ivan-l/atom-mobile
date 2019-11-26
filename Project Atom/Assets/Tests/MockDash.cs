using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockDash : IDash
{
    public bool CoDashCalled;
   
    public IEnumerator CoDash(int times)
    {
        yield return null;
        CoDashCalled = true;
    }
}
