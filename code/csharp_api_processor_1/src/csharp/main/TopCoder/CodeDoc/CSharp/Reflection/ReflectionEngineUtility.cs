// ReflectionEngineUtility.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Reflection;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// This internal and sealed class represents a utiltity of ReflectionEngine that contains a set of static methods
    /// that will be used by ReflectionEngine. The constructor of this class is private so that we can not create an
    /// instance of this class. Currently it contains the methods to get the unique ID of types and members according to
    /// the naming rules specified in http://msdn2.microsoft.com/en-us/library/fsbx0t7x(VS.71).aspx and the methods to
    /// get the visibility and modifiers of types and members. The developer can add any other helper methods that will
    /// be used ReflectionEngine here.
    /// </summary>
    ///
    /// <threadsafety>
    /// <para>Thread Safety: This class is immutable and thread safe.</para>
    /// </threadsafety>
    ///
    /// <author>urtks</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class ReflectionEngineUtility
    {
        /// <summary>
        /// <para>Private constructor to prevent this class being instantiated.</para>
        /// </summary>
        private ReflectionEngineUtility()
        {
        }

        /// <summary>
        /// <para>Used by GetUniqueID(Type type) and by GetFullNamespaceName(MemberInfo member) functions to build/ up
        /// the uniqueID of most of the /doc member.</para>
        /// </summary>
        /// <param name="type">the type to get the namespace of</param>
        /// <exception cref="ArgumentNullException">if type is null.</exception>
        private static string GetTypeNamespaceName(Type type)
        {
            Helper.ValidateNotNull(type, "type");

            //Array types need a [] postscript
            if (type.IsArray)
            {
                return GetTypeNamespaceName(type.GetElementType()) + "[]";
            }
            //Passed by reference or by pointer
            else if (type.HasElementType)
            {
                return GetTypeNamespaceName(type.GetElementType());
            }
            return type.FullName.Replace('+', '.');
        }

        /// <summary>
        /// <para>Used by all the GetMemberName() functions except the Type one. It returns the unique id of most of the
        /// /doc member.</para>
        /// </summary>
        /// <param name="member">the member to get namespace of</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        private static string GetFullNamespaceName(MemberInfo member)
        {
            Helper.ValidateNotNull(member, "member");
            return GetTypeNamespaceName(member.ReflectedType);
        }

        /// <summary>
        /// <para>Derives the member name ID for a member (including type, field, event, property and method). Used to
        /// match nodes in the /doc XML. This is an aggregate method which delegates the logic to the other overloaded
        /// methods.</para>
        /// </summary>
        /// <param name="member">the member to get unique id of</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetUniqueID(MemberInfo member)
        {
            Helper.ValidateNotNull(member, "member");

            switch (member.MemberType)
            {
                //Methods
                case MemberTypes.Constructor:
                case MemberTypes.Method:
                {
                    return GetUniqueID((MethodBase)member);
                }

                //Events
                case MemberTypes.Event:
                {
                    return GetUniqueID((EventInfo)member);
                }

                //Fields
                case MemberTypes.Field:
                {
                    return GetUniqueID((FieldInfo)member);
                }

                //Property
                case MemberTypes.Property:
                {
                    return GetUniqueID((PropertyInfo)member);
                }

                //Type
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                {
                    return GetUniqueID((Type)member);
                }
            }
            return "unknown";
        }

        /// <summary>
        /// <para>Gets the member name ID for a type. Used to match nodes in the /doc XML.</para>
        /// </summary>
        /// <param name="type">the type to derive the member name ID from</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetUniqueID(Type type)
        {
            Helper.ValidateNotNull(type, "type");
            return "T:" + GetTypeNamespaceName(type);
        }

        /// <summary>
        /// <para>Gets the member name ID for a field. Used to match nodes in the /doc XML.</para>
        /// </summary>
        /// <param name="field">the FieldInfo to derive the member name ID from</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetUniqueID(FieldInfo field)
        {
            Helper.ValidateNotNull(field, "field");
            return "F:" + GetFullNamespaceName(field) + "." + field.Name;
        }

        /// <summary>
        /// <para>Gets the member name ID for an event. Used to match nodes in the /doc XML.</para>
        /// </summary>
        /// <param name="eventInfo">the EventInfo to derive the member name ID from</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetUniqueID(EventInfo eventInfo)
        {
            Helper.ValidateNotNull(eventInfo, "eventInfo");
            return "E:" + GetFullNamespaceName(eventInfo) + "." + eventInfo.Name.Replace('.', '#').Replace('+', '#');
        }

        /// <summary>
        /// <para>Gets the member name ID for a property. Used to match nodes in the /doc XML.</para>
        /// </summary>
        /// <param name="property">the PropertyInfo to derive the member name ID from</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetUniqueID(PropertyInfo property)
        {
            Helper.ValidateNotNull(property, "property");

            string memberName = "P:" + GetFullNamespaceName(property) +
                         "." + property.Name.Replace('.', '#').Replace('+', '#');

            //Get the name in case of indexer property
            if (property.GetIndexParameters().Length > 0)
            {
                memberName += "(";

                int i = 0;

                foreach (ParameterInfo parameter in property.GetIndexParameters())
                {
                    if (i > 0)
                    {
                        memberName += ",";
                    }

                    memberName += GetTypeNamespaceName(parameter.ParameterType);

                    ++i;
                }

                memberName += ")";
            }

            return memberName;
        }

        /// <summary>
        /// <para>Gets the member name ID for a method. Used to match nodes in the /doc XML.</para>
        /// </summary>
        /// <param name="method">the MethodBase to derive the member name ID from</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetUniqueID(MethodBase method)
        {
            Helper.ValidateNotNull(method, "method");

            string memberName = "M:" + GetFullNamespaceName(method) + "."
                + method.Name.Replace('.', '#').Replace('+', '#');

            int i = 0;
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                if (i == 0)
                {
                    memberName += "(";
                }
                else
                {
                    memberName += ",";
                }

                string parameterName = GetTypeNamespaceName(parameter.ParameterType);

                parameterName = parameterName.Replace(",", ",0:");
                parameterName = parameterName.Replace("[,", "[0:,");

                memberName += parameterName.Replace('&', '@');

                ++i;
            }

            if (i > 0)
            {
                memberName += ")";
            }

            if (method is MethodInfo)
            {
                MethodInfo mi = (MethodInfo)method;
                if (mi.Name == "op_Implicit" || mi.Name == "op_Explicit")
                {
                    memberName += "~" + mi.ReturnType;
                }
            }

            return memberName;
        }

        /// <summary>
        /// <para>Gets the visibility string of a field. The visibility string of a field can be 'public', 'internal',
        /// 'private', 'protected internal'.</para>
        /// </summary>
        /// <param name="field">the field fo which to get the visibility.</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetFieldVisibility(FieldInfo field)
        {
            Helper.ValidateNotNull(field, "field");

            string result = "unknown";

            switch (field.Attributes & FieldAttributes.FieldAccessMask)
            {
                case FieldAttributes.Public:
                    result = "public";
                    break;
                case FieldAttributes.Family:
                    result = "protected";
                    break;
                case FieldAttributes.FamORAssem:
                    result = "protected internal";
                    break;
                case FieldAttributes.Assembly:
                    result = "internal";
                    break;
                case FieldAttributes.FamANDAssem:
                    result = "unknown";
                    break;
                case FieldAttributes.Private:
                    result = "private";
                    break;
                case FieldAttributes.PrivateScope:
                    result = "unknown";
                    break;
            }

            return result;
        }

        /// <summary>
        /// <para>Gets the modifiers of a field. The modifier of a property could be 'static', 'readonly' or
        /// 'const'.Returns null if no modifiers are there.</para>
        /// </summary>
        /// <param name="field">the field fo which to get the modifiers.</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetFieldModifiers(FieldInfo field)
        {
            Helper.ValidateNotNull(field, "field");

            string result = string.Empty;

            if (field.IsStatic)
            {
                result += "static ";
            }

            if (field.IsInitOnly)
            {
                result += "readonly ";
            }

            if (field.IsLiteral)
            {
                result += "const ";
            }

            result = result.Trim();

            if (result.Equals(string.Empty))
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// <para>Gets the visibility string of a property.
        /// The visibility string of a field can be 'public', 'internal', 'private', 'protected internal'.</para>
        /// </summary>
        /// <param name="property">the property fo which to get the visibilty.</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetPropertyVisibility(PropertyInfo property)
        {
            Helper.ValidateNotNull(property, "property");

            MethodInfo method;

            if (property.GetGetMethod(true) != null)
            {
                method = property.GetGetMethod(true);
            }
            else
            {
                method = property.GetSetMethod(true);
            }

            return GetMethodVisibility(method);
        }

        /// <summary>
        /// <para>Gets the modifiers of a property.
        /// The modifier of a property could be 'static', 'abstract' or 'virtual' or 'override' along with
        /// 'read', 'write' or 'read-write'. Returns null if no modifiers are there.</para>
        /// </summary>
        /// <param name="property">the property fo which to get the modifiers.</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetPropertyModifiers(PropertyInfo property)
        {
            Helper.ValidateNotNull(property, "property");

            string[] result = new string[] { string.Empty, string.Empty };

            MethodInfo getter = property.GetGetMethod(true);
            MethodInfo setter = property.GetSetMethod(true);

            //Get property modifier
            string modifier = GetMethodModifiers(getter != null ? getter : setter);
            modifier = modifier == null ? string.Empty : modifier + " ";

            if (getter != null)
            {
                result[0] += "read";
            }
            if (setter != null)
            {
                result[1] += "write";
            }

            //example: "virtual read-write"
            if (result[0] != string.Empty && result[1] != string.Empty)
            {
                return modifier + result[0] + "-" + result[1];
            }
            //example: "virtual read"
            else
            {
                return modifier + result[0] + result[1];
            }
        }

        /// <summary>
        /// <para>Gets the visibility string of a method. The visibility string of a method could be 'public',
        /// 'internal', 'private', 'protected internal'.</para>
        /// </summary>
        /// <param name="method">the method for which to get the visibility.</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetMethodVisibility(MethodBase method)
        {
            Helper.ValidateNotNull(method, "method");

            string result;

            switch (method.Attributes & MethodAttributes.MemberAccessMask)
            {
                case MethodAttributes.Public:
                    result = "public";
                    break;
                case MethodAttributes.Family:
                    result = "protected";
                    break;
                case MethodAttributes.FamORAssem:
                    result = "protected internal";
                    break;
                case MethodAttributes.Assembly:
                    result = "internal";
                    break;
                case MethodAttributes.FamANDAssem:
                    result = "unknown";
                    break;
                case MethodAttributes.Private:
                    result = "private";
                    break;
                case MethodAttributes.PrivateScope:
                    result = "unknown";
                    break;
                default:
                    result = "unknown";
                    break;
            }

            return result;
        }

        /// <summary>
        /// <para>Gets the modifiers of a method. The modifier of a method could be 'static',
        /// 'abstract' or 'virtual' or 'override'. Returns null if no modifiers are there.</para>
        /// </summary>
        /// <param name="method">the method for which to get the modifiers.</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetMethodModifiers(MethodBase method)
        {
            Helper.ValidateNotNull(method, "method");

            string result = null;
            MethodAttributes methodAttributes = method.Attributes;

            if ((methodAttributes & MethodAttributes.Static) > 0)
            {
                result = "static";
            }
            else if ((methodAttributes & MethodAttributes.Abstract) > 0)
            {
                result = "abstract";
            }
            else if ((methodAttributes & MethodAttributes.Virtual) > 0)
            {
                if ((methodAttributes & MethodAttributes.NewSlot) > 0)
                {
                    result = "virtual";
                }
                else
                {
                    result = "override";
                }
            }
            return result;
        }

        /// <summary>
        /// <para>Gets the visibility string of a type. The visibility string of a type could be 'public', 'internal',
        /// 'private', 'protected internal'.</para>
        /// </summary>
        /// <param name="type">the method for which to get the visibility.</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetTypeVisibility(Type type)
        {
            Helper.ValidateNotNull(type, "type");

            string result = string.Empty;

            switch (type.Attributes & TypeAttributes.VisibilityMask)
            {
                case TypeAttributes.Public:
                    result = "public";
                    break;
                case TypeAttributes.NotPublic:
                    result = "internal";
                    break;
                case TypeAttributes.NestedPublic:
                    result = "public";
                    break;
                case TypeAttributes.NestedFamily:
                    result = "protected";
                    break;
                case TypeAttributes.NestedFamORAssem:
                    result = "protected internal";
                    break;
                case TypeAttributes.NestedAssembly:
                    result = "internal";
                    break;
                case TypeAttributes.NestedFamANDAssem:
                    result = "unknown";
                    break;
                case TypeAttributes.NestedPrivate:
                    result = "private";
                    break;
            }

            return result;
        }

        /// <summary>
        /// <para>Gets the modifiers of a type. The modifier of a type could be 'sealed', 'abstract' or the combination
        /// of them. Returns null if no modifiers are there.</para>
        /// </summary>
        /// <param name="type">the method for which to get the modifiers.</param>
        /// <exception cref="ArgumentNullException">if argument is null.</exception>
        public static string GetTypeModifiers(Type type)
        {
            Helper.ValidateNotNull(type, "type");

            string result = string.Empty;

            if (type.IsAbstract)
            {
                result += "abstract ";
            }

            if (type.IsSealed)
            {
                result += "sealed ";
            }

            result = result.Trim();
            if (result.Equals(string.Empty))
            {
                return null;
            }
            return result;
        }
    }
}
