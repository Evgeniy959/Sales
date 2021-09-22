namespace SalesLib
{
    public class Buyer
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Добавил для домашнего задания 
        public uint Discount { get; set; }

        public Buyer()
        {
            Id = 0;
            Name = "Unknown";
            Type = "Unknown"; // Добавил для домашнего задания
            Discount = 0;
        }
    }
}