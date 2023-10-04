using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Leveler
{
    public class ReflectedTypes<T>
    {
        public readonly Type[] Types;
        public readonly string[] TypesNames;
        public int TypeChoiceIndex;

        public ReflectedTypes()
        {
            Types = GetInheritedClasses(typeof(T));

            TypesNames = new string[Types.Length];
            for (int i = 0; i < TypesNames.Length; i++)
                TypesNames[i] = ObjectNames.NicifyVariableName(Types[i].Name);
        }

        public Type GetTypeFromString(string typeName)
        {
            int index = Array.IndexOf(TypesNames, typeName);
            return Types[index];
        }
        
        private static Type[] GetInheritedClasses(Type parentType)
        {
            //if you want the abstract classes drop the !TheType.IsAbstract but it is probably to instance so its a good idea to keep it.
            return Assembly.GetAssembly(parentType).GetTypes()
                .Where(type => type.IsClass && type.IsSubclassOf(parentType)).ToArray();
        }
    }
}