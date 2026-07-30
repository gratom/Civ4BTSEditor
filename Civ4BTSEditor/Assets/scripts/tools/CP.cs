//developer -> gratomov@gmail.com

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// CP (Cached Property) is a generic utility that lazily evaluates a value once and caches it.
    /// Intended for values that behave like constants after being retrieved (e.g., components, managers).
    /// Subsequent accesses return the cached value directly for performance and predictability.
    /// </summary>
    /// Usage example:
    /// <code>
    /// private CP CP&lt;Balance&gt; balance = new CP&lt;Balance&gt;(() => Settings.Instance.GetBalanceSettingCP&lt;Balance&gt;());
    /// </code>
    /// 
    /// Note: Do not use CP&lt;T&gt; for values expected to change or need refreshing.
    /// <typeparam name="T">Type of the cached value</typeparam>
    public class CP<T>
    {
        private readonly Func<T> resolver;
        private T cachedValue;
        private bool isCached;
        
        /// <summary>
        /// Gets the cached value. The first access invokes the resolver, then caches the result.
        /// </summary>
        public T Value
        {
            get
            {
                if (isCached)
                {
                    return cachedValue;
                }
                isCached = true;
                cachedValue = resolver();
#if UNITY_EDITOR
                if (EqualityComparer<T>.Default.Equals(cachedValue, default))
                {
                    Debug.LogWarning($"[CP<{typeof(T).Name}>] Resolved value is null or default.");
                }
#endif
                return cachedValue;
            }
        }

        /// <summary>
        /// Constructor with a resolver function to compute the value.
        /// </summary>
        /// <param name="resolver">Function to retrieve the value initially</param>
        public CP(Func<T> resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            isCached = false;
        }

        /// <summary>
        /// Allows implicit conversion of CP<T> to T.
        /// </summary>
        public static implicit operator T(CP<T> cp)
        {
            return cp.Value;
        }
    }
}