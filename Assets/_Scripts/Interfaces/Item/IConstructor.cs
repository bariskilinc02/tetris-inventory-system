using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConstructor<T>
{
    public T Construct();
    public T Construct(string id);
}
