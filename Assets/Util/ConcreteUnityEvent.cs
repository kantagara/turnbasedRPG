﻿using UnityEngine.Events;

namespace Util
{
    public interface IConcreteUnityEvent { }

    public class ConcreteUnityEvent : UnityEvent, IConcreteUnityEvent { }

    public class ConcreteUnityEvent<T> : UnityEvent<T>, IConcreteUnityEvent { }

    public class ConcreteUnityEvent<T0, T1> : UnityEvent<T0, T1>, IConcreteUnityEvent { }

    public class ConcreteUnityEvent<T0, T1, T2> : UnityEvent<T0, T1, T2>, IConcreteUnityEvent { }
}