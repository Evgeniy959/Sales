namespace SalesLib
{
    public class Buyer
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Добавил для домашнего задания 
        public uint Discount { get; set; }
        public string First_Name { get; set; } // Добавил для домашнего задания
        public string Last_Name { get; set; } // Добавил для домашнего задания
        public string Phone { get; set; } // Добавил для домашнего задания

        public Buyer()
        {
            Id = 0;
            Name = "Unknown";
            First_Name = "Unknown"; // Добавил для домашнего задания
            Last_Name = "Unknown"; // Добавил для домашнего задания
            Phone = "Unknown"; // Добавил для домашнего задания
            Type = "Unknown"; // Добавил для домашнего задания
            Discount = 0;
        }
    }
}