using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Erethan.ScriptableServices
{
    public interface IService
    {
        event Action InitializationComplete;
        event Action FreeComplete;

        void Startup();

        void Free();
    }
}

