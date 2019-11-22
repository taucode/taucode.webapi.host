using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Host.Validation
{
    public class CommandValidatorSource : ICommandValidatorSource
    {
        private static readonly object[] EmptyArgs = { };

        /// <summary>
        /// Key is command type, Value is command validator constructor
        /// </summary>
        private readonly Dictionary<Type, ConstructorInfo> _commandValidatorConstructors;

        public CommandValidatorSource(Assembly validatorsAssembly)
        {
            if (validatorsAssembly == null)
            {
                throw new ArgumentNullException(nameof(validatorsAssembly));
            }

            _commandValidatorConstructors = validatorsAssembly
                .GetTypes()
                .Select(GetCommandValidatorInfo)
                .Where(x => x != null)
                .ToDictionary(x => x.Item1, x => x.Item2);
        }

        private Tuple<Type, ConstructorInfo> GetCommandValidatorInfo(Type supposedCommandValidatorType)
        {
            var type = supposedCommandValidatorType; // lazy

            var interfaces = type.GetInterfaces();

            // search for IValidator<TCommand> where TCommand: ICommand
            foreach (var @interface in interfaces)
            {
                var isGeneric = @interface.IsConstructedGenericType;
                if (!isGeneric)
                {
                    continue;
                }

                var getGenericTypeDefinition = @interface.GetGenericTypeDefinition();
                if (getGenericTypeDefinition != typeof(IValidator<>))
                {
                    continue;
                }

                var supposedCommandType = @interface.GetGenericArguments().Single();
                var argInterfaces = supposedCommandType.GetInterfaces();
                if (argInterfaces.Contains(typeof(ICommand)))
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                    {
                        throw new ArgumentException($"Type '{type.FullName}' does not have a parameterless constructor.");
                    }

                    return Tuple.Create(supposedCommandType, constructor);
                }
            }

            return null;
        }

        public Type[] GetCommandTypes() => _commandValidatorConstructors.Keys.ToArray();

        public IValidator<TCommand> GetValidator<TCommand>() where TCommand : ICommand
        {
            _commandValidatorConstructors.TryGetValue(typeof(TCommand), out var ctor);
            if (ctor == null)
            {
                return null;
            }

            var validator = ctor.Invoke(EmptyArgs);
            return (IValidator<TCommand>)validator;
        }
    }
}
