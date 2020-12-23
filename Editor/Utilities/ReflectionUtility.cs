using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MuffinDev.MultipleEditors.Utilities.EditorOnly
{

	///<summary>
	/// Contains some utility methods for C# reflection.
	///</summary>
	public static class ReflectionUtility
	{
        
        public const BindingFlags INSTANCE = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        public const BindingFlags STATIC = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public const string UNITY_CSHARP_ASSEMBLY_NAME = "Assembly-CSharp";
        public const string UNITY_EDITOR_CSHARP_ASSEMBLY_NAME = "Assembly-CSharp-Editor";
        private static readonly string[] NOT_PROJECT_ASSEMBLIES =
        {
            "mscorlib",
            "UnityEngine",
            "UnityEditor",
            "Unity.",
            "System",
            "Mono.",
            "netstandard",
            "Microsoft"
        };

        /// <summary>
        /// Gets all the types that implement the given generic type in the given assembly.
        /// NOTE: This method is inspired by this Stack Overflow answer: https://stackoverflow.com/a/8645519/6699339
        /// </summary>
        /// <param name="_GenericType">The type of the generic class you want to find inheritors. Uses the "open generic" syntax. As an
        /// example, if your class is MyGenericClass<T>, use typeof(MyGenericClass<>), without any value inside the less-than/greater-than
        /// characters.</param>
        /// <param name="_Assembly">The assembly where you want to find the given generic type implementations.</param>
        /// <returns>Returns an enumerable that contains all the found types.</returns>
        public static IEnumerable<Type> GetAllTypesImplementingGenericType(Type _GenericType, Assembly _Assembly)
        {
            return from type in _Assembly.GetTypes()
                   let baseType = type.BaseType
                   where baseType != null && baseType.IsGenericType && !type.IsGenericType && _GenericType.IsAssignableFrom(baseType.GetGenericTypeDefinition())
                   select type;
        }

        /// <summary>
        /// Gets an Assembly with the given name.
        /// </summary>
        /// <param name="_AssemblyName">The name of the Assembly you want to get.</param>
        /// <returns>Returns the found assembly, otherwise null.</returns>
        public static Assembly GetAssemblyByName(string _AssemblyName)
        {
            if (string.IsNullOrEmpty(_AssemblyName))
                return null;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == _AssemblyName)
                {
                    return assembly;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the Unity's CSharp assembly, which is the assembly that contains your custom scripts.
        /// </summary>
        public static Assembly GetUnityProjectAssembly()
        {
            return GetAssemblyByName(UNITY_CSHARP_ASSEMBLY_NAME);
        }

        /// <summary>
        /// Gets the Unity's CSharp Editor assembly, which is the assembly that contains your custom scripts in /Editor directories.
        /// </summary>
        public static Assembly GetUnityEditorProjectAssembly()
        {
            return GetAssemblyByName(UNITY_EDITOR_CSHARP_ASSEMBLY_NAME);
        }

        /// <summary>
        /// Gets all the project assemblies, ignoring all native .NET assemblies, UnityEngine assemblies, UnityEditor assemblies, etc.
        /// </summary>
        /// <returns>Returns the found assemblies.</returns>
        public static Assembly[] GetProjectAssemblies()
        {
            List<Assembly> projectAssemblies = new List<Assembly>();
            // For each assembly
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string assemblyName = assembly.GetName().Name;
                bool isProjectAssembly = true;
                // If the assembly starts with one of the "not-project" assemblies, skip it
                foreach(string notProjectAssemblyName in NOT_PROJECT_ASSEMBLIES)
                {
                    if (assemblyName.StartsWith(notProjectAssemblyName))
                    {
                        isProjectAssembly = false;
                        break;
                    }
                }

                // Add the assembly to the list if possible
                if (isProjectAssembly)
                    projectAssemblies.Add(assembly);
            }

            return projectAssemblies.ToArray();
        }
        
        /// <summary>
        /// Gets the value of the named field (member variable) from the given target.
        /// </summary>
        /// <param name="_FieldName">The name of the field you want to get.</param>
        /// <param name="_Target">The object from which you want to get the field.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <typeparam name="TFieldType">The expected type of the field.</typeparam>
        /// <returns>Returns the found field value, or the default type value if the field doesn't exist on the
        /// target.</returns>
        public static TFieldType GetFieldValue<TFieldType>
        (
            string _FieldName,
            object _Target,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            object value = GetFieldValue(_FieldName, _Target, _BindingFlags);
            return value != null ? (TFieldType)value : default;
        }
        
        /// <summary>
        /// Gets the value of the named field (member variable) from the given target.
        /// </summary>
        /// <param name="_FieldName">The name of the field you want to get.</param>
        /// <param name="_Target">The object from which you want to get the field.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <returns>Returns the found field value, or null if the field doesn't exist on the target.</returns>
        public static object GetFieldValue
        (
            string _FieldName,
            object _Target,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            FieldInfo field = _Target.GetType().GetField(_FieldName, _BindingFlags);
            if (field != null)
                return field.GetValue((_BindingFlags & BindingFlags.Static) != 0 ? null : _Target);
            
            Debug.LogWarning($"The field {_FieldName} doesn't exist on the given object's type ({_Target.GetType()})");
            return default;
        }

        /// <summary>
        /// Sets the value of the named field on the given target.
        /// </summary>
        /// <param name="_FieldName">The name of the field you want to set.</param>
        /// <param name="_Value">The value you want to set on the field.</param>
        /// <param name="_Target">The object of which you want to set the field.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <typeparam name="TFieldType">The type of the field to set.</typeparam>
        public static void SetFieldValue<TFieldType>
        (
            string _FieldName,
            TFieldType _Value,
            object _Target,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            FieldInfo field = _Target.GetType().GetField(_FieldName, _BindingFlags);
            if (field != null)
                field.SetValue((_BindingFlags & BindingFlags.Static) != 0 ? null : _Target, _Value);
            else
                Debug.LogWarning($"The field {_FieldName} doesn't exist on the given object's type ({_Target.GetType()})");
        }
        
        /// <summary>
        /// Gets the value of the named property (the "get" accessor) from the given target.
        /// </summary>
        /// <param name="_PropertyName">The name of the property you want to get.</param>
        /// <param name="_Target">The object from which you want to get the property.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <typeparam name="TPropertyType">The expected type of the property.</typeparam>
        /// <returns>Returns the found property value, or the default type value if the property doesn't exist on the
        /// target.</returns>
        public static TPropertyType GetPropertyValue<TPropertyType>
        (
            string _PropertyName,
            object _Target,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            object value = GetPropertyValue(_PropertyName, _Target, _BindingFlags);
            return value != null ? (TPropertyType) value : default;
        }
        
        /// <summary>
        /// Gets the value of the named property (the "get" accessor) from the given target.
        /// </summary>
        /// <param name="_PropertyName">The name of the property you want to get.</param>
        /// <param name="_Target">The object from which you want to get the property.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <returns>Returns the found property value, or null if the property doesn't exist on the target.</returns>
        public static object GetPropertyValue
        (
            string _PropertyName,
            object _Target,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            PropertyInfo property = _Target.GetType().GetProperty(_PropertyName, _BindingFlags);
            if (property != null)
                return property.GetValue((_BindingFlags & BindingFlags.Static) != 0 ? null : _Target);
            
            Debug.LogWarning($"The property {_PropertyName} doesn't exist on the given object's type ({_Target.GetType()})");
            return default;
        }
        
        /// <summary>
        /// Sets the value of the named property on the given target.
        /// </summary>
        /// <param name="_PropertyName">The name of the property you want to set.</param>
        /// <param name="_Value">The value you want to set on the property.</param>
        /// <param name="_Target">The object of which you want to set the property.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <typeparam name="TPropertyType">The type of the property to set.</typeparam>
        public static void SetPropertyValue<TPropertyType>
        (
            string _PropertyName,
            TPropertyType _Value,
            object _Target,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            PropertyInfo property = _Target.GetType().GetProperty(_PropertyName, _BindingFlags);
            if (property != null)
                property.SetValue((_BindingFlags & BindingFlags.Static) != 0 ? null : _Target, _Value);
            else
                Debug.LogWarning($"The field {_PropertyName} doesn't exist on the given object's type ({_Target.GetType()})");
        }

        /// <summary>
        /// Gets the named method from the given target.
        /// </summary>
        /// <param name="_MethodName">The name of the method to get.</param>
        /// <param name="_Target">The object from which you want to get the method.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <returns>Returns the found method, or the default return type value if the method doesn't exist on the
        /// target.</returns>
        public static MethodInfo GetMethod(string _MethodName, object _Target, BindingFlags _BindingFlags = INSTANCE)
        {
            return _Target.GetType().GetMethod(_MethodName, _BindingFlags);
        }

        /// <summary>
        /// Calls the named method from the given target.
        /// </summary>
        /// <param name="_MethodName">The name of the method to call.</param>
        /// <param name="_Target">The object from which you want to call the method.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        public static void CallMethod
        (
            string _MethodName,
            object _Target,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            CallMethod(_MethodName, _Target, null, _BindingFlags);
        }

        /// <summary>
        /// Calls the named method from the given target.
        /// </summary>
        /// <param name="_MethodName">The name of the method to call.</param>
        /// <param name="_Target">The object from which you want to call the method.</param>
        /// <param name="_Parameters">The eventual parameters to pass for the method call.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        public static void CallMethod
        (
            string _MethodName,
            object _Target,
            object[] _Parameters,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            MethodInfo method = GetMethod(_MethodName, _Target, _BindingFlags);
            if (method != null)
                // Invoke the static method of Static flag is enabled (so use null as obj parameter)
                method.Invoke((_BindingFlags & BindingFlags.Static) != 0 ? null : _Target, _Parameters);
            else
                Debug.LogWarning($"The method {_MethodName}() doesn't exist on the given object's type ({_Target.GetType()})");
        }
        
        /// <summary>
        /// Calls the named method from the given target, and get its returned value.
        /// </summary>
        /// <param name="_MethodName">The name of the method to call.</param>
        /// <param name="_Target">The object from which you want to call the method.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <typeparam name="TReturnType">The expected return value type of the method.</typeparam>
        /// <returns>Returns the method's returned value, or the default return type value if the method doesn't exist
        /// on the target.</returns>
        public static TReturnType CallMethod<TReturnType>
        (
            string _MethodName,
            object _Target,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            return CallMethod<TReturnType>(_MethodName, _Target, null, _BindingFlags);
        }
        
        /// <summary>
        /// Calls the named method from the given target, and get its returned value.
        /// </summary>
        /// <param name="_MethodName">The name of the method to call.</param>
        /// <param name="_Target">The object from which you want to call the method.</param>
        /// <param name="_Parameters">The eventual parameters to pass for the method call.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <typeparam name="TReturnType">The expected return value type of the method.</typeparam>
        /// <returns>Returns the method's returned value, or the default return type value if the method doesn't exist
        /// on the target.</returns>
        public static TReturnType CallMethod<TReturnType>
        (
            string _MethodName,
            object _Target,
            object[] _Parameters,
            BindingFlags _BindingFlags = INSTANCE
        )
        {
            MethodInfo method = GetMethod(_MethodName, _Target, _BindingFlags);
            if (method != null)
                // Invoke the static method of Static flag is enabled (so use null as obj parameter)
                return (TReturnType)method.Invoke((_BindingFlags & BindingFlags.Static) != 0 ? null : _Target, _Parameters);
            
            Debug.LogWarning($"The method {_MethodName}() doesn't exist on the given object's type ({_Target.GetType()})");
            return default;
        }

        /// <summary>
        /// Gets the named nested type from the given target.
        /// </summary>
        /// <param name="_NestedTypeName">The name of the nested type you're searching for.</param>
        /// <param name="_Target">The object from which you want to get the nested type.</param>
        /// <param name="_BindingFlags">The BindingFlags to use for the reflection operation. You can combine flags by
        /// using the bitwise OR operator ( | ) (e.g. BindingFlags.Public | BindingFlags.NonPublic).</param>
        /// <returns>Returns the found nested type, or null if the nested type doesn't exist on the target.</returns>
        public static Type GetNestedType
        (
            string _NestedTypeName,
            object _Target,
            BindingFlags _BindingFlags = BindingFlags.NonPublic | BindingFlags.Public
        )
        {
            Type nestedType = _Target.GetType().GetNestedType(_NestedTypeName, _BindingFlags);
            if (nestedType != null)
                return nestedType;
            
            Debug.LogWarning($"The nested type {_NestedTypeName}() doesn't exist on the given object's type ({_Target.GetType()})");
            return null;
        }
        
    }

}