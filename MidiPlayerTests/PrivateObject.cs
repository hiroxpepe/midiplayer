
using System;
using System.Reflection;

namespace MidiPlayer {
    /// <summary>
    /// PrivateObject for MSTest
    /// </summary>
    public class PrivateObject {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        private readonly object _obj;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public PrivateObject(object obj) {
            _obj = obj;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public object Invoke(string methodName, params object[] args) {
            var type = _obj.GetType();
            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance;
            try {
                return type.InvokeMember(methodName, bindingFlags, null, _obj, args);
            } catch (Exception ex) {
                throw ex.InnerException;
            }
        }

        public object Invoke(string methodName) {
            var type = _obj.GetType();
            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance;
            try {
                return type.InvokeMember(methodName, bindingFlags, null, _obj, null);
            } catch (Exception ex) {
                throw ex.InnerException;
            }
        }
    }
}
