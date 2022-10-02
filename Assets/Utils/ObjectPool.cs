using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class ObjectPool<T, O>
    {
        private List<T> _freeObjects;
        private HashSet<T> _usedObjects;

        private Func<O, T> _getFunc;
        private Func<O> _createFunc;
        private Action<T> _freeFunc;
        
        public ObjectPool(Func<O, T> getFunc, Func<O> createFunc, Action<T> freeFunc)
        {
            _freeObjects = new List<T>();
            _usedObjects = new HashSet<T>();
            _getFunc = getFunc;
            _createFunc = createFunc;
            _freeFunc = freeFunc;
        }

        public T GetObject()
        {
            if (_freeObjects.Count <= 0)
                _freeObjects.Add(_getFunc(_createFunc()));

            var obj = _freeObjects.ExtractAt(0);
            _usedObjects.Add(obj);
            return obj;
        }

        public void FreeObject(T obj)
        {
            if (!_usedObjects.Contains(obj))
                Debug.LogError("Obj not found!");

            _usedObjects.Remove(obj);
            _freeObjects.Add(obj);
            _freeFunc(obj);
        }

        public void FreeAll()
        {
            while(_usedObjects.Count > 0)
                FreeObject(_usedObjects.ElementAt(0));
        }
    }
}
