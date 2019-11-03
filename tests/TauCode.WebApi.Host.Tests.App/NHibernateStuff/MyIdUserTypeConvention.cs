﻿using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using TauCode.Domain.Identities;

namespace TauCode.WebApi.Host.Tests.App.NHibernateStuff
{
    public class MyIdUserTypeConvention : IPropertyConvention, IIdConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            var propertyType = instance.Type.GetUnderlyingSystemType();
            var identityUserType = this.GetUserType(propertyType);

            if (identityUserType == null)
            {
                return;
            }

            instance.CustomType(identityUserType);
        }

        public void Apply(IIdentityInstance instance)
        {
            var propertyType = instance.Type.GetUnderlyingSystemType();
            var identityUserType = this.GetUserType(propertyType);

            if (identityUserType == null)
            {
                return;
            }

            instance.CustomType(identityUserType);
        }

        private Type GetUserType(Type propertyType)
        {
            // This convention only applies to identity properties
            if (!typeof(IId).IsAssignableFrom(propertyType))
            {
                return null;
            }

            var genericIdentityUserType = typeof(MyIdUserType<>);
            Type[] typeArgs = { propertyType };
            var identityUserType = genericIdentityUserType.MakeGenericType(typeArgs);

            return identityUserType;
        }
    }
}
