namespace MyWebApp2.Models
{
    public class Voc
    {
        public int Id { get; set; }
        public string Vocabulary { get; set; }
        public string PartOfSpeeach { get; set; }
        public string Meaning { get; set; }
        public string Sentence { get; set; }
        public int StackId { get; set; }
        public int Count { get; set; }
        public Voc()
        {

        }
    }
}
