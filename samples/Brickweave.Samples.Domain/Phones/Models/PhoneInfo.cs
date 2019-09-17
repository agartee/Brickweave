namespace Brickweave.Samples.Domain.Phones.Models
{
    public class PhoneInfo
    {
        public PhoneInfo(PhoneId id, PhoneType phoneType, string number)
        {
            Id = id;
            PhoneType = phoneType;
            Number = number;
        }

        public PhoneId Id { get; }
        public PhoneType PhoneType { get; }
        public string Number { get; }
    }
}
