using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PayOn.Models;

namespace PayOn.Core
{
    public static class ExtensionMethods
    {
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static MessageBase CreateMaskedClone(this MessageBase obj)
        {
            MessageBase clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }

        public static PaymentBase CreateMaskedClone(this PaymentBase obj)
        {
            PaymentBase clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }

        public static PaymentRequest CreateMaskedClone(this PaymentRequest obj)
        {
            PaymentRequest clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }

        public static PaymentResponse CreateMaskedClone(this PaymentResponse obj)
        {
            PaymentResponse clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }
        public static RegistrationRequest CreateMaskedClone(this RegistrationRequest obj)
        {
            RegistrationRequest clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }
     
        public static RegistrationResponse CreateMaskedClone(this RegistrationResponse obj)
        {
            RegistrationResponse clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }

        public static ThreeDSecurePaymentRequest CreateMaskedClone(this ThreeDSecurePaymentRequest obj)
        {
            ThreeDSecurePaymentRequest clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }

        public static ThreeDSecurePaymentResponse CreateMaskedClone(this ThreeDSecurePaymentResponse obj)
        {
            ThreeDSecurePaymentResponse clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }

        public static ThreeDSecurePaymentStatusResponse CreateMaskedClone(this ThreeDSecurePaymentStatusResponse obj)
        {
            ThreeDSecurePaymentStatusResponse clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }

        public static ThreeDSecureResponse CreateMaskedClone(this ThreeDSecureResponse obj)
        {
            ThreeDSecureResponse clone = obj.DeepClone();
            clone.Card?.Mask();
            return clone;
        }
        
        public static string ToDecimalString(this int value)
        {
            return ((decimal) value / 100)
                .ToString("N2");
        }
    }
}