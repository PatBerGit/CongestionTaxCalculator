namespace CongestionModels.Models.Configs
{
    
    public record TaxHours
    {
        private string _start;
        private string _end;
        public string Start
        {
            set
            {
                _start = value;
                StartOnly = ConvertStrToTimeOnly(value);
            }
        }
        public TimeOnly StartOnly { get; set; }
        public string End
        {
            set
            {
                _end = value;
                EndOnly = ConvertStrToTimeOnly(value);
            }
        }
        public TimeOnly EndOnly { get; set; }
        public int Tax { get; set; }

        private TimeOnly ConvertStrToTimeOnly(string input)
        {
            if (input == null)
                throw new ArgumentNullException(input);

            var splited = input.Split(":");
            return new TimeOnly(int.Parse(splited[0]), int.Parse(splited[1]));
        }
    }
}
