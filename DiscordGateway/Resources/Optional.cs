using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using DiscordGateway.Resources.Serialization;

namespace DiscordGateway.Resources
{
    public static class Optional
    {
        public static Optional<T> FromValue<T>(T value)
            => Optional<T>.FromValue(value);

        public static Optional<T> Unspecified<T>()
            => Optional<T>.Unspecified;
    }

    [JsonConverter(typeof(OptionalJsonConverter))]
    public readonly struct Optional<T>
        : IEquatable<Optional<T>>
    {
        public static readonly Optional<T> Unspecified
            = new(false, default(T)!);

        public static Optional<T> FromValue(T value)
            => new(true, value);

        private Optional(
            bool    isSpecified,
            T       value)
        {
            _isSpecified    = isSpecified;
            _value          = value;
        }

        public bool IsSpecified
            => _isSpecified;

        public T Value
            => _isSpecified
                ? _value
                : throw new InvalidOperationException("Unable to retrieve an unspecified optional value");

        public bool Equals(Optional<T> optional)
            => (_isSpecified == optional._isSpecified)
                && EqualityComparer<T>.Default.Equals(_value, optional._value);

        public override bool Equals(object obj)
            => (obj is Optional<T> optional)
                && Equals(optional);

        public override int GetHashCode()
            => HashCode.Combine(_isSpecified, _value);

        public override string ToString()
            => _isSpecified
                ? "Unspecified"
                : $"{{{_value}}}";

        public static implicit operator Optional<T>(T value)
            => FromValue(value);

        public static bool operator ==(Optional<T> x, Optional<T> y)
            => x.Equals(y);

        public static bool operator !=(Optional<T> x, Optional<T> y)
            => x.Equals(y);

        private readonly bool   _isSpecified;
        private readonly T      _value;
    }
}
