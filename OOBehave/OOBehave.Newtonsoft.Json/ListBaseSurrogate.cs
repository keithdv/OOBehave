using Newtonsoft.Json;
using OOBehave.Core;
using OOBehave.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public IDictionary<int, IRuleResult> RuleResults { get; set; }

        public bool IsNew { get; set; }
        public bool IsChild { get; set; }

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
                return typeof(IListBase).IsAssignableFrom(objectType);
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
                var ruleProp = validateType.GetProperty("RuleExecute", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var ruleExecute = (IRuleExecute)ruleProp.GetValue(list);
                ruleExecute.GetType().InvokeMember("SetSerializedResults", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.FlattenHierarchy, null, ruleExecute, new object[] { surrogate.RuleResults });
            }

            var editType = GetEditListBase(list.GetType());
            if(editType != null)
            {
                editType.InvokeMember(nameof(IEditBase.IsNew), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.FlattenHierarchy, null, list, new object[] { surrogate.IsNew });
                editType.InvokeMember(nameof(IEditBase.IsChild), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.FlattenHierarchy, null, list, new object[] { surrogate.IsChild });
            }

            return list;
        }

        private Type GetListBase(Type type)
        {
            do
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ListBase<>))
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
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValidateListBase<>))
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
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(EditListBase<>))
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

            var itemType = GetListBase(value.GetType()).GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(itemType);
            var list = (IList)Activator.CreateInstance(listType, value);

            // Get PropertyValueManager property
            var pvmProp = value.GetType().GetProperty("PropertyValueManager", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.FlattenHierarchy);
            var pvm = (IPropertyValueManager)pvmProp.GetValue(value);
            var surrogate = new ListBaseSurrogate(value.GetType(), list, pvm);

            // ValidateListBase
            var validateType = GetValidateListBase(value.GetType());
            if (validateType != null)
            {
                var ruleProp = validateType.GetProperty("RuleExecute", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var ruleExecute = (IRuleExecute)ruleProp.GetValue(value);
                var ruleResultsProp = ruleExecute.GetType().GetProperty("Results", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var ruleResults = (IDictionary<int, IRuleResult>)ruleResultsProp.GetValue(ruleExecute);
                surrogate.RuleResults = ruleResults;
            }

            if(value is IEditBase edit)
            {
                surrogate.IsNew = edit.IsNew;
                surrogate.IsChild = edit.IsChild;
            }
             

            serializer.Serialize(writer, surrogate);

        }
    }
}
