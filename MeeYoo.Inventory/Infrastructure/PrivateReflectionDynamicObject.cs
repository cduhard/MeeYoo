using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace MeeYoo.Inventory.Infrastructure
{
    internal class PrivateReflectionDynamicObject : DynamicObject
    {
        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static readonly IDictionary<Type, IDictionary<string, IProperty>> _propertiesOnType =
            new ConcurrentDictionary<Type, IDictionary<string, IProperty>>();

        // Simple abstraction to make field and property access consistent


        private object RealObject { get; set; }

        internal static object WrapObjectIfNeeded(object o)
        {
            // Don't wrap primitive types, which don't have many interesting internal APIs
            if (o == null || o.GetType().IsPrimitive || o is string)
                return o;

            return new PrivateReflectionDynamicObject {RealObject = o};
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var prop = GetProperty(binder.Name);

            // Get the property value
            result = prop.GetValue(RealObject, index: null);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = WrapObjectIfNeeded(result);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var prop = GetProperty(binder.Name);

            // Set the property value
            prop.SetValue(RealObject, value, index: null);

            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            // The indexed property is always named "Item" in C#
            var prop = GetIndexProperty();
            result = prop.GetValue(RealObject, indexes);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = WrapObjectIfNeeded(result);

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            // The indexed property is always named "Item" in C#
            var prop = GetIndexProperty();
            prop.SetValue(RealObject, value, indexes);
            return true;
        }

        // Called when a method is called
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = InvokeMemberOnType(RealObject.GetType(), RealObject, binder.Name, args);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = WrapObjectIfNeeded(result);

            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = Convert.ChangeType(RealObject, binder.Type);
            return true;
        }

        public override string ToString()
        {
            return RealObject.ToString();
        }

        private IProperty GetIndexProperty()
        {
            // The index property is always named "Item" in C#
            return GetProperty("Item");
        }

        private IProperty GetProperty(string propertyName)
        {
            // Get the list of properties and fields for this type
            var typeProperties = GetTypeProperties(RealObject.GetType());

            // Look for the one we want
            IProperty property;
            if (typeProperties.TryGetValue(propertyName, out property))
            {
                return property;
            }

            // The property doesn't exist

            // Get a list of supported properties and fields and show them as part of the exception message
            // For fields, skip the auto property backing fields (which name start with <)
            var propNames = typeProperties.Keys.Where(name => name[0] != '<').OrderBy(name => name);
            throw new ArgumentException(
                String.Format(
                    "The property {0} doesn't exist on type {1}. Supported properties are: {2}",
                    propertyName, RealObject.GetType(), String.Join(", ", propNames)));
        }

        private static IDictionary<string, IProperty> GetTypeProperties(Type type)
        {
            // First, check if we already have it cached
            IDictionary<string, IProperty> typeProperties;
            if (_propertiesOnType.TryGetValue(type, out typeProperties))
            {
                return typeProperties;
            }

            // Not cache, so we need to build it

            typeProperties = new ConcurrentDictionary<string, IProperty>();

            // First, add all the properties
            foreach (var prop in type.GetProperties(bindingFlags).Where(p => p.DeclaringType == type))
            {
                typeProperties[prop.Name] = new Property {PropertyInfo = prop};
            }

            // Now, add all the fields
            foreach (var field in type.GetFields(bindingFlags).Where(p => p.DeclaringType == type))
            {
                typeProperties[field.Name] = new Field {FieldInfo = field};
            }

            // Finally, recurse on the base class to add its fields
            if (type.BaseType != null)
            {
                foreach (var prop in GetTypeProperties(type.BaseType).Values)
                {
                    typeProperties[prop.Name] = prop;
                }
            }

            // Cache it for next time
            _propertiesOnType[type] = typeProperties;

            return typeProperties;
        }

        private static object InvokeMemberOnType(Type type, object target, string name, object[] args)
        {
            try
            {
                // Try to incoke the method
                return type.InvokeMember(
                    name,
                    BindingFlags.InvokeMethod | bindingFlags,
                    null,
                    target,
                    args);
            }
            catch (MissingMethodException)
            {
                // If we couldn't find the method, try on the base class
                if (type.BaseType != null)
                {
                    return InvokeMemberOnType(type.BaseType, target, name, args);
                }
                //quick greg hack to allow methods to not exist!
                return null;
            }
        }

        #region Nested type: Field

        private class Field : IProperty
        {
            internal FieldInfo FieldInfo { get; set; }

            #region IProperty Members

            string IProperty.Name
            {
                get { return FieldInfo.Name; }
            }


            object IProperty.GetValue(object obj, object[] index)
            {
                return FieldInfo.GetValue(obj);
            }

            void IProperty.SetValue(object obj, object val, object[] index)
            {
                FieldInfo.SetValue(obj, val);
            }

            #endregion
        }

        #endregion

        #region Nested type: IProperty

        private interface IProperty
        {
            string Name { get; }
            object GetValue(object obj, object[] index);
            void SetValue(object obj, object val, object[] index);
        }

        #endregion

        // IProperty implementation over a PropertyInfo

        #region Nested type: Property

        private class Property : IProperty
        {
            internal PropertyInfo PropertyInfo { get; set; }

            #region IProperty Members

            string IProperty.Name
            {
                get { return PropertyInfo.Name; }
            }

            object IProperty.GetValue(object obj, object[] index)
            {
                return PropertyInfo.GetValue(obj, index);
            }

            void IProperty.SetValue(object obj, object val, object[] index)
            {
                PropertyInfo.SetValue(obj, val, index);
            }

            #endregion
        }

        #endregion
    }
}