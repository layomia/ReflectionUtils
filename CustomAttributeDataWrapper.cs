﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace System.Reflection
{
    internal class CustomAttributeDataWrapper : CustomAttributeData
    {
        public CustomAttributeDataWrapper(AttributeData a, MetadataLoadContext metadataLoadContext)
        {
            var namedArguments = new List<CustomAttributeNamedArgument>();
            foreach (var na in a.NamedArguments)
            {
                var member = a.AttributeClass!.GetMembers(na.Key).First();
                namedArguments.Add(new CustomAttributeNamedArgument(new MemberInfoWrapper(member, metadataLoadContext), na.Value.Value));
            }

            var constructorArguments = new List<CustomAttributeTypedArgument>();
            foreach (var ca in a.ConstructorArguments)
            {
                constructorArguments.Add(new CustomAttributeTypedArgument(ca.Type.AsType(metadataLoadContext), ca.Value));
            }
            Constructor = new ConstructorInfoWrapper(a.AttributeConstructor!, metadataLoadContext);
            NamedArguments = namedArguments;
            ConstructorArguments = constructorArguments;
        }

        public override ConstructorInfo Constructor { get; }

        public override IList<CustomAttributeNamedArgument> NamedArguments { get; }

        public override IList<CustomAttributeTypedArgument> ConstructorArguments { get; }
    }
}