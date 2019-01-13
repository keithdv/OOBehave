using Newtonsoft.Json;
using OOBehave.Core;
using OOBehave.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Newtonsoft.Json
{
    // intermediate class that can be serialized by JSON.net
    // and contains the same data as ListBaseCollection
    public class ListBaseSurrogate
    {
        public ListBaseSurrogate(Type listType, ICollection collection, IPropertyValueManager propertyValueManager)
        {
            ListType = listType;
            Collection = collection;
            PropertyValueManager = propertyValueManager;
        }

        public Type ListType { get; }

        // the collection of ListBase elements
        public ICollection Collection { get; }
        // the properties of ListBaseCollection to serialize
        /// <summary>
        /// Relying on Newtosoft the resolve this so it has it's dependencies
        /// </summary>
        public IPropertyValueManager PropertyValueManager { get; }

        public IRuleResultList RuleResultList { get; set; }

        public IRuleResult OverrideResult { get; set; }

        public bool IsNew { get; set; }
        public bool IsChild { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ListBaseCollectionConverter : JsonConverter
    {

        public ListBaseCollectionConverter(IServiceScope scope)
        {
            Scope = scope;
        }

        public IServiceScope Scope { get; }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsInterface)
            {
                return objectType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IListBase<>));
            }
            else
            {
                return GetListBase(objectType) != null;
            }
        }

        public override object ReadJson(
            JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            var surrogate = serializer.Deserialize<ListBaseSurrogate>(reader);

            var list = (IListBase)Scope.Resolve(surrogate.ListType);

            foreach (var i in surrogate.Collection)
            {
                list.Add(i);
            }

            GetListBase(list.GetType()).InvokeMember("PropertyValueManager", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.FlattenHierarchy, null, list, new object[] { surrogate.PropertyValueManager });

            // ValidateListBase
            var validateType = GetValidateListBase(list.GetType());
            if (validateType != null)
            {
                var ruleProp = validateType.GetProperty("RuleManager", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var ruleManager = (IRuleManager)ruleProp.GetValue(list);
                ruleManager.GetType().InvokeMember("SetSerializedResults", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.FlattenHierarchy, null, ruleManager, new object[] { surrogate.RuleResultList, surrogate.OverrideResult });
            }

            var editType = GetEditListBase(list.GetType());
            if (editType != null)
            {
                editType.InvokeMember(nameof(IEditBase.IsNew), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.FlattenHierarchy, null, list, new object[] { surrogate.IsNew });
                editType.InvokeMember(nameof(IEditBase.IsChild), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.FlattenHierarchy, null, list, new object[] { surrogate.IsChild });
                editType.InvokeMember(nameof(IEditBase.IsDeleted), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.FlattenHierarchy, null, list, new object[] { surrogate.IsDeleted });
            }

            return list;
        }

        private Type GetListBase(Type type)
        {
            do
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ListBase<,>))
                {
                    return type;
                }
                type = type.BaseType;
            } while (type != null);
            return null;
        }

        private Type GetValidateListBase(Type type)
        {
            do
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValidateListBase<,>))
                {
                    return type;
                }
                type = type.BaseType;
            } while (type != null);
            return null;
        }

        private Type GetEditListBase(Type type)
        {
            do
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(EditListBase<,>))
                {
                    return type;
                }
                type = type.BaseType;
            } while (type != null);
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {

            var itemType = GetListBase(value.GetType()).GetGenericArguments()[1];
            var listType = typeof(List<>).MakeGenericType(itemType);
            var list = (IList)Activator.CreateInstance(listType, value);

            // Get PropertyValueManager property
            var pvmProp = GetListBase(value.GetType()).GetProperty("PropertyValueManager", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var pvm = (IPropertyValueManager)pvmProp.GetValue(value);
            var surrogate = new ListBaseSurrogate(value.GetType(), list, pvm);

            // ValidateListBase
            var validateType = GetValidateListBase(value.GetType());
            if (validateType != null)
            {
                var ruleProp = validateType.GetProperty("RuleManager", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var ruleManager = (IRuleManager)ruleProp.GetValue(value);
                var ruleResultsProp = ruleManager.GetType().GetProperty("Results", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var ruleResults = (IRuleResultList)ruleResultsProp.GetValue(ruleManager);
                surrogate.OverrideResult = ruleResults.OverrideResult;
                surrogate.RuleResultList = ruleResults;
            }

            if (value is IEditBase edit)
            {
                surrogate.IsNew = edit.IsNew;
                surrogate.IsChild = edit.IsChild;
                surrogate.IsDeleted = edit.IsDeleted;
            }


            serializer.Serialize(writer, surrogate);

        }
    }
}
