// Automatically generated by Interoptopus.

#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using My.Company;
#pragma warning restore 0105

namespace My.Company
{
    public static partial class Interop
    {
        public const string NativeLib = "testing";

        static Interop()
        {
        }


        /// Function using the type.
        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "my_function")]
        public static extern Vec2 my_function(Vec2 input);

    }

    /// A simple type in our FFI layer.
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Vec2
    {
        public float x;
        public float y;
    }

    public static class DeepClone{

        public static T? Clone<T>(T cloneTarget){

            // If null is inputted, return a null object of T
            if (cloneTarget == null)
            {
                return default(T);
            }

            // If the object is serializeable, we can proceed. Otherwise, throw an operation exception.
            if (typeof(T).IsSerializable){
                
                // Grab the type of the object and create an instance of it
                Type targetType = cloneTarget.GetType();
                object? clonedObject = Activator.CreateInstance(targetType);

                // If the clonedObject is null, return the null object of T
                if (clonedObject == null)
                {
                    return default(T);
                }

                // Get each property into a list. Each one will be checked and the value
                //  of the source in each instance will be moved to the clone.
                PropertyInfo[] cloneTargetProperties = targetType.GetProperties(
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance
                );

                // Local variable used incase a string is encountered
                string? clonedString;
                
                foreach (PropertyInfo cloneTargetProperty in cloneTargetProperties){

                    // If we can edit the property, proceed.
                    if (cloneTargetProperty.CanWrite){
                        
                        // If the property is a string, clone and set the string
                        if (cloneTargetProperty.PropertyType == typeof(String))
                        {
                            // Grab the string from the original object
                             clonedString = (string?)cloneTargetProperty.GetValue(cloneTarget, null);

                            // If a string was grabbed, create a new string based on it and assign it to the clone object.
                            if (clonedString != null)
                            {
                                cloneTargetProperty.SetValue(clonedObject, new string(clonedString), null);
                            }
                        }

                        // If the property is a value type (int, bool, double, etc.) or enum, we can set the property without issue.
                        else if (
                            (cloneTargetProperty.PropertyType == typeof(ValueType) ||
                            cloneTargetProperty.PropertyType == typeof(Enum)) ||
                            cloneTargetProperty.PropertyType == typeof(short) ||
                            cloneTargetProperty.PropertyType == typeof(ushort) ||
                            cloneTargetProperty.PropertyType == typeof(int) ||
                            cloneTargetProperty.PropertyType == typeof(uint) ||
                            cloneTargetProperty.PropertyType == typeof(long) ||
                            cloneTargetProperty.PropertyType == typeof(ulong) ||
                            cloneTargetProperty.PropertyType == typeof(char) ||
                            cloneTargetProperty.PropertyType == typeof(float) ||
                            cloneTargetProperty.PropertyType == typeof(double) ||
                            cloneTargetProperty.PropertyType == typeof(bool) ||
                            cloneTargetProperty.PropertyType == typeof(decimal) ||
                            cloneTargetProperty.PropertyType == typeof(byte) ||
                            cloneTargetProperty.PropertyType == typeof(sbyte)
                            )
                        {
                            cloneTargetProperty.SetValue(clonedObject, cloneTargetProperty.GetValue(cloneTarget, null), null);
                        }
                        // If the property is not a value type (i.e. complex), we need to transfer the objects inside it 
                        //  through a recursive call.
                        else{

                            // Grab the value representing the property
                            var cloneTargetPropertyValue = cloneTargetProperty.GetValue(
                                cloneTarget,
                                null
                            );

                            // If nothing was returned, then we assume that the value is empty. Set
                            //  that property as null
                            if (cloneTargetPropertyValue == null){
                                cloneTargetProperty.SetValue(
                                    clonedObject,
                                    null,
                                    null
                                );
                            }
                            // If something was returned, it is in fact complex so we need to recursively call
                            //  and add it's smaller value types to a cloned complex type.
                            else{
                                cloneTargetProperty.SetValue(
                                    clonedObject,
                                    Clone(cloneTargetPropertyValue),
                                    null
                                );
                            }
                        }

                    }
                }

                // Return once all properties have been cloned over
                return (T)clonedObject;
            }
            else{
                throw new InvalidOperationException("Object trying to be cloned is not Serializeable. Make sure the object you are trying to perform a deep clone on is Serializeable.");
            }
 
        }
    }


    public class InteropException<T> : Exception
    {
        public T Error { get; private set; }

        public InteropException(T error): base($"Something went wrong: {error}")
        {
            Error = error;
        }
    }

}
