
using System;
using System.Reflection;

namespace MidiPlayer {
    /// <summary>
    /// PrivateObject for MSTest
    /// </summary>
    public class PrivateObject {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields [nouns, noun phrases]

        private readonly object obj;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public PrivateObject(object obj) {
            this.obj = obj;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb, verb phrases]

        public object Invoke(string methodName, params object[] args) {
            var _type = obj.GetType();
            var _bindingFlags = BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance;
            try {
                return _type.InvokeMember(methodName, _bindingFlags, null, obj, args);
            } catch (Exception ex) {
                throw ex.InnerException;
            }
        }

        public object Invoke(string methodName) {
            var _type = obj.GetType();
            var _bindingFlags = BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance;
            try {
                return _type.InvokeMember(methodName, _bindingFlags, null, obj, null);
            } catch (Exception ex) {
                throw ex.InnerException;
            }
        }
    }
}
