namespace MediNet_BE.DtoCreate.Users
{
    public class CustomerCreate : UserCreate
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Gender { get; set; }
        public DateTime Date_Of_Birth { get; set; }
    }
}
