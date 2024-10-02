using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestA
{
    protected internal int a;
}

public class TestB
{
    internal int b;
    public TestB()
    {
        TestA test = new TestA();
        test.a = 100;
    }
}
