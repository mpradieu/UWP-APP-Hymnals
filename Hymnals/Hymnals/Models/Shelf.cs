namespace Hymnals.Models
{
    //[Table("Shelves")]
    public class Shelf
    {
        //public Shelf()
        //{

        //}

        //public Shelf(string name, string detail)
        //{
        //    Name = name;
        //    Descr = detail;
        //}


        //[PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //[NotNull]
        public string Name { get; set; }

        //[NotNull]
        public string Descr { get; set; }

        //public List<Book> Books { get; set; }
    }
}
