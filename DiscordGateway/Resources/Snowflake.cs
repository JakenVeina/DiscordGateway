using System;
using System.Text.Json.Serialization;

using DiscordGateway.Resources.Serialization;

namespace DiscordGateway.Resources
{
    [JsonConverter(typeof(SnowflakeJsonConverter))]
    public readonly struct Snowflake
        : IEquatable<Snowflake>,
            IComparable<Snowflake>
    {
        public const ulong DiscordEpoch
            = 1420070400000;

        public static bool TryParse(string value, out Snowflake result)
        {
            if(ulong.TryParse(value, out var snowflakeValue))
            {
                result = snowflakeValue;
                return true;
            }

            result = default;
            return false;
        }

        public Snowflake(ulong value)
            => _value = value;

        public ushort Increment
            => (ushort)(_value & 0xFFF);

        public ulong Value
            => _value;

        public DateTimeOffset Timestamp
            => DateTimeOffset.FromUnixTimeMilliseconds((long)((_value >> 22) + DiscordEpoch));

        public byte InternalWorkerId
            => (byte)((_value & 0x3E0000) >> 17);

        public byte InternalProcessId
            => (byte)((_value & 0x1F000) >> 12);

        public int CompareTo(Snowflake other)
            => _value.CompareTo(other._value);

        public override bool Equals(object? obj)
            => (obj is Snowflake other)
                && Equals(other);

        public bool Equals(Snowflake other)
            => (_value == other._value);

        public override int GetHashCode()
            => _value.GetHashCode();

        public override string ToString()
            => _value.ToString();

        public static implicit operator ulong(Snowflake snowflake)
            => snowflake._value;

        public static implicit operator Snowflake(ulong value)
            => new(value);

        public static bool operator ==(Snowflake left, Snowflake right)
            => left._value == right._value;

        public static bool operator !=(Snowflake left, Snowflake right)
            => left._value != right._value;

        public static bool operator <(Snowflake left, Snowflake right)
            => left._value < right._value;

        public static bool operator <=(Snowflake left, Snowflake right)
            => left._value <= right._value;

        public static bool operator >(Snowflake left, Snowflake right)
            => left._value < right._value;

        public static bool operator >=(Snowflake left, Snowflake right)
            => left._value <= right._value;

        private readonly ulong _value;
    }
}
